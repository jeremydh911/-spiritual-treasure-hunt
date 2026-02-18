Spiritual Systems — Fruits & Armor (design spec)

Overview
- This document specifies the in‑game representation for the Fruits of the Spirit (collectible "energy layer") and the Spiritual Armor (Ephesians) mechanics.
- Goals: make spiritual growth visible and collectible without endorsing superstition; ensure Spiritual Armor is universal and free for every player.

Fruits of the Spirit — core concept
- Each fruit is a collectible energy piece tied to a virtue: Love, Joy, Peace, Patience, Kindness, Goodness, Faithfulness, Gentleness, Self‑control.
- Appearance: a colored ribbon/particle layer, small emblem, or gem that sits as an "energy layer" around the character.
- Colors:
  - Love — #E53935
  - Joy — #FFD54F
  - Peace — #4FC3F7
  - Patience — #FFB74D
  - Kindness — #F48FB1
  - Goodness — #66BB6A
  - Faithfulness — #9575CD
  - Gentleness — #80CBC4
  - Self‑control — #5C6BC0

Scripture usage note
- Scripture in gameplay is pedagogical first: memory, prayer, encouragement, and discernment. It may also be used to "dispel lies" in the world (e.g., remove a TruthBarrier) or provide metaphorical spiritual 'weapon' boosts in appropriate contexts. See `Design/ScripturePolicy.md` for canonical vs secondary sources guidance.

Collection & Display
- Players earn fruit pieces by completing virtue‑focused quests/mini‑games.
- Each fruit is individually wearable. Players may equip 1–9 fruits at a time.
- When all fruits are collected, players unlock the "Rainbow Fruit Layer" cosmetic which visually composes all fruit colors.
- Fruit mechanics are cosmetic + pedagogical (e.g., small community morale bonuses, memory/recall rewards) — never couched as magical.

Gameplay & Progression
- Fruit acquisition rate: balanced to reward steady play and learning (not grindy, not monetized).
- Fruits may be gifted (as revive tokens, encouragement) but cannot be sold as pay‑to‑win items.
- Mature Mode can show theological notes about each fruit (Galatians 5) behind age verification and church override.

Spiritual Armor (Ephesians) — design rules
- Every player receives the full Ephesians set at profile creation:
  - Belt of Truth
  - Breastplate of Righteousness
  - Shoes of the Gospel of Peace
  - Shield of Faith
  - Helmet of Salvation
  - Sword of the Spirit (Word of God)
- Armor serves as both a symbolic identity layer and as lightweight gameplay modifiers (e.g., small defense/stat bonuses tied to spiritual practices).
- Armor is NOT purchasable and must not be gated behind IAP. Visual upgrades may be earned through spiritual growth and community service.
- UI: each armor piece is visible in the Player Profile with short Scripture refs and an explanation of biblical meaning.

Data model (high level)
- CosmeticItem fields: isFruitsLayer, fruitName, fruitColorHex, isSpiritualArmorPiece, armorPieceName
- PlayerProfile fields: ownedFruits[], ownedArmorPieces[] (EnsureSpiritualArmor() called on profile creation)

Vetting & Theological rules
- All fruit and armor copy must include scriptureRefs or theologicalNote.
- No wording that implies objects have independent supernatural power.
- Per‑denom approvals required for iconography or sacramental language.

Acceptance criteria
- Players can collect each fruit and combine into a Rainbow Fruit Layer.
- New profiles show HasAllArmorPieces() == true.
- Playtests confirm children interpret fruits as symbolic/reminders (not magical) ≥ 80%.

UI/UX samples
- Fruit tooltip: "Love (Fruit) — reminds us to put others first. Galatians 5:22"
- Armor tooltip: "Belt of Truth — stand firm in God's truth. Ephesians 6:14"

UI Wireframe (Fruit Collection & Rainbow)
- Main screen area: "My Fruits" list showing owned fruits as color chips with name + small icon.
  - Each fruit entry: color swatch, name, Equip / Unequip button, short scripture link.
  - When equipped, the fruit chip shows a subtle glow and changes the character preview.
- Rainbow section: shows progress bar (collected 6/9) and a large "Equip Rainbow" button when unlocked.
- Quick access: small "Armor" panel always visible showing the six Ephesians pieces (icons + scripture refs).
- Accessibility: text labels and high‑contrast outlines for colorblind users; tooltip read‑aloud for TTS.

Notes for engineers
- Represent fruit ownership as lightweight IDs (string keys) and avoid storing sensitive player behavior tied to fruit collection.
- Ensure armor is created on the server for authoritative saves, but mirrored locally for offline play.

Open items
- Decide whether rainbow layer should be equippable with conditional visual effects (performance budget consideration for low‑end devices).
- Add sample art direction and color swatches for artist implementation.