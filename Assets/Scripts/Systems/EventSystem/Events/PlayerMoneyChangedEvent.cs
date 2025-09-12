public class PlayerMoneyChangedEvent : Event
{
    public float CurrentMoney;

    public PlayerMoneyChangedEvent(float currentMoney)
    {
        CurrentMoney = currentMoney;
    }
}
