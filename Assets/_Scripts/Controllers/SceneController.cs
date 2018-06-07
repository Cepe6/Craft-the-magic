using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
   // private static SceneController _instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnNewGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
