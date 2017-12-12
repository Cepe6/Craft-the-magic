using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSettings : MonoBehaviour {
    //World water settings
    [SerializeField]
    private float _waterFrequency;
    [SerializeField]
    private float _waterSize;

    //World
    
    // Use this for initialization
	void Awake () {
        DontDestroyOnLoad(transform.gameObject);
	}
}
