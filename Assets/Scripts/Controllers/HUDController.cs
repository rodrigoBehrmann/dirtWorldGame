using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Player Data")]
    [SerializeField] private PlayerDataSO _playerData;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _moneyText;

    [Header("Pause Menu")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private InputManager _inputManager;

    private AudioManager _audioManager;
    void Start()
    {
        _audioManager = AudioManager.Instance;

        _inputManager = InputManager.Instance;

        _inputManager.InputControl.UI.Pause.started += ctx =>
        {
            PauseGame();
        };

        if (_playerData.CurrentMoney <= 0)
        {
            _moneyText.text = _playerData.InitialMoney.ToString();

            _playerData.CurrentMoney = _playerData.InitialMoney;
        }
        else
        {
            _moneyText.text = _playerData.CurrentMoney.ToString();
        }

        _pauseMenu.SetActive(false);

        _yesButton.onClick.AddListener(() =>
        {
            _audioManager.PlayButtonClickSound();
            Application.Quit();
        });

        _noButton.onClick.AddListener(() =>
        {
            _audioManager.PlayButtonClickSound();
            PauseGame();
        });
    }

    private void OnEnable()
    {
        EventBus.Subscribe<BuyItemEvent>(OnBuyItemEvent);

        EventBus.Subscribe<SellTrashEvent>(OnSellTrashEvent);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<BuyItemEvent>(OnBuyItemEvent);

        EventBus.Unsubscribe<SellTrashEvent>(OnSellTrashEvent);
    }

    private void OnSellTrashEvent(SellTrashEvent evt)
    {
        UpdateMoneyUI();
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
    
    private void PauseGame()
    {
        if (!_pauseMenu.activeSelf)
        {
            _pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            _pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
