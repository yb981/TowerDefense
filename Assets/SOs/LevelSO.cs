using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelSO : ScriptableObject
{
    [SerializeField] private int amountOfWaves;

    public int GetAmountOfWaves()
    {
        return amountOfWaves;
    }
}
