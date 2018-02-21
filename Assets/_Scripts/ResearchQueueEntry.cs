using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResearchQueueEntry : SlotAbstract
{
    private RecipeSlot _recipe;

    private List<GameObject> _results = new List<GameObject> ();

    public void Init(Item item, RecipeSlot recipe, List<GameObject> results)
    {
        _item = item;

        _itemIcon.sprite = _item.itemIcon;

        _itemIcon.gameObject.SetActive(true);

        _recipe = recipe;
        _results = results;
    }

    public RecipeSlot Recipe
    {
        get
        {
            return _recipe;
        }
    }

    public List<GameObject> Results
    {
        get
        {
            return _results;
        }
    }

    public void Nullify()
    {
        _itemIcon.gameObject.SetActive(false);
    }
}
