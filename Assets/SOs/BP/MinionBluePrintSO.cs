using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenuAttribute]
public class MinionBluePrintSO : ScriptableObject
{
    [SerializeField] private Transform prefab;
    [SerializeField] private GridBuildingSystem.FieldType type;
    [SerializeField] private int cost;

    public Transform GetTransform()
    {
        return prefab;
    }

    public int GetCost()
    {
        return cost;
    }

    public GridBuildingSystem.FieldType GetBuildType()
    {
        return type;
    }
}
