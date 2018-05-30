using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public static SceneController _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        } else
        {
            Destroy(_instance.gameObject);
            _instance = this;
        }
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
