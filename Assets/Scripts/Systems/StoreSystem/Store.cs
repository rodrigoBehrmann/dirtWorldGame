using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    [Header("Store Settings")]
    [SerializeField] private StoreType _storeType;
    [SerializeField] private GameObject _storePanel;
    [SerializeField] private GameObject _storeContainer;
    [SerializeField] private TextMeshProUGUI _currentMoneyText;

    [Header("Store Items")]
    [SerializeField] private StoreItemSO[] _storeItems;
    [SerializeField] private GameObject _storeItemPrefab;

    [Header("UI Elements")]
    [SerializeField] private Button _closeButton;

    void Start()
    {
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

            storeItem.GetComponent<StoreItem>().SetItemData(item);
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

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
    }

    public void CloseStore()
    {
        _storePanel.SetActive(false);

        Time.timeScale = 1f; 

        Cursor.lockState = CursorLockMode.Locked;
        
        Cursor.visible = false;
    }
}
