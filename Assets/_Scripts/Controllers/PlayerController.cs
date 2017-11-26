using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    //Mouse button constants
    private const int LEFT_CLICK = 0;
    private const int RIGHT_CLICK = 1;
    private const int MIDDLE_CLICK = 2;

    //Player movement variables
    [SerializeField]
    private float _movementSpeed = 5f;

    private Rigidbody _rigidbody;
    private Quaternion _lastRotation = new Quaternion(0f, 0f, 0f, 0f);

	// Use this for initialization
	void Start () {
        _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
    }

    //Executed every frame. The player movement function
    private void Move() {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        if(horizontalAxis != 0f || verticalAxis != 0f) { 
            Vector3 movementDir = new Vector3(Mathf.CeilToInt(horizontalAxis), 0f, Mathf.CeilToInt(verticalAxis));
            transform.position += movementDir * _movementSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(movementDir);
        }
    }
}

