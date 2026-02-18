using UnityEngine;

[CreateAssetMenu(fileName = "NewVirtue", menuName = "SpiritualTreasure/Virtue")]
public class VirtueItem : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea(2,4)] public string description;
    public string[] scriptureRefs;
    public string[] livesWith; // related virtues
    public bool requiresMatureMode = false; // deeper theological notes gated
    [TextArea(2,6)] public string theologicalNote;
}