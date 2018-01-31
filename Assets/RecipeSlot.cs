using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeSlot : SlotAbstract, IPointerClickHandler {
    [SerializeField]
    private List<Item> _requiredItems = new List<Item>();
    [SerializeField]
    private List<int> _requiredAmmounts = new List<int>();
    [SerializeField]
    private float _craftingTime;

    private PlayerInventoryController _inventory;

    private void Awake()
    {
        base.Awake();

        _inventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventoryController>();

        _itemIcon.sprite = _item.itemIcon;
        _itemIcon.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for(int i = 0; i < _requiredItems.Count; i++)
        {
            if(!_inventory.CheckForItem(_requiredItems[i], _requiredAmmounts[i]))
            {
                return;
            }
        }

        for(int i = 0; i < _requiredItems.Count; i++)
        {
            _inventory.GetItem(_requiredItems[i], _requiredAmmounts[i]);
        }

        CraftingQueue.Instance.AddEntry(_item, 1, this);
    }

    public float CraftTime
    {
        get { return _craftingTime;  }
    }
}
