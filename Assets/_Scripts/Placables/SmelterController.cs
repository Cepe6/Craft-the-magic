using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SmelterController : InventoryAbstract
{
    [SerializeField]
    private GameObject _UIPanel;

    private GameObject _currentInstance;

    private GameObject _inventory;
    private UIController _uiController;

    private bool _open = false;

    private Slot _fuelSlot;
    private Slot _inputSlot;
    private Slot _outputSlot;
    private Slider _outputProgressSlider;
    private Slider _fuelAmmountLeft;
    
    private bool _fuelBurning;
    private float _currentSmeltTime = 0f;
    private float _currentFuelLeft;

    private void OnEnable()
    {
        _inventory = GameObject.FindGameObjectWithTag("PlayerInventory");
        _uiController = GameObject.FindGameObjectWithTag("World Manager").GetComponent<UIController>();

        _currentInstance = Instantiate(_UIPanel, _inventory.transform);
        _fuelSlot = _currentInstance.transform.Find("FuelSlot").GetComponent<Slot>();
        _inputSlot = _currentInstance.transform.Find("InputSlot").GetComponent<Slot>();
        _outputSlot = _currentInstance.transform.Find("OutputSlot").GetComponent<Slot>();
        _outputProgressSlider = _currentInstance.transform.Find("OutputProgressSlider").GetComponent<Slider>();
        _fuelAmmountLeft = _currentInstance.transform.Find("FuelAmmountLeft").GetComponent<Slider>();
        _currentInstance.SetActive(false);

        _slots = new Slot[3];
        _slotsCount = 3;

        _slots[0] = _fuelSlot;
        _slots[1] = _outputSlot;
        _slots[2] = _inputSlot;
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

        if (_currentSmeltTime < GlobalVariables.SMELTER_SMELT_TIME)
        {
            if (_inputSlot.item != null)
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
                    _currentSmeltTime += Time.deltaTime;
                }
            } else
            {
                _currentSmeltTime = 0f;
            }
        }
        else if (_currentSmeltTime >= GlobalVariables.SMELTER_SMELT_TIME)
        {
            if (_outputSlot.currentAmmount < GlobalVariables.MAX_STACK_AMMOUNT)
            {
                _inputSlot.currentAmmount--;
                if (_outputSlot.item == null)
                {
                    _outputSlot.InitItem(_inputSlot.item.interactionResult, 1);
                }
                else if(_outputSlot.item == _inputSlot.item.interactionResult)
                {
                    _outputSlot.currentAmmount++;
                }
                _currentSmeltTime = 0;
            }
        }

        _fuelAmmountLeft.value = Mathf.Lerp(0f, 1f, _currentFuelLeft / GlobalVariables.COAL_FUEL_AMMOUNT);
        _outputProgressSlider.value = Mathf.Lerp(0f, 1f, _currentSmeltTime / GlobalVariables.SMELTER_SMELT_TIME);
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

    private void OnMouseOver()
    {
        if (isActiveAndEnabled && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(1) && !_open && GetComponent<Interactable>().IsInteractable())
            {
                _currentInstance.SetActive(true);
                _uiController.ShowInventory();
                
                _open = true;
            }
        }
    }

    private void OnDestroy()
    {
        Destroy(_currentInstance);
    }
}
