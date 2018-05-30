using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour {
    private static GameSettings _instance;

    [SerializeField]
    private InputField _seedInputField;

    private int _seed = 0;

	// Use this for initialization
	void Start () {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(_instance.gameObject);
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
	}

    public void SeedChange()
    {
        if(_seedInputField == null)
            _seedInputField = GameObject.FindGameObjectWithTag("SeedInputField").GetComponent<InputField> ();

        if (!int.TryParse(_seedInputField.text, out _seed))
        {
            _seed = 0;
        }

        Debug.Log("Seed: " + _seed);
    }

    public int GetSeed()
    {
        return _seed;
    }

    public static GameSettings Instance()
    {
        return _instance;
    }
}
