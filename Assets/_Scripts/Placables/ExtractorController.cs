using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExtractorController : MonoBehaviour {
    [SerializeField]
    private GameObject _UIPanel;

    private GameObject _currentInstance;

    private GameObject _inventory;
    private UIController _uiController;
    
    private bool _open = false;

    private TileData[,] _tilesUnder;
    private Vector2 _size;

    private Slot _fuelSlot;
    private Slot _outputSlot;
    private Slider _outputProgressSlider;
    private Slider _fuelAmmountLeft;

    private bool _mineWaiting = true;
    private bool _fuelBurning;
    private float _currentMineTime = 0f;
    private float _currentFuelLeft;
    private TileData _currentMined;

    private void OnEnable()
    {
        _inventory = GameObject.FindGameObjectWithTag("PlayerInventory");
        _uiController = GameObject.FindGameObjectWithTag("World Manager").GetComponent<UIController>();
        _size = GetComponent<Placable>().GetSize();
        _tilesUnder = GetComponent<Placable>().GetTilesUnder();

        _currentInstance = Instantiate(_UIPanel, _inventory.transform);
        _fuelSlot = _currentInstance.transform.Find("FuelSlot").GetComponent<Slot>();
        _outputSlot = _currentInstance.transform.Find("OutputSlot").GetComponent<Slot>();
        _outputProgressSlider = _currentInstance.transform.Find("OutputProgressSlider").GetComponent<Slider>();
        _fuelAmmountLeft = _currentInstance.transform.Find("FuelAmmountLeft").GetComponent<Slider>();
        _currentInstance.SetActive(false);        
    }

    private void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            if (_open)
            {
                _open = false;
                _currentInstance.SetActive(false);
            }
        }


        if(_currentMineTime < GlobalVariables.EXTRACTOR_MINE_TIME)
        {
            if (_currentMined == null || _currentMined.GetObjectAbove() == null)
                _currentMined = RandomTile();

            if (_currentMined != null && _currentMined.GetObjectAbove() != null)
            {
                if (!_fuelBurning)
                {
                    if (_fuelSlot.currentAmmount > 0)
                    {
                        StartCoroutine(BurnFuel());
                    }
                }

                if (_fuelBurning)
                {
                    _currentMineTime += Time.deltaTime;
                }
            }
        } else if(_currentMineTime >= GlobalVariables.EXTRACTOR_MINE_TIME && _currentMined != null && _currentMined.GetObjectAbove() != null) 
        {
            if (_outputSlot.currentAmmount < GlobalVariables.MAX_STACK_AMMOUNT)
            {
                _currentMined.GetObjectAbove().GetComponent<ResourceController>().ammount--;
                if(_outputSlot.item == null)
                {
                    _outputSlot.InitItem(_currentMined.GetObjectAbove().GetComponent<ResourceController>().item, 1);
                } else
                {
                    _outputSlot.currentAmmount++;
                }
                _currentMineTime = 0;
            }
        }

        _fuelAmmountLeft.value = Mathf.Lerp(0f, 1f, _currentFuelLeft / GlobalVariables.COAL_FUEL_AMMOUNT);
        _outputProgressSlider.value = Mathf.Lerp(0f, 1f, _currentMineTime / GlobalVariables.EXTRACTOR_MINE_TIME);
    }

    private IEnumerator BurnFuel()
    {
        _fuelBurning = true;
        _fuelSlot.currentAmmount--;
        _currentFuelLeft = GlobalVariables.COAL_FUEL_AMMOUNT;
        while (_currentFuelLeft > 0)
        { 
            _currentFuelLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _fuelBurning = false;
        yield return null;
    }

    private TileData RandomTile()
    {
        int x = (int)Random.Range(0, _size.x);
        int y = (int)Random.Range(0, _size.y);
        while(_tilesUnder[x, y].GetTileType() != TilesEnum.IRON_ORE && _tilesUnder[x, y].GetTileType() != TilesEnum.COAL && _tilesUnder[x, y].GetObjectAbove() == null)
        {
            x = (int)Random.Range(0, _size.x);
            y = (int)Random.Range(0, _size.y);
        }

        return _tilesUnder[x, y];
    }

    private void OnMouseOver()
    {
        if (isActiveAndEnabled && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(1) && !_open && GetComponent<Interactable> ().IsInteractable())
            {
                _currentInstance.SetActive(true);
                _uiController.ShowInventory();

                _currentInstance.transform.Find("FuelSlot").GetComponent<Slot>();
                _open = true;
            }
        }
    }

    private void OnDestroy()
    {
        Destroy(_currentInstance);
    }   
}
