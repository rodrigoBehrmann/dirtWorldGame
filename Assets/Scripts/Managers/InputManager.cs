using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public InputControl InputControl;  

    private void Awake()
    {
        InputControl = new InputControl();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        if(InputControl == null)
        {
            InputControl = new InputControl();
        }
        InputControl.Enable();
    }

    private void OnDisable()
    {
        InputControl.Disable();
    }
}