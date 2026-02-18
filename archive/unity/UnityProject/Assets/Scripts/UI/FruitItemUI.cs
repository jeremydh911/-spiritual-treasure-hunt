using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Small UI controller for a single fruit item in the collection list.
/// Attach this to a prefab containing: Image (color), Text (name), Equip Button, Unequip Button.
/// </summary>
public class FruitItemUI : MonoBehaviour
{
    public Image colorImage;
    public Text nameText;
    public Button equipButton;
    public Button unequipButton;

    private string fruitName;
    private FruitCollectionUI parentUI;

    public void Setup(string fruitName, Color color, bool equipped, FruitCollectionUI parent)
    {
        this.fruitName = fruitName;
        parentUI = parent;
        nameText.text = fruitName;
        colorImage.color = color;
        equipButton.gameObject.SetActive(!equipped);
        unequipButton.gameObject.SetActive(equipped);

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() => parentUI.OnEquipFruit(fruitName));

        unequipButton.onClick.RemoveAllListeners();
        unequipButton.onClick.AddListener(() => parentUI.OnUnequipFruit(fruitName));
    }
}