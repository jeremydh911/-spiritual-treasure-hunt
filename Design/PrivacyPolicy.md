Privacy Policy (short — developer/internal summary)

Principles
- Minimize data collection: collect only what is necessary for core gameplay and safety.
- Child‑first defaults: analytics OFF; anonymous crash reports ON by default (opt‑outable).
- Parental consent required for PII and cloud features (cloud save, friend lists, chat).
- No advertising or 3rd‑party trackers for child accounts.

Telemetry & crash reporting
- Analytics: OFF by default. Any analytics collection must be explicit, aggregated, and opt‑in for child accounts.
- Crash reports: anonymized summaries (scene, exception stack, app version) are queued locally and uploaded only in anonymized form; parent/guardian can opt‑out.
- Support logs and screenshots: sent only when user/parent explicitly submits them.

Cloud saves & PII
- Cloud saves require parental consent; local saves are the default for child accounts.
- We store minimal profile data (display name, progress states) and encrypt any PII at rest.
- Parents can request data export or deletion; audit logs of consents are retained per legal requirements.

Data retention & deletion
- Crash summaries: retained 90 days by default (adjustable per legal needs).
- Consent logs: retained for compliance as required.
- Telemetry events are considered ephemeral and will be purged periodically (default 90 days) unless anonymized aggregates are needed for legal or safety reasons.
- Deletion requests remove PII and unlink cloud saves; aggregate, non‑PII telemetry (if any) may be retained in anonymized form.

Compliance
- COPPA / GDPR mindful defaults; parental consent flows are implemented for features that surface PII or social features.

Contact
- For privacy requests, use the Parent Portal → Privacy Center (placeholder) or contact the studio email in `README`.