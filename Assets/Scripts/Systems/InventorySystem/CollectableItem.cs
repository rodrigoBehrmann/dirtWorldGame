using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [Header("Collectable Settings")]
    public ItemType ItemType;
    public int Amount = 1;
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

        _inventoryItem.ItemType = ItemType;
        _inventoryItem.Amount = Amount;

        _inputManager = InputManager.Instance;

        _inputManager.InputControl.Actions.Interact.started += ctx =>
        {
            CollectItem();
        };
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