using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{
    private Animator _animator;

    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
        _animator.SetFloat(id, value);
    }
    

}
