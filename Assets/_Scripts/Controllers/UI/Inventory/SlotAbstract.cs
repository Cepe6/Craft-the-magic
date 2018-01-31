using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SlotAbstract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected Item _item;
    protected Image _itemIcon;


    private Color _originalColor;

    protected void Awake()
    {
        _itemIcon = transform.Find("Image").GetComponent<Image>();
        _originalColor = GetComponent<Image>().color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = SerializedGlobalVariables.instance.slotOnHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
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
}
