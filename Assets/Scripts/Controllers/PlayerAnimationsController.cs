using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{
    private Animator _animator;



    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<AddItemToInventoryEvent>(OnAddItemToInventory);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<AddItemToInventoryEvent>(OnAddItemToInventory);
    }

    private void OnAddItemToInventory(AddItemToInventoryEvent eventData)
    {
        if (eventData.ActiveAnimation)
        {
            SetTriggerAnimator("Looting");            
        }
    }

    public void SetTriggerAnimator(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }

    public void SetIntAnimator(int id, int value)
    {
        _animator.SetInteger(id, value);
    }

    public void SetBoolAnimator(int id, bool value)
    {
        _animator.SetBool(id, value);
    }

    public void SetFloatAnimator(int id, float value)
    {
        _animator.SetFloat(id, value, 0.3f, Time.deltaTime);
    }

    public bool GetBoolAnimator(int id)
    {
       return _animator.GetBool(id);
    }

    public float GetFloatAnimator(int id)
    {
        return _animator.GetFloat(id);
    }

    public int GetIntAnimator(int id)
    {
        return _animator.GetInteger(id);
    }
    


}
