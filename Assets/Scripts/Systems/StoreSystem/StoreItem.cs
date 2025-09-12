using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private StoreItemSO _itemData;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemPriceText;
    [SerializeField] private Button _buyButton;

    private bool _playerHasItem = false;
    private InventorySO _inventoryData;
    private PlayerDataSO _playerData;

    void Start()
    {
        _itemPriceText.text = _itemData.Price.ToString();

        _itemIcon.sprite = _itemData.ItemIcon;

        _buyButton.onClick.AddListener(OnBuyButtonClicked);

        HandleBuyButtonState();      
    }
    
    private void HandleBuyButtonState()
    {
        foreach (var item in _inventoryData.InventoryItems)
        {
            if (item.ItemName == _itemData.ItemName)
            {
                _buyButton.interactable = false;
                _playerHasItem = true;
                return;
            }
        }
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

        _buyButton.interactable = false;

        HandleBuyButtonState();
    }

    public void SetItemData(StoreItemSO itemData, InventorySO inventoryData, PlayerDataSO playerData)
    {
        _inventoryData = inventoryData;
        _itemData = itemData;
        _itemPriceText.text = _itemData.Price.ToString();
        _itemIcon.sprite = _itemData.ItemIcon;
        _playerData = playerData;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerMoneyChangedEvent>(OnPlayerMoneyChanged);
        
        HandleBuyButtonState();

        if (!_playerHasItem)
        {
            _buyButton.interactable = _playerData.CurrentMoney >= _itemData.Price;
        }
        else
        {
            _buyButton.interactable = false;
        }
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerMoneyChangedEvent>(OnPlayerMoneyChanged);
    }

    private void OnPlayerMoneyChanged(PlayerMoneyChangedEvent evt)
    {
        if (_playerHasItem)
        {
            _buyButton.interactable = false;
        }
        else
        {
            _buyButton.interactable = evt.CurrentMoney >= _itemData.Price;            
        }
    }
}
