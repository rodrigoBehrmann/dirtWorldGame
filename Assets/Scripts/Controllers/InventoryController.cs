using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private InventorySO _inventoryData;

    [Header("Inventory Panel Settings")]
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Button _inventoryPanelButton;
    [SerializeField] private Button _equipamentPanelButton;

    private InputManager _inputManager;

    void Start()
    {
        _inputManager = InputManager.Instance;

        _inputManager.InputControl.Inventory.OpenInventory.started += ctx =>
        {
            OnInventoryOpened();
        };

    }    

    private void OnInventoryOpened()
    {
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);

        if( _inventoryPanel.activeSelf)
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
            if (invItem.ItemType == inventoryItem.ItemType)
            {
                invItem.Amount += inventoryItem.Amount;
                return;
            }
        }
        _inventoryData.InventoryItems.Add(inventoryItem); 
    }

    public void RemoveItemFromInventory(InventoryItem inventoryItem)
    {
        foreach (var invItem in _inventoryData.InventoryItems)
        {
            if (invItem.ItemType == inventoryItem.ItemType)
            {
                invItem.Amount -= inventoryItem.Amount;
                if (invItem.Amount <= 0)
                {
                    _inventoryData.InventoryItems.Remove(invItem);
                }
                return;
            }
        }
    }
}
