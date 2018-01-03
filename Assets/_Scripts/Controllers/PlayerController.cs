﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour { 
    //Player movement variables
    [SerializeField]
    private float _movementSpeed = 5f;
    [SerializeField]
    private ChunksController _chunksController;

    private Rigidbody _rigidbody;
    private Quaternion _lastRotation = new Quaternion(0f, 0f, 0f, 0f);

    private bool _controlsEnabled = true;

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
        if ((horizontalAxis != 0f || verticalAxis != 0f) && _controlsEnabled)
        {
            movementDir = new Vector3(Mathf.Ceil(horizontalAxis), 0f, Mathf.Ceil(verticalAxis));
            transform.rotation = Quaternion.LookRotation(movementDir);
            movementDir.x *= _chunksController.IsWalkable(transform.position.x + movementDir.x * 2, transform.position.z);
            movementDir.z *= _chunksController.IsWalkable(transform.position.x, transform.position.z + movementDir.z * 2);

            _rigidbody.velocity = movementDir * _movementSpeed;
        } else
        {
            _rigidbody.velocity = Vector3.zero;
        } 
    }

    public void EnableControls()
    {
        _controlsEnabled = true;
    }

    public void DisableControls()
    {
        _controlsEnabled = false;
    }
}

