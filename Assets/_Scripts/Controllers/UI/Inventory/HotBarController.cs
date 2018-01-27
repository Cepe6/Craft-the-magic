using UnityEngine;
using UnityEngine.UI;

public class HotBarController : StaticInventory {
    //Singleton
    private static HotBarController _instance;
    public static HotBarController Instance
    {
        get { return _instance; }
    }


    private int _lastPos = -1;

    [SerializeField]
    private Sprite _hotbarSelectedSprite = null;
    [SerializeField]
    private Sprite _hotbarNotSelectedSprite = null;

    private bool _executedInEditMode = false;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

        _slotsCount = GlobalVariables.HOTBAR_SIZE;
    }

    private void Update()
    {
        if (_lastPos != -1 && _slots[_lastPos].item != null && _slots[_lastPos].item.type == ItemTypesEnum.Placable && GlobalVariables.CURRENT_PLACABLE == null)
        {
            GlobalVariables.CURRENT_PLACABLE = Instantiate(_slots[_lastPos].item.placableGO);
        }
    }

    public Slot GetCurrentSlot()
    {
        return _slots[_lastPos];
    }
    
    public void ChangePosition(int index)
    {
        if (_lastPos != -1)
        {
            GameObject oldSlot = _slots[_lastPos].gameObject;
            oldSlot.GetComponent<Image>().sprite = _hotbarNotSelectedSprite;

            if (_slots[_lastPos].item != null && _slots[_lastPos].item.type == ItemTypesEnum.Placable)
            {
                if (GlobalVariables.CURRENT_PLACABLE != null)
                    Destroy(GlobalVariables.CURRENT_PLACABLE);
            }
        }

        GameObject newSlot = _slots[index].gameObject;
        newSlot.GetComponent<Image>().sprite = _hotbarSelectedSprite;
        

        _lastPos = index;
    }
}
