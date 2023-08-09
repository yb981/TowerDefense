using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BuildTileData
{
    [System.Serializable]
    public struct values
    {
        public GridBuildingSystem.FieldType[] row;
    }

    public values[] rows = new values[10];

}