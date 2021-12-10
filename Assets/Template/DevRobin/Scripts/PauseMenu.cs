using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PMenu;
    // Start is called before the first frame update
    void Start()
    {
        PMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnShowHide()
    {
        if (!PMenu.activeSelf)
        {
            PMenu.SetActive(true);
            GameManager.instance.Pause();
        }
        else
        {
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
