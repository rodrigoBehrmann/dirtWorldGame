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

    private InputManager _inputManager;

    void Start()
    {
        _inputManager = InputManager.Instance;

        _inputManager.InputControl.Inventory.OpenInventory.started += ctx =>
        {
            OnInventoryOpened();
        };

        CreateInventorySlots();
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
                inventoryItem.GetComponent<InventoryItemSlot>().SetItem(_inventoryData.InventoryItems[i]);
            }
        }
        
        foreach (var item in _inventoryData.InventoryItems)
        {
            GameObject slot = Instantiate(_itemSlotPrefab, _inventorySlotsContainer.transform);
        }
    }

    private void UpdateInventoryItems(InventoryItem inventoryItem)
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

                itemSlot.SetItem(inventoryItem);
            }
            else
            {
                for (int i = 0; i < _inventorySlotsContainer.transform.childCount; i++)
                {
                    Transform slot = _inventorySlotsContainer.transform.GetChild(i);
                    if (slot.childCount == 0)
                    {
                        InventoryItemSlot itemSlot = Instantiate(_inventoryItemPrefab, slot.transform).GetComponent<InventoryItemSlot>();

                        itemSlot.SetItem(inventoryItem);

                        itemSlot.RootCanvas = _rootCanvas.transform;

                        break;
                    }
                }
            }
        }
    }

    private void OnInventoryOpened()
    {
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);

        if (_inventoryPanel.activeSelf)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (_inventoryPanel.activeSelf)
        {
            //_inventoryPanelButton.Select();
        }
    }

    void OnEnable()
    {
        EventBus.Subscribe<AddItemToInventoryEvent>(OnAddItemToInventory);

        EventBus.Subscribe<RemoveItemFromInventoryEvent>(OnRemoveItemFromInventory);
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
        RemoveItemFromInventory(evt.InventoryItem);
    }

    public void AddItemToInventory(InventoryItem inventoryItem)
    {
        foreach (var invItem in _inventoryData.InventoryItems)
        {
            if (invItem.ItemName == inventoryItem.ItemName)
            {
                invItem.Amount += inventoryItem.Amount;

                UpdateInventoryItems(inventoryItem);

                return;
            }
        }
        _inventoryData.InventoryItems.Add(inventoryItem); 

        UpdateInventoryItems(inventoryItem);
    }

    public void RemoveItemFromInventory(InventoryItem inventoryItem)
    {
        foreach (var invItem in _inventoryData.InventoryItems)
        {
            if (invItem.ItemName == inventoryItem.ItemName)
            {
                invItem.Amount -= inventoryItem.Amount;

                UpdateInventoryItems(inventoryItem);

                if (invItem.Amount <= 0)
                {
                    _inventoryData.InventoryItems.Remove(invItem);
                }
                
                return;
            }
        }
    }
}
