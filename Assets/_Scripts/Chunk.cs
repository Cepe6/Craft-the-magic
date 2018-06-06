using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    //The material of the terrain containing the textures of the tiles
    [SerializeField]
    private Material _terrainMaterial;
    //The X by X dimention of the sprite sheet used for finding the right tile texture
    [SerializeField]
    private int _spriteSheetSize = 2;
    //Iron ore prefabs with different models
    [SerializeField]
    private List<GameObject> _ironOrePrefabs;
    [SerializeField]
    private List<GameObject> _coalPrefabs;
    private ChunksController _chunksController;

    private bool _visited = false;

    private float _seed;

    //The layer maps of the chunk
    TileData[,] _tilesDataMap = new TileData[GlobalVariables.TILE_PER_CHUNK_AXIS, GlobalVariables.TILE_PER_CHUNK_AXIS];
    bool[,] _notWalkableMap = new bool[GlobalVariables.TILE_PER_CHUNK_AXIS, GlobalVariables.TILE_PER_CHUNK_AXIS];
    
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;

    //Stores the chunk coordinates local to the list of generated chunks
    private Vector2 _chunkCoordinates;

    // Use this for initialization
    void Awake () {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = _meshFilter.mesh;

        _chunksController = GameObject.FindGameObjectWithTag("World Manager").GetComponent<ChunksController>();
    }

    public void InitializeChunk(Vector2 coordinates, float seed)
    {
        _chunkCoordinates = coordinates;
        _seed = seed;
        GenerateChunk();
    }

    public void GenerateChunk()
    {
        InitializeDataMap();
        GenerateTexture();
    }

    private void InitializeDataMap()
    {
        //Initialize different tile types maps
        TilesEnum[,] _biomesMap = PerlinNoiseGenerator.GenerateBiomesMap(_chunkCoordinates + new Vector2(_seed, _seed));
        TilesEnum[,] _waterMap = PerlinNoiseGenerator.GenerateWaterMap(_chunkCoordinates + new Vector2(_seed, _seed));
        bool[,] _ironOreMap = PerlinNoiseGenerator.GenerateIronOreMap(_chunkCoordinates + new Vector2(_seed, _seed));
        bool[,] _coalMap = PerlinNoiseGenerator.GenerateCoalMap(_chunkCoordinates + new Vector2(_seed, _seed));
      
        for (int x = 0; x < 64; x++)
        {
            for(int y = 0; y < 64; y++)
            {
                if (_waterMap[x, y] != 0) //If there is water on this tile
                {
                    _tilesDataMap[x, y] = new TileData(_waterMap[x, y]);
                    _notWalkableMap[x, y] = true;
                }
                else if (_ironOreMap[x, y]) //Else if there is iron on this tile
                {
                    _tilesDataMap[x, y] = new TileData(TilesEnum.IRON_ORE, NewResource(TilesEnum.IRON_ORE, x, y));
                }
                else if (_coalMap[x, y]) //Else if there is copper on this tile
                {
                    _tilesDataMap[x, y] = new TileData(TilesEnum.COAL, NewResource(TilesEnum.COAL, x, y));
                }
                else //Else get the biome map tile
                {
                    _tilesDataMap[x, y] = new TileData(_biomesMap[x, y]);
                }
            }
        }

        MapController.Instance.AddChunk(_chunkCoordinates.x, _chunkCoordinates.y, _tilesDataMap);
    }

    private GameObject NewResource(TilesEnum type, int x, int y)
    {
        float xCoord = transform.position.x + (x * GlobalVariables.TILE_SIZE + GlobalVariables.TILE_SIZE / 2);
        float yCoord = transform.position.z + (y * GlobalVariables.TILE_SIZE + GlobalVariables.TILE_SIZE / 2);
        int modelRotation = 90 * Random.Range(-4, 4);
        GameObject instance = null;
        switch(type)
        {
            case TilesEnum.IRON_ORE:
                instance = Instantiate(_ironOrePrefabs[Random.Range(0, _ironOrePrefabs.Count)], new Vector3(xCoord, -2f, yCoord), new Quaternion(0f, modelRotation, 0f, 0f));
                break;
            case TilesEnum.COAL:
                instance = Instantiate(_coalPrefabs[Random.Range(0, _coalPrefabs.Count)], new Vector3(xCoord, -2f, yCoord), new Quaternion(0f, modelRotation, 0f, 0f));
                break;
        }

        if (instance != null)
        {
            instance.transform.SetParent(transform.Find("Resources"));
        }
        return instance;
    }

    private void GenerateTexture()
    {
        
        List<Vector3> verticies = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        int blockCount = 0;
 
        for (int x = 0; x < GlobalVariables.TILE_PER_CHUNK_AXIS; x++)
        {
            for (int y = 0; y < GlobalVariables.TILE_PER_CHUNK_AXIS; y++)
            {
                int id = (int)_tilesDataMap[x, y].GetTileType();

                //Create the verticies
                float xCoord = (x) * GlobalVariables.TILE_SIZE;
                float yCoord = (y) * GlobalVariables.TILE_SIZE;

                verticies.Add(new Vector3(xCoord, yCoord + GlobalVariables.TILE_SIZE, 0));
                verticies.Add(new Vector3(xCoord + GlobalVariables.TILE_SIZE, yCoord + GlobalVariables.TILE_SIZE, 0));
                verticies.Add(new Vector3(xCoord + GlobalVariables.TILE_SIZE, yCoord, 0));
                verticies.Add(new Vector3(xCoord, yCoord, 0));

                //Make a guideline for the building on the triangles
                triangles.Add(0 + blockCount * 4);
                triangles.Add(1 + blockCount * 4);
                triangles.Add(3 + blockCount * 4);
                triangles.Add(1 + blockCount * 4);
                triangles.Add(2 + blockCount * 4);
                triangles.Add(3 + blockCount * 4);


                //Select the right texture for the tile based on the tile id
                float unit = 1f / _spriteSheetSize;

                float idToSpriteSheetSize = (float)id / _spriteSheetSize;
                float xUnit = idToSpriteSheetSize - (int)idToSpriteSheetSize;
                float yUnit = (int)idToSpriteSheetSize * unit;

                uv.Add(new Vector2(xUnit + 0.01f, yUnit + 0.01f));
                uv.Add(new Vector2(xUnit + unit - 0.01f, yUnit + 0.01f));
                uv.Add(new Vector2(xUnit + unit - 0.01f, yUnit + unit - 0.01f));
                uv.Add(new Vector2(xUnit + 0.01f, yUnit + unit - 0.01f));

                blockCount++;
            }
        }
        
        //Initialize mesh based on the tiles data
        _mesh.Clear();
        _mesh.vertices = verticies.ToArray();
        _mesh.triangles = triangles.ToArray();
        
        _mesh.uv = uv.ToArray();
        _mesh.RecalculateNormals();

        //Initialize mesh collider
        GetComponent<MeshCollider>().sharedMesh = _mesh;
        
        //Initialize tiles texures
        GetComponent<Renderer>().material = _terrainMaterial;
    }

    public Vector2 GetCoordinates()
    {
        return _chunkCoordinates;
    }

    public bool IsWalkable(int x, int y)
    {
        if(_notWalkableMap[x, y])
        {
            return false;
        }

        return true;
    }

    public TileData GetTile(int x, int y)
    {
        return _tilesDataMap[x, y];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_visited)
        {
            _chunksController.GenerateChunksAround(_chunkCoordinates);
        }

        _visited = true;
    }
}
