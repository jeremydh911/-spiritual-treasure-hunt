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
  console.log('Starting backend server for admin/church tests...');
  const srv = spawn('node', ['server.js'], { cwd: path.join(__dirname, '..'), env: process.env, stdio: ['ignore','pipe','pipe'] });

  srv.stdout.on('data', d => process.stdout.write(`[srv] ${d}`));
  srv.stderr.on('data', d => process.stderr.write(`[srv.err] ${d}`));

  try {
    await waitForServer('http://localhost:4000', 8000);

    // 1) set church override and ensure status returns
    let r = await fetch('http://localhost:4000/admin/church/override', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ churchId: 'church-1', adultModeDisabled: true }) });
    let j = await r.json();
    if (!j.ok) throw new Error('church override failed');

    r = await fetch('http://localhost:4000/admin/church/church-1/status');
    j = await r.json();
    if (!j.adultModeDisabled) throw new Error('church override status incorrect');

    // 2) ensure sync/profile applies church override to profile
    const profile = { playerId: 'pchurch', dob: '1990-01-01', churchId: 'church-1' };
    r = await fetch('http://localhost:4000/sync/profile', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'pchurch', profile }) });
    j = await r.json();
    if (!j.ok) throw new Error('sync/profile failed for church profile');

    r = await fetch('http://localhost:4000/profile/pchurch');
    j = await r.json();
    if (!j.profile || !j.profile.adultModeDisabledByChurch) throw new Error('church override not applied to profile');

    console.log('\nAll admin/church tests passed.');
    process.exitCode = 0;
  } catch (err) {
    console.error('\nTest failed:', err.message);
    process.exitCode = 2;
  } finally {
    srv.kill();
  }
})();