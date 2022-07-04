using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    [Tooltip("Id number of the item.")] public int itemId;
    [Tooltip("Name of the item.")] public string itemName;
    [Tooltip("Specify what type of the item.")] public ItemType typeOfItem;
    public enum ItemType
    {
        consumable,
        material,
        keyItem
    };
    [Tooltip("Describe the item's effect, usability.")][TextArea(3,10)] public string itemDescription;
    [Tooltip("Historical description of the item.")][TextArea(3, 10)] public string itemLore;
}