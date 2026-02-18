using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple collection UI that lists owned fruits and allows equipping/unequipping.
/// This is a runtime-only example to be wired to Unity UI prefabs in the editor.
/// </summary>
public class FruitCollectionUI : MonoBehaviour
{
    [Header("References")]
    public PlayerProfile playerProfile;
    public Transform listParent; // container for fruit item instances
    public GameObject fruitItemPrefab; // prefab with FruitItemUI component
    public Button equipRainbowButton;
    public Text rainbowStatusText;

    // color map (should match Design/SpiritualSystems.md)
    private Dictionary<string, Color> colorMap = new Dictionary<string, Color>
    {
        {"Love", new Color(0.894f,0.227f,0.207f)},
        {"Joy", new Color(1f,0.835f,0.31f)},
        {"Peace", new Color(0.31f,0.764f,0.969f)},
        {"Patience", new Color(1f,0.718f,0.302f)},
        {"Kindness", new Color(0.957f,0.561f,0.694f)},
        {"Goodness", new Color(0.4f,0.733f,0.4f)},
        {"Faithfulness", new Color(0.584f,0.463f,0.8f)},
        {"Gentleness", new Color(0.502f,0.8f,0.773f)},
        {"Self-control", new Color(0.361f,0.42f,0.753f)}
    };

    private void OnEnable()
    {
        RefreshUI();
        if (equipRainbowButton != null) equipRainbowButton.onClick.AddListener(OnEquipRainbowPressed);
    }

    private void OnDisable()
    {
        if (equipRainbowButton != null) equipRainbowButton.onClick.RemoveListener(OnEquipRainbowPressed);
    }

    public void RefreshUI()
    {
        if (playerProfile == null) return;

        // clear previous
        foreach (Transform t in listParent) Destroy(t.gameObject);

        var owned = playerProfile.ownedFruits ?? new string[0];
        foreach (var f in owned)
        {
            var go = Instantiate(fruitItemPrefab, listParent, false);
            var ui = go.GetComponent<FruitItemUI>();
            bool equipped = System.Array.Exists(playerProfile.GetEquippedFruits(), x => x == f) || playerProfile.isRainbowEquipped;
            Color col = colorMap.ContainsKey(f) ? colorMap[f] : Color.white;
            ui.Setup(f, col, equipped, this);
        }

        UpdateRainbowUI();
    }

    public void OnEquipFruit(string fruitName)
    {
        if (playerProfile == null) return;
        var ok = playerProfile.EquipFruit(fruitName);
        if (ok) RefreshUI();
    }

    public void OnUnequipFruit(string fruitName)
    {
        if (playerProfile == null) return;
        playerProfile.UnequipFruit(fruitName);
        RefreshUI();
    }

    private void UpdateRainbowUI()
    {
        if (playerProfile == null) return;
        bool unlocked = playerProfile.IsRainbowUnlocked();
        if (rainbowStatusText != null) rainbowStatusText.text = unlocked ? "Rainbow unlocked!" : "Collect all fruits to unlock the Rainbow";
        if (equipRainbowButton != null)
        {
            equipRainbowButton.interactable = unlocked;
            equipRainbowButton.GetComponentInChildren<Text>().text = playerProfile.isRainbowEquipped ? "Unequip Rainbow" : "Equip Rainbow";
        }
    }

    private void OnEquipRainbowPressed()
    {
        if (playerProfile == null) return;
        if (playerProfile.isRainbowEquipped) playerProfile.UnequipRainbow(); else playerProfile.EquipRainbow();
        RefreshUI();
    }
}