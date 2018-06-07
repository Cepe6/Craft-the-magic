using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGamesController : MonoBehaviour {
    [SerializeField]
    private GameObject _savedGameButton;
    [SerializeField]
    private Transform _savedGamesPanelTransform;
    [SerializeField]
    private SceneController _sceneController; 


	// Use this for initialization
	void Start () {
        InstantiateSavedGamesButtons();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Cancel") && this.GetComponent<Canvas> ().isActiveAndEnabled)
        {
            ClosePanel();
        }
	}
    
    private void InstantiateSavedGamesButtons()
    {
        string savedGames = PlayerPrefs.GetString("SavedGames");
        if(savedGames == "")
        {
            return;
        }

        string[] savedGamesNames = savedGames.Split(',');
        foreach(string game in savedGamesNames)
        {
            Button button = Instantiate(_savedGameButton, _savedGamesPanelTransform).GetComponent<Button> ();
            button.GetComponentInChildren<Text>().text = game;
            button.onClick.AddListener(delegate { Load(button.GetComponentInChildren<Text>().text); } );
        }
    }

    public void Load(string saveGameName)
    {
        string json = PlayerPrefs.GetString(saveGameName);
        GameInfo loadedGameInfo = JsonUtility.FromJson<GameInfo>(json);
        GameSettings.Instance().SetSaved();
        GameSettings.Instance().SetSeed(loadedGameInfo.seed);
        GameSettings.Instance().SetPlayerCoords(loadedGameInfo.playerPosition);
        GameSettings.Instance().SetProtectedChunks(loadedGameInfo.savedChunksCoords);
        GameSettings.Instance().SetChangedResourcesCoords(loadedGameInfo.changedResourcesCoords);
        GameSettings.Instance().SetChangedResourcesAmmount(loadedGameInfo.changedResourcesAmmount);
        GameSettings.Instance().SetPlacedMachines(loadedGameInfo.placedMachines);
        GameSettings.Instance().SetPlacedMachinesInventories(loadedGameInfo.placedMachinesInventory);
        GameSettings.Instance().SetSavedInventories(loadedGameInfo.savedInventories);
        GameSettings.Instance().SetSavedInventoryItems(loadedGameInfo.savedInventoriesItems);

        _sceneController.OnNewGame();
    }

    public void ShowPanel()
    {
        this.GetComponent<Canvas>().enabled = true;
    }

    private void ClosePanel()
    {
        this.GetComponent<Canvas>().enabled = false;
    }
}
