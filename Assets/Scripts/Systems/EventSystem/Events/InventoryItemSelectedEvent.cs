public class InventoryItemSelectedEvent : Event
{
    public InventoryItem InventoryItem = new InventoryItem();

    public InventoryItemSelectedEvent(InventoryItem inventoryItem)
    {
        InventoryItem.ItemName = inventoryItem.ItemName;
        InventoryItem.ItemIcon = inventoryItem.ItemIcon;
        InventoryItem.ItemType = inventoryItem.ItemType;
        InventoryItem.ItemCategory = inventoryItem.ItemCategory;
        InventoryItem.Amount = inventoryItem.Amount;
    }
}
