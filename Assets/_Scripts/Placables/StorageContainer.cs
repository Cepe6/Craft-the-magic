using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageContainer : InventoryAbstract
{
    [SerializeField]
    private GameObject _UIPanel;

    private GameObject _currentInstance;

    private GameObject _inventory;
    private UIController _uiController;

    private bool _open = false;

    private void OnEnable()
    {
        _inventory = GameObject.FindGameObjectWithTag("PlayerInventory");
        _uiController = GameObject.FindGameObjectWithTag("World Manager").GetComponent<UIController>();

        _currentInstance = Instantiate(_UIPanel, _inventory.transform);
        _currentInstance.SetActive(false);

        int i = 0;

        _slotsCount = _currentInstance.GetComponentsInChildren<Slot>().Length;
        _slots = new Slot[_slotsCount];
        foreach(Slot slot in _currentInstance.GetComponentsInChildren<Slot> ())
        {
            _slots[i++] = slot;

        }
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
}
