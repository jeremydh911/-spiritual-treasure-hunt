using UnityEngine;

[CreateAssetMenu(fileName = "NewActivity", menuName = "SpiritualTreasure/Activity")]
public class ActivityItem : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea(2,4)] public string description;
    public string[] scriptureRefs;
    public bool requiresMatureMode = false;
    public string theologicalNote;
}