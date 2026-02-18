Virtue System — design brief

Overview
- Virtues are a separate, permanent upgrade layer distinct from Fruits-of-the-Spirit and Spiritual Armor.
- Purpose: teach personified, scriptural virtues (Wisdom, Common Sense, Understanding, Discernment, Stewardship, etc.) as lasting character qualities that unlock narrative/dialogue options, titles, and virtue‑specific quests. **Stewardship is emphasized as a core kingdom responsibility — every player is encouraged to practice stewardship through activities, quests, and kingdom duties.**
- Persistence: virtues are permanent unlocks saved on the PlayerProfile (ownedVirtues[]).

Core rules
- Acquisition: virtues are earned through scripture‑anchored quests and church events (never IAP).
- Effect: narrative/roleplay benefits (unique dialog choices, access to virtue quests, titles). No default gameplay buffs.
- Vetting: every VirtueItem and related quest must include scriptureRefs and a theologicalNote.
- Visibility: virtues appear in the Virtue Hall UI with short Scripture anchors and in‑game location hints.

Data model
- VirtueItem (ScriptableObject): id, name, description, scriptureRefs[], livesWith[], childFriendlyText, maturityFlag
- PlayerProfile.ownedVirtues[]: string[] keys

UI
- Virtue Hall: lists owned and discoverable virtues, shows where to find them, scripture anchor, and rewards.
- Virtue badges and titles shown on player profile when earned.

Example virtues (Phase‑1)
- Wisdom — Proverbs (Lady Wisdom), lives with Common Sense and Understanding
- CommonSense (Prudence) — practical judgment in daily life
- Understanding — interpretive wisdom and meaning
- Discernment — test teachings; know true from false
- Stewardship — care for creation and gifts

Sample acceptance criteria
- Player can earn and permanently store at least 5 virtues in Phase‑1.
- Virtue unlocks a new dialogue option and a title/badge in the Player Profile.
- All Virtue content includes theological notes and passes denominational vetting when flagged sensitive.

Developer notes
- Implement ownership as lightweight string keys. Keep server authoritative for saves.
- Avoid granting gameplay advantage that could be pay‑to‑win.
- Provide Mature Mode theological expansions behind age verification and church override.