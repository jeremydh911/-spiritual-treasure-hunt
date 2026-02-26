const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors');
const ageProvider = require('./providers/ageProvider');

const app = express();
app.use(cors());
app.use(bodyParser.json());

// In-memory stores (demo only)
const parentalConsents = {}; // key: parentId_childId => { parentId, childId, consentId }
const profiles = {}; // key: playerId => profile JSON
const churchOverrides = {}; // key: churchId => { adultModeDisabled: boolean }
const contentVetting = {}; // key: contentId => { vetStatus: 'pending'|'vetted'|'rejected' }
// denominational approvals: contentId => { churchId: boolean }
const denomApprovals = {}; // demo store

app.post('/consent/grant', (req, res) => {
  const { parentId, childId } = req.body || {};
  if (!parentId || !childId) return res.status(400).json({ error: 'parentId and childId required' });
  const consentId = `consent_${Date.now()}`;
  parentalConsents[`${parentId}_${childId}`] = { parentId, childId, consentId };
  return res.json({ consentId });
});

app.post('/consent/verify', (req, res) => {
  const { parentId, childId } = req.body || {};
  const key = `${parentId}_${childId}`;
  const found = parentalConsents[key] ? true : false;
  return res.json({ consent: found });
});

// Demo age-verification endpoint (production: integrate a verified provider)
app.post('/verify/age', (req, res) => {
  const { playerId, dob } = req.body || {};
  if (!dob) return res.status(400).json({ error: 'dob required' });

  function computeAge(dobStr) {
    const d = new Date(dobStr);
    if (isNaN(d)) return null;
    const today = new Date();
    let age = today.getUTCFullYear() - d.getUTCFullYear();
    const m = today.getUTCMonth() - d.getUTCMonth();
    if (m < 0 || (m === 0 && today.getUTCDate() < d.getUTCDate())) age--;
    return age;
  }

  const age = computeAge(dob);
  if (age === null) return res.status(400).json({ error: 'invalid dob' });
  const verified = age >= 18;

  if (playerId) {
    const existing = profiles[playerId] || {};
    existing.dob = dob;
    existing.ageVerified = verified;
    existing.ageVerifiedAt = new Date().toISOString();
    existing.ageVerifiedMethod = 'demo'; // production: replace with provider id
    profiles[playerId] = existing;

    // persist profile to disk for demo
    try {
      const fs = require('fs');
      const path = require('path');
      const dataDir = path.join(__dirname, 'data');
      if (!fs.existsSync(dataDir)) fs.mkdirSync(dataDir);
      fs.writeFileSync(path.join(dataDir, `${playerId}.json`), JSON.stringify(existing, null, 2));
    } catch (e) {
      console.warn('Failed to persist profile (demo):', e.message);
    }
  }

  appendAudit('age.verify', { playerId, dob, age, verified });
  return res.json({ ageVerified: verified, age });
});

// Provider-based verification endpoint (accepts provider token)
app.post('/verify/age/provider', async (req, res) => {
  const { playerId, providerName, token } = req.body || {};
  if (!providerName || !token) return res.status(400).json({ error: 'providerName and token required' });
  if (!ageProvider.supportedProviders.includes(providerName)) return res.status(400).json({ error: 'unsupported provider' });

  try {
    const resp = await ageProvider.verifyToken(providerName, token);
    const { verified, age, providerId } = resp || { verified: false };
    if (playerId && verified) {
      const existing = profiles[playerId] || {};
      existing.ageVerified = true;
      existing.ageVerifiedAt = new Date().toISOString();
      existing.ageVerifiedMethod = providerName;
      existing.ageVerifiedProviderId = providerId || null;
      profiles[playerId] = existing;
    }
    appendAudit('age.verify.provider', { playerId, providerName, verified, age, providerId });
    return res.json({ ageVerified: verified, age, providerId });
  } catch (err) {
    return res.status(500).json({ error: 'provider verification failed', reason: err.message });
  }
});

// Provider webhook callback (providers POST verification results here)
app.post('/verify/age/webhook', (req, res) => {
  const payload = req.body || {};
  const { playerId, providerName, providerId, verified, age } = payload;
  if (!playerId || !providerName) return res.status(400).json({ error: 'playerId and providerName required' });

  // Production: validate webhook signature using provider keys. Demo accepts the payload for demo providers.
  if (!ageProvider.verifyWebhookSignature(providerName, req.headers, payload)) {
    appendAudit('age.verify.webhook.rejected', { providerName, payload });
    return res.status(403).json({ error: 'invalid webhook signature' });
  }

  const existing = profiles[playerId] || {};
  existing.ageVerified = !!verified;
  existing.ageVerifiedAt = new Date().toISOString();
  existing.ageVerifiedMethod = providerName;
  existing.ageVerifiedProviderId = providerId || null;
  profiles[playerId] = existing;

  appendAudit('age.verify.webhook', { playerId, providerName, providerId, verified, age });
  return res.json({ ok: true });
});

// --- Admin: church override endpoints (demo only) ---
app.post('/admin/church/override', (req, res) => {
  const { churchId, adultModeDisabled } = req.body || {};
  if (!churchId || typeof adultModeDisabled !== 'boolean') return res.status(400).json({ error: 'churchId and adultModeDisabled(boolean) required' });
  churchOverrides[churchId] = { adultModeDisabled };
  appendAudit('admin.church.override', { churchId, adultModeDisabled });
  return res.json({ ok: true, churchId, adultModeDisabled });
});

app.get('/admin/church/:churchId/status', (req, res) => {
  const st = churchOverrides[req.params.churchId] || { adultModeDisabled: false };
  return res.json(st);
});

// --- Admin: content vetting endpoints (demo only) ---
app.post('/admin/content/vet', (req, res) => {
  const { contentId, vetStatus } = req.body || {};
  if (!contentId || !vetStatus) return res.status(400).json({ error: 'contentId and vetStatus required' });
  contentVetting[contentId] = { vetStatus };
  appendAudit('admin.content.vet', { contentId, vetStatus });
  return res.json({ ok: true, contentId, vetStatus });
});

// denom approval endpoint
app.post('/admin/content/approve', (req, res) => {
  const { contentId, churchId, approved } = req.body || {};
  if (!contentId || !churchId || typeof approved !== 'boolean') {
    return res.status(400).json({ error: 'contentId, churchId and approved(boolean) required' });
  }
  if (!denomApprovals[contentId]) denomApprovals[contentId] = {};
  denomApprovals[contentId][churchId] = approved;
  appendAudit('admin.content.approve', { contentId, churchId, approved });
  return res.json({ ok: true, contentId, churchId, approved });
});

app.get('/content/vet-status', (req, res) => {
  // Merge local content list with vetStatus (demo)
  const fs = require('fs');
  const path = require('path');
  const truthsPath = path.join(__dirname, '..', 'Content', 'Truths', 'truths_index.json');
  let truths = [];
  try { truths = JSON.parse(fs.readFileSync(truthsPath, 'utf8')); } catch (e) { /* ignore */ }
  const merged = truths.map(t => {
    const base = { ...t, vetStatus: (contentVetting[t.id] && contentVetting[t.id].vetStatus) || t.vetStatus || 'pending' };
    if (denomApprovals[t.id]) {
      base.denomApprovals = { ...denomApprovals[t.id] };
    }
    return base;
  });
  return res.json({ items: merged });
});

app.post('/sync/profile', (req, res) => {
  const { playerId, consentId, profile } = req.body || {};
  if (!playerId) return res.status(400).json({ error: 'playerId required' });

  // COPPA enforcement: disallow telemetry for child accounts under 13
  if (profile && profile.telemetryEnabled && profile.dob) {
    const dob = new Date(profile.dob);
    const today = new Date();
    let age = today.getUTCFullYear() - dob.getUTCFullYear();
    const m = today.getUTCMonth() - dob.getUTCMonth();
    if (m < 0 || (m === 0 && today.getUTCDate() < dob.getUTCDate())) age--;
    if (age < 13) return res.status(403).json({ error: 'COPPA: telemetry forbidden for accounts under 13' });
  }

  // Consent check: require parental consent for cloud save if profile.cloudSaveEnabled is true
  const consentFound = Object.values(parentalConsents).some(c => c.consentId === consentId);
  if (profile && profile.cloudSaveEnabled && !consentFound) return res.status(403).json({ error: 'parental consent required for cloud save' });

  // Merge church override flag if exists for player's churchId
  const churchId = profile && profile.churchId ? profile.churchId : null;
  if (churchId && churchOverrides[churchId] && churchOverrides[churchId].adultModeDisabled) {
    profile.adultModeDisabledByChurch = true;
  }

  profiles[playerId] = profile;

  // persist to disk for the demo
  try {
    const fs = require('fs');
    const path = require('path');
    const dataDir = path.join(__dirname, 'data');
    if (!fs.existsSync(dataDir)) fs.mkdirSync(dataDir);
    fs.writeFileSync(path.join(dataDir, `${playerId}.json`), JSON.stringify(profile, null, 2));
  } catch (e) {
    console.warn('Failed to persist profile to disk (demo):', e.message);
  }
  appendAudit('sync.profile', { playerId, consentId, cloudSaveEnabled: !!(profile && profile.cloudSaveEnabled) });
  return res.json({ ok: true, consent: consentFound || false });
});

app.get('/profile/:playerId', (req, res) => {
  const p = profiles[req.params.playerId] || null;
  return res.json({ profile: p });
});

// Serve printable guides from backend/guides
app.get('/guides/:name', (req, res) => {
  const name = req.params.name;
  const path = require('path').join(__dirname, 'guides', name);
  res.sendFile(path, err => {
    if (err) res.status(404).send('Guide not found');
  });
});

// Serve PDF guide if available (demo static files in guides_pdfs)
app.get('/guides/pdf/:name', (req, res) => {
  const name = req.params.name;
  const path = require('path').join(__dirname, 'guides_pdfs', name + '.pdf');
  res.sendFile(path, err => {
    if (err) res.status(404).send('PDF guide not found');
  });
});

// Simple demo auth (no production security) — returns demo token
app.post('/auth/demo-login', (req, res) => {
  const { playerId } = req.body || {};
  if (!playerId) return res.status(400).json({ error: 'playerId required' });
  const token = `demo-token-${playerId}`;
  appendAudit('auth.login', { playerId, token });
  return res.json({ token, playerId });
});

// Pastor contact requests (stored and audited)
const contactRequests = [];
app.post('/contact/pastor', (req, res) => {
  const { playerId, parentId, message } = req.body || {};
  if (!playerId || !message) return res.status(400).json({ error: 'playerId and message required' });
  const reqObj = { id: `contact_${Date.now()}`, playerId, parentId: parentId || null, message, createdAt: new Date().toISOString() };
  contactRequests.push(reqObj);
  appendAudit('contact.pastor', reqObj);
  return res.json({ ok: true, requestId: reqObj.id });
});

// Generic audit log endpoint (admin/demo)
app.post('/audit/log', (req, res) => {
  const { action, details } = req.body || {};
  if (!action) return res.status(400).json({ error: 'action required' });
  appendAudit(action, details || {});
  return res.json({ ok: true });
});

app.get('/audit', (req, res) => {
  const fs = require('fs');
  const path = require('path');
  const p = path.join(__dirname, 'audit.log');
  if (!fs.existsSync(p)) return res.json({ logs: [] });
  const raw = fs.readFileSync(p, 'utf8');
  return res.type('text').send(raw);
});

// Small helper to append to audit log
function appendAudit(action, payload) {
  try {
    const fs = require('fs');
    const path = require('path');
    const p = path.join(__dirname, 'audit.log');
    const entry = `${new Date().toISOString()} \t ${action} \t ${JSON.stringify(payload)}\n`;
    fs.appendFileSync(p, entry);
  } catch (e) {
    console.warn('appendAudit failed:', e.message);
  }
}

const port = process.env.PORT || 4000;

// Export app for tests and for embedding in other runners.
module.exports = app;

// If server.js is run directly, start the listener (normal demo mode)
if (require.main === module) {
  app.listen(port, () => console.log(`Backend demo server listening on http://localhost:${port}`));
}
