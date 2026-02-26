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
  console.log('Starting backend server for telemetry tests...');
  const srv = spawn('node', ['server.js'], { cwd: path.join(__dirname, '..'), env: process.env, stdio: ['ignore','pipe','pipe'] });

  srv.stdout.on('data', d => process.stdout.write(`[srv] ${d}`));
  srv.stderr.on('data', d => process.stderr.write(`[srv.err] ${d}`));

  try {
    await waitForServer('http://localhost:4000', 8000);

    // enable telemetry for a child account under 13
    const profile = { playerId: 'kid2', dob: '2015-01-01', telemetryEnabled: true };
    let r = await fetch('http://localhost:4000/sync/profile', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'kid2', profile }) });
    if (r.status !== 403) throw new Error('COPPA guard should block telemetry-enabled profile sync');

    // create an older profile and enable telemetry
    const profile2 = { playerId: 'teen1', dob: '2005-01-01', telemetryEnabled: true };
    r = await fetch('http://localhost:4000/sync/profile', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'teen1', profile: profile2 }) });
    if (r.status !== 200) throw new Error('telemetry-enabled profile sync should succeed for older users');

    // send telemetry event for teen1
    r = await fetch('http://localhost:4000/telemetry/event', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'teen1', event: 'test_event' }) });
    if (r.status !== 200) throw new Error('telemetry event should be accepted for allowed user');

    // send telemetry event for kid2 directly (not sync'd) - still blocked
    r = await fetch('http://localhost:4000/telemetry/event', { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify({ playerId: 'kid2', event: 'test_event' }) });
    if (r.status !== 403) throw new Error('telemetry event should be rejected for underage user');

    console.log('\nAll telemetry tests passed.');
    process.exitCode = 0;
  } catch (err) {
    console.error('\nTest failed:', err.message);
    process.exitCode = 2;
  } finally {
    srv.kill();
  }
})();
