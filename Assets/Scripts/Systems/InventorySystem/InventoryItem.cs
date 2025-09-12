using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public string ItemName;
    public Sprite ItemIcon;
    public ItemType ItemType;
    public ItemCategory ItemCategory;
    public int Amount;
}
