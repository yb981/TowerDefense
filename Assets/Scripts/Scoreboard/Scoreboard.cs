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
        public string time;

        public ScoreBoardValues(string name, int score, int waves, string time)
        {
            this.name = name;
            this.score = score;
            this.waves = waves;
            this.time = time;
        }
    }

    // Because cant json lists
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
        InitScoreBoard();

        if (PersistenceManager.Instance.LastScene == GameConstants.SCENE_GAME)
        {
            // change from timepsan to string
            TimeSpan timeSpan = PersistenceManager.Instance.GetPlayTime();
            string time = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";

            ScoreBoardValues values = new ScoreBoardValues("???", PersistenceManager.Instance.LoadScore(), PersistenceManager.Instance.GetWaveCount(), time);
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
        ScoreBoardSaveLoad.SaveScoreBoard(scoreboardElements);
    }

    public void AddRandomEntry()
    {
        int randomScore = UnityEngine.Random.Range(0, 99999);
        int randomWave = UnityEngine.Random.Range(0, 99);
        float randomTime = UnityEngine.Random.Range(0f, 1000f);
        TimeSpan timeSpan = TimeSpan.FromSeconds(randomTime);
        ScoreBoardValues randomValues = new ScoreBoardValues("Rndm", randomScore, randomWave, $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}");
        TryAddNewScoreBoardEntry(randomValues);
    }

    private void TryAddNewScoreBoardEntry(ScoreBoardValues values)
    {
        // add if less than max entries
        if (scoreboardElements.Count < 10)
        {
            AddNewScoreBoardEntryAndSave(values);
        }
        else
        {
            // if max entries, check for lowest score entry
            if (values.score > scoreboardElements[maxEntries - 1].GetComponent<ScoreBoardElement>().GetScore())
            {
                // if bigger, remove lowest and add new one
                DeleteScoreBoardEntryAt(maxEntries - 1);

                AddNewScoreBoardEntryAndSave(values);
            }
        }
    }

    private void AddNewScoreBoardEntryAndSave(ScoreBoardValues values)
    {
        AddNewScoreBoardEntryNoSave(values);
        ScoreBoardSaveLoad.SaveScoreBoard(scoreboardElements);
        NameInput();
        currentEntry.GetComponent<ScoreBoardElement>().SetAsActive();
    }

    private void AddNewScoreBoardEntryNoSave(ScoreBoardValues values)
    {
        GameObject newElement = Instantiate(scoreboardElementPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        ScoreBoardElement scoreBoardOfNewElement = newElement.GetComponent<ScoreBoardElement>();
        currentEntry = newElement;
        scoreBoardOfNewElement.SetStats(values);
        scoreboardElements.Add(newElement);
        newElement.transform.SetParent(components.transform);
        ScoreBoardSorting.quickSort(scoreboardElements, 0, scoreboardElements.Count - 1);
        StartCoroutine(RefreshRanks());
    }

    private void NameInput()
    {
        nameField.Show();
    }

    private void InitScoreBoard()
    {
        SaveObject loadedSave = ScoreBoardSaveLoad.LoadScoreBoard();
        ClearScoreBoard();

        if (loadedSave == null) return;

        foreach (ScoreBoardValues values in loadedSave.scoreBoardValues)
        {
            AddNewScoreBoardEntryNoSave(values);
        }
    }

    public void DeleteScoreBoard()
    {
        ScoreBoardSaveLoad.DeleteScoreBoard();
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
}
