using System.Collections;
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
    private int _biomesScale = 20;
    [SerializeField]
    private int _seed;

    private int _chunkSize;

    List<GameObject> _generatedChunks;
    GameObject[] _visibleChunks;

	// Use this for initialization
	void Awake () {
        _chunkSize = _tileSize * _chunkTileSize;
        GenerateChunkAt(0, 0);
        GenerateChunkAt(0, 1);
        GenerateChunkAt(1, 0);
        GenerateChunkAt(1, 1);
	}

    private void GenerateChunkAt(int x, int y)
    {
        GameObject instance =  Instantiate(_chunkPrefab);
        instance.transform.position = new Vector3(x * _chunkSize, 0f, y * _chunkSize);
        instance.GetComponent<ChunkController>().GenerateChunk(x * _chunkSize, y * _chunkSize, _chunkTileSize, _biomesScale, _seed);
        instance.GetComponent<ChunkController>().GenerateMesh(_tileSize);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPosition = _player.transform.position;
        Vector2 currentChunkPosition = new Vector2(playerPosition.x / _chunkSize, playerPosition.z / _chunkSize);
        Debug.Log("player: " + playerPosition + "chunk: " + Mathf.Floor(currentChunkPosition.x) + ", " + Mathf.Floor(currentChunkPosition.y));
	}
}