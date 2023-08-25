using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainTileEffect
{
    bloody,
    none
}

public class TileEffects : MonoBehaviour
{
    [Header("BloodyTile")]
    [SerializeField] private int bloodyBonusDamage = 2;
    
    public void ApplyTileBonus(Transform trans, MainTileEffect mainEffect)
    {
        switch(mainEffect)
        {
            case MainTileEffect.bloody: 
                Minion minion = trans.GetComponent<Minion>();
                if(minion != null)
                {
                    minion.AddDamage(bloodyBonusDamage);
                }
                break;
            case MainTileEffect.none: break;
        }
    }
}
