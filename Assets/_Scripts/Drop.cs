using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField]
    private Item _item;
    [SerializeField]
    private int _ammount;
    [SerializeField]
    private Material _dropMaterial;
    [SerializeField]
    private int _spriteSheetSize;

    private PlayerController _playerController;
    private PlayerInventoryController _inventoryController;
    
    void Awake()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _inventoryController = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventoryController>();

    }

    private void Update()
    {
        if (_ammount == 0)
        {
            _playerController.RemoveDrop(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void InitWithItem(Item item, int ammount) {
        _item = item;
        _ammount = ammount;
        BuildTexture();
    }

    private void BuildTexture()
    {

        List<Vector3> verticies = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        verticies.Add(new Vector3(-GlobalVariables.DROP_SIZE / 2, -GlobalVariables.DROP_SIZE / 2, 0));
        verticies.Add(new Vector3(GlobalVariables.DROP_SIZE / 2, -GlobalVariables.DROP_SIZE / 2, 0));
        verticies.Add(new Vector3(GlobalVariables.DROP_SIZE / 2, GlobalVariables.DROP_SIZE / 2, 0));
        verticies.Add(new Vector3(-GlobalVariables.DROP_SIZE / 2, GlobalVariables.DROP_SIZE / 2, 0));

        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(3);
        triangles.Add(1);
        triangles.Add(2);
        triangles.Add(3);

        float id = (float)_item.type;
        float unit = 1f / _spriteSheetSize;

        float idToSpriteSheetSize = id / _spriteSheetSize;
        float xUnit = idToSpriteSheetSize - (int)idToSpriteSheetSize;
        float yUnit = (int)idToSpriteSheetSize * unit;

        uv.Add(new Vector2(xUnit, yUnit));
        uv.Add(new Vector2(xUnit + unit, yUnit));
        uv.Add(new Vector2(xUnit + unit, yUnit + unit));
        uv.Add(new Vector2(xUnit, yUnit + unit));
        
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        mesh.Clear();
        mesh.vertices = verticies.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        meshRenderer.material = _dropMaterial;
        mesh.RecalculateNormals();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().AddDrop(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().RemoveDrop(this.gameObject);
        }
    }

    public void AddToInventory()
    {
        _ammount = _inventoryController.AddItemAndReturnRemainingAmmount(_item, _ammount);
    }

    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
        }
    }

    public int ammount
    {
        get { return _ammount; }
        set
        {
            _ammount = value;
        }
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
}
