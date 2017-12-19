using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    //REFACTOR
    [SerializeField]
    private Material _uvTexture;
    [SerializeField]
    private int _spriteSheetSize = 2;
    private ChunksController _chunksController;

    private bool _visited = false;
    

    private int _seed;

    //The layer maps of the chunk
    int[,] _texturesMap = new int[64, 64];
    int[,] _biomesMap;
    int[,] _waterMap;
    int[,] _ironOreMap;
    int[,] _copperOreMap;
    int[,] _coalMap;
    
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

    public void InitializeChunk(Vector2 coordinates)
    {
        _chunkCoordinates = coordinates;
        GenerateChunk();
    }

    public void GenerateChunk()
    {
        InitializeMaps();

        InitializeTextureMap();
        GenerateTextureTiles();
    }

    private void InitializeMaps()
    {
        _biomesMap = PerlinNoiseGenerator.GenerateBiomesMap(_chunkCoordinates);
        _waterMap = PerlinNoiseGenerator.GenerateWaterMap(_chunkCoordinates);
        _ironOreMap = PerlinNoiseGenerator.GenerateIronOreMap(_chunkCoordinates);
        _copperOreMap = PerlinNoiseGenerator.GenerateCopperOreMap(_chunkCoordinates);
        _coalMap = PerlinNoiseGenerator.GenerateCoalMap(_chunkCoordinates); 
    }

    private void InitializeTextureMap()
    {
        for (int x = 0; x < 64; x++)
        {
            for(int y = 0; y < 64; y++)
            {
                if(_waterMap[x, y] != 0) //If there is water on this tile
                {
                    _texturesMap[x, y] = 2 + (_waterMap[x, y] - 1) * _spriteSheetSize; 
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
                    _texturesMap[x, y] = _biomesMap[x, y];
                }
            }
        }
    }

    private void GenerateTextureTiles()
    {
        
        List<Vector3> verticies = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        int blockCount = 0;
 
        for (int x = 0; x < Constants.TILE_PER_CHUNK_AXIS; x++)
        {
            for (int y = 0; y < Constants.TILE_PER_CHUNK_AXIS; y++)
            {
                int id = _texturesMap[x, y];

                //Create the verticies
                float xCoord = (x) * Constants.TILE_SIZE;
                float yCoord = (y) * Constants.TILE_SIZE;

                verticies.Add(new Vector3(xCoord, yCoord + Constants.TILE_SIZE, 0));
                verticies.Add(new Vector3(xCoord + Constants.TILE_SIZE, yCoord + Constants.TILE_SIZE, 0));
                verticies.Add(new Vector3(xCoord + Constants.TILE_SIZE, yCoord, 0));
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

    public Vector2 GetCoordinates()
    {
        return _chunkCoordinates;
    }

    public bool IsWalkable(int x, int y)
    {
        if(_waterMap[x, y] != 0)
        {
            return false;
        }

        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + _chunkCoordinates);
        if (!_visited)
        {
            _chunksController.GenerateChunksAround(_chunkCoordinates);
        }

        _visited = true;
    }
}
