using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "SpiritualTreasure/Quest")]
public class QuestSpec : ScriptableObject
{
    public string id;
    public string title;
    [TextArea(2,4)] public string shortDescription;
    public string[] scriptureRefs;
    public string learningGoal;
    public string gameplayType;
    public string ageRange;
    public string[] virtueTags;
    public string sensitivity;
    public string[] denomTags;
    public bool whimsyTag;
    public bool preFallTag;
    [TextArea(3,6)] public string childCopy;
    [TextArea(4,8)] public string matureNote;
    public string[] rewards;
    public string vetStatus;
    [TextArea(2,6)] public string acceptanceCriteria;
}