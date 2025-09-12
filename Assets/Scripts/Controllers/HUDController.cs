using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("Player Data")]
    [SerializeField] private PlayerDataSO _playerData;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _moneyText;

    void Start()
    {
        _moneyText.text = _playerData.InitialMoney.ToString();
        
        _playerData.CurrentMoney = _playerData.InitialMoney;
    }     

    private void OnEnable()
    {
        EventBus.Subscribe<BuyItemEvent>(OnBuyItemEvent);
    }  

    private void OnDisable()
    {
        EventBus.Unsubscribe<BuyItemEvent>(OnBuyItemEvent);
    }

    private void OnBuyItemEvent(BuyItemEvent evt)
    {
        if (_playerData.CurrentMoney >= evt.StoreItem.Price)
        {
            _playerData.CurrentMoney -= evt.StoreItem.Price;

            EventBus.Invoke(new PlayerMoneyChangedEvent(_playerData.CurrentMoney));
            UpdateMoneyUI();
        }
    }

    private void UpdateMoneyUI()
    {
        _moneyText.text = _playerData.CurrentMoney.ToString();
    }
}
