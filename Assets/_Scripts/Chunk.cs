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
    private float[] _scales;

    private int[,,] _blocks;

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

        _perlinNoiseGenerator = new PerlinNoiseGenerator();
    }

    public void InitializeChunk(Vector2 coordinates, float tileSize, int tileCount, int seed)
    {
        _tileCount = tileCount;
        _tileSize = tileSize;
        _chunkXCoord = coordinates.x;
        _chunkYCoord = coordinates.y;
        _seed = seed;
        _blocks = new int[3, _tileCount, _tileCount];
        GenerateChunk();
    }

    public void GenerateChunk()
    {
        InitializeBiomes();
        InitializeResources();
        InitializeWater();
        InitializeTrees();

        GenerateWater();
        GenerateTrees();
        GenerateTiles();
    }

    private void InitializeBiomes()
    {
        for (int i = 0; i < _tileCount; i++)
        {
            for (int j = 0; j < _tileCount; j++)
            {
                _blocks[0, i, j] = 0; //Default biome is grassland with tile texture id = 0

                float perlinValue = PerlinValue(i, j, 0);
                if (perlinValue < .90f)
                {
                    _blocks[0, i, j] = 1; //Change textures layer tile value with savannah texture id = 1
                }
            }
        }
    }

    private void InitializeResources()
    {
        for (int i = 0; i < _tileCount; i++)
        {
            for (int j = 0; j < _tileCount; j++)
            {
                _blocks[1, i, j] = 0;

                float perlinValue = PerlinValue(i, j, 1);
                if (perlinValue < .70f)
                {
                    _blocks[2, i, j] = 1; //Change resource layer tile value
                    _blocks[0, i, j] = 3; //Change texture layer tile value
                }
            }
        }
    }

    private void InitializeWater()
    {
        for (int i = 0; i < _tileCount; i++)
        {
            for (int j = 0; j < _tileCount; j++)
            {
                _blocks[1, i, j] = 0;
                float perlinValue = PerlinValue(i, j, 2);
                if(perlinValue < .70f)
                {
                    _blocks[2, i, j] = 0; //Nullify all resources on current tile
                    _blocks[1, i, j] = 1; //Change water layer tile value
                }

                if (perlinValue < .60f)
                {
                    _blocks[0, i, j] = 10; //Change textures layer tile value
                }
                else if (perlinValue < .65f)
                {
                    _blocks[0, i, j] = 6; //Change textures layer tile value
                }
                else if (perlinValue < .70f)
                {
                    _blocks[0, i, j] = 2; //Change textures layer tile value
                }
            }
        }
    }

    private void GenerateWater()
    {
        for (int i = 0; i < _tileCount; i++)
        {
            for (int j = 0; j < _tileCount; j++)
            {
                if (_blocks[1, i, j] == 1)
                {
                    if (IsGroundAround(i, j))
                    {
                        Instantiate(_colliderBlock, new Vector3(i * _tileSize + transform.position.x, 0f, (j - 1) * _tileSize + transform.position.z), Quaternion.identity, transform);
                    }
                }
            }
        }
    }

    private void GenerateTiles()
    {
        List<Vector3> verticies = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        int blockCount = 0;

        for (int x = 0; x < _tileCount; x++)
        {
            for (int y = 0; y < _tileCount; y++)
            {
                int id = _blocks[0, x, y];

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

    private bool IsGroundAround(int x, int y)
    {
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if(PerlinValue(x + i, y + j, 2) >= .70f)
                {
                    return true;
                } 
            }
        }

        return false;
    }
   
    private void InitializeTrees()
    {

    }
    private void GenerateTrees()
    {

    }

    //Generate perlin value for x, y of layer (0 = Biome; 1 = Resources; 2 = Water; 3 = Tree)
    private float PerlinValue(int x, int y, int layer)
    {
        int offset;
        switch(layer)
        {
            case 0: offset = Constants.BIOMES_OFFSET;
                _perlinNoiseGenerator.SetFrequency(500f);
                _perlinNoiseGenerator.SetOctaves(8);
                break;
            case 1:
                offset = Constants.RESOURCES_OFFSET;
                _perlinNoiseGenerator.SetFrequency(150f);
                _perlinNoiseGenerator.SetOctaves(6);
                break;
            case 2:
                 offset = Constants.WATER_OFFSET;
                _perlinNoiseGenerator.SetFrequency(250f);
                _perlinNoiseGenerator.SetOctaves(6);
                break;
            default:
                offset = Constants.TREES_OFFSET;
                break;
        }
        
        float xCoord = offset + (x + _chunkXCoord * _tileCount);
        float yCoord = offset + (y + _chunkYCoord * _tileCount);
        return _perlinNoiseGenerator.GetPerlinNoiseValueAt(xCoord, yCoord);
    }

    public Vector2 GetCoordinates()
    {
        return new Vector2(_chunkXCoord, _chunkYCoord);
    }
    
    public int[,,] GetTilesMap()
    {
        return _blocks;
    }
	// Update is called once per frame
	void Update () {

    }
}
