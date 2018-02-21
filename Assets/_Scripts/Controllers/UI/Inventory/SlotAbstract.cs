using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SlotAbstract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected Item _item;
    protected Image _itemIcon;

    protected GameObject _tooltip;

    private Color _originalColor;

    protected void Awake()
    {
        _itemIcon = transform.Find("Image").GetComponent<Image>();
        _originalColor = GetComponent<Image>().color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item != null && GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED == null)
        {
            _tooltip = Instantiate(SerializedGlobalVariables.instance.ItemInfoPanel, new Vector3(Input.mousePosition.x, Input.mousePosition.y - GetComponent<RectTransform>().rect.height, 0f), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            _tooltip.GetComponent<ItemToolTipController>().Initialize(_item.name);
        }
        GetComponent<Image>().color = SerializedGlobalVariables.instance.slotOnHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_tooltip != null)
            Destroy(_tooltip);
        GetComponent<Image>().color = _originalColor;
    }

    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
        }
    }

    private void OnDestroy()
    {
        if(_tooltip != null)
        {
            Destroy(_tooltip);
        }   
    }
}
