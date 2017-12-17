using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [SerializeField]
    private GameObject _inventoryPanel;

    [SerializeField]
    private List<Image> _hotbarSlots;
    [SerializeField]
    private Sprite _hotbarSelectedSprite;
    [SerializeField]
    private Sprite _hotbarNotSelectedSprite;
    private int _lastHotbarPosition = 1;

    private void Start()
    {
        ChangeHotbarPosition(1);
    }

    public void ShowInventory()
    {
        _inventoryPanel.SetActive(true);
    }

    public void CloseInventory()
    {
        _inventoryPanel.SetActive(false);
    }

    public bool InventoryState()
    {
        return _inventoryPanel.activeSelf;
    }

    public void ChangeHotbarPosition(int index)
    { 
        Image oldSlot = _hotbarSlots.Where(slot => slot.name.Equals("Slot" + _lastHotbarPosition)).SingleOrDefault();
        oldSlot.sprite = _hotbarNotSelectedSprite;

        Image newSlot = _hotbarSlots.Where(slot => slot.name.Equals("Slot" + index)).SingleOrDefault();
        newSlot.sprite = _hotbarSelectedSprite;

        _lastHotbarPosition = index;
    }
}
