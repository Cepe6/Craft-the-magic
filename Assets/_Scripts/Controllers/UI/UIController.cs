using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [SerializeField]
    private GameObject _inventoryPanel;

    [SerializeField]
    private HotBarController _hotBarController;
    [SerializeField]
    private InventoryController _inventoryController;
    
    public void ShowInventory()
    {
        _inventoryController.Toggle(true);
    }

    public void CloseInventory()
    {
        _inventoryController.Toggle(false);
    }

    public bool InventoryState()
    {
        return _inventoryController.IsVisible();
    }

    public void ChangeHotbarPosition(int index)
    {
        _hotBarController.ChangePosition(index);
    }
}
