using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour {
    [SerializeField]
    private Item _item;
    private int _ammount;
    
    [SerializeField]
    private Slider _actionSlider;


    PlayerController _player;
    PlayerInventoryController _inventory;
    private float _currentMineSessionTime = 0f;
    private bool _currentlyMining = false;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _inventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventoryController>();
        _actionSlider = GameObject.FindGameObjectWithTag("ActionSlider").GetComponent<Slider> ();
    }

    // Use this for initialization
    void Start () {
        InitializeAmmount();
	}

    private void InitializeAmmount()
    {
        _ammount = 1;// (Random.Range(-50, 50) + WorldSettings.DEFAULT_RESOURCE_CAPACITY) + WorldSettings.INCREASE_RESOURCE_CAPACITY * (WorldSettings.DEFAULT_RESOURCE_CAPACITY / 4);
    }
	
	// Update is called once per frame
	void Update () {
        if (_ammount == 0)
        {
            Destroy(gameObject);
        }
	}
    
    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
        }
    }

    public int ammount
    {
        get { return _ammount; }
        set
        {
            _ammount = value;
        }
    }

    private void OnMouseExit()
    {
        if(!_player.GetControlsEnabled())
        {
            _player.EnableControls();
        }

        if(_actionSlider.GetComponent<Canvas> ().isActiveAndEnabled)
        {
            _actionSlider.GetComponent<Canvas>().enabled = false;
        }
    }

    private void OnMouseOver()
    {
        bool inRange = Vector3.Distance(transform.position, _player.transform.position) <= GlobalVariables.INTERACT_DISTANCE;
        if (Input.GetMouseButton(1) && inRange)
        {
            Mine();

        } else if(!Input.GetMouseButton(1))
        {
            if (!_player.GetControlsEnabled())
            {
                _player.EnableControls();
            }

            if (_actionSlider.GetComponent<Canvas> ().isActiveAndEnabled)
            {
                _actionSlider.GetComponent<Canvas>().enabled = false;
            }
        }
    }

    public void Mine()
    {
        if (!_currentlyMining)
        {
            _currentlyMining = true;
            _actionSlider.GetComponent<Canvas>().enabled = true;
            _player.DisableControls();
        }
        else
        {
            if (_currentMineSessionTime >= GlobalVariables.IRON_PICK_MINE_TIME)
            {
                _inventory.AddOrDrop(item, 1);
                _ammount--;
                _currentMineSessionTime = 0f;
                _currentlyMining = false;
                _actionSlider.GetComponent<Canvas>().enabled = false;
                _player.EnableControls();
            }
            else
            {
                if (!_actionSlider.GetComponent<Canvas>().isActiveAndEnabled)
                {
                    _actionSlider.GetComponent<Canvas>().enabled = true;
                }
                _actionSlider.value = Mathf.Lerp(0f, 1f, _currentMineSessionTime / GlobalVariables.IRON_PICK_MINE_TIME);

                _currentMineSessionTime += Time.deltaTime;
            }
        }
    }
}
