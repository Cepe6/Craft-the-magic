using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour { 
    //Player movement variables
    [SerializeField]
    private float _movementSpeed = 5f;

    private Rigidbody _rigidbody;
    private Quaternion _lastRotation = new Quaternion(0f, 0f, 0f, 0f);

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    //Executed every frame. The player movement function
    private void Move()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        Vector3 movementDir = Vector3.zero;
        if (horizontalAxis != 0f || verticalAxis != 0f)
        {
            movementDir = new Vector3(Mathf.CeilToInt(horizontalAxis), 0f, Mathf.CeilToInt(verticalAxis));
            transform.rotation = Quaternion.LookRotation(movementDir);
            _rigidbody.velocity = movementDir * _movementSpeed;
        } else
        {
            _rigidbody.velocity = Vector3.zero;
        } 
    }
}

