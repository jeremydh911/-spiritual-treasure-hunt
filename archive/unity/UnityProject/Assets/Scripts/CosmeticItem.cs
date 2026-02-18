using UnityEngine;

[CreateAssetMenu(fileName = "NewCosmetic", menuName = "SpiritualTreasure/Cosmetic")]
public class CosmeticItem : ScriptableObject
{
    public string id;
    public string displayName;
    public string artistId;
    public string[] assetRefs; // GUIDs or paths
    public string[] tags;
    public bool requiresAdultMode = false;
    public bool approved = false;
    public string[] allowedDenoms; // empty => allowed for all
    public string status; // draft | pending | approved | rejected
    public string theologicalNote;

    // Fruits-of-the-Spirit energy layer flags (collectible, colored pieces)
    public bool isFruitsLayer = false;
    public string fruitName; // e.g., "Love", "Joy"
    public string fruitColorHex; // UI color for fruit visual, e.g. "#E53935"

    // Spiritual Armor (Ephesians) pieces — always available to players by design
    public bool isSpiritualArmorPiece = false;
    public string armorPieceName; // e.g., "BeltOfTruth", "ShieldOfFaith"
} 