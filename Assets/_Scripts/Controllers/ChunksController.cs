using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChunksController : MonoBehaviour {
    [SerializeField]
    private GameObject _chunkPrefab;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private int _fieldOfView = 3;

    [SerializeField]
    private Text _seedText;

    [SerializeField]
    private GameObject _extractorPrefab;
    [SerializeField]
    private GameObject _smelterPrefab;
    [SerializeField]
    private GameObject _storageContainerPrefab;

    private int _chunkSize = GlobalVariables.TILE_PER_CHUNK_AXIS * GlobalVariables.TILE_SIZE;

    List<GameObject> _generatedChunks = new List<GameObject>();

    GameObject chunkWrapper;
    
    public static bool _changed = false;

    private int _seed;

    // Use this for initialization
    void Start()
    {
        chunkWrapper = new GameObject("Chunk wrapper");
        int seed = GameSettings.Instance().GetSeed();
        //int seed = 0;
        if (seed == 0)
        {
            _seed = Mathf.CeilToInt(Random.Range(1000f, 99999f));
            GameSaver.GameInfo.seed = _seed;
            _seedText.text = "Seed: " + _seed;  
        } else
        {
            _seed = seed;
            GameSaver.GameInfo.seed = _seed;
            _seedText.text = "Seed: " + (_seed);
        }



        Vector3 playerPosition = _player.transform.position;
        if (GameSettings.Instance().IsSaved())
        {
            foreach(string coords in GameSettings.Instance().GetProtectedChunks())
            {
                InstantiateChunkAt(new Vector2(float.Parse(coords.Split(',')[0]), float.Parse(coords.Split(',')[1]))).GetComponent<Chunk> ().Protect();
            }

            playerPosition = GameSettings.Instance().GetPlayerCoords();
        }
        Vector2 coordinates = new Vector2((int)Mathf.Floor(playerPosition.x / _chunkSize) - playerPosition.x < 0 ? 1 : 0, (int)Mathf.Floor(playerPosition.z / _chunkSize) - playerPosition.z < 0 ? 1 : 0);
        GenerateChunksAround(coordinates);


        if(GameSettings.Instance().IsSaved())
        {
            LoadMachines();
        }
    }

    public void LoadMachines()
    {
        foreach(string machine in GameSettings.Instance().GetPlacedMachines())
        {
            Vector3 coordinates = new Vector3(float.Parse(machine.Split(',')[1]), 0.1f, float.Parse(machine.Split(',')[2]));

            if(machine.Contains("ResourceExtractor"))
            {
                Instantiate(_extractorPrefab, coordinates, Quaternion.Euler(new Vector3(0f, float.Parse(machine.Split(',')[3]), 0f))).GetComponent<Placable>().Place();
            } else if(machine.Contains("Smelter"))
            {
                Instantiate(_smelterPrefab, coordinates, Quaternion.Euler(new Vector3(0f, float.Parse(machine.Split(',')[3]), 0f))).GetComponent<Placable>().Place();
            } else if(machine.Contains("StorageContainer"))
            { 
                Instantiate(_storageContainerPrefab, coordinates, Quaternion.Euler(new Vector3(0f, float.Parse(machine.Split(',')[3]), 0f))).GetComponent<Placable>().Place();
            }
        }
    }

    //Generate the chunks around the player
    public void GenerateChunksAround(Vector2 coordinates)
    {
        List<Vector2> notGeneratedChunks = GetListOfNotGeneratedChunksAround(coordinates);
        for (int i = 0; i < notGeneratedChunks.Count; i++)
        {
            InstantiateChunkAt(notGeneratedChunks[i]);
        }

        DeleteDistantChunks(coordinates);
    }

    public void DeleteDistantChunks(Vector2 coordinates)
    {

        List<GameObject> deletedChunks = new List<GameObject>();
        foreach(GameObject chunkGO in _generatedChunks)
        {
            Chunk chunk = chunkGO.GetComponent<Chunk>();
            if(IsFar(chunk, coordinates))
            {
                if (!chunk.IsProtected())
                {
                    deletedChunks.Add(chunk.gameObject);
                    Destroy(chunk.gameObject);
                }
            }
        }

        _generatedChunks = _generatedChunks.Except(deletedChunks).ToList();
    }

    //Generate the chunk and initialize its mesh
    private GameObject InstantiateChunkAt(Vector2 coordinates)
    {
        GameObject instance = Instantiate(_chunkPrefab);
        instance.transform.position = new Vector3(coordinates.x * _chunkSize, 0f, coordinates.y * _chunkSize);
        instance.name = "Chunk " + coordinates;
        instance.transform.parent = chunkWrapper.transform;
        instance.GetComponent<Chunk>().InitializeChunk(new Vector2(coordinates.x, coordinates.y), _seed);
        _generatedChunks.Add(instance);

        return instance;
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

    public void ProtectChunk(float x, float y)
    {
        GameObject targetChunk = GetChunkFromCoords(x, y);
        if (targetChunk != null) {
            targetChunk.GetComponent<Chunk>().Protect();
        }
    }

    private bool IsFar(Chunk chunk, Vector2 coordinates)
    {
        Chunk currentChunk = _generatedChunks.Where(chunkGO => chunkGO.GetComponent<Chunk>().GetCoordinates() == coordinates).SingleOrDefault().GetComponent<Chunk> ();
        return (Mathf.Abs(currentChunk.GetCoordinates().x - chunk.GetCoordinates().x) > 2) || (Mathf.Abs(currentChunk.GetCoordinates().y - chunk.GetCoordinates().y) > 2);
    }
}