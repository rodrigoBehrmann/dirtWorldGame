public class RemoveItemFromInventoryEvent : Event
{
    public InventoryItem InventoryItem;
    public int Amount;

    public RemoveItemFromInventoryEvent(InventoryItem inventoryItem, int amount = 1)
    {
        InventoryItem = inventoryItem;
        Amount = amount;
    }
}
