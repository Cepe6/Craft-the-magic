using UnityEngine;
using UnityEngine.UI;

public class HotBarController : InventoryAbstract {
    private int _lastPos = 0;

    [SerializeField]
    private Sprite _hotbarSelectedSprite = null;
    [SerializeField]
    private Sprite _hotbarNotSelectedSprite = null;

    private bool _executedInEditMode = false;

    private void Awake()
    {
        _slotsCount = GlobalVariables.HOTBAR_SIZE;
    }

    private void Update()
    {

    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            ChangePosition(0);
        }
    }

    public void ChangePosition(int index)
    {
        GameObject oldSlot = _slots[_lastPos].gameObject;
        oldSlot.GetComponent<Image>().sprite = _hotbarNotSelectedSprite;

        GameObject newSlot = _slots[index].gameObject;
        newSlot.GetComponent<Image>().sprite = _hotbarSelectedSprite;

        _lastPos = index;
    }
}
