using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    [SerializeField]
    private Material _uvTexture;
    private int _spriteSheetSize = 4;
    
    private int _seed;
    private int _tileCount;
    private int _tileSize;
    private float _scale;

    private int[,] _blocks;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;

    private float _chunkXCoord;
    private float _chunkYCoord;
    
    // Use this for initialization
    void Awake () {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = _meshFilter.mesh;
    }

    public void InitializeChunk(float x, float y, int tileCount, int tileSize, float scale, int seed)
    {
        _scale = scale;
        _tileCount = tileCount;
        _tileSize = tileSize;
        _chunkXCoord = x;
        _chunkYCoord = y;
        _seed = seed;
        _blocks = new int[_tileCount, _tileCount];
        GenerateChunk();
    }

    public void GenerateChunk()
    {
        for (int i = 0; i < _tileCount; i++)
        {
            for (int j = 0; j < _tileCount; j++)
            {
                _blocks[i, j] = 0;

                float perlinValue = GetPerlinNoiseValueAt(i, j);
               if(perlinValue > .3f)
                {
                    _blocks[i, j] = 7;
                } 
            }
        }
    }

    public void GenerateMesh()
    {
        List<Vector3> verticies = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        int blockCount = 0;

        for (int x = 0; x < _tileCount; x++)
        {
            for (int y = 0; y < _tileCount; y++)
            {
                int id = _blocks[x, y];

                //Create the verticies
                int xCoord = (x) * _tileSize;
                int yCoord = (y) * _tileSize;
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
	
    private float GetPerlinNoiseValueAt(int x, int y)
    {
        float xCoord = 32768 + _seed + ((float)x / _tileCount + _chunkXCoord) * _scale;
        float yCoord = 32768 + _seed  + ((float)y / _tileCount + _chunkYCoord) * _scale; 
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    public Vector2 GetCoordinates()
    {
        return new Vector2(_chunkXCoord, _chunkYCoord);
    }

	// Update is called once per frame
	void Update () {

    }
}
