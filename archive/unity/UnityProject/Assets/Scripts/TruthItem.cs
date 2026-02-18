using UnityEngine;

[CreateAssetMenu(fileName = "NewTruth", menuName = "SpiritualTreasure/Truth")]
public class TruthItem : ScriptableObject
{
    public string id;
    public string title;
    [TextArea(2,4)] public string description;
    public string[] scriptureRefs;
    public bool requiresMatureMode = false;
    [TextArea(2,6)] public string theologicalNote;
}