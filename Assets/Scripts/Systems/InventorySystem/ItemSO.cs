using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory System/Inventory Item")]
public class ItemSO : ScriptableObject
{
    public string ItemName;
    public Sprite ItemIcon;
    public ItemType ItemType;
    public ItemCategory ItemCategory;
    public int Amount = 1;
}
