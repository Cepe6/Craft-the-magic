using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour { 
    //Player movement variables
    [SerializeField]
    private float _movementSpeed = 5f;

    private ChunksController _chunksController;

    private Rigidbody _rigidbody;
    private Quaternion _lastRotation = new Quaternion(0f, 0f, 0f, 0f);

    private List<GameObject> _currentCollidingDrops = new List<GameObject>();
    private Animator _animator;

    private bool _controlsEnabled = true;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _chunksController = GameObject.FindGameObjectWithTag("World Manager").GetComponent<ChunksController>();
    }

    private void Update()
    {
        if(Input.GetAxis("PickItem") != 0)
        {
            PickDrops();
        }
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
            bool movingDiagonally = horizontalAxis * verticalAxis != 0;

            if(!_animator.GetBool("isRunning"))
            {
                _animator.SetBool("isRunning", true);
            }

            movementDir = new Vector3(Mathf.Ceil(horizontalAxis), 0f, Mathf.Ceil(verticalAxis));
            transform.rotation = Quaternion.LookRotation(movementDir);
            movementDir.x *= _chunksController.IsWalkable(transform.position.x + movementDir.x * 2, transform.position.z);
            movementDir.z *= _chunksController.IsWalkable(transform.position.x, transform.position.z + movementDir.z * 2);
            
            _rigidbody.velocity = movementDir * _movementSpeed / (movingDiagonally ? Mathf.Sqrt(2f) : 1);
        } else
        {
            if(_animator.GetBool("isRunning"))
            {
                _animator.SetBool("isRunning", false);
            }
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

    public void AddDrop(GameObject drop)
    {
        _currentCollidingDrops.Add(drop);
    }

    public void RemoveDrop(GameObject drop)
    {
        _currentCollidingDrops.Remove(drop);
    }

    private void PickDrops()
    {
        foreach(GameObject drop in _currentCollidingDrops)
        {
            drop.GetComponent<Drop> ().AddToInventory();
        }
    }

    public List<GameObject> GetCollidingDrops()
    {
        return _currentCollidingDrops;
    }

    public bool GetControlsEnabled()
    {
        return _controlsEnabled;
    }

    public void Dig()
    {
        if (!_animator.GetBool("isDigging"))
        {
            _animator.SetBool("isDigging", true);
        }
    }

    public void StopDigging()
    {
        if (_animator.GetBool("isDigging"))
        {
            _animator.SetBool("isDigging", false);
        }
    }
}

