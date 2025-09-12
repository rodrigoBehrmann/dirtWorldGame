using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance { get; private set; }

    public InventorySO Inventory;
    public PlayerDataSO PlayerData;

    private const string _inventoryKey = "Inventory";
    private const string _playerDataKey = "PlayerData";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            PlayerPrefs.DeleteAll();
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LoadData(_inventoryKey, Inventory);
        LoadData(_playerDataKey, PlayerData);
    }

    

    public void LoadData(string key, ScriptableObject data)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string json = PlayerPrefs.GetString(key);
            JsonUtility.FromJsonOverwrite(json, data);
            Debug.Log("Loaded data from JSON: " + json);
        }
    }
    

    public void SaveData(ScriptableObject data, string key)
    {
        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(key, json);

        PlayerPrefs.Save();

        Debug.Log("ScriptableObject saved as JSON: " + json);
    }

    void OnDestroy()
    {
        SaveData(Inventory, _inventoryKey);
        SaveData(PlayerData, _playerDataKey);
    }
}
