public class AddItemToInventoryEvent : Event
{
    public InventoryItem InventoryItem = new InventoryItem();

    public AddItemToInventoryEvent(InventoryItem inventoryItem)
    {
        InventoryItem.ItemType = inventoryItem.ItemType;
        InventoryItem.Amount = inventoryItem.Amount;
    }
}
