using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
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

    private int _chunkSize;

    List<GameObject> _generatedChunks = new List<GameObject>();
    Vector2[,] _activeChunks;

	// Use this for initialization
	void Awake () {
        _chunkSize = _tileSize * _chunkTileSize;
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = _player.transform.position;
        Vector2 currentChunkPosition = new Vector2(Mathf.Floor(playerPosition.x / _chunkSize), Mathf.Floor(playerPosition.z / _chunkSize));
        GenerateChunksAround((int)currentChunkPosition.x, (int)currentChunkPosition.y);
    }


    //Generate the chunk and initialize its mesh
    private void GenerateChunkAt(int x, int y)
    {
        GameObject instance =  Instantiate(_chunkPrefab);
        instance.transform.position = new Vector3(x * _chunkSize, 0f, y * _chunkSize);
        instance.name = "Chunk " + new Vector2(x, y);
        instance.GetComponent<ChunkController>().InitializeChunk(x * _chunkSize, y * _chunkSize, _chunkTileSize, _biomesScale, _seed);
        instance.GetComponent<ChunkController>().GenerateMesh(_tileSize);
        _generatedChunks.Add(instance);
    }


    //Generate the chunks around the player
    private void GenerateChunksAround(int x, int y)
    {
        List<Vector2> notGeneratedChunks = GetListOfNotGeneratedChunksAround(x, y);
        for(int i = 0; i < notGeneratedChunks.Count; i++)
        {
            GenerateChunkAt((int)notGeneratedChunks[i].x, (int)notGeneratedChunks[i].y);
        }
    }
 	
	private List<Vector2> GetListOfNotGeneratedChunksAround(int x, int y)
    {
        List<Vector2> chunks = new List<Vector2>();
        Vector2[,] nearChunkCoordinates = new Vector2[5, 5] {{new Vector2(x-2,y+2), new Vector2(x-1,y+2), new Vector2(x,y+2), new Vector2(x+1,y+2), new Vector2(x+2,y+2)},
                                                             {new Vector2(x-2,y+1), new Vector2(x-1,y+1), new Vector2(x,y+1), new Vector2(x+1,y+1), new Vector2(x+2,y+1)},
                                                             {new Vector2(x-2,  y), new Vector2(x-1,  y), new Vector2(x,  y), new Vector2(x+1,  y), new Vector2(x+2,  y)},
                                                             {new Vector2(x-2,y-1), new Vector2(x-1,y-1), new Vector2(x,y-1), new Vector2(x+1,y-1), new Vector2(x+2,y-1)},
                                                             {new Vector2(x-2,y-2), new Vector2(x-1,y-2), new Vector2(x,y-2), new Vector2(x+1,y-2), new Vector2(x+2,y-2)}};

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++) { 
                if (_generatedChunks.Where(chunk => chunk.name == "Chunk " + nearChunkCoordinates[i, j]).SingleOrDefault() == null)
                    chunks.Add(nearChunkCoordinates[i, j]);
            }
        }

        return chunks;
    }
}