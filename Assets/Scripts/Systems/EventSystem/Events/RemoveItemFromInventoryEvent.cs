public class RemoveItemFromInventoryEvent : Event
{
    public InventoryItem InventoryItem;

    public RemoveItemFromInventoryEvent(InventoryItem inventoryItem)
    {
        InventoryItem = inventoryItem;
    }
}
