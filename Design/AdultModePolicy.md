Adult Mode & Mature Content — Policy Summary

Purpose
- Define how Apocrypha and fuller theological/mature explanations are gated and approved.
- Ensure parental consent, church controls, and denominational preferences are enforceable.

Key rules
- Minimum age for mature-mode content: 18+ (age verification required; parental consent is not required for Mature Mode access).
- Church controls: per‑church/denom settings may disable mature mode for linked accounts.
- Mature Mode includes: non‑canonical secondary materials (clearly labeled), expanded theological notes, historical liturgy context, and deeper explanations of difficult passages.

Age verification flow (brief)
- The app verifies a player's age using the `dob` field and a verification UI (production: integrate a reliable age‑verification provider). Church admins can still disable Mature Mode for linked accounts. Parental consent flows remain necessary for PII, cloud saves, and social features.

Verification flow (brief)
- Player provides DOB / completes age verification (production: integrate a reliable age‑verification provider) → system marks profile as age‑verified for 18+.
- If church disables adult mode for a linked account, age verification is ineffective until church enables or grants an exception.

Data fields added
- UserProfile.adultModeEnabled (bool) — legacy/admin flag
- UserProfile.dob (ISO date string) used for age verification
- ChurchSettings.apocryphaAllowed (bool), adultModeDisabledByChurch (bool)

Security & compliance
- COPPA-safe defaults (no self‑enable for under‑13)
- PIN hashed + rate limiting
- Consent audit logs retained for compliance

Appeals
- Parents can request a church review if access is blocked; church admins can grant per‑member exceptions (audit logged).