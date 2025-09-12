using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image ItemIcon;
    public TextMeshProUGUI ItemAmountText;
    [HideInInspector] public ItemType ItemType;
    [HideInInspector] public ItemCategory ItemCategory;
    [HideInInspector] public string ItemName;
    [HideInInspector] public int ItemAmount = 0;

    [HideInInspector] public Transform ParentAfterDrag;
    [HideInInspector] public Transform RootCanvas;

    private Vector3 _mousePosition;
    private InputManager _inputManager;
    private Button _itemButton;
    private InventoryItem _inventoryItem;

    private AudioManager _audioManager;

    void Awake()
    {
        _itemButton = GetComponent<Button>();
    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _inputManager = InputManager.Instance;
        _inputManager.InputControl.UI.Point.performed += ctx =>
        {
            _mousePosition = ctx.ReadValue<Vector2>();
        };

        _itemButton.onClick.AddListener(OnItemButtonClicked);
    }

    public void SetItem(InventoryItem item, bool addItem, int amount = 1)
    {
        _inventoryItem = item;

        ItemName = item.ItemName;

        ItemIcon.sprite = item.ItemIcon;

        ItemCategory = item.ItemCategory;

        ItemType = item.ItemType;

        if (addItem)
        {
            ItemAmount += amount;
        }else
        {
            ItemAmount -= amount;
        }

        item.Amount = ItemAmount;

        ItemAmountText.text = ItemAmount.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemIcon.raycastTarget = false;

        ParentAfterDrag = transform.parent;

        transform.SetAsLastSibling();

        transform.SetParent(RootCanvas);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemIcon.raycastTarget = true;

        transform.SetParent(ParentAfterDrag);

        transform.localPosition = Vector3.zero;
    }

    private void OnItemButtonClicked()
    {
        _audioManager.PlayButtonClickSound();
        _inventoryItem.Amount = ItemAmount;        
        EventBus.Invoke(new InventoryItemSelectedEvent(_inventoryItem));
    }

}
