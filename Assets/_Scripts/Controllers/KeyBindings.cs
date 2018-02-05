using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class KeyBindings : MonoBehaviour {    
    private Dictionary<string, KeyCode> _bindings = new Dictionary<string, KeyCode>();

    private string _bindingsFilePath = null;
    
    private void Awake()
    {
#if UNITY_EDITOR
        _bindingsFilePath = Application.dataPath + "/Resources/Settings/KeyBindings.json";
#elif UNITY_STANDALONE
        _bindingsFilePath = Application.dataPath + "/Resources/Settings/KeyBindings.json";
#endif
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        LoadBindings();
    }

    // Update is called once per frame
    void Update () {
		
	}

    //Only use this function when something goes wrong with the settings file. This is the default values of all keys
    private void InitalizeDefaultBindings()
    {
        //Movement
        _bindings.Add("Up", KeyCode.W);
        _bindings.Add("Down", KeyCode.S);
        _bindings.Add("Left", KeyCode.A);
        _bindings.Add("Right", KeyCode.D);

        //Inventory
        _bindings.Add("OpenInv", KeyCode.E);
    }

    public KeyCode GetKeyCodeFromName(string name)
    {
        KeyCode output;
        _bindings.TryGetValue(name, out output);
        return output;
    }

    private void SaveBindings()
    {
        StreamWriter streamWriter = new StreamWriter(_bindingsFilePath);
        foreach(string key in _bindings.Keys)
        {
            KeyCode value;
            _bindings.TryGetValue(key, out value);
            streamWriter.WriteLine(key + " : " + value.ToString());
        }
        streamWriter.Close();

#if UNITY_EDITOR
        AssetDatabase.ImportAsset(_bindingsFilePath);
#endif
    }

    private void LoadBindings()
    {
        StreamReader reader = new StreamReader(_bindingsFilePath);
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            Regex regex = new Regex("([A-Za-z]+)\\s+\\:\\s+([A-Za-z0-9]+)");
            Match match = regex.Match(line);
            if (match.Success)
            {
                string key = match.Groups[1].Value;
                KeyCode value = (KeyCode)System.Enum.Parse(typeof(KeyCode), match.Groups[2].Value);

                if(_bindings.ContainsKey(key))
                {
                    _bindings[key] = value;
                } else
                {
                    _bindings.Add(key, value);
                }
            }
        }
    }
}
