using System;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [Header("Collectable Settings")]
    public ItemSO Item;
    private InventoryItem _inventoryItem = new InventoryItem();

    [Header("UI Settings")]
    [SerializeField] private GameObject InteractableCanvas;
    private Camera _mainCamera;

    private bool _canCollect = false;

    private InputManager _inputManager;

    void Start()
    {
        _mainCamera = Camera.main;

        InteractableCanvas.SetActive(false);

        _inputManager = InputManager.Instance;

        _inputManager.InputControl.Actions.Interact.started += ctx =>
        {
            CollectItem();
        };

        SetItem(Item);
    }

    private void SetItem(ItemSO item)
    {
        _inventoryItem.ItemName = item.ItemName;
        
        _inventoryItem.ItemIcon = item.ItemIcon;

        _inventoryItem.ItemType = item.ItemType;

        _inventoryItem.ItemCategory = item.ItemCategory;

        _inventoryItem.Amount = item.Amount;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canCollect = true;
            InteractableCanvas.SetActive(true);
            //gameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (!InteractableCanvas.activeSelf) return;

        InteractableCanvas.transform.LookAt(InteractableCanvas.transform.position + _mainCamera.transform.forward);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canCollect = false;
            InteractableCanvas.SetActive(false);
        }
    }

    private void CollectItem()
    {
        if (_canCollect)
        {
            EventBus.Invoke(new AddItemToInventoryEvent(_inventoryItem));
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        _canCollect = false;
        _inputManager.InputControl.Actions.Interact.started -= ctx =>
        {
            CollectItem();
        };
    }
}