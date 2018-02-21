using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class  CraftingQueueEntry : SlotAbstract, IPointerDownHandler  {
    private int _ammount;

    private Text _ammountText;

    protected RecipeSlot _recipe;

    public RecipeSlot Recipe
    {
        get { return _recipe; }
    }

    public int Ammount {
        get
        {
            return _ammount;
        }
    }

    public void Nullify()
    {
        _itemIcon.gameObject.SetActive(false);
        _ammountText.gameObject.SetActive(false);
    }

    private void Awake()
    {
        base.Awake();
        _ammountText = transform.Find("Ammount").GetComponent<Text>();
    }

    private void Update()
    {
        if(_ammount == 0 && _item != null)
        {
            Destroy(this.gameObject);
        }
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


    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Dictionary<Item, int> requiredItems = _recipe.GetRecipeItems();

            for(int i = 0; i < requiredItems.Count; i++)
            {
                Item item = requiredItems.Keys.ElementAt(i);

                PlayerInventoryController.Instance.AddOrDrop(item, requiredItems[item]);
            }

            Destroy(this.gameObject);
        } else if(eventData.button == PointerEventData.InputButton.Right)
        {
            DecrementAmmount();
            if(_ammount == 0)
            {
                Dictionary<Item, int> requiredItems = _recipe.GetRecipeItems();

                for (int i = 0; i < requiredItems.Count; i++)
                {
                    Item item = requiredItems.Keys.ElementAt(i);

                    PlayerInventoryController.Instance.AddOrDrop(item, requiredItems[item]);
                }
            }
        }
    }
}
