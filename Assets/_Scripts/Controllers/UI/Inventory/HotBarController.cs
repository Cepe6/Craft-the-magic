using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarController : MonoBehaviour {
    private const int _slotsCount = 10;

    private int _lastPos = 0;
    [SerializeField]
    private Slot[] _slots = new Slot[_slotsCount];

    [SerializeField]
    private Sprite _hotbarSelectedSprite = null;
    [SerializeField]
    private Sprite _hotbarNotSelectedSprite = null;
    

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _slots[i] = transform.GetChild(i).gameObject.GetComponent<Slot> ();
        }

        ChangePosition(0);
    }

    public void ChangePosition(int index)
    {
        GameObject oldSlot = _slots[_lastPos].gameObject;
        oldSlot.GetComponent<Image>().sprite = _hotbarNotSelectedSprite;

        GameObject newSlot = _slots[index].gameObject;
        newSlot.GetComponent<Image>().sprite = _hotbarSelectedSprite;

        _lastPos = index;
    }

    public int AddItemAndReturnRemainingAmmount(Item item, int ammount)
    {
        int ammountLeft = ammount;

        //Iterate once and add to the slots that already have this item in them
        for (int i = 0; i < _slotsCount; i++)
        {
            if (_slots[i].item != null && _slots[i].item.Equals(item))
            {
                ammountLeft = _slots[i].AddToAmmountAndReturnRemaining(ammountLeft);
            }

            if (ammountLeft == 0) { return 0; }
        }

        //Iterate once more and add the remaining to the slots that do not have item in them
        for (int i = 0; i < _slotsCount; i++)
        {
            if (_slots[i].item == null)
            {
                _slots[i].InitItem(item, ammountLeft);

                return 0;
            }
        }

        return ammountLeft;
    }
}
