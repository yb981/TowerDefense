using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreBoardSaveLoad
{

    private static string SAVE_FILE = Application.dataPath + "/scoreboardData.json";

    public static void SaveScoreBoard(List<GameObject> scoreboardElements)
    {
        Scoreboard.SaveObject newSave = new Scoreboard.SaveObject(scoreboardElements);

        string json = JsonUtility.ToJson(newSave);

        string filePath = SAVE_FILE;
        System.IO.File.WriteAllText(filePath,json);
    }

    public static Scoreboard.SaveObject LoadScoreBoard()
    {
        string filePath = SAVE_FILE;
        if(System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            Scoreboard.SaveObject loadedSave = JsonUtility.FromJson<Scoreboard.SaveObject>(json);

            return loadedSave;

        }else{
            Debug.Log("no file found");
            return null;
        }
    }

    public static void DeleteScoreBoard()
    {
        string filePath = Application.dataPath + "/scoreboardData.json";
        if(System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }

}
