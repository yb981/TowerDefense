using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonUI : MonoBehaviour
{
    [SerializeField] private MinionBluePrintSO monsterSO;
    private Button button;
    private GridBuildingSystem gridBuildingSystem;
    private bool enoughMoney;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    private void Start() 
    {
        gridBuildingSystem = FindObjectOfType<GridBuildingSystem>();
        gridBuildingSystem.OnBuilt += GridBuildingSystem_OnBuilt;
        LevelManager.instance.OnLevelPhaseBuild += LevelManager_OnLevelPhaseBuild;
        LevelManager.instance.OnLevelPhasePlay += LevelManager_OnLevelPhasePlay;
        PlayerStats.OnCreditsChanged += PlayerStats_OnCreditsChanged;
        UpdateMoneyControlVariable();
    }

    private void PlayerStats_OnCreditsChanged()
    {
        UpdateMoneyControlVariable();
    }

    private void GridBuildingSystem_OnBuilt()
    {
        if(!enoughMoney)
        {
            button.interactable = false;
        }
    }

    private void LevelManager_OnLevelPhaseBuild()
    {
        TurnOnButtonForBuyPhase();
    }

    private void LevelManager_OnLevelPhasePlay()
    {
        button.interactable = false;
    }

    private void UpdateMoneyControlVariable()
    {
        if(PlayerStats.Instance.GetCredits() < monsterSO.GetCost())
        {
            enoughMoney = false;
        }else{
            enoughMoney = true;
        }
    }

    private void TurnOnButtonForBuyPhase()
    {
        if(enoughMoney)
        {
            button.interactable = true;
        }else{
            button.interactable = false;
        }
    }

    private void ButtonClicked()
    {
        gridBuildingSystem.StartBuilding(monsterSO);
    }

    private void OnDestroy() 
    {
        button.onClick.RemoveListener(ButtonClicked);
    }

    public MinionBluePrintSO GetMonsterSO()
    {
        return monsterSO;
    }
}
