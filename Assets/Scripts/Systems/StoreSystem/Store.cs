using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    [Header("Store Settings")]
    [SerializeField] private StoreType _storeType;
    [SerializeField] private GameObject _storePanel;
    [SerializeField] private GameObject _storePanelBorder;
    [SerializeField] private GameObject _storeContainer;
    [SerializeField] private TextMeshProUGUI _currentMoneyText;

    [Header("Store Items")]
    [SerializeField] private StoreItemSO[] _storeItems;
    [SerializeField] private GameObject _storeItemPrefab;

    [Header("UI Elements")]
    [SerializeField] private Button _closeButton;

    [Header("SO")]
    [SerializeField] private InventorySO _inventoryData;
    [SerializeField] private PlayerDataSO _playerData;

    private AudioManager _audioManager;

    void Start()
    {
        _audioManager = AudioManager.Instance;

        _closeButton.onClick.AddListener(CloseStore);

        CreateStoreItems();
    }

    private void CreateStoreItems()
    {
        foreach (Transform child in _storeContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in _storeItems)
        {
            var storeItem = Instantiate(_storeItemPrefab, _storeContainer.transform);

            storeItem.GetComponent<StoreItem>().SetItemData(item, _inventoryData, _playerData);
        }
    }
    
    private void OnEnable()
    {
        EventBus.Subscribe<PlayerMoneyChangedEvent>(OnPlayerMoneyChangedEvent);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerMoneyChangedEvent>(OnPlayerMoneyChangedEvent);
    }

    private void OnPlayerMoneyChangedEvent(PlayerMoneyChangedEvent evt)
    {
        _currentMoneyText.text = evt.CurrentMoney.ToString();
    }

    public void OpenStore()
    {
        _storePanel.SetActive(true);

        _storePanelBorder.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
    }

    public void CloseStore()
    {
        _audioManager.PlayButtonClickSound();

        _storePanel.SetActive(false);

        _storePanelBorder.SetActive(false);

        Time.timeScale = 1f; 

        Cursor.lockState = CursorLockMode.Locked;
        
        Cursor.visible = false;
    }
}
