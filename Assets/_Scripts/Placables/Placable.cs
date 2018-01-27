using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class Placable : MonoBehaviour {
    [SerializeField]
    private int sizeX;
    [SerializeField]
    private int sizeY;
    [SerializeField]
    private TilesEnum[] filteredTiles;
    //Bool variable used to specify whether we use the tiles in the filteredTiles array as only the ones that you cannot build on or only the ones you can build ok
    [SerializeField]
    private bool _onlyThese = false;

    private ChunksController _chunksController;
    private TileData[,] _tilesUnder;

    private GameObject _model;
    private bool _currentlyPlacable;
    private GameObject[,] _placableIndicators;
    private Material _placableIndicatorsMaterial;
    private Color _placableAllowIndicatorsColor = new Color(0f, 1f, 0f, 0.2f);
    private Color _placableNotAllowedIndicatorsColor = new Color(1f, 0f, 0f, 0.2f);

    private BoxCollider _collider;
    private List<GameObject> _nonResourceCollidingGOs = new List<GameObject> ();
    

    private void Start()
    {
        _chunksController = GameObject.FindGameObjectWithTag("World Manager").GetComponent<ChunksController>();
        _placableIndicatorsMaterial = new Material(Shader.Find("Transparent/Diffuse"));

        _collider = gameObject.AddComponent<BoxCollider>();
        _collider.size = new Vector3((sizeX * GlobalVariables.TILE_SIZE) - 0.1f, 10f, (sizeY * GlobalVariables.TILE_SIZE) - 0.1f);
        _collider.center = new Vector3((float)GlobalVariables.TILE_SIZE * (sizeX) / 2, 0f, (float)GlobalVariables.TILE_SIZE * (sizeY) / 2);
        _collider.isTrigger = true;

        _model = transform.Find("Model").gameObject;
        _model.transform.localPosition = new Vector3((float)GlobalVariables.TILE_SIZE * (sizeX) / 2, 0f, (float)GlobalVariables.TILE_SIZE * (sizeY) / 2);
        _model.GetComponent<MeshRenderer>().materials = new Material[] { _model.GetComponent<MeshRenderer>().material, _placableIndicatorsMaterial };

        _tilesUnder = new TileData[sizeX, sizeY];
        _placableIndicators = new GameObject[sizeX, sizeY];
        int i = 0;
        for (int x = 0; x <= sizeX && i < sizeX; x++, i++)
        {
            int j = 0;
            for(int y = 0; y <= sizeY && j < sizeY; y++, j++)
            {
                _placableIndicators[i, j] = GameObject.CreatePrimitive(PrimitiveType.Quad);
                _placableIndicators[i, j].transform.SetParent(gameObject.transform);
                _placableIndicators[i, j].transform.localScale = new Vector3(GlobalVariables.TILE_SIZE, GlobalVariables.TILE_SIZE, 1f);
                _placableIndicators[i, j].transform.localPosition = new Vector3(x * GlobalVariables.TILE_SIZE + GlobalVariables.TILE_SIZE / 2, 0.1f, y * GlobalVariables.TILE_SIZE + GlobalVariables.TILE_SIZE / 2);
                _placableIndicators[i, j].transform.localRotation = Quaternion.LookRotation(Vector3.down);
                _placableIndicators[i, j].GetComponent<MeshRenderer>().material = _placableIndicatorsMaterial;
            }
        }
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            HideGO();
        }
        else
        {
            ShowGO();
        }

        Vector3 mousePoisition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3((float)GlobalVariables.TILE_SIZE * (sizeX - 1) / 2, 0f, (float)GlobalVariables.TILE_SIZE * (sizeY - 1) / 2);
        transform.position = new Vector3(ClosestTen(mousePoisition.x), 0.1f, ClosestTen(mousePoisition.z));

        UpdateIndicators();
    }

    private void OnMouseDown()
    {
        if (_currentlyPlacable && !EventSystem.current.IsPointerOverGameObject())
        {
            _model.GetComponent<MeshRenderer>().materials = new Material[] { _model.GetComponent<MeshRenderer>().materials[0] };
            _collider.isTrigger = false;
            _collider.size -= new Vector3(0f, .1f, 0f);

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    _tilesUnder[i, j] = _chunksController.GetTile(_placableIndicators[i, j].transform.position.x, _placableIndicators[i, j].transform.position.z);
                    Destroy(_placableIndicators[i, j]);
                }
            }

            foreach (MonoBehaviour script in GetComponents<MonoBehaviour>())
            {
                script.enabled = true;
            }

            GlobalVariables.CURRENT_PLACABLE = null;


            if (GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED != null)
            {
                GlobalVariables.ITEM_CONTAINER_BEING_DRAGGED.currentAmmount--;
            }
            else
            {
                HotBarController.Instance.GetCurrentSlot().currentAmmount--;
            }

            Destroy(this);
        }
    }

    private void UpdateIndicators()
    {
        _currentlyPlacable = true;
        for(int x = 0; x < sizeX; x++)
        {
            for(int y = 0; y < sizeY; y++) {
                if((filteredTiles.Where(tile => tile.Equals(_chunksController.GetTile(_placableIndicators[x, y].transform.position.x, _placableIndicators[x, y].transform.position.z).GetTileType())).SingleOrDefault() != 0) == _onlyThese && _nonResourceCollidingGOs.Count == 0)
                {
                    _placableIndicators[x, y].GetComponent<MeshRenderer>().material.color = _placableAllowIndicatorsColor;
                    _model.GetComponent<MeshRenderer>().materials[1].color = _placableAllowIndicatorsColor;
                } else
                {
                    _currentlyPlacable = false;
                    _placableIndicators[x, y].GetComponent<MeshRenderer>().material.color = _placableNotAllowedIndicatorsColor;
                    _model.GetComponent<MeshRenderer>().materials[1].color = _placableNotAllowedIndicatorsColor;
                }
            }
        }
    }

    private int ClosestTen(float number)
    {
        int result = (int)number / 10;
        return result * 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Rigidbody> () != null)
        {
            _nonResourceCollidingGOs.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            _nonResourceCollidingGOs.Remove(other.gameObject);
        }
    }

    public TileData[,] GetTilesUnder()
    {
        return _tilesUnder;
    }

    public Vector2 GetSize()
    {
        return new Vector2(sizeX, sizeY);
    }

    private void OnDisable()
    {
        if(GlobalVariables.CURRENT_PLACABLE == this.gameObject)
        {
            GlobalVariables.CURRENT_PLACABLE = null;
        }
    }

    private void HideGO()
    {
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }
    }

    private void ShowGO()
    {
        foreach(MeshRenderer renderer in GetComponentsInChildren<MeshRenderer> ())
        {
            renderer.enabled = true;
        }
    }
}
