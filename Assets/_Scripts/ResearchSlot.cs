using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ResearchSlot : RecipeSlot, IPointerClickHandler
{
    [SerializeField]
    private List<GameObject> _results = new List<GameObject>();

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
            OnSuccessfulClick();
        }
    }

    private void OnSuccessfulClick()
    {
        ResearchQueue.Instance.AddEntry(_item, this, _results);

        Destroy(this.gameObject);
    }
}
