using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private StoreItemSO _itemData;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemPriceText;
    [SerializeField] private Button _buyButton;
    
    void Start()
    {
        _itemPriceText.text = _itemData.Price.ToString();

        _itemIcon.sprite = _itemData.ItemIcon;

        _buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    private void OnBuyButtonClicked()
    {
        EventBus.Invoke(new BuyItemEvent(_itemData));

        InventoryItem inventoryItem = new InventoryItem
        {
            ItemName = _itemData.ItemName,
            ItemIcon = _itemData.ItemIcon,
            ItemType = _itemData.ItemType,
            ItemCategory = _itemData.ItemCategory,
            Amount = _itemData.Amout,            
        };

        EventBus.Invoke(new AddItemToInventoryEvent(inventoryItem, false));
    }

    public void SetItemData(StoreItemSO itemData)
    {
        _itemData = itemData;
        _itemPriceText.text = _itemData.Price.ToString();
        _itemIcon.sprite = _itemData.ItemIcon;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerMoneyChangedEvent>(OnPlayerMoneyChanged);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerMoneyChangedEvent>(OnPlayerMoneyChanged);
    }

    private void OnPlayerMoneyChanged(PlayerMoneyChangedEvent evt)
    {
        _buyButton.interactable = evt.CurrentMoney >= _itemData.Price;
    }
}
