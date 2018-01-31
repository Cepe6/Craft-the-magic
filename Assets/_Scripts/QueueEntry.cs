using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueEntry : SlotAbstract {
    private int _ammount;

    private Text _ammountText;

    private RecipeSlot _recipe;

    private void Awake()
    {
        base.Awake();
        _ammountText = transform.Find("Ammount").GetComponent<Text>();
    }

    public void Init(Item item, int ammount, RecipeSlot recipe)
    {
        _item = item;
        _ammount = ammount;

        _itemIcon.sprite = _item.itemIcon;
        _ammountText.text = ammount.ToString();

        _itemIcon.gameObject.SetActive(true);
        _ammountText.gameObject.SetActive(true);

        _recipe = recipe;
    }

    public void DecrementAmmount()
    {
        _ammount--;
        _ammountText.text = _ammount.ToString();
    }

    public RecipeSlot Recipe
    {
        get { return _recipe; }
    }

    public int Ammount
    {
        get { return _ammount; }
    }

    public void Nullify()
    {
        _itemIcon.gameObject.SetActive(false);
        _ammountText.gameObject.SetActive(false);
    }
}
