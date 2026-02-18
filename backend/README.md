Backend demo (local only)

This is a tiny Node/Express demo server used for prototype cloud sync and parental consent verification.

Commands
- Install deps: npm install
- Run server: npm start

Endpoints
- POST /consent/grant  { parentId, childId } -> { consentId }
- POST /consent/verify { parentId, childId } -> { consent: boolean }
- POST /sync/profile  { playerId, consentId, profile } -> { ok: true }
- GET  /profile/:playerId -> { profile }
- GET  /guides/:name -> returns Markdown guide
- GET  /guides/pdf/:name -> returns prebuilt PDF (demo)
- POST /contact/pastor { playerId, message } -> { ok, requestId }
- POST /auth/demo-login { playerId } -> { token }
- POST /verify/age { playerId, dob } -> { ageVerified, age }  # demo age-check (18+)
- POST /verify/age/provider { playerId, providerName, token } -> { ageVerified, age, providerId }  # provider token verification (demo stub)
- POST /verify/age/webhook { playerId, providerName, providerId, verified, age } -> { ok }  # provider callback (demo)

Configuration (self‑hosted provider)
- To enable the built‑in self‑hosted provider (freeware option), set `SELF_PROVIDER_SECRET` in the environment. Example: `export SELF_PROVIDER_SECRET=supersecret`
- Token format (self provider): `playerId:timestamp:signature` where `signature = HMAC_SHA256(SELF_PROVIDER_SECRET, "playerId:timestamp")`.
- Webhook signature (self provider): include header `x-self-signature: sha256=<hex>` where the hex is HMAC_SHA256(SELF_PROVIDER_SECRET, <body‑json>).
- POST /audit/log { action, details } -> audit recorded
- GET  /audit -> raw audit log (admin/demo only)

Notes
- This is only a prototype. For production use, replace with secure auth, database, consent flows, and hardened audit logging. The demo includes file‑based persistence and static PDF guides.