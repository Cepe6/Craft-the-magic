using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public abstract class InventoryAbstract : MonoBehaviour
{
    protected int _slotsCount;
    
    [SerializeField]
    protected Slot[] _slots;

    private void Awake()
    {
        _slots = new Slot[_slotsCount];
    }

    public Slot[] GetSlots()
    {
        return _slots;
    }

    public int SlotsCount()
    {
        return _slotsCount;
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

    public void SortInventory()
    {
        //FIRST fill any incomplete stacks of items
        for (int i = 0; i < _slotsCount - 1; i++)
        {
            if (_slots[i].item != null && _slots[i].currentAmmount < GlobalVariables.MAX_STACK_AMMOUNT)
            {
                for (int j = i + 1; j < _slotsCount; j++)
                {
                    if (_slots[j].item != null && _slots[i].item.Equals(_slots[j].item))
                    {
                        _slots[j].currentAmmount = _slots[i].AddToAmmountAndReturnRemaining(_slots[j].currentAmmount);
                    }

                    if (_slots[j].currentAmmount == GlobalVariables.MAX_STACK_AMMOUNT) { break; }
                }
            }
        }

        //SECOND sort the array of slots
        for (int passings = 0; passings < _slotsCount - 1; passings++)
        {
            for (int i = 0; i < _slotsCount - 1; i++)
            {
                if (_slots[i].item == null && _slots[i + 1].item != null)
                {
                    _slots[i].InitItem(_slots[i + 1].item, _slots[i + 1].currentAmmount);
                    _slots[i + 1].Nullify();
                }
                else if (_slots[i].item != null && _slots[i + 1].item != null && _slots[i].item.type > _slots[i + 1].item.type)
                {
                    Slot tempSlot = new Slot();
                    tempSlot.CopySlotProperties(_slots[i]);
                    _slots[i].InitItem(_slots[i + 1].item, _slots[i + 1].currentAmmount);
                    _slots[i + 1].InitItem(tempSlot.item, tempSlot.currentAmmount);
                }
            }
        }
    }
}
