using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class RecipeSlot : SlotAbstract, IPointerEnterHandler
{
    [SerializeField]
    protected List<Item> _requiredItems = new List<Item>();
    [SerializeField]
    protected List<int> _requiredAmmounts = new List<int>();
    [SerializeField]
    private float _actionTime;

    [SerializeField]
    private GameObject _recipeRequirementSlot;

    protected PlayerInventoryController _inventory;

    private void Awake()
    {
        base.Awake();

        _inventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventoryController>();

        _itemIcon.sprite = _item.itemIcon;
        _itemIcon.gameObject.SetActive(true);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item != null)
        {
            _tooltip = Instantiate(SerializedGlobalVariables.instance.ItemInfoPanel, new Vector3(Input.mousePosition.x, Input.mousePosition.y - GetComponent<RectTransform>().rect.height, 0f), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            List<GameObject> _slots = new List<GameObject> ();

            for (int i = 0; i < _requiredItems.Count; i++)
            {
                _slots.Add(Instantiate(_recipeRequirementSlot));
                _slots[i].GetComponent<RecipeRequirementSlot>().Initialize(_requiredItems[i], _requiredAmmounts[i]);
            }

            _tooltip.GetComponent<ItemToolTipController>().Initialize(_item.name, _slots.ToArray());
        }
        GetComponent<Image>().color = SerializedGlobalVariables.instance.slotOnHoverColor;
    }

    public float ActionTime
    {
        get { return _actionTime;  }
    }

    public Dictionary<Item, int> GetRecipeItems()
    {
        Dictionary<Item, int> dictionary = new Dictionary<Item, int>();

        for(int i = 0; i < _requiredItems.Count; i++)
        {
            dictionary.Add(_requiredItems[i], _requiredAmmounts[i]);
        }

        return dictionary;
    }
}
