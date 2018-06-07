using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSaver : MonoBehaviour {
    private static GameInfo gameInfo = new GameInfo();

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        gameInfo.playerPosition = player.transform.position.x + "," + player.transform.position.y + "," + player.transform.position.z;

        string json = JsonUtility.ToJson(gameInfo);

        PlayerPrefs.SetString(System.DateTime.Now.ToString("yyyy:MM:dd hh:mm:ss"), json);

        string savedGames = PlayerPrefs.GetString("SavedGames");
        if (savedGames == "")
        {
            PlayerPrefs.SetString("SavedGames", System.DateTime.Now.ToString("yyyy:MM:dd hh:mm:ss"));
        } else
        {
            PlayerPrefs.SetString("SavedGames", savedGames + "," + System.DateTime.Now.ToString("yyyy:MM:dd hh:mm:ss"));
        }

//        string path;
//#if UNITY_EDITOR
//        path = "Assets/Resources/Saves/" + System.DateTime.Now.ToString("yyyy:MM:dd hh:mm:ss") + ".json";
//#elif UNITY_STANDALONE
//        path = "MyGame_Data/Resources/Saves/" + System.DateTime.Now.ToString("yyyy /MM/dd hh:mm:ss") + ".json";
//#endif

        //        using (FileStream fs = new FileStream(path, FileMode.Create))
        //        {
        //            using(StreamWriter writer = new StreamWriter(fs))
        //            {
        //                writer.Write(json);
        //            }
        //        }

        //#if UNITY_EDITOR
        //        UnityEditor.AssetDatabase.Refresh();
        //#endif
    }

    public static GameInfo GameInfo
    {
        get
        {
            return gameInfo;
        }
    }
}
