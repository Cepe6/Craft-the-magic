using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Item : ScriptableObject
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private ItemTypesEnum _type;
    [SerializeField]
    private Sprite _itemIcon;
    [SerializeField]
    private bool _isStackable;
    [SerializeField]
    private Item _interactionResult;

    [SerializeField]
    private GameObject _placableGO;

    public int id
    {
        get { return _id; }
        set { _id = value; }
    }

    public ItemTypesEnum type {
        get { return _type; }
        set { _type = value;  }
    }
    public Sprite itemIcon
    {
        get { return _itemIcon; }
        set { _itemIcon = value; }
    }
    public bool isStackable
    {
        get { return _isStackable; }
        set { _isStackable = value; }
    }    
    public Item interactionResult
    {
        get { return _interactionResult; }
        set { _interactionResult = value; }
    }

    public GameObject placableGO
    {
        get { return _placableGO; }
    }
}
