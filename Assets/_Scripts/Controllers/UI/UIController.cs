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
    
    public void ShowInventory()
    {
        _inventoryPanel.GetComponent<Canvas> ().enabled = true;
    }

    public void CloseInventory()
    {
        _inventoryPanel.GetComponent<Canvas> ().enabled = false;
    }

    public bool InventoryState()
    {
        return _inventoryPanel.GetComponent<Canvas>().isActiveAndEnabled;
    }

    public void ChangeHotbarPosition(int index)
    {
        _hotBarController.ChangePosition(index);
    }
}
