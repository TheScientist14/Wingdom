using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] GameObject firstSelectedButton;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    public void OnShowHide()
    {
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
        if (pauseMenuPanel.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
            GameManager.instance.Pause();
        }
        else
        {
            GameManager.instance.Resume();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    // demo
    public void Restart()
    {
        GameManager.instance.Restart();
    }
}
