using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour {
    private ResourcesEnum _type;
    private int _ammount;

    [SerializeField]
    private Material _onHoverMaterial;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Use this for initialization
    void Start () {
        InitializeAmmount();
	}

    private void InitializeAmmount()
    {
        _ammount = (int)((Random.Range(-50, 50) + WorldSettings.DEFAULT_RESOURCE_CAPACITY) + WorldSettings.INCREASE_RESOURCE_CAPACITY * (WorldSettings.DEFAULT_RESOURCE_CAPACITY / 4));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public ResourcesEnum type
    {
        get { return _type; }
        set
        {
            _type = value;
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

    private void OnMouseEnter()
    {
        Debug.Log("Mouse enter");
        List<Material> materials = new List<Material>();
        materials.Add(_meshRenderer.materials[0]);
        materials.Add(_onHoverMaterial);
        GetComponent<MeshRenderer>().materials = materials.ToArray();
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse exit");
        List<Material> materials = new List<Material>();
        materials.Add(_meshRenderer.materials[0]);
        GetComponent<MeshRenderer>().materials = materials.ToArray();
    }
}
