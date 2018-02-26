using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    void FixedUpdate()
    {
        Vector3 pos = _player.transform.position;
        pos.y += _cameraHeight;
        pos.x += _xOffset;
        pos.z += _zOffset;
        transform.position = Vector3.Lerp(transform.position, pos, _cameraSpeed * Time.deltaTime);
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            float mouseWheelAxis = Input.GetAxisRaw("Mouse ScrollWheel") / 100;
            float currSize = GetComponent<Camera>().orthographicSize;

            if (mouseWheelAxis < 0 && currSize < 250)
            {
                GetComponent<Camera>().orthographicSize += 20;
            }
            else if (mouseWheelAxis > 0 && currSize > 50)
            {
                GetComponent<Camera>().orthographicSize -= 20;
            }
        }
    }
}
