using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    private Canvas selectionCanvas;
    private GameObject selection;

    public void SetSelection(GameObject newSelection)
    {
        selection = newSelection;
        selectionCanvas.transform.position = newSelection.transform.position;
    }
}
