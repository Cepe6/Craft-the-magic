using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MachineController : InventoryAbstract
{
    [SerializeField]
    private GameObject _UIPanel;

    private GameObject _currentInstance;

    private GameObject _inventory;
    private UIController _uiController;

    protected Dictionary<string, Slot> _slotsDictionary = new Dictionary<string, Slot>();
    protected Dictionary<string, Slider> _slidersDictionary = new Dictionary<string, Slider>();

    private bool _open = false;

    protected void OnEnable()
    {
        _inventory = GameObject.FindGameObjectWithTag("PlayerInventory");
        _uiController = GameObject.FindGameObjectWithTag("World Manager").GetComponent<UIController>();

        _currentInstance = Instantiate(_UIPanel, _inventory.transform);

        
        foreach(Slot slot in _currentInstance.gameObject.GetComponentsInChildren<Slot> ())
        {
            _slotsDictionary.Add(slot.gameObject.name, slot);
        }

        _slotsCount = _slotsDictionary.Values.Count;
        _slots = _slotsDictionary.Values.ToArray();

        foreach(Slider slider in _currentInstance.gameObject.GetComponentsInChildren<Slider> ())
        {
            _slidersDictionary.Add(slider.gameObject.name, slider);
        }


        _currentInstance.SetActive(false);
    }

    protected void Update()
    {
        if (Input.GetButton("Cancel"))  
        {
            if (_open)
            {
                _open = false;
                _currentInstance.SetActive(false);
            }
        }

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
        if (_currentInstance != null)
            Destroy(_currentInstance);
    }
}
