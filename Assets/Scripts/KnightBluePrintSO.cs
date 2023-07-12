using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute]
public class KnightBluePrintSO : ScriptableObject
{
    [SerializeField] private Transform prefab;
    [SerializeField] private GridBuildingSystem.FieldType type;
    [SerializeField] private int health;
    [SerializeField] private int cost;
    [SerializeField] private float movementSpeed;

    public Transform GetTransform()
    {
        return prefab;
    }

    public int GetCost()
    {
        return cost;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public GridBuildingSystem.FieldType GetBuildType()
    {
        return type;
    }
}
