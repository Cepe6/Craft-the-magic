using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ChunksController : MonoBehaviour {
    [SerializeField]
    private GameObject _chunkPrefab;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private int _chunkTileSize = 64;
    [SerializeField]
    private int _tileSize = 10;
    [SerializeField]
    private float _biomesScale = 20;
    [SerializeField]
    private int _seed;
    [SerializeField]
    private int _fieldOfView = 3;

    private int _chunkSize;

    List<GameObject> _generatedChunks = new List<GameObject>();
    bool _enabledStartingArea = false;

    Vector2 _movementDisableCoordinates;
    Vector2 _lastFrameCoordinates;

    GameObject chunkWrapper;
 
    // Use this for initialization
    void Awake()
    {
        _chunkSize = _tileSize * _chunkTileSize;
        chunkWrapper = new GameObject("Chunk wrapper");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = _player.transform.position;
        Vector2 coordinates = new Vector2((int)Mathf.Floor(playerPosition.x / _chunkSize), (int)Mathf.Floor(playerPosition.z / _chunkSize));
        GenerateChunksAround(coordinates);
        Vector2 playerMovement;
        if (!_enabledStartingArea) { 
            EnableChunksAround(coordinates);
            _enabledStartingArea = true;
        } else if((playerMovement = coordinates - _lastFrameCoordinates) != new Vector2(0, 0)) {
            EnableChunksAccordingToPlayerMovement(coordinates, playerMovement);
        }


        _lastFrameCoordinates = coordinates;
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
        instance.name = "Chunk " + new Vector2(coordinates.x, coordinates.y);
        instance.transform.parent = chunkWrapper.transform;
        instance.GetComponent<Chunk>().InitializeChunk(coordinates.x, coordinates.y, _chunkTileSize, _tileSize, _biomesScale, _seed);
        instance.GetComponent<Chunk>().GenerateMesh();
        _generatedChunks.Add(instance);
        instance.SetActive(false);
    }

    //Get list of not generated chunks around the coordinates point with range _fieldOfView + 2
    private List<Vector2> GetListOfNotGeneratedChunksAround(Vector2 coordinates)
    {
        List<Vector2> chunks = new List<Vector2>();
        int generationRadius = _fieldOfView + 2;

        int lowestXOffset = -generationRadius / 2;
        int highestYOffset = generationRadius / 2;

        for (int i = 0; i < generationRadius; i++)
        { 
            for (int j = 0; j < generationRadius; j++)
            { 
                Vector2 currentChunkCoords = new Vector2(coordinates.x + lowestXOffset + i, coordinates.y + highestYOffset - j);
                if (_generatedChunks.Where(chunk => chunk.GetComponent<Chunk>().GetCoordinates() == currentChunkCoords).SingleOrDefault() == null)
                    chunks.Add(currentChunkCoords);
            }
        }

        return chunks;
    }

    //Enable chunks around the coordinates
    private void EnableChunksAround(Vector2 coordinates)
    {
        int lowestXOffset = -_fieldOfView / 2;
        int highestYOffset = _fieldOfView / 2;
        for (int i = 0; i < _fieldOfView; i++)
        {
            for(int j = 0; j < _fieldOfView; j++)
            {
                Vector2 currentChunkCoords = new Vector2(coordinates.x + lowestXOffset + i, coordinates.y + highestYOffset - j);
                GameObject chunkGO = _generatedChunks.Where(chunk => chunk.GetComponent<Chunk>().GetCoordinates() == currentChunkCoords).SingleOrDefault();
                if(!chunkGO.activeSelf)
                {
                    chunkGO.SetActive(true);
                }
            }
        }
    }

    //Enable new chunks in the player position and disable the farest in the opposite direction
    private void EnableChunksAccordingToPlayerMovement(Vector2 newPosition, Vector2 movement)
    {
        if(movement.x != 0)
        {
            float newXCoordinate = newPosition.x + movement.x;
            float farestOpositeXCoordinate = newPosition.x - movement.x * (_fieldOfView - 1);
            int highestYOffset = _fieldOfView / 2;
            for(int y = 0; y < _fieldOfView; y++)
            {
                GameObject chunkToEnableGO = _generatedChunks.Where(chunk => chunk.GetComponent<Chunk>().GetCoordinates() == new Vector2(newXCoordinate, newPosition.y + highestYOffset - y)).SingleOrDefault();
                if (!chunkToEnableGO.activeSelf)
                    chunkToEnableGO.SetActive(true);

                GameObject chunkToDisableGO = _generatedChunks.Where(chunk => chunk.GetComponent<Chunk>().GetCoordinates() == new Vector2(farestOpositeXCoordinate, newPosition.y + highestYOffset - y)).SingleOrDefault();
                if (chunkToDisableGO.activeSelf)
                    chunkToDisableGO.SetActive(false);
            }
        }

        if (movement.y != 0) {
            float newYCoordinate = newPosition.y + movement.y;
            float farestOpositeYCoordinate = newPosition.y - movement.y * (_fieldOfView - 1);
            int lowestXOffset = -_fieldOfView / 2;
            for (int x = 0; x < _fieldOfView; x++)
            {
                GameObject chunkToEnableGO = _generatedChunks.Where(chunk => chunk.GetComponent<Chunk>().GetCoordinates() == new Vector2(x + lowestXOffset + newPosition.x, newYCoordinate)).SingleOrDefault();
                if (!chunkToEnableGO.activeSelf)
                    chunkToEnableGO.SetActive(true);

                GameObject chunkToDisableGO = _generatedChunks.Where(chunk => chunk.GetComponent<Chunk>().GetCoordinates() == new Vector2(x + lowestXOffset + newPosition.x, farestOpositeYCoordinate)).SingleOrDefault();
                if (chunkToDisableGO.activeSelf)
                    chunkToDisableGO.SetActive(false);
            }
        }
    }
}