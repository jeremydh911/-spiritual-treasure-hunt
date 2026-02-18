using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the player's Spiritual Armor pieces and short scripture references.
/// This UI is informational and confirms the armor is present by default for all players.
/// </summary>
public class ArmorPanelUI : MonoBehaviour
{
    public PlayerProfile playerProfile;
    public Transform armorListParent;
    public GameObject armorItemPrefab; // prefab with Text fields

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (playerProfile == null) return;
        foreach (Transform t in armorListParent) Destroy(t.gameObject);

        var armor = playerProfile.ownedArmorPieces;
        if (armor == null) return;
        foreach (var a in armor)
        {
            var go = Instantiate(armorItemPrefab, armorListParent, false);
            var texts = go.GetComponentsInChildren<Text>();
            if (texts.Length > 0) texts[0].text = a;
            if (texts.Length > 1) texts[1].text = GetArmorScripture(a);
        }
    }

    private string GetArmorScripture(string armorKey)
    {
        switch (armorKey)
        {
            case "BeltOfTruth": return "Eph 6:14";
            case "BreastplateOfRighteousness": return "Eph 6:14";
            case "GospelOfPeaceShoes": return "Eph 6:15";
            case "ShieldOfFaith": return "Eph 6:16";
            case "HelmetOfSalvation": return "Eph 6:17";
            case "SwordOfTheSpirit": return "Eph 6:17";
            default: return "Ephesians 6";
        }
    }
}