public class AddItemToInventoryEvent : Event
{
    public InventoryItem InventoryItem = new InventoryItem();

    public AddItemToInventoryEvent(InventoryItem inventoryItem)
    {
        InventoryItem.ItemName = inventoryItem.ItemName;
        InventoryItem.ItemIcon = inventoryItem.ItemIcon;
        InventoryItem.ItemType = inventoryItem.ItemType;
        InventoryItem.Amount = inventoryItem.Amount;
    }
}
