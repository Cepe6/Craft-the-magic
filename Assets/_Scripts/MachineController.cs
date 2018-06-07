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

    protected void UpdateInventoryForSave()
    {
        List<string> items = new List<string>();
        foreach (KeyValuePair<string, Slot> slotEntry in _slotsDictionary)
        {
            string item = slotEntry.Key.ToString();
            if(slotEntry.Value.item != null)
            {
                item += "," + slotEntry.Value.item.name + "," + slotEntry.Value.currentAmmount;
                items.Add(item);
            }
             
        }
        GameSaver.GameInfo.ChangePlacedMachineInventory(name + "," + transform.position.x + "," + transform.position.z + "," + transform.eulerAngles.y, items);
    }

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

        if(GameSettings.Instance().IsSaved()) {
            int machineIndex;
            if ((machineIndex = GameSettings.Instance().GetPlacedMachines().IndexOf(name + "," + transform.position.x + "," + transform.position.z + "," + transform.eulerAngles.y)) != -1)
            {
                foreach(string slot in GameSettings.Instance().GetPlacedMachinesInventories().ElementAt(machineIndex).list)
                {
                    string slotName = slot.Split(',')[0];
                    string itemName = slot.Split(',')[1];
                    int ammount = int.Parse(slot.Split(',')[2]);
               
                    _slotsDictionary[slotName].InitItem(Resources.Load<Item>("ScriptableObjects/" + itemName), ammount);
                }
            }
        }
    }

    protected void Update()
    {

        UpdateInventoryForSave();
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
