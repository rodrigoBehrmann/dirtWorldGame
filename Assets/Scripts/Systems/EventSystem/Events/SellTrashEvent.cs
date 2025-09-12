public class SellTrashEvent : Event
{
    public float CurrentMoney;

    public SellTrashEvent(float currentMoney)
    {
        CurrentMoney = currentMoney;
    }
}
