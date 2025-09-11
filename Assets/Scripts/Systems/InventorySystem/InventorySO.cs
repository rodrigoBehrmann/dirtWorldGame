using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory System/Inventory Data")]
public class InventorySO : ScriptableObject
{
    public List<InventoryItem> InventoryItems = new List<InventoryItem>();
}
