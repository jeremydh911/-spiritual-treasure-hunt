using UnityEngine;

[CreateAssetMenu(fileName = "NewAngel", menuName = "SpiritualTreasure/Angel")]
public class AngelItem : ScriptableObject
{
    public string id;
    public string displayName;
    public string role; // e.g., Messenger, Protector, Guide, Reviver
    public string[] scriptureRefs;
    [TextArea(2,6)] public string description;
    public string effectType; // e.g., "revive","guide","protect","message"
}