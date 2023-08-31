using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class MouseSelection : MonoBehaviour
{

    private TooltipUI tooltipUI;
    private TileGrid tileGridComp;
    private Grid<TileGrid.GridTileObject> tileGrid;

    private void Start()
    {
        tooltipUI = FindObjectOfType<TooltipUI>();
        tileGridComp = FindObjectOfType<TileGrid>();
        tileGrid = tileGridComp.GetTileGrid();

        StartCoroutine(FindSelection());
    }

    private IEnumerator FindSelection()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            TileGrid.GridTileObject currentHover = tileGrid.GetGridObject(UtilsClass.GetMouseWorldPosition());
            tooltipUI.NewHoverTile(currentHover);
        }
    }
}
