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
  console.log('Starting backend server for denom approval tests...');
  const srv = spawn('node', ['server.js'], { cwd: path.join(__dirname, '..'), env: process.env, stdio: ['ignore','pipe','pipe'] });

  srv.stdout.on('data', d => process.stdout.write(`[srv] ${d}`));
  srv.stderr.on('data', d => process.stderr.write(`[srv.err] ${d}`));

  try {
    await waitForServer('http://localhost:4000', 8000);

    // mark a truth as vetted first
    const truthsResp = await fetch('http://localhost:4000/content/vet-status');
    const truthsJson = await truthsResp.json();
    const testItem = truthsJson.items[0];
    const contentId = testItem.id;

    // add denom approval
    let r = await fetch('http://localhost:4000/admin/content/denom-approval', {
      method: 'POST', headers: {'Content-Type':'application/json'},
      body: JSON.stringify({ contentId, denomId: 'denom-1', status: 'approved', reviewerId: 'rev123' })
    });
    let j = await r.json();
    if (!j.ok) throw new Error('denom approval endpoint failed');
    if (!j.entry || j.entry.denomId !== 'denom-1') throw new Error('unexpected approval response');

    // fetch vet-status again and ensure approval appears
    r = await fetch('http://localhost:4000/content/vet-status');
    j = await r.json();
    const found = j.items.find(x => x.id === contentId);
    if (!found || !found.denomApprovals || found.denomApprovals.length === 0) throw new Error('approval not merged');

    console.log('\nDenom approval tests passed.');
    process.exitCode = 0;
  } catch (err) {
    console.error('\nTest failed:', err.message);
    process.exitCode = 2;
  } finally {
    srv.kill();
  }
})();
