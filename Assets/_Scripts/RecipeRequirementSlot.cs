using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeRequirementSlot : MonoBehaviour {
    private PlayerInventoryController _inventory;

    private Item _item;
    private int _ammount;

    [SerializeField]
    private Image _slotBG;
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private Text _itemText;
    [SerializeField]
    private Text _itemName;

    [SerializeField]
    private Sprite _itemAvailableSprite;
    [SerializeField]
    private Sprite _itemNotAvailableSprite;

    private void Start()
    {
        _inventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventoryController>();   
    }

    public void Initialize(Item item, int ammount)
    {
        _item = item;
        _ammount = ammount;

        _itemName.text = _item.name;
        _itemImage.sprite = item.itemIcon;
        _itemText.text = ammount.ToString();
    }

    private void Update()
    {
        if (_inventory.CheckForItem(_item, _ammount))
        {
            _slotBG.sprite = _itemAvailableSprite;
        }
        else
        {
            _slotBG.sprite = _itemNotAvailableSprite;
        }
    }
}
