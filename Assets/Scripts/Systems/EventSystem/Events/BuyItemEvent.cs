public class BuyItemEvent : Event
{
    public StoreItemSO StoreItem;

    public BuyItemEvent(StoreItemSO storeItem)
    {
        StoreItem = storeItem;
    }
}
