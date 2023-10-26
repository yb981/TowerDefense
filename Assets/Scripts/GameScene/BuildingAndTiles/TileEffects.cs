using System;
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
    public event EventHandler<OnNewTileBonusEventArgs> OnNewTileBonus;
    public class OnNewTileBonusEventArgs : EventArgs
    {
        public Transform bonusObject;
        public string bonusType;
        public string bonusAmount;
    }
   
    [Header("BloodyTile")]
    [SerializeField] private int bloodyBonusDamage = 2;

    private static string BLOODY_BONUS = "Unit bonus damage";

    [Header("Heights")]
    [SerializeField] private float extraRangePerHeight = 1f;
    
    public void ApplyTileBonus(Transform trans, GridBuildingSystem.SubTileGridObject subTileGridObject)
    {
        switch(subTileGridObject.GetTileEffect())
        {
            case MainTileEffect.bloody: 
                Minion minion = trans.GetComponent<Minion>();
                if(minion != null)
                {
                    minion.AddDamage(bloodyBonusDamage);
                    TriggerEventOnBonus(trans,"Extra Damage", bloodyBonusDamage);
                }
                break;
            case MainTileEffect.none: break;
        }

        ApplySubTileHeight(trans, subTileGridObject);


    }

    private void ApplySubTileHeight(Transform trans, GridBuildingSystem.SubTileGridObject subTileGridObject)
    {
        Tower tower = trans.GetComponent<Tower>();
        if(tower == null) return;
        float additionalRange = subTileGridObject.GetSubTileGroundLevel()*extraRangePerHeight;

        tower.AddRange(additionalRange);
        Debug.Log("Additional Range: "+additionalRange);

        TriggerEventOnBonus(trans,"Range", additionalRange);
    }

    public string GetMainTileEffectText(MainTileEffect mainTileEffect)
    {
        switch(mainTileEffect)
        {
            case MainTileEffect.none:   return "-";
            case MainTileEffect.bloody: return BLOODY_BONUS + ": " + bloodyBonusDamage;
        }
        return "-";
    }

    private void TriggerEventOnBonus(Transform trans, string bonusType, float amount)
    {
        OnNewTileBonus?.Invoke(this, new OnNewTileBonusEventArgs()
        {
            bonusObject = trans, bonusType = bonusType, bonusAmount = amount.ToString()
        });
    }
}
