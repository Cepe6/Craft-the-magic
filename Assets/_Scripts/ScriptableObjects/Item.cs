using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Item : ScriptableObject
{
    [SerializeField]
    private ItemsEnum _type;
    [SerializeField]
    private Sprite _itemIcon;
    [SerializeField]
    private bool _isStackable;


    public ItemsEnum type {
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
}
