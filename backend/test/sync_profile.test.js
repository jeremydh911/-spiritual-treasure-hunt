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
  console.log('Starting backend server for sync tests...');
  const srv = spawn('node', ['server.js'], { cwd: path.join(__dirname, '..'), env: process.env, stdio: ['ignore','pipe','pipe'] });

  srv.stdout.on('data', d => process.stdout.write(`[srv] ${d}`));
  srv.stderr.on('data', d => process.stderr.write(`[srv.err] ${d}`));

  try {
    await waitForServer('http://localhost:4000', 8000);

    // 1) sync/profile should reject when cloudSaveEnabled=true and no consent
    const profile = { playerId: 'p1', cloudSaveEnabled: true, dob: '2015-01-01' };
    let r = await fetch('http://localhost:4000/sync/profile', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'p1', profile }) });
    if (r.status !== 403) throw new Error('sync/profile must require parental consent for cloud saves');

    // 2) grant consent and then sync/profile should succeed
    r = await fetch('http://localhost:4000/consent/grant', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ parentId: 'par1', childId: 'p1' }) });
    let j = await r.json();
    const consentId = j.consentId;
    r = await fetch('http://localhost:4000/sync/profile', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'p1', consentId, profile }) });
    if (r.status !== 200) throw new Error('sync/profile should succeed when parental consent provided');

    // 3) COPPA: telemetryEnabled for under-13 should be rejected
    const childProfile = { playerId: 'kid1', dob: '2014-01-01', telemetryEnabled: true };
    r = await fetch('http://localhost:4000/sync/profile', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'kid1', profile: childProfile }) });
    if (r.status !== 403) throw new Error('COPPA: telemetry must be rejected for child accounts');

    console.log('\nAll sync/profile tests passed.');
    process.exitCode = 0;
  } catch (err) {
    console.error('\nTest failed:', err.message);
    process.exitCode = 2;
  } finally {
    srv.kill();
  }
})();