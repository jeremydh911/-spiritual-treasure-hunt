Angels — design & implementation notes

Goal
- Include angels in the game as biblical, helpful ministering beings (messengers, protectors, guides, revivers). Angels always benefit the player and are explicitly scripturally contextualized.

Biblical roles to model
- Messenger: brings God‑centered messages (e.g., announce a quest or truth). (Luke 1)
- Protector / Guardian: protects the faithful (Psalm 91; Hebrews 1:14)
- Ministering / Reviver: helps restore life or aids in rescue (Acts 12:7; ministering spirits)
- Guide / Escort: leads people to safety or truth (angelic guidance in OT narratives)

Design rules
- Always attribute agency to God/Scripture — angels serve God's purposes (no occult framing).
- Angels provide clear player benefits (revive, guide to hidden treasure, unlock devotional content).
- Avoid supernatural gamification that suggests objects or rituals cause angelic action.
- All angel content must include scriptureRefs and theologicalNote for vetting.

How angels appear in gameplay
- NPC encounter: an angel appears after certain quests or at seed locations (Eden flashbacks, temple sites).
- Passive aid: angels can auto‑revive a player once per day (configurable) or provide guidance markers to hidden scripture treasures.
- Reward anchor: angels may unlock a short devotional or play an encouraging audio message.

Adult / Mature Mode angel helps
- In Mature Mode (18+ via age verification), angels may provide deeper, pastoral‑level help such as:
  - Adult counsel summaries for complex topics (grief, doubt, sexuality, leadership ethics)
  - Comfort in grief and guidance for pastoral care
  - Concise exegesis on difficult texts (scriptural interpretation assistance)
- These adult helps are strictly framed as God's care, not as supernatural rituals, and should point players to pastoral resources.

Angelic assets & UI
- Angel icon and small animation (no occult glyphs). Visuals are bright, service‑oriented (wings motif, light ribbons) and clearly symbolic.
- Tooltip: "Angel (Messenger) — helps guide you; Scriptural reference: Hebrews 1:14." (Mature Mode contains exegesis.)

Data model
- AngelItem (id, name, role, scriptureRefs[], description, effectType)
- PlayerProfile: track lastAngelReviveAt (UTC timestamp) and angelInteractions[]

Server & moderation
- Track angel interactions in audit log for safety and moderation (why an angel revived a player, who requested it).

Sample in‑game uses
- AngelGuide quest: an angel points the player to a hidden Scripture fragment (teaching + reward).
- AngelRevive event: friend or church member can request the angel‑style revive token to restore a downed player.
- Adult assistance: angel provides counseling prompts or points the player to mature guides (gated).

Vetting
- All angel scenes pass theological vetting; per‑denom sensitivity flagged if iconography is denominationally sensitive.

Acceptance criteria
- Angel encounters provide only positive, non‑superstitious benefits and include scripture references.
- Players can use an angel revive no more than the configured limit and see a clear parental/church note about the mechanic.