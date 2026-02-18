QuestSpec — Template and Guidelines

Purpose
- Define the canonical structure for every quest, side‑quest and mini‑game.
- Ensure each item includes learning goals, scripture anchors, sensitivity metadata, and denominational approvals.

Quest template (required fields)
- id: unique string (e.g., GARD‑006)
- title: short, child‑friendly title
- shortDescription: 1–2 sentence summary
- scriptureRefs: array of book:chapter:verse or book ranges
- learningGoal: memorize | understand | apply | worship | service
- gameplayType: mini‑game | puzzle | narrative | scavenger | rhythm
- ageRange: 7–9 | 10–14
- virtueTags: array (Love, Wisdom, Truth, Joy, Peace, Patience, Kindness, Goodness, Faithfulness, Gentleness, SelfControl)
- sensitivity: low | medium | high
- denomTags: array of denomination ids (empty = default/neutral)
- whimsyTag (optional): boolean (true if whimsical element included)
- preFallTag (optional): boolean (true if scene is Eden/pre‑Fall or vision)
- childCopy: text shown to child players
- matureNote: theological note for Mature Mode / reviewers
- rewards: array (spiritualStrength, musicTrack, fruitReward, virtueReward, title)
- vetStatus: draft | pending | vetted | denomApproved | published

Notes on virtueReward
- `virtueReward`: optional field used to award a permanent VirtueToken (e.g., "Wisdom", "CommonSense").
- Virtues are distinct from Fruits and Spiritual Armor and unlock narrative/dialogue options rather than gameplay power by default.
- acceptanceCriteria: testable conditions (playtest & QA targets)

Required vetting fields
- theologicalNote (required for whimsy, preFall, or sensitive content)
- denomApprovals: [{denomId, status, reviewerId, notes, reviewedAt}]
- auditLogRef (reference to review history)

Content rules & policies (high level)
- All scripture used must be properly cited and follow chosen translations (default: ESV; player choice optional).
- "Whimsy not magic": playful elements allowed but never attributed to occult/pagan causation.
- Spiritual Armor (Ephesians) is mandatory and granted to every player as core gameplay equipment; it is not a purchasable cosmetic and should be provided by default.
- Fruits‑of‑the‑Spirit are collectible energy‑layer cosmetics (colored pieces) that can be earned individually or combined into a wearable rainbow; they emphasize spiritual growth and community impact.
- Stewardship is treated as a core, recurring kingdom responsibility and should be surfaced prominently in virtue, activity, and kingdom‑leadership flows.
- Sensitive scenes (violent, supernatural) must be age‑safe rewritten for child mode; Mature Mode may expose fuller explanations behind age verification and church override.
- Per‑denomination overrides must be supported and honored at runtime.

Sample quest examples (three book-level samples)

1) Genesis — "Memorial Stone" (example)
- id: GEN‑STONE
- scriptureRefs: ["Joshua 4:5-7", "Genesis 28:18-22"]
- learningGoal: remember God's faithfulness (apply)
- gameplayType: narrative + collectible
- childCopy: "Collect stones of thanks and build a memory wall!"
- matureNote: Explains memorial stones in Scripture as corporate remembrance, not magical talismans.

2) Luke — "Good Neighbor" (example)
- id: LUKE‑SAMARITAN
- scriptureRefs: ["Luke 10:25-37"]
- learningGoal: apply kindness and cross‑community love
- gameplayType: scenario‑response
- childCopy: "Choose how to help the hurt traveler — what will you do?"
- matureNote: Discusses the cultural context, Christ's interpretation, and pastoral application.

3) Romans — "Fruit Practice" (example)
- id: ROM‑FRUIT
- scriptureRefs: ["Galatians 5:22-23"]
- learningGoal: recognize fruits of the Spirit in daily actions
- gameplayType: mini‑game / skill practice
- childCopy: "Practice the fruit moves — show love, gentleness, and patience!"
- matureNote: Theological framing on Spirit‑formation and sanctification.

Publishing workflow (brief)
1. Author creates quest draft in `Content/Quests/` and sets `vetStatus: draft`.
2. Automated pre‑checks: profanity, image checks, required fields.
3. Editorial review (tech + pedagogy): vetStatus -> pending.
4. Theological review (internal + denominational, if sensitive): denomApprovals added.
5. Publish after all required approvals.

Acceptance criteria (examples)
- Memorization quests: player must recall target verse ≥ 75% in testing.
- Comprehension quests: player answers 3/4 checkpoint questions correctly.
- Vetting: every item flagged as "whimsy" or "sensitive" must have theological approval and denomApproval entries.

Files
- Example Phase‑1 JSON: `Content/Quests/phase1_quests.json` (sample entries)
- See `Design/WhimsyPolicy.md` and `Design/AdultModePolicy.md` for related policy details.