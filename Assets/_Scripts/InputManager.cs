using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

    private UIController _uiController;
    private PlayerController _player;

    // Use this for initialization
    void Start() {
        _uiController = GetComponent<UIController>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        ReadCancel();
        ReadInventory();
        ReadNums();
    }

    private void ReadCancel()
    {
        if (Input.GetButton("Cancel"))
        {
            if (_uiController.InventoryState())
            {
                _uiController.CloseInventory();
                _uiController.CloseCraftingPanel();
                _uiController.CloseResearchPanel();
                _player.EnableControls();
            }
        }
    }

    private void ReadInventory()
    {
        if (!_uiController.InventoryState())
        {
            if (Input.GetButton("Inventory"))
            {
                _uiController.ShowInventory();
                _uiController.ShowCraftingPanel();
                _player.DisableControls();
            } else if(Input.GetButton("Research"))
            {
                _uiController.ShowInventory();
                _uiController.ShowResearchPanel();
                _player.DisableControls();
            }

            
        }
    }

    private void ReadNums()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            _uiController.ChangeHotbarPosition(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            _uiController.ChangeHotbarPosition(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            _uiController.ChangeHotbarPosition(2);
        } else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            _uiController.ChangeHotbarPosition(3);
        } else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            _uiController.ChangeHotbarPosition(4);
        } else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            _uiController.ChangeHotbarPosition(5);
        } else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            _uiController.ChangeHotbarPosition(6);
        } else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        { 
            _uiController.ChangeHotbarPosition(7);
        } else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            _uiController.ChangeHotbarPosition(8);
        } else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            _uiController.ChangeHotbarPosition(9);
        }
    }
}
