using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PMenu;

    private bool _isShow;
    // Start is called before the first frame update
    void Start()
    {
        PMenu.SetActive(false);
        _isShow = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnShowHide()
    {
        
        if (_isShow == false)
        {
            _isShow = true;
            print("affiché");
            PMenu.SetActive(true);
            GameManager.instance.Pause();
        }
        else
        {
            _isShow = false;

            print("caché");
            PMenu.SetActive(false);
            GameManager.instance.Resume();
        }
    }

    public void Quit()
    {
        print("QUIT");
        Application.Quit();
    }
}
