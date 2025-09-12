using UnityEngine;

[CreateAssetMenu(fileName = "StoreItemSO", menuName = "Store/Store Item")]
public class StoreItemSO : ScriptableObject
{
    public string ItemName;
    public float Price;
    [HideInInspector] public int Amout = 1;
    public Sprite ItemIcon;
    public ItemType ItemType;
    public ItemCategory ItemCategory;
}
