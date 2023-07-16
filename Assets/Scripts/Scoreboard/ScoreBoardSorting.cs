using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreBoardSorting
{
    private static int partition(List<GameObject> list, int startIndex, int endIndex)
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
    public static void quickSort(List<GameObject> list, int startIndex, int lastIndex)
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

    private static void SwapElements(List<GameObject> list, int firstElement, int secondElement)
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
