using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour {
    private static GameSettings _instance;

    [SerializeField]
    private InputField _seedInputField;

    private int _seed = 0;

    private bool _isSaved;
    private Vector3 _playerCoords;
    private List<string> _protectedChunks = new List<string>();
    private List<StringListWrapper> _changedResourcesCoords = new List<StringListWrapper>();
    private List<IntListWrapper> _changedResourcesAmmount = new List<IntListWrapper>();
    private List<string> _placedMachines = new List<string>();
    private List<StringListWrapper> _placedMachinesInventories = new List<StringListWrapper>();
    private List<string> _savedInventories = new List<string>();
    private List<StringListWrapper> _savedInventoriesItems = new List<StringListWrapper>();

	// Use this for initialization
	void Start () {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(_instance.gameObject);
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
	}

    public void SeedChange()
    {
        if(_seedInputField == null)
            _seedInputField = GameObject.FindGameObjectWithTag("SeedInputField").GetComponent<InputField> ();

        if (!int.TryParse(_seedInputField.text, out _seed))
        {
            _seed = 0;
        }
    }

    public void SetSeed(int seed)
    {
        _seed = seed;
    }

    public void SetPlayerCoords(string playerCoords)
    {
        Vector3 newCoords = new Vector3(float.Parse(playerCoords.Split(',')[0]), float.Parse(playerCoords.Split(',')[1]), float.Parse(playerCoords.Split(',')[2]));
        _playerCoords = newCoords;
    }

    public Vector3 GetPlayerCoords()
    {
        return _playerCoords;
    }

    public int GetSeed()
    {
        return _seed;
    }

    public static GameSettings Instance()
    {
        return _instance;
    }

    public void SetSaved()
    {
        _isSaved = true;
    }

    public bool IsSaved()
    {
        return _isSaved;
    }

    public void SetProtectedChunks(List<string> chunks)
    {
        _protectedChunks = chunks;
    }

    public List<string> GetProtectedChunks()
    {
        return _protectedChunks;
    }

    public void SetChangedResourcesCoords(List<StringListWrapper> coords)
    {
        _changedResourcesCoords = coords;
    }

    public List<StringListWrapper> GetChangedResourcesCoords()
    {
        return _changedResourcesCoords;
    }

    public void SetChangedResourcesAmmount(List<IntListWrapper> ammounts)
    {
        _changedResourcesAmmount = ammounts;
    }

    public List<IntListWrapper> GetChangedResourcesAmmount()
    {
        return _changedResourcesAmmount;
    }

    public void SetPlacedMachines(List<string> placed)
    {
        _placedMachines = placed;
    }

    public List<string> GetPlacedMachines()
    {
        return _placedMachines;
    }

    public void SetPlacedMachinesInventories(List<StringListWrapper> items)
    {
        _placedMachinesInventories = items;
    }

    public List<StringListWrapper> GetPlacedMachinesInventories()
    {
        return _placedMachinesInventories;
    }

    public void SetSavedInventories(List<string> inventories)
    {
        _savedInventories = inventories;
    }

    public List<string> GetSavedInventories()
    {
        return _savedInventories;
    }

    public void SetSavedInventoryItems(List<StringListWrapper> items)
    {
        _savedInventoriesItems = items;
    }

    public List<StringListWrapper> GetSavedInventoryItems()
    {
        return _savedInventoriesItems;
    }
}
