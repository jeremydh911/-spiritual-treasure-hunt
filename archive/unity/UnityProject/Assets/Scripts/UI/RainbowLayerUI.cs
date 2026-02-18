using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Small UI for displaying the Rainbow layer status and quick equip/preview.
/// </summary>
public class RainbowLayerUI : MonoBehaviour
{
    public PlayerProfile playerProfile;
    public Image previewImage; // optional composite preview
    public Button toggleButton;
    public Text statusText;

    private void OnEnable()
    {
        Refresh();
        if (toggleButton != null) toggleButton.onClick.AddListener(OnToggle);
    }

    private void OnDisable()
    {
        if (toggleButton != null) toggleButton.onClick.RemoveListener(OnToggle);
    }

    public void Refresh()
    {
        if (playerProfile == null) return;
        var unlocked = playerProfile.IsRainbowUnlocked();
        statusText.text = unlocked ? "Rainbow ready" : "Rainbow locked";
        toggleButton.interactable = unlocked;
        toggleButton.GetComponentInChildren<Text>().text = playerProfile.isRainbowEquipped ? "Unequip Rainbow" : "Equip Rainbow";
    }

    private void OnToggle()
    {
        if (playerProfile == null) return;
        if (playerProfile.isRainbowEquipped) playerProfile.UnequipRainbow(); else playerProfile.EquipRainbow();
        Refresh();
    }
}