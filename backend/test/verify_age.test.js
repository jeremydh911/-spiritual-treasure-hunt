const { spawn } = require('child_process');
const fetch = (...args) => import('node-fetch').then(({default:fetch}) => fetch(...args));

async function waitForServer(url, timeoutMs = 5000) {
  const start = Date.now();
  while (Date.now() - start < timeoutMs) {
    try {
      const r = await fetch(url + '/guides/ParentGuide.md');
      if (r.status === 200 || r.status === 404) return true;
    } catch (e) {
      // retry
    }
    await new Promise(r => setTimeout(r, 200));
  }
  throw new Error('Server did not respond in time');
}

(async () => {
  const path = require('path');
  // ensure self-provider secret is set for tests
  process.env.SELF_PROVIDER_SECRET = process.env.SELF_PROVIDER_SECRET || 'test-self-secret';
  console.log('Starting backend server for tests...');
  const srv = spawn('node', ['server.js'], { cwd: path.join(__dirname, '..'), env: process.env, stdio: ['ignore','pipe','pipe'] });

  srv.stdout.on('data', d => process.stdout.write(`[srv] ${d}`));
  srv.stderr.on('data', d => process.stderr.write(`[srv.err] ${d}`));

  try {
    await waitForServer('http://localhost:4000', 8000);

    // 1) DOB-based verification (under 18 -> false, over 18 -> true)
    const dobUnder = new Date(); dobUnder.setFullYear(dobUnder.getFullYear() - 16);
    const dobOver = new Date(); dobOver.setFullYear(dobOver.getFullYear() - 20);

    let r = await fetch('http://localhost:4000/verify/age', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'test1', dob: dobUnder.toISOString().slice(0,10) }) });
    let j = await r.json();
    if (j.ageVerified) throw new Error('Underage should not be verified');

    r = await fetch('http://localhost:4000/verify/age', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'test2', dob: dobOver.toISOString().slice(0,10) }) });
    j = await r.json();
    if (!j.ageVerified) throw new Error('Over-18 should be verified');

    // 2) Provider token verification (demo provider)
    r = await fetch('http://localhost:4000/verify/age/provider', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'test3', providerName: 'demo', token: 'demo-verify-true' }) });
    j = await r.json();
    if (!j.ageVerified) throw new Error('Provider token should verify in demo');

    // 3) Webhook callback handling (demo provider)
    r = await fetch('http://localhost:4000/verify/age/webhook', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'test4', providerName: 'demo', providerId: 'prov-1', verified: true, age: 21 }) });
    j = await r.json();
    if (!j.ok) throw new Error('Webhook should return ok');

    // 4) Self-hosted provider: token verification (HMAC)
    const crypto = require('crypto');
    const secret = process.env.SELF_PROVIDER_SECRET || 'test-self-secret';
    const player = 'selfPlayer';
    const ts = Date.now();
    const payload = `${player}:${ts}`;
    const sig = crypto.createHmac('sha256', secret).update(payload).digest('hex');
    const token = `${player}:${ts}:${sig}`;

    r = await fetch('http://localhost:4000/verify/age/provider', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: player, providerName: 'self', token }) });
    j = await r.json();
    if (!j.ageVerified) throw new Error('Self provider token should verify');

    // 5) Self-hosted webhook signature verification (valid signature)
    const webhookPayload = { playerId: 'selfWeb', providerName: 'self', providerId: 'self-1', verified: true, age: 30 };
    const bodyStr = JSON.stringify(webhookPayload);
    const webhookSig = 'sha256=' + crypto.createHmac('sha256', secret).update(bodyStr).digest('hex');
    r = await fetch('http://localhost:4000/verify/age/webhook', { method: 'POST', headers: {'Content-Type':'application/json', 'x-self-signature': webhookSig }, body: bodyStr });
    j = await r.json();
    if (!j.ok) throw new Error('Self webhook should return ok');

    // 6) Self-hosted webhook signature verification (invalid signature should fail)
    r = await fetch('http://localhost:4000/verify/age/webhook', { method: 'POST', headers: {'Content-Type':'application/json', 'x-self-signature': 'sha256=deadbeef' }, body: JSON.stringify(webhookPayload) });
    if (r.status !== 403) throw new Error('Invalid self webhook signature should be rejected');

    console.log('\nAll backend verification tests passed.');
    process.exitCode = 0;
  } catch (err) {
    console.error('\nTest failed:', err.message);
    process.exitCode = 2;
  } finally {
    srv.kill();
  }
})();