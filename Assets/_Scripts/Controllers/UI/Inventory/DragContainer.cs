using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragContainer : MonoBehaviour {
    public int slotId { get; set; }
    public Item item { get; set; }
    public int currentAmmount { get; set; }
    
    private InventoryController _inventory;

    [SerializeField]
    private Image _itemIcon;
    [SerializeField]
    private Text _itemAmmount;

    private void Awake()
    {
        _inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryController>();
    }

    private void Start()
    {
        BeginDrag();
    }

    private void Update()
    {
        if (currentAmmount == 0 || item == null)
        {
            Destroy(this.gameObject); 
        }
        else
        {
            _itemAmmount.text = currentAmmount.ToString();
        }
    }

    public void InitItem(Item newItem, int ammount)
    {
        item = newItem;
        currentAmmount = ammount;
        _itemIcon.sprite = newItem.itemIcon;
        _itemIcon.gameObject.SetActive(true);

        if (!item.isStackable)
        {
            _itemAmmount.gameObject.SetActive(false);
        } else
        {
            _itemAmmount.gameObject.SetActive(true);
        }
    }

    public void BeginDrag()
    {
        transform.position = Input.mousePosition;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        GlobalNonConstantVariables.ITEM_CONTAINER_BEING_DRAGGED = this;

        StartCoroutine(Drag());
    }
   
    private IEnumerator Drag()
    {
        while(true) {
            transform.position = Input.mousePosition;
            yield return null;
        }

    }
}
