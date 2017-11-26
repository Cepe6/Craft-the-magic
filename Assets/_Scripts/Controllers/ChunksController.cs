using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ChunksController : MonoBehaviour {
    [SerializeField]
    private GameObject _chunkPrefab;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private int _chunkTileCount = 64;
    [SerializeField]
    private int _tileSize = 10;
    [SerializeField]
    private int _seed;
    [SerializeField]
    private int _fieldOfView = 3;
    [SerializeField]
    private float[] _scales;

    private int _chunkSize;

    List<GameObject> _generatedChunks = new List<GameObject>();

    GameObject chunkWrapper;
 
    // Use this for initialization
    void Awake()
    {
        _chunkSize = _tileSize * _chunkTileCount;
        chunkWrapper = new GameObject("Chunk wrapper");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = _player.transform.position;
        Vector2 coordinates = new Vector2((int)Mathf.Floor(playerPosition.x / _chunkSize), (int)Mathf.Floor(playerPosition.z / _chunkSize));
        GenerateChunksAround(coordinates);
    }

    //Generate the chunks around the player
    private void GenerateChunksAround(Vector2 coordinates)
    {
        List<Vector2> notGeneratedChunks = GetListOfNotGeneratedChunksAround(coordinates);
        for (int i = 0; i < notGeneratedChunks.Count; i++)
        {
            InstantiateChunkAt(notGeneratedChunks[i]);
        }
    }

    //Generate the chunk and initialize its mesh
    private void InstantiateChunkAt(Vector2 coordinates)
    {
        GameObject instance = Instantiate(_chunkPrefab);
        instance.transform.position = new Vector3(coordinates.x * _chunkSize, 0f, coordinates.y * _chunkSize);
        instance.name = "Chunk " + coordinates;
        instance.transform.parent = chunkWrapper.transform;
        instance.GetComponent<Chunk>().InitializeChunk(coordinates, _tileSize, _chunkTileCount, _seed);
        _generatedChunks.Add(instance);
    }

    //Get list of not generated chunks around the coordinates point with range _fieldOfView
    private List<Vector2> GetListOfNotGeneratedChunksAround(Vector2 coordinates)
    {
        List<Vector2> chunks = new List<Vector2>();

        int lowestXOffset = -_fieldOfView / 2;
        int highestYOffset = _fieldOfView / 2;

        for (int i = 0; i < _fieldOfView; i++)
        { 
            for (int j = 0; j < _fieldOfView; j++)
            { 
                Vector2 currentChunkCoords = new Vector2(coordinates.x + lowestXOffset + i, coordinates.y + highestYOffset - j);
                if (_generatedChunks.Where(chunk => chunk.GetComponent<Chunk>().GetCoordinates() == currentChunkCoords).SingleOrDefault() == null)
                    chunks.Add(currentChunkCoords);
            }
        }

        return chunks;
    }
}