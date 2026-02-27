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
  console.log('Starting backend server for content vetting tests...');
  const srv = spawn('node', ['server.js'], { cwd: path.join(__dirname, '..'), env: process.env, stdio: ['ignore','pipe','pipe'] });

  srv.stdout.on('data', d => process.stdout.write(`[srv] ${d}`));
  srv.stderr.on('data', d => process.stderr.write(`[srv.err] ${d}`));

  try {
    await waitForServer('http://localhost:4000', 8000);

    // read a truth ID from content file
    const fs = require('fs');
    // tests execute from backend/test, but truth files live in the repo root
    const truthsPath = path.join(__dirname, '..', '..', 'Content', 'Truths', 'truths_index.json');
    if (!fs.existsSync(truthsPath)) throw new Error(`truths index not found at ${truthsPath}`);
    const raw = JSON.parse(fs.readFileSync(truthsPath, 'utf8'));
    const truths = Array.isArray(raw) ? raw : (Array.isArray(raw.truths) ? raw.truths : []);
    if (truths.length === 0) throw new Error('no truths in index');
    const testId = truths[0].id;

    // fetch vet status list
    let r = await fetch('http://localhost:4000/content/vet-status');
    let j = await r.json();
    if (!j.items || !Array.isArray(j.items)) throw new Error('expected items array');

    // mark the first item as "vetted"
    r = await fetch('http://localhost:4000/admin/content/vet', {
      method: 'POST', headers: {'Content-Type':'application/json'},
      body: JSON.stringify({ contentId: testId, vetStatus: 'vetted' })
    });
    j = await r.json();
    if (!j.ok) throw new Error('admin/vet endpoint failed');

    // fetch again and verify update
    r = await fetch('http://localhost:4000/content/vet-status');
    j = await r.json();
    const found = j.items.find(x => x.id === testId);
    if (!found || found.vetStatus !== 'vetted') throw new Error('vetStatus not updated');

    console.log('\nAll content vetting tests passed.');
    process.exitCode = 0;
  } catch (err) {
    console.error('\nTest failed:', err.message);
    process.exitCode = 2;
  } finally {
    srv.kill();
  }
})();