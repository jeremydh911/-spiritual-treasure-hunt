Data Minimalism & Implementation Notes

Goals
- Zero unnecessary telemetry in child builds.
- Any required telemetry is anonymized, local‑first, and opt‑outable.

Technical rules
- Do NOT collect: contacts, location, device advertising IDs, phone numbers, email addresses, or other PII without parental consent.
- Crash reports: sanitize and redact PII before writing to disk or uploading.
- Analytics: only aggregated, non‑PII metrics; default OFF for child accounts.
- Cloud saves: encrypted at rest and transmitted over TLS; parental consent required.

Implementation checklist
- TelemetryManager enforces defaults at runtime.
- CrashReporter queues anonymized reports locally; uploader gated by TelemetryManager.
- Parent Portal must provide controls to view/export/delete child data and toggle telemetry.
- No third‑party trackers included in child builds by default; add only after legal review.

Testing
- Unit tests for TelemetryManager defaults and opt‑out behavior.
- Privacy QA: verify no PII in saved crash JSONs and no analytics calls when AnalyticsEnabled == false.

Notes for engineers
- Treat telemetry as a gated feature. Any telemetry event must be reviewed and approved by product + legal.