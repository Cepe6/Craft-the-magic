using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    //REFACTOR
    [SerializeField]
    private Material _uvTexture;
    [SerializeField]
    private int _spriteSheetSize = 2;
    [SerializeField]
    private GameObject _colliderBlock;

    private int _tileCount;
    private float _tileSize;
    private float _chunkSize;

    private int _seed;

    //This variable contains the data sets as ints for all the layer masks of a chunk
    //0 -> Textures layer (used for storing the IDs of the tiles and later used to get their texture in the sprite sheet
    //1 -> Biomes layer (used for storing the IDs of the biomes on tiles - for example tile with ID=1 has grassland biome on it)
    //2 -> Iron Ore layer (used for storing 0 or 1 (0 for no iron ore on that tile and 1 for present iron ore on that tile)
    //3 -> Copper Ore layer (as iron ore layer)
    //4 -> Coal layer (as #2 and #3)
    //5 -> Water layer (0 for no water on that tile and 1 for present water on that tile)
    private Dictionary<int, int[,]> _tilesMap;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;

    private float _chunkXCoord;
    private float _chunkYCoord;

    private PerlinNoiseGenerator _perlinNoiseGenerator;

    // Use this for initialization
    void Awake () {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = _meshFilter.mesh;

        _tilesMap = new Dictionary<int, int[,]>();

        _perlinNoiseGenerator = new PerlinNoiseGenerator();
    }

    public void InitializeChunk(Vector2 coordinates, float tileSize, int tileCount, int seed)
    {
        _tileCount = tileCount;
        _tileSize = tileSize;
        _chunkXCoord = coordinates.x;
        _chunkYCoord = coordinates.y;
        _seed = seed;
        GenerateChunk();
    }

    public void GenerateChunk()
    {
        InitializeBiomes();
        InitializeResources();
        InitializeWater();

        CreateTextureLayer();
        GenerateTextureTiles();
    }

    private void InitializeBiomes()
    {
        _tilesMap.Add(1, _perlinNoiseGenerator.GenerateBiomesMap(_chunkXCoord * _tileCount, _chunkYCoord * _tileCount));
    }

    private void InitializeResources()
    {
        _tilesMap.Add(2, _perlinNoiseGenerator.GenerateIronOreMap(_chunkXCoord * _tileCount, _chunkYCoord * _tileCount));
        _tilesMap.Add(3, _perlinNoiseGenerator.GenerateCopperOreMap(_chunkXCoord * _tileCount, _chunkYCoord * _tileCount));
        _tilesMap.Add(4, _perlinNoiseGenerator.GenerateCoalMap(_chunkXCoord * _tileCount, _chunkYCoord * _tileCount));
    }

    private void InitializeWater()
    {
        int[,] tempWaterMap = _perlinNoiseGenerator.GenerateWaterMap(_chunkXCoord * _tileCount, _chunkYCoord * _tileCount);
        int[,] waterMap = new int[64, 64];
        for(int x = 1; x < 65; x++)
        {
            for(int y = 1; y < 65; y++)
            {
                waterMap[x - 1, y - 1] = tempWaterMap[x, y];
                if (waterMap[x - 1, y - 1] != 0)
                {
                    if (IsGroundAround(x, y, tempWaterMap))
                    {
                        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                        boxCollider.size = new Vector3(_tileSize, _tileSize, 20);
                        boxCollider.center = new Vector3(((x - 1) * _tileSize + _chunkXCoord * _chunkSize) + _tileSize / 2, ((y - 1) * _tileSize + _chunkYCoord * _chunkSize) - _tileSize/ 2,  - _tileSize / 2);
                    }
                }
            }
        }


        _tilesMap.Add(5, waterMap);
    }

    private bool IsGroundAround(int x, int y, int[,] waterMap)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (waterMap[x + i, y + j] == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void CreateTextureLayer()
    {
        int[,] texturesMap = new int[64, 64];

        int[,] waterMap;
        int[,] coalMap;
        int[,] copperMap;
        int[,] ironMap;
        int[,] biomesMap;
        _tilesMap.TryGetValue(5, out waterMap);
        _tilesMap.TryGetValue(4, out coalMap);
        _tilesMap.TryGetValue(3, out copperMap);
        _tilesMap.TryGetValue(2, out ironMap);
        _tilesMap.TryGetValue(1, out biomesMap);

        for (int x = 0; x < 64; x++)
        {
            for(int y = 0; y < 64; y++)
            {
                if(waterMap[x, y] != 0) //If there is water on this tile
                {
                    texturesMap[x, y] = 2 + (waterMap[x, y] - 1) * _spriteSheetSize; 
               // } else if(coalMap[x, y] != 0) //Else if there is coal on this tile
               // {
               //     texturesMap[x, y] = 7;
              //  } else if(copperMap[x, y] != 0) //Else if there is copper on this tile
              //  {
              //      texturesMap[x, y] = 7;
               // } else if(ironMap[x, y] != 0) //Else if there is iron on this tile
               // {
               //     texturesMap[x, y] = 3;
                } else //Else get the biome map tile
                {
                    texturesMap[x, y] = biomesMap[x, y];
                }
            }
        }

        _tilesMap.Add(0, texturesMap);
    }

    private void GenerateTextureTiles()
    {
        
        List<Vector3> verticies = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        int blockCount = 0;

        int[,] texturesMap;
        bool getTexturesMap = _tilesMap.TryGetValue(0, out texturesMap);
        if (getTexturesMap)
        {
            for (int x = 0; x < _tileCount; x++)
            {
                for (int y = 0; y < _tileCount; y++)
                {
                    int id = texturesMap[x, y];

                    //Create the verticies
                    float xCoord = (x) * _tileSize;
                    float yCoord = (y) * _tileSize;
                    verticies.Add(new Vector3(xCoord, yCoord, 0));
                    verticies.Add(new Vector3(xCoord + _tileSize, yCoord, 0));
                    verticies.Add(new Vector3(xCoord + _tileSize, yCoord - _tileSize, 0));
                    verticies.Add(new Vector3(xCoord, yCoord - _tileSize, 0));

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

                    uv.Add(new Vector2(xUnit, yUnit));
                    uv.Add(new Vector2(xUnit + unit, yUnit));
                    uv.Add(new Vector2(xUnit + unit, yUnit + unit));
                    uv.Add(new Vector2(xUnit, yUnit + unit));

                    blockCount++;
                }
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
        GetComponent<Renderer>().material = _uvTexture;
    }
    
    public Vector2 GetCoordinates()
    {
        return new Vector2(_chunkXCoord, _chunkYCoord);
    }
}
