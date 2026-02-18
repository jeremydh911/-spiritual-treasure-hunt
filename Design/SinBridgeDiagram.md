# Sin → Jesus → Kingdom visual

## Purpose
A short, child‑friendly visual sequence that shows how *sin separates* us from the Kingdom and how *Jesus covers/bridges* the gap so we can enter. Use it in tutorials, quest rewards, parent/child discussion popups, or the Identity/Truths teaching flow.

---

## Recommended in‑game placement ✅
- Intro lesson / onboarding for the Salvation / Identity Truths chapter
- A short popup after a relevant quest (example: "Help the neighbor")
- Printable guide or discussion card (use the static illustration variant)

---

## Visual flow (simple) 🔁

```mermaid
flowchart LR
  A[You / Human] -->|Sin| B[Separation / Barrier]
  B -->|Jesus covers / cross becomes bridge| C[Bridge (Jesus)]
  C --> D[Kingdom / Gate] 
  style A fill:#cfe9ff,stroke:#1b72d1
  style B fill:#ffd6d6,stroke:#d12b2b
  style C fill:#fff4cc,stroke:#d1a500
  style D fill:#e6ffec,stroke:#0a7a2a
```

---

## Scriptures & short captions (suggestions)
- Romans 3:23 — "All have sinned" (label for barrier)
- Romans 6:23 — "The consequence of sin is separation" (tooltip)
- Romans 5:8 / John 3:16 — "But God showed His love — Jesus covers" (bridge label)
- John 14:6 — "Jesus is the way" (entrance callout)

---

## Style & text options (child‑friendly) 🎨
- Shapes: big rounded boxes, single‑word labels, friendly colors
- Labels: `You` / `Sin` / `Jesus covers` / `Kingdom`
- Tone: reassuring — highlight "covered" and "welcome"

---

## Implementation notes
- Use `SinBridgeUI` (runtime Unity UI) for in‑game animated sequence.
- Use the static SVG/PNG alternative for docs or printable cards.

> Designer tip: prefer the *bridge* metaphor rather than punishment imagery for younger children.

---

## Variants
- Animated: short 3‑step animation (barrier → cross appears → child walks across)
- Static: single illustration for UI background or printed guide
- Storyboard: 3 panels for small teaching card

---

If you want, I can: (pick one)
1. Generate a static SVG/PNG illustration file for docs. 
2. Add a prefab + art placeholders into the Unity project (I already added a runtime script + Editor menu). 
3. Create an animated cutscene version (timeline/Animator setup).
