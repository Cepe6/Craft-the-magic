using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ChunksController : MonoBehaviour {
    [SerializeField]
    private GameObject _chunkPrefab;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private int _fieldOfView = 3;

    private int _chunkSize = GlobalVariables.TILE_PER_CHUNK_AXIS * GlobalVariables.TILE_SIZE;

    List<GameObject> _generatedChunks = new List<GameObject>();

    GameObject chunkWrapper;
    
    public static bool _changed = false;
    
    // Use this for initialization
    void Start()
    {
        chunkWrapper = new GameObject("Chunk wrapper");

        Vector3 playerPosition = _player.transform.position;
        Vector2 coordinates = new Vector2((int)Mathf.Floor(playerPosition.x / _chunkSize) - playerPosition.x < 0 ? 1 : 0, (int)Mathf.Floor(playerPosition.z / _chunkSize) - playerPosition.z < 0 ? 1 : 0);
        GenerateChunksAround(coordinates);
    }

    //Generate the chunks around the player
    public void GenerateChunksAround(Vector2 coordinates)
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
        instance.GetComponent<Chunk>().InitializeChunk(new Vector2(coordinates.x, coordinates.y));
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

    public float IsWalkable(float x, float y)
    {
        Chunk containingChunk = GetChunkFromCoords(x, y).GetComponent<Chunk>();
        if (containingChunk.IsWalkable(Mathf.Abs((int)(x - containingChunk.GetCoordinates().x * _chunkSize) / 10),  Mathf.Abs((int)(y - containingChunk.GetCoordinates().y * _chunkSize)) / 10)) {
            return 1f;
        }
       
        return 0f;
    }

    public TileData GetTile(float x, float y)
    {
        Chunk containingChunk = GetChunkFromCoords(x, y).GetComponent<Chunk>();

        return containingChunk.GetTile(Mathf.Abs((int)(x - containingChunk.GetCoordinates().x * _chunkSize) / GlobalVariables.TILE_SIZE), Mathf.Abs((int)(y - containingChunk.GetCoordinates().y * _chunkSize)) / GlobalVariables.TILE_SIZE);
    }

    public GameObject GetChunkFromCoords(float x, float y)
    {
        Vector2 convertedCoords = new Vector2((int)(x / _chunkSize) - (x < 0 ? 1 : 0), (int)(y / _chunkSize) - (y < 0 ? 1 : 0));
        return _generatedChunks.Where(chunk => chunk.GetComponent<Chunk>().GetCoordinates().Equals(convertedCoords)).SingleOrDefault();
    }
}