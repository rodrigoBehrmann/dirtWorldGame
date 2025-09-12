using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private Button _closeTutorialButton;
    [SerializeField] private GameObject tutorialPanel;
    void Start()
    {
        //pause the game
        Time.timeScale = 0f;
        _closeTutorialButton.onClick.AddListener(CloseTutorial);
    }

    private void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
