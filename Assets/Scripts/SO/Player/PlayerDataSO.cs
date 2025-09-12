using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Player/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    public float InitialMoney;
    public float CurrentMoney;
    public InventorySO Inventory;
}
