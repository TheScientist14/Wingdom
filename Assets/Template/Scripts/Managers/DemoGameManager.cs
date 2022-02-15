using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DemoGameManager : GameManager
{
    void OnReload()
    {
        Restart();
    }
}
