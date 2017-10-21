using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private GameObject _player;
    [SerializeField]
    private float _cameraHeight = 20.0f;
    [SerializeField]
    private float _cameraSpeed = 5f;
    [SerializeField]
    private float _xOffset;
    [SerializeField]
    private float _zOffset;

    // Use this for initialization
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = _player.transform.position;
        pos.y += _cameraHeight;
        pos.x += _xOffset;
        pos.z += _zOffset;
        transform.position = Vector3.Lerp(transform.position, pos, _cameraSpeed * Time.deltaTime);
    }
}
