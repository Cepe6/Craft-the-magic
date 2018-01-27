using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

[System.Serializable]
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField]
    private Item _item;
    [SerializeField]
    private int _ammount;
    
    private Image _itemIcon;
    private Text _itemAmmount;
    
    private GameObject _dragContainer;
    [SerializeField]
    private bool _filtered ;
    [SerializeField]
    private List<ItemTypesEnum> _allowedTypes;
    
    private Color _originalColor;

    [SerializeField]
    private bool _inventorySlot = true;

    private bool _hovered = false;

    private void Awake()
    {
        _itemIcon = transform.Find("Image").GetComponent<Image> ();
        _itemAmmount = transform.Find("Ammount").GetComponent<Text>();

        _originalColor = GetComponent<Image>().color;
        if(_item != null)
        {
            InitItem(_item, _ammount);
        }

    }

    private void Start()
    {
        _dragContainer = SerializedGlobalVariables.instance.dragContainerPrefab;
    }

    private void Update()
    {
        if (_ammount == 0 || _item == null)
        {
            _item = null;

            _itemAmmount.gameObject.SetActive(false);
            _itemIcon.gameObject.SetActive(false);
        }
        else
        {
            if(!_itemAmmount.gameObject.activeSelf && _item.isStackable) { _itemAmmount.gameObject.SetActive(true); }
            if(!_itemIcon.gameObject.activeSelf) { _itemIcon.gameObject.SetActive(true); }

            _itemAmmount.text = _ammount.ToString();
        }

        //This is if in the inventory inspector I specify item with ammount > GlobalVariables.MAX_STACK_AMMOUNT
        if(_ammount > GlobalVariables.MAX_STACK_AMMOUNT) { _ammount = GlobalVariables.MAX_STACK_AMMOUNT; }
    }

    public bool InitItem(Item newItem, int ammount)
    {
        if (_filtered)
        {
            if (_allowedTypes.Where(type => newItem.type.Equals(type)).ToList().Count == 0)
            {
                return false;
            }
        }

        _item = newItem;
        if (item.isStackable)
        {
            _ammount = ammount;
        } else
        {
            _ammount = 1;
        }
         
        _itemIcon.sprite = newItem.itemIcon;

        return true;
    }

    public void TakeHalfStack()
    {
        int halfAmmount = (int)Mathf.Ceil(_ammount / 2f);
        _ammount -= halfAmmount;

        GameObject dragContainer = Instantiate(_dragContainer, Input.mousePosition, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        dragContainer.GetComponent<DragContainer>().InitItem(item, halfAmmount);
    }

    public void TakeFullStack()
    {
        GameObject dragContainer = Instantiate(_dragContainer, Input.mousePosition, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        dragContainer.GetComponent<DragContainer>().InitItem(item, _ammount);

        _ammount = 0;
    }

    public int AddToAmmountAndReturnRemaining(int ammount)
    {
        if (_ammount + ammount < GlobalVariables.MAX_STACK_AMMOUNT)
        {
            _ammount += ammount;
            return 0;
        }
        else
        {
            int returnVal = _ammount + ammount - GlobalVariables.MAX_STACK_AMMOUNT;
            _ammount = GlobalVariables.MAX_STACK_AMMOUNT;
            return returnVal;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //If the slot is clicked without a container being dragged then we are going to interact with the slot directly
        if(GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED == null && _item != null)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //IF the shift is being held... then we are going to transfer items between inventories (hotbar-inventory, inventory-ore_gatherer_inventory, etc.)
                if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    //IF the item is from the player inventory, then we know we will be transferring it to the currently open inventory from some machine or the hotbar if there is no machine inventory open
                    if(_inventorySlot) {
                        HotBarController _hotbar = GameObject.FindGameObjectWithTag("Hotbar").GetComponent<HotBarController>();
                        _ammount = _hotbar.AddItemAndReturnRemainingAmmount(_item, _ammount);
                    }
                    
                    //Else we know that we need to transfer item from the currently open machine inventory to the player inventory
                    else
                    {
                        GameObject _inventory = GameObject.FindGameObjectWithTag("PlayerInventory");
                        if(_inventory == null)
                        {
                            return;
                        }

                        PlayerInventoryController _inventoryController = _inventory.GetComponent<PlayerInventoryController>();

                        _ammount = _inventoryController.AddItemAndReturnRemainingAmmount(_item, _ammount);
                    }
                }
                //Else the shift is not being held and we are going to take the full ammount of the item in the slot and create a new drag container with it
                else { 
                    TakeFullStack();
                }
          
            }

            //Else if the button clicked is the right mouse button we will take only half the current ammount of the item in the slot and create a new drag container with it
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                TakeHalfStack();
            }
        }

        //Else the slot is clicked and we have a container being dragged, execute interaction between the slot and the container
        else if(GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED != null)
        {
            //IF this slot is filtered to only some items then we will check if the item in the drag container is in the list of allowed items IF it is not in the list then we just return
            if(_filtered)
            {
                if (_allowedTypes.Where(type => GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.item.type.Equals(type)).ToList().Count == 0)
                {
                    return;
                } 
            }

            //IF the left mouse button is clicked
            if (eventData.button == PointerEventData.InputButton.Left)
            {

                //IF there is no item in this slot, drop the item from the drag container into the slot
                if (_item == null)
                {
                    InitItem(GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.item, GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount);
                    GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount = 0;
                }

                //IF items are equal then add ammount form the drag container
                else if (_item.Equals(GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.item))
                {
                    GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount = AddToAmmountAndReturnRemaining(GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount);
                }

                //IF items are different swap them
                else
                {
                    Item tempItem = _item;
                    int tempAmmount = _ammount;

                    InitItem(GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.item, GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount);
                    GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.InitItem(tempItem, tempAmmount);
                }
            }

            //ELSE IF the right mouse button is clicked
            else if (eventData.button == PointerEventData.InputButton.Right)
            {

                //IF there is no item, initialize the slot with this item with ammount of one and decrease the ammount from the drag container with 1
                if (_item == null)
                {
                    InitItem(GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.item, 1);
                    GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount--;
                }

                //IF the item in the slot is equal then just increase its ammount if possible and decrease the ammount in the drag container
                else if (_item.Equals(GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.item))
                {
                    if (_ammount < GlobalVariables.MAX_STACK_AMMOUNT)
                    {
                        _ammount++;
                        GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount--;
                    }
                }
            }

            //ELSE IF the middle mouse button is clicked
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                //IF the item is null first initialize it for the item in the container and then execute the mouse wheel control coroutine
                if (_item == null)
                {
                    InitItem(GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.item, 0);
                    StartCoroutine(MouseWheelControl());
                }

                //IF the item is equal to the drag container ammount then execute the mouse wheel control coroutine
                else if (_item.Equals(GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.item)) {
                    StartCoroutine(MouseWheelControl());
                }
            }
        }
    }

    private IEnumerator MouseWheelControl()
    {
        while (true)
        {
            //IF escape btn is clicked or the mouse left or right buttons or the drag container has been destroyed or the ammount of the item in this slot has reached 0 then we exit the  loop and finish the mouse wheel control
            if (Input.GetButton("Cancel") 
                || Input.GetMouseButton(0) 
                || Input.GetMouseButton(1) 
                || _item == null 
                || GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED == null 
                || _ammount == 0)
            {
                break;
            }

            //Disable the cursor so we can clearly see the ammount of the items in both this slot and the drag container
            Cursor.visible = false;

            //Get the mouse wheel value then increase/decrease currentAmmount and decrease/increase the ammount of the item in the container based on the value of mouseWheelValue
            float mouseWheelValue = Input.GetAxis("Mouse ScrollWheel");
            if (mouseWheelValue > 0 && _ammount > 0 && GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount < 99)
            {
                _ammount--;
                GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount++;
            } else if(mouseWheelValue < 0 && _ammount < 99 && GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount > 0)
            {

                _ammount++;
                GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount--;
            }
            yield return null;
        }
        
        //Make the cursor visible again since we finished the controlling with the mouse wheel
        Cursor.visible = true;
        yield return null;
    }

    public bool IsFiltered()
    {
        return _filtered;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = SerializedGlobalVariables.instance.slotOnHoverColor;
        _hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = _originalColor;
        _hovered = false;
    }

    public void CopySlotProperties(Slot targetSlot)
    {
        _item = targetSlot._item;
        _ammount = targetSlot._ammount;
    }

    public void Nullify()
    {
        _item = null;
        _ammount = 0;
    }

    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
        }
    }

    public int currentAmmount
    {
        get { return _ammount; }
        set
        {
            _ammount = value;
        }
    }
} 
