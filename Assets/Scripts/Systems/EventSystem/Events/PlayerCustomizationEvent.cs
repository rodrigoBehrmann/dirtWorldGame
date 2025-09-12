public class PlayerCustomizationEvent : Event
{ 
    public string ItemName;
    public ItemCategory ItemCategory;
    public PlayerCustomizationEvent(string itemName, ItemCategory itemCategory)
    {
        ItemName = itemName;
        ItemCategory = itemCategory;
    }
}
