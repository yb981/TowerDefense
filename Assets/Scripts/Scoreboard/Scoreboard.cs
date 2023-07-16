using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private ScoreBoardFieldName nameField;
    [SerializeField] private GameObject scoreboardElementPrefab;
    [SerializeField] private GameObject components;
    [SerializeField] private int maxEntries = 10;
    private List<GameObject> scoreboardElements = new List<GameObject>();
    private GameObject currentEntry;

    [System.Serializable]
    public class ScoreBoardValues
    {
        public string name;
        public int score;
        public int waves;
        public TimeSpan time;

        public ScoreBoardValues(string name, int score, int waves, TimeSpan time)
        {
            this.name = name;
            this.score = score;
            this.waves = waves;
            this.time = time;
        }
    }

    [System.Serializable]
    public class SaveObject
    {

        public List<ScoreBoardValues> scoreBoardValues = new List<ScoreBoardValues>();

        public SaveObject(List<GameObject> scoreBoardElements)
        {
            foreach (GameObject element in scoreBoardElements)
            {
                scoreBoardValues.Add(element.GetComponent<ScoreBoardElement>().GetScoreBoardValues());      
            }
        }
    }

    private void Start()
    {
        LoadScoreBoard();

        if(PersistenceManager.Instance.LastScene == GameConstants.SCENE_GAME)
        {
            ScoreBoardValues values = new ScoreBoardValues("???",PersistenceManager.Instance.LoadScore(),PersistenceManager.Instance.GetWaveCount(),PersistenceManager.Instance.GetPlayTime());
            TryAddNewScoreBoardEntry(values);
        }
    }

    public void SetPlayerName(string name)
    {
        currentEntry.GetComponent<ScoreBoardElement>().SetName(name);
        foreach (GameObject item in scoreboardElements)
        {
            Debug.Log(item.GetComponent<ScoreBoardElement>().GetName());
        }
        SaveScoreBoard();
    }

    public void AddRandomEntry()
    {
        int randomScore = UnityEngine.Random.Range(0,99999);
        int randomWave = UnityEngine.Random.Range(0,99);
        float randomTime = UnityEngine.Random.Range(0f,1000f);
        ScoreBoardValues randomValues = new ScoreBoardValues("Rndm",randomScore,randomWave,TimeSpan.FromSeconds(randomTime));
        TryAddNewScoreBoardEntry(randomValues);
    }

    private void TryAddNewScoreBoardEntry(ScoreBoardValues values)
    {
        // add if less than max entries
        if(scoreboardElements.Count < 10)
        {
            AddNewScoreBoardEntryAndSave(values);
        }else{
            // if max entries, check for lowest score entry
            if(values.score > scoreboardElements[maxEntries-1].GetComponent<ScoreBoardElement>().GetScore())
            {
                // if bigger, remove lowest and add new one
                DeleteScoreBoardEntryAt(maxEntries-1);

                AddNewScoreBoardEntryAndSave(values);
            }
        }
    }

    private void AddNewScoreBoardEntryAndSave(ScoreBoardValues values)
    {
        AddNewScoreBoardEntryNoSave(values);
        SaveScoreBoard();
        NameInput();
        currentEntry.GetComponent<ScoreBoardElement>().SetAsActive();
    }

    private void AddNewScoreBoardEntryNoSave(ScoreBoardValues values)
    {
        GameObject newElement = Instantiate(scoreboardElementPrefab,new Vector3(0,0,0),Quaternion.identity);
        ScoreBoardElement scoreBoardOfNewElement = newElement.GetComponent<ScoreBoardElement>();
        currentEntry = newElement;
        scoreBoardOfNewElement.SetStats(values);
        scoreboardElements.Add(newElement);
        newElement.transform.SetParent(components.transform);
        quickSort(scoreboardElements,0,scoreboardElements.Count-1);
        StartCoroutine(RefreshRanks());   
    }

    private void NameInput()
    {
        nameField.Show();
    }

    private void SaveScoreBoard()
    {
        SaveObject newSave = new SaveObject(scoreboardElements);

        string json = JsonUtility.ToJson(newSave);
        Debug.Log(json);

        string filePath = Application.dataPath + "/scoreboardData.json";
        System.IO.File.WriteAllText(filePath,json);
    }

    private void LoadScoreBoard()
    {
        string filePath = Application.dataPath + "/scoreboardData.json";
        if(System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            SaveObject loadedSave = JsonUtility.FromJson<SaveObject>(json);

            ClearScoreBoard();

            foreach (ScoreBoardValues values in loadedSave.scoreBoardValues)
            {
                AddNewScoreBoardEntryNoSave(values);
            }
        }else{
            Debug.Log("no file found");
        }
    }

    public void DeleteScoreBoard()
    {
        string filePath = Application.dataPath + "/scoreboardData.json";
        if(System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
        ClearScoreBoard();
    }

    private void ClearScoreBoard()
    {
        foreach (GameObject element in scoreboardElements)
        {
            Destroy(element);
        }

        scoreboardElements.Clear();
    }

    private IEnumerator RefreshRanks()
    {
        // Coroutine to avoid race condition: new element set before old one deleted
        yield return new WaitForEndOfFrame();
        foreach (GameObject entry in scoreboardElements)
        {
            ScoreBoardElement scoreBoardElement = entry.GetComponent<ScoreBoardElement>();
            scoreBoardElement.SetRank(entry.transform.GetSiblingIndex());
        }
    }
    
    private void DeleteScoreBoardEntryAt(int position)
    {
        GameObject gameobjectToDestroy = scoreboardElements[position];
        scoreboardElements.RemoveAt(position);
        Destroy(gameobjectToDestroy);
    }
 
    private int partition(List<GameObject> list, int startIndex, int endIndex)
    {
        // Choosing the pivot
        GameObject pivot = list[endIndex];
 
        // Index of smaller element and indicates
        // the right position of pivot found so far
        int i = (startIndex - 1);
 
        for (int j = startIndex; j <= endIndex - 1; j++) {
 
            // If current element is smaller than the pivot
            if (list[j].GetComponent<ScoreBoardElement>().GetScore() > pivot.GetComponent<ScoreBoardElement>().GetScore()) {
 
                // Increment index of smaller element
                i++;
                SwapElements(list, i, j);
            }
        }
        SwapElements(list, i + 1, endIndex);
        return (i + 1);
    }
 
    // The main function that implements QuickSort
    // arr[] --> Array to be sorted,
    // low --> Starting index,
    // high --> Ending index
    private void quickSort(List<GameObject> list, int startIndex, int lastIndex)
    {
        if (startIndex < lastIndex) {
 
            // pi is partitioning index, arr[p]
            // is now at right place
            int pi = partition(list, startIndex, lastIndex);
 
            // Separately sort elements before
            // and after partition index
            quickSort(list, startIndex, pi - 1);
            quickSort(list, pi + 1, lastIndex);
        }
    }

    private void SwapElements(List<GameObject> list, int firstElement, int secondElement)
    {
        GameObject first = list[firstElement];
        // sibling index
        int firstSiblingIndex = first.transform.GetSiblingIndex();
        int secondSiblingIndex = list[secondElement].transform.GetSiblingIndex();

        list[secondElement].transform.SetSiblingIndex(firstSiblingIndex);
        list[firstElement] = list[secondElement];

        list[secondElement] = first;
        list[secondElement].transform.SetSiblingIndex(secondSiblingIndex);
    }
}
