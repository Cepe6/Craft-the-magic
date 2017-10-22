using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour {
    [SerializeField]
    private Material _uvTexture;
    private int _spriteSheetSize = 4;

    [SerializeField]
    private int _seed;
    [SerializeField]
    private int _tileSize = 1;
    [SerializeField]
    private int _scale = 10;
    [SerializeField]
    private int _width = 64;
    [SerializeField]
    private int _height = 64;

    private int[,] _blocks;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;

    // Use this for initialization
    void Start () {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void GenerateChunk()
    {
        _blocks = new int[_width, _height];
        for(int i = 0; i < _width; i++)
        {
            for(int j = 0; j < _height; j++)
            {
                float perlinValue = GetPerlinNoiseValueAt(i, j);
                if (perlinValue < .3f)
                {
                    _blocks[i, j] = 1;
                } else if(perlinValue >.3f)
                {
                    _blocks[i, j] = 2;
                }
            } 
        }

        GenerateMesh();
    }

    private void GenerateMesh()
    {
        List<Vector3> verticies = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        int blockCount = 0;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_blocks[x, y] != 0)
                {

                    int id = _blocks[x, y] - 1;

                    //Create the verticies
                    int xCoord = x * _tileSize;
                    int yCoord = y * _tileSize;
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

        //Generate mesh collider
        GetComponent<MeshCollider>().sharedMesh = _mesh;
    }
	
    private float GetPerlinNoiseValueAt(int x, int y)
    {
        float xCoord = (float) x / _width * _scale + _seed;
        float yCoord = (float) y / _height * _scale + _seed;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

	// Update is called once per frame
	void Update () {
        if (_meshFilter != null && _mesh == null)
        {
            _mesh = _meshFilter.mesh;
            GenerateChunk();
            GetComponent<Renderer>().material = _uvTexture;
        }
    }
}
