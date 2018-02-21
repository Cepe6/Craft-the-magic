using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CraftingSlot : RecipeSlot, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            for (int i = 0; i < _requiredItems.Count; i++)
            {
                if (!_inventory.CheckForItem(_requiredItems[i], _requiredAmmounts[i]))
                {
                    return;
                }
            }

            for (int i = 0; i < _requiredItems.Count; i++)
            {
                _inventory.GetItem(_requiredItems[i], _requiredAmmounts[i]);
            }
            OnSuccessfulClick(1);
        } else if(eventData.button == PointerEventData.InputButton.Right)
        {
            for (int i = 0; i < _requiredItems.Count; i++)
            {
                if (!_inventory.CheckForItem(_requiredItems[i], _requiredAmmounts[i] * 5))
                {
                    return;
                }
            }

            for (int i = 0; i < _requiredItems.Count; i++)
            {
                _inventory.GetItem(_requiredItems[i], _requiredAmmounts[i] * 5);
            }
            OnSuccessfulClick(5);
        }

    }

    private void OnSuccessfulClick(int ammount)
    {
        CraftingQueue.Instance.AddEntry(_item, ammount, this);
    }
}
