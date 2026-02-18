Cosmetics Policy — Vetting & Denominational Review

Overview
- Spiritual cosmetics (fruits‑of‑the‑Spirit energy layer, robes, music, titles) emphasize spiritual growth and community impact rather than physical appearance. The game represents spiritual beauty through collectible "fruits" (colored energy layers) and through Spiritual Armor (Ephesians), not through physical attractiveness.
- Every cosmetic must pass technical QA, theological vetting, and per‑denom approvals where required.

Data model (high level)
- CosmeticItem: {id, displayName, artistId, assetRefs, tags, requiresAdultMode, status, denomApprovals[]}
- denomApprovals: [{denomId, status, reviewerId, notes, reviewedAt}]

Submission & review workflow
1. Upload: creator/artist submits cosmetic via Creator Portal.
2. Automated checks: profanity/image scan, size/perf checks.
3. QA: technical/visual checks.
4. Theological review: internal + church/denom reps (hybrid model).
5. Denom-level approval: item allowed/blocked per denomination.
6. Publish: cosmetic visible only for denominations with approve status; parental and church overrides honored.

Vet checklist
- theologicalNote present and ties cosmetic to Scripture or spiritual principle
- does not encourage occult/ritual behavior
- per‑denom sensitivity flagged for symbolic items (incense-like, icons, titles)

Runtime enforcement
- CosmeticManager filters cosmetics by player.profile.denomId, parental overrides, and church settings.
- Unapproved cosmetics are hidden in UI with an explanatory tooltip.

Fruits of the Spirit energy layer
- Concept: each of the Fruits of the Spirit is represented as a collectible, colored "energy" piece (not a mystical aura). Pieces: Love, Joy, Peace, Patience, Kindness, Goodness, Faithfulness, Gentleness, Self‑control.
- Colors & examples (default UI palette):
  - Love — #E53935 (red)
  - Joy — #FFD54F (gold)
  - Peace — #4FC3F7 (sky blue)
  - Patience — #FFB74D (amber)
  - Kindness — #F48FB1 (pink)
  - Goodness — #66BB6A (green)
  - Faithfulness — #9575CD (purple)
  - Gentleness — #80CBC4 (teal)
  - Self‑control — #5C6BC0 (indigo)
- Mechanics: players earn individual fruit pieces through mini‑games/quests; pieces can be worn one at a time or combined into a full "Rainbow Fruit Layer" that displays all collected fruits together.
- Design rules: fruits are spiritual reminders with explicit scripture/context; no fruit grants occult or inherent supernatural powers. Fruits may be earned, gifted, or discovered — not sold as pay‑to‑win.

Spiritual Armor (Ephesians) — mandatory
- Every player receives the full set of Spiritual Armor by default (Belt of Truth, Breastplate of Righteousness, Shoes of the Gospel of Peace, Shield of Faith, Helmet of Salvation, Sword of the Spirit).
- Armor is core gameplay equipment (cosmetic + symbolic), not a monetized premium item; it may gain visual upgrades tied to spiritual progression but is always present for every player.
- Vetting: armor art and copy must reference Ephesians and avoid triumphalistic or superstitious language. Per‑denom notes may clarify sacramental or symbolic differences.
- Runtime: Spiritual Armor items are visible/available to all players regardless of cosmetic approval flags; they remain subject to theological QA for iconography/text.

SLA & metrics
- Target review SLA: 72 hours for initial theological review.
- Metrics tracked: approval time, approval rate by denom, player reports per cosmetic.

Appeals & moderation
- Players/parents can request review; moderators log decisions and provide reasons. 