using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour {
    [SerializeField]
    private Material _uvTexture;
    private int _spriteSheetSize = 4;
    
    private int _seed;
    private int _size;
    private float _scale;

    private int[,] _blocks;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;

    private int _chunkXCoord;
    private int _chunkYCoord;
    
    // Use this for initialization
    void Awake () {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = _meshFilter.mesh;
    }

    public void InitializeChunk(int x, int y, int chunkSize, float scale, int seed)
    {
        _scale = scale;
        _size = chunkSize;
        _chunkXCoord = x;
        _chunkYCoord = y;
        _seed = seed;
        _blocks = new int[_size, _size];
        GenerateChunk();
    }

    public void GenerateChunk()
    {
        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                float perlinValue = GetPerlinNoiseValueAt(i, j);
                if (perlinValue < .2f)
                {
                    _blocks[i, j] = 1;
                }
                else if (perlinValue > .2f)
                {
                    _blocks[i, j] = 2;
                }
            }
        }
    }

    public void GenerateMesh(int tileSize)
    {
        List<Vector3> verticies = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        int blockCount = 0;

        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                if (_blocks[x, y] != 0)
                {

                    int id = _blocks[x, y] - 1;

                    //Create the verticies
                    int xCoord = x * tileSize;
                    int yCoord = y * tileSize;
                    verticies.Add(new Vector3(xCoord, yCoord, 0));
                    verticies.Add(new Vector3(xCoord + tileSize, yCoord, 0));
                    verticies.Add(new Vector3(xCoord + tileSize, yCoord - tileSize, 0));
                    verticies.Add(new Vector3(xCoord, yCoord - tileSize, 0));

                    //Make a guideline for the building on the triangles
                    triangles.Add(0 + blockCount * 4);
                    triangles.Add(1 + blockCount * 4);
                    triangles.Add(3 + blockCount * 4);
                    triangles.Add(1 + blockCount * 4);
                    triangles.Add(2 + blockCount * 4);
                    triangles.Add(3 + blockCount * 4);


                    //Select the right texture for the tile based on the tile id
                    int left = (id % _spriteSheetSize);
                    int right = left + 1;
                    int top = _spriteSheetSize - (id / _spriteSheetSize);
                    int bot = top + 1;
                    uv.Add(new Vector2(left / (float)_spriteSheetSize, top / (float)_spriteSheetSize));
                    uv.Add(new Vector2(right / (float)_spriteSheetSize, top / (float)_spriteSheetSize));
                    uv.Add(new Vector2(right / (float)_spriteSheetSize, bot / (float)_spriteSheetSize));
                    uv.Add(new Vector2(left / (float)_spriteSheetSize, bot / (float)_spriteSheetSize));

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
	
    private float GetPerlinNoiseValueAt(int x, int y)
    {
        float xCoord = (float)x / _size * _scale + _chunkXCoord * _seed;
        float yCoord = (float)y / _size  * _scale + _chunkYCoord * _seed;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

	// Update is called once per frame
	void Update () {

    }
}
