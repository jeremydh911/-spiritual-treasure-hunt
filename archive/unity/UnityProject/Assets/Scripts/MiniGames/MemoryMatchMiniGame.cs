using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple memory‑match mini‑game example. Uses non‑blocking logic and calls CompleteMiniGame(questId) when win condition met.
/// - Hook up a GridLayoutGroup + card prefab (button with Image/Text).
/// - This is intentionally minimal for prototyping; replace visuals as needed.
/// </summary>
public class MemoryMatchMiniGame : MiniGameBase
{
    [Header("Game Settings")]
    public string questId; // quest to complete when the mini-game is won
    public int pairsToMatch = 3; // number of pairs required to win

    [Header("UI References")]
    public GameObject cardPrefab; // must contain a Button and a Text/Image for symbol
    public Transform gridParent; // GridLayoutGroup parent

    // Runtime
    private List<Card> cards = new List<Card>();
    private Card firstSelected;
    private Card secondSelected;
    private int matchedPairs = 0;

    [System.Serializable]
    private class Card
    {
        public string symbol;
        public GameObject go;
        public Button btn;
        public Text label;
        public bool matched;
    }

    private void Start()
    {
        if (cardPrefab == null || gridParent == null)
        {
            Debug.LogWarning("MemoryMatchMiniGame: assign cardPrefab and gridParent in inspector.");
            return;
        }
        SetupBoard();
    }

    private void SetupBoard()
    {
        // Simple symbol set for prototype — in production use art/sprites
        var symbols = new string[] { "★", "✿", "♪", "❤", "⚖" };
        var selected = new List<string>();
        for (int i = 0; i < pairsToMatch; i++) selected.Add(symbols[i % symbols.Length]);

        var pool = new List<string>();
        foreach (var s in selected) { pool.Add(s); pool.Add(s); }

        // shuffle
        for (int i = 0; i < pool.Count; i++)
        {
            int r = Random.Range(i, pool.Count);
            var tmp = pool[i]; pool[i] = pool[r]; pool[r] = tmp;
        }

        // create cards
        foreach (var sym in pool)
        {
            var go = Instantiate(cardPrefab, gridParent, false);
            var btn = go.GetComponent<Button>();
            var txt = go.GetComponentInChildren<Text>();
            if (txt != null) txt.text = ""; // hide symbol initially
            var card = new Card { symbol = sym, go = go, btn = btn, label = txt, matched = false };
            cards.Add(card);
            btn.onClick.AddListener(() => OnCardClicked(card));
        }
    }

    private void OnCardClicked(Card card)
    {
        if (card.matched) return;
        if (firstSelected != null && secondSelected != null) return; // wait until comparison

        // reveal
        if (card.label != null) card.label.text = card.symbol;

        if (firstSelected == null) { firstSelected = card; return; }
        if (secondSelected == null && card != firstSelected)
        {
            secondSelected = card;
            // allow player to see — delay may be reduced by an equipped scripture (weapon/prayer effects)
        Invoke(nameof(CheckMatch), GetRevealDelay()); // allow player to see
        }
    }

    private void CheckMatch()
    {
        if (firstSelected.symbol == secondSelected.symbol)
        {
            firstSelected.matched = true;
            secondSelected.matched = true;
            matchedPairs++;
            // disable buttons
            firstSelected.btn.interactable = false;
            secondSelected.btn.interactable = false;
        }
        else
        {
            // hide
            if (firstSelected.label != null) firstSelected.label.text = "";
            if (secondSelected.label != null) secondSelected.label.text = "";
        }
        firstSelected = null;
        secondSelected = null;

        if (matchedPairs >= pairsToMatch)
        {
            OnWin();
        }
    }

    /// <summary>
    /// Returns the reveal delay used before checking for a match; scripture effects can reduce this delay.
    /// Public for testing and tuning.
    /// </summary>
    public float GetRevealDelay()
    {
        float baseDelay = 0.6f;
        if (playerProfile == null) return baseDelay;
        var eq = playerProfile.GetEquippedScripture();
        if (string.IsNullOrEmpty(eq)) return baseDelay;
        var eff = ScriptureManager.UseScripture(eq);
        if (eff == ScriptureManager.ScriptureEffect.WeaponBoost) return 0.35f;
        if (eff == ScriptureManager.ScriptureEffect.PrayerBuff) return 0.5f;
        return baseDelay;
    }

    private void OnWin()
    {
        // award quest reward via MiniGameBase's helper
        CompleteMiniGame(questId);
        // optional: show celebratory UI, animations, sound
    }
}