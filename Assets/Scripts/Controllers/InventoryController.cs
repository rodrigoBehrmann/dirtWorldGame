using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private InventorySO _inventoryData;

    [Header("Inventory Panel Settings")]
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _itemSlotPrefab;
    [SerializeField] private GameObject _inventoryItemPrefab;
    [SerializeField] private GameObject _inventorySlotsContainer;
    [SerializeField] private GameObject _rootCanvas;
    [SerializeField] private Button _closeInventoryButton;

    [Header("Item Selected Settings")]
    [SerializeField] private GameObject _itemSelectedPanel;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemAmountText;
    [SerializeField] private Image _itemIconImage;
    [SerializeField] private Button _removeItemButton;
    private InventoryItem _selectedItem;

    private InputManager _inputManager;
    private AudioManager _audioManager;

    void Start()
    {
        _audioManager = AudioManager.Instance;
        _inputManager = InputManager.Instance;

        _inputManager.InputControl.Inventory.OpenInventory.started += ctx =>
        {
            OnInventoryOpened();
        };

        CreateInventorySlots();

        _closeInventoryButton.onClick.AddListener(OnInventoryOpened);

        _removeItemButton.onClick.AddListener(OnRemoveItemButtonClicked);
    }    
    
    private void CreateInventorySlots()
    {
        foreach (Transform child in _inventorySlotsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _inventoryData.MaxInventoryItems; i++)
        {
            GameObject slot = Instantiate(_itemSlotPrefab, _inventorySlotsContainer.transform);
            if (i < _inventoryData.InventoryItems.Count)
            {
                GameObject inventoryItem = Instantiate(_inventoryItemPrefab, slot.transform);
                inventoryItem.GetComponent<InventoryItemSlot>().SetItem(_inventoryData.InventoryItems[i], true, _inventoryData.InventoryItems[i].Amount);
                
                inventoryItem.GetComponent<InventoryItemSlot>().RootCanvas = _rootCanvas.transform;
            }
        }
    }

    private void VerifyEmptyItemsInSlots()
    {
        for (int i = 0; i < _inventorySlotsContainer.transform.childCount; i++)
        {
            Transform slot = _inventorySlotsContainer.transform.GetChild(i);
            if (slot.childCount > 0)
            {
                InventoryItemSlot itemSlot = slot.GetChild(0).GetComponent<InventoryItemSlot>();

                bool itemHaveAmount = itemSlot.ItemAmount > 0;
                if (!itemHaveAmount)
                {
                    Destroy(itemSlot.gameObject);
                }
            }
        }
    }

    private void UpdateInventoryItems(InventoryItem inventoryItem, bool addItem, int amount = 0)
    {
        if (_inventoryData.InventoryItems.Count > 0)
        {
            int index = -1;
            for (int i = 0; i < _inventorySlotsContainer.transform.childCount; i++)
            {
                Transform slot = _inventorySlotsContainer.transform.GetChild(i);
                if (inventoryItem != null)
                {
                    if (slot.childCount > 0)
                    {
                        InventoryItemSlot itemSlot = slot.GetChild(0).GetComponent<InventoryItemSlot>();

                        if (itemSlot.ItemName == inventoryItem.ItemName)
                        {
                            index = i;
                            if (!addItem)
                            {

                                if (itemSlot.ItemAmount <= 0)
                                {
                                    Destroy(itemSlot.gameObject);
                                    return;
                                }
                                else
                                {
                                    itemSlot.SetItem(inventoryItem, addItem, amount);
                                }

                                return;
                            }

                            break;
                        }
                    }
                }
                else
                {
                    if (slot.childCount == 0)
                    {
                        index = -1;
                        break;
                    }
                }
            }

            if (index != -1)
            {
                Transform slot = _inventorySlotsContainer.transform.GetChild(index);

                InventoryItemSlot itemSlot = slot.GetChild(0).GetComponent<InventoryItemSlot>();

                itemSlot.SetItem(inventoryItem, addItem);

                if(itemSlot.RootCanvas == null)
                {
                    itemSlot.RootCanvas = _rootCanvas.transform;
                }

                if (inventoryItem.Amount <= 0 && !addItem)
                {
                    Destroy(itemSlot.gameObject);
                }

            }
            else
            {
                for (int i = 0; i < _inventorySlotsContainer.transform.childCount; i++)
                {
                    Transform slot = _inventorySlotsContainer.transform.GetChild(i);
                    if (slot.childCount == 0)
                    {
                        InventoryItemSlot itemSlot = Instantiate(_inventoryItemPrefab, slot.transform).GetComponent<InventoryItemSlot>();

                        itemSlot.SetItem(inventoryItem, addItem);

                        itemSlot.RootCanvas = _rootCanvas.transform;

                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < _inventorySlotsContainer.transform.childCount; i++)
            {
                Transform slot = _inventorySlotsContainer.transform.GetChild(i);
                if (slot.childCount > 0)
                {
                    Destroy(slot.GetChild(0).gameObject);
                }
            }
        }
    }

    private void OnInventoryOpened()
    {
        _audioManager.PlayButtonClickSound();
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);

        if (_inventoryPanel.activeSelf)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            VerifyEmptyItemsInSlots();
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _itemSelectedPanel.SetActive(false);
        }
    }


    void OnEnable()
    {
        EventBus.Subscribe<AddItemToInventoryEvent>(OnAddItemToInventory);

        EventBus.Subscribe<RemoveItemFromInventoryEvent>(OnRemoveItemFromInventory);

        EventBus.Subscribe<InventoryItemSelectedEvent>(OnInventoryItemSelected);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<AddItemToInventoryEvent>(OnAddItemToInventory);

        EventBus.Unsubscribe<RemoveItemFromInventoryEvent>(OnRemoveItemFromInventory);
    }

    void OnDestroy()
    {
        _inventoryData.InventoryItems.Clear();
    }

    private void OnAddItemToInventory(AddItemToInventoryEvent evt)
    {
        AddItemToInventory(evt.InventoryItem);
    }

    private void OnRemoveItemFromInventory(RemoveItemFromInventoryEvent evt)
    {
        RemoveItemFromInventory(evt.InventoryItem, evt.Amount);
    }

    private void OnInventoryItemSelected(InventoryItemSelectedEvent evt)
    {
        _itemSelectedPanel.SetActive(true);

        _selectedItem = new InventoryItem
        {
            ItemName = evt.InventoryItem.ItemName,
            ItemIcon = evt.InventoryItem.ItemIcon,
            ItemType = evt.InventoryItem.ItemType,
            ItemCategory = evt.InventoryItem.ItemCategory,
            Amount = evt.InventoryItem.Amount
        };

        _itemNameText.text = _selectedItem.ItemName;

        _itemIconImage.sprite = _selectedItem.ItemIcon;

        _itemAmountText.text = _selectedItem.Amount.ToString();

        if (_selectedItem.ItemType == ItemType.Equipment)
        {
            _removeItemButton.interactable = false;
            _removeItemButton.gameObject.SetActive(false);
        }
        else
        {
            _removeItemButton.gameObject.SetActive(true);
            _removeItemButton.interactable = _selectedItem.Amount > 0;
        }
        
    }

    private void OnRemoveItemButtonClicked()
    {
        _audioManager.PlayButtonClickSound();
        _itemSelectedPanel.SetActive(false);
        RemoveItemFromInventory(_selectedItem,_selectedItem.Amount);
    }

    private void SetItemSelectedInfo(InventoryItem inventoryItem)
    {
        _itemSelectedPanel.SetActive(true);

        _itemNameText.text = inventoryItem.ItemName;

        _itemIconImage.sprite = inventoryItem.ItemIcon;
    }

    public void AddItemToInventory(InventoryItem inventoryItem)
    {
        foreach (var invItem in _inventoryData.InventoryItems)
        {
            if (invItem.ItemName == inventoryItem.ItemName)
            {
                invItem.Amount += inventoryItem.Amount;

                UpdateInventoryItems(inventoryItem, true);

                VerifyEmptyItemsInSlots();

                return;
            }
        }
        _inventoryData.InventoryItems.Add(inventoryItem);

        UpdateInventoryItems(inventoryItem, true);
        
        VerifyEmptyItemsInSlots();
    }

    public void RemoveItemFromInventory(InventoryItem inventoryItem, int amount = 1)
    {
        foreach (var invItem in _inventoryData.InventoryItems)
        {
            if (invItem.ItemName == inventoryItem.ItemName)
            {
                invItem.Amount -= amount;

                if (invItem.Amount <= 0)
                {
                    _inventoryData.InventoryItems.Remove(invItem);
                }

                UpdateInventoryItems(inventoryItem, false, amount);

                EventBus.Invoke(new InventoryHasChangedEvent(invItem.Amount));

                VerifyEmptyItemsInSlots();
                
                return;
            }
        }
    }
}
