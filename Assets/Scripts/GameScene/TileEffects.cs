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
                }
                break;
            case MainTileEffect.none: break;
        }

        ApplySubTileHeight(trans, subTileGridObject);
    }

    private void ApplySubTileHeight(Transform trans, GridBuildingSystem.SubTileGridObject subTileGridObject)
    {
        Tower tower = trans.GetComponent<Tower>();
        if(tower != null)
        {
            tower.AddRange(subTileGridObject.GetSubTileGroundLevel()*extraRangePerHeight);
        }
        else if(subTileGridObject.GetFieldType() == GridBuildingSystem.FieldType.building)
        {
            Debug.LogError("Can't find tower component, but placed object is a building");
        }
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
}
