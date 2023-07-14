using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private GameObject scoreboardElementPrefab;
    [SerializeField] private GameObject components;
    [SerializeField] private int maxEntries = 10;
    private List<GameObject> scoreboardElements = new List<GameObject>();

    public void AddRandomEntry()
    {
        int score = Random.Range(0,99999);
        TryAddNewScoreBoardEntry(score);
    }

    public void TryAddNewScoreBoardEntry(int score)
    {
        // add if less than max entries
        if(scoreboardElements.Count < 10)
        {
            AddNewScoreBoardEntry(score);
        }else{
            // if max entries, check for lowest score entry
            if(score > scoreboardElements[maxEntries-1].GetComponent<ScoreBoardElement>().GetScore())
            {
                // if bigger, remove lowest and add new one
                DeleteScoreBoardEntryAt(maxEntries-1);

                AddNewScoreBoardEntry(score);
            }
        }
    }

    private void AddNewScoreBoardEntry(int newScore)
    {
        GameObject newElement = Instantiate(scoreboardElementPrefab,new Vector3(0,0,0),Quaternion.identity);
        ScoreBoardElement scoreBoardOfNewElement = newElement.GetComponent<ScoreBoardElement>();
        scoreBoardOfNewElement.SetScore(newScore);
        scoreboardElements.Add(newElement);
        newElement.transform.SetParent(components.transform);
        quickSort(scoreboardElements,0,scoreboardElements.Count-1);
        StartCoroutine(RefreshRanks());
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
