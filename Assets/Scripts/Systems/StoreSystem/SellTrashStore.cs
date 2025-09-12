using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellTrashStore : MonoBehaviour
{
    [Header("Store Settings")]
    [SerializeField] private StoreType _storeType;
    [SerializeField] private GameObject _storePanel;
    [SerializeField] private TextMeshProUGUI _currentMoneyText;

    [Header("UI Elements")]
    [SerializeField] private Button _closeButton;

    [Header("Inventory SO")]
    [SerializeField] private InventorySO _inventoryData;
    [SerializeField] private PlayerDataSO _playerData;


    [Header("Trash Store Settings")]
    [SerializeField] private int _trashSellPrice = 5;
    [SerializeField] private TextMeshProUGUI _trashSellCountText;
    [SerializeField] private TextMeshProUGUI _trashPrice;
    [SerializeField] private TextMeshProUGUI _moneyToReceiveText;
    [SerializeField] private Button _sellTrashButton;
    [SerializeField] private Button _addTrashCountButton;
    [SerializeField] private Button _subTrashCountButton;
    private InventoryItem _trashItem;
    private int _currentTrashCount = 0;
    private int _maxTrashCount = 0;

    void Start()
    {
        _closeButton.onClick.AddListener(CloseStore);

        _addTrashCountButton.interactable = false;

        _subTrashCountButton.interactable = false;

        SetMaxTrashCount(0);

        _trashPrice.text = _trashSellPrice.ToString();

        _sellTrashButton.onClick.AddListener(OnSellTrashButtonClicked);

        _addTrashCountButton.onClick.AddListener(OnAddTrashCountButtonClicked);
        
        _subTrashCountButton.onClick.AddListener(OnSubTrashCountButtonClicked);
    }

    private void SetMaxTrashCount(int currentItemCount = 0)
    {
        if (_storeType == StoreType.TrashStore)
        {
            if (_inventoryData.InventoryItems.Count > 0)
            {
                foreach (var item in _inventoryData.InventoryItems)
                {
                    if (item.ItemCategory == ItemCategory.Trash)
                    {
                        _maxTrashCount = item.Amount;

                        Debug.Log($"Max trash count updated: {item.Amount}");

                        _trashItem = item;

                        break;
                    }
                    else
                    {
                        _currentTrashCount = 0;
                        _maxTrashCount = 0;
                        _addTrashCountButton.interactable = false;
                        _subTrashCountButton.interactable = false;
                    }
                }
            }
            else
            {
                Debug.Log("Inventory is empty.");
                _currentTrashCount = 0;
                _maxTrashCount = 0;
                _addTrashCountButton.interactable = false;
                _subTrashCountButton.interactable = false;
            }
        }

        _currentTrashCount = 0;

        _trashSellCountText.text = _currentTrashCount.ToString();

        if (_currentTrashCount <= 0)
        {
            _sellTrashButton.interactable = false;

            _moneyToReceiveText.text = "0";
        }
        else
        {
            _sellTrashButton.interactable = true;

            _moneyToReceiveText.text = (_currentTrashCount * _trashSellPrice).ToString();
        }

        if (_maxTrashCount <= 0)
        {
            _addTrashCountButton.interactable = false;
            _subTrashCountButton.interactable = false;
        }
        else
        {
            _addTrashCountButton.interactable = true;
        }
    }


    private void OnAddTrashCountButtonClicked()
    {
        if (_currentTrashCount < _maxTrashCount)
        {            
            _currentTrashCount++;

            _subTrashCountButton.interactable = true;

            _trashSellCountText.text = _currentTrashCount.ToString();

            _moneyToReceiveText.text = (_currentTrashCount * _trashSellPrice).ToString();
        }
        
        if(_currentTrashCount == _maxTrashCount)
        {
            _addTrashCountButton.interactable = false;
        }
        
        if (_currentTrashCount <= 0)
        {
            _sellTrashButton.interactable = false;
        }
        else
        {
            _sellTrashButton.interactable = true;
        }   
    }

    private void OnSubTrashCountButtonClicked()
    {
        if (_currentTrashCount > 0)
        {            
            _currentTrashCount--;

            _addTrashCountButton.interactable = true;

            Debug.Log($"Current trash count: {_currentTrashCount}");
            _trashSellCountText.text = _currentTrashCount.ToString();
            _moneyToReceiveText.text = (_currentTrashCount * _trashSellPrice).ToString();
        }else
        {
            _subTrashCountButton.interactable = false;
        }

        if (_currentTrashCount <= 0)
        {
            _sellTrashButton.interactable = false;
        }
        else
        {
            _sellTrashButton.interactable = true;
        }   
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerMoneyChangedEvent>(OnPlayerMoneyChangedEvent);

        EventBus.Subscribe<InventoryHasChangedEvent>(OnInventoryHasChangedEvent);
    }

    private void OnDisable()
    {        
        EventBus.Unsubscribe<PlayerMoneyChangedEvent>(OnPlayerMoneyChangedEvent);

        EventBus.Unsubscribe<InventoryHasChangedEvent>(OnInventoryHasChangedEvent);
    }
    
    private void OnInventoryHasChangedEvent(InventoryHasChangedEvent evt)
    {
        SetMaxTrashCount(evt.CurrentItemCount);
    }

    private void OnSellTrashButtonClicked()
    {
        if (_currentTrashCount <= 0) return;

        int totalMoneyEarned = _currentTrashCount * _trashSellPrice;

        EventBus.Invoke(new RemoveItemFromInventoryEvent(_trashItem,_currentTrashCount));

        if (_currentTrashCount == _maxTrashCount)
        {
            _maxTrashCount = 0;
            _currentTrashCount = 0;
        }

        _playerData.CurrentMoney += totalMoneyEarned;

        EventBus.Invoke(new PlayerMoneyChangedEvent(_playerData.CurrentMoney));

        EventBus.Invoke(new SellTrashEvent(_playerData.CurrentMoney));

    }

    private void OnPlayerMoneyChangedEvent(PlayerMoneyChangedEvent evt)
    {
        _currentMoneyText.text = evt.CurrentMoney.ToString();
    }

    public void OpenStore()
    {
        _storePanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;

        SetMaxTrashCount();
    }

    public void CloseStore()
    {
        _storePanel.SetActive(false);

        Time.timeScale = 1f; 

        Cursor.lockState = CursorLockMode.Locked;
        
        Cursor.visible = false;
    }
}
