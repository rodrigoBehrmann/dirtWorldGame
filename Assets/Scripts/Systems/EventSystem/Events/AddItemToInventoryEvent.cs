public class AddItemToInventoryEvent : Event
{
    public InventoryItem InventoryItem = new InventoryItem();
    public bool ActiveAnimation = true;

    public AddItemToInventoryEvent(InventoryItem inventoryItem, bool activeAnimation = true)
    {
        InventoryItem.ItemName = inventoryItem.ItemName;
        InventoryItem.ItemIcon = inventoryItem.ItemIcon;
        InventoryItem.ItemType = inventoryItem.ItemType;
        InventoryItem.ItemCategory = inventoryItem.ItemCategory;
        InventoryItem.Amount = inventoryItem.Amount;
        ActiveAnimation = activeAnimation;
    }
}
