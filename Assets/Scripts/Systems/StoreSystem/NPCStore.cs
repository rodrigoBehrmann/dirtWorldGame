using UnityEngine;

public class NPCStore : MonoBehaviour
{
    [Header("Store Settings")]
    [SerializeField] private Store _storeType;

    [Header("UI Elements")]
    [SerializeField] private GameObject _interactableCanvas;

    private InputManager _inputManager;

    private bool _canInteract = false;

    void Start()
    {
        _inputManager = InputManager.Instance;

        _inputManager.InputControl.Actions.Interact.started += ctx =>
        {
            if (_canInteract)
            {
                _storeType.OpenStore();
            }
        };
        
        _interactableCanvas.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = true;
            _interactableCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = false;
            _interactableCanvas.SetActive(false);
        }
    }
}
