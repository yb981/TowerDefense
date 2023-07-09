using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class GridBuildingSystem : MonoBehaviour
{

    public event Action OnBuilt;

    [SerializeField] private Transform monsterPrefab;

    private Grid<GridObject> monsterGrid;
    private int gridWidth = 5;
    private int gridHeight = 3;
    private float cellSize = 10f;
    private bool building = false;
    private KnightBluePrintSO currentBlueprint;
    [SerializeField] private Transform ghost;
    private SpriteRenderer ghostRenderer;

    private int credits = 100;

    void Awake()
    {
        monsterGrid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, transform.position, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));
    }

    private void Start()
    {
        LevelManager.instance.OnLevelPhaseBuild += LevelManager_OnLevelPhaseBuild;
        LevelManager.instance.OnLevelPhasePlay += LevelManager_OnLevelPhasePlay;

        ghostRenderer = ghost.GetComponent<SpriteRenderer>();
        ghostRenderer.enabled = false;
    }

    private void LevelManager_OnLevelPhaseBuild()
    {
        building = true;
    }

    private void LevelManager_OnLevelPhasePlay()
    {
        building = false;
    }

    private void Update()
    {
        if (building)
        {
            
            GhostBuildingDisplay();

            if (Input.GetMouseButtonDown(0))
            {
                TryBuildSelectedGameObject();
            }
        }
    }

    private void GhostBuildingDisplay()
    {
        Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
        monsterGrid.GetXY(mousePos, out int x, out int y);
        GridObject gridObject = monsterGrid.GetGridObject(x,y);
        if(gridObject != null)
        {
            ghostRenderer.sprite = currentBlueprint.GetTransform().GetComponentInChildren<SpriteRenderer>().sprite;
            ghost.transform.position = monsterGrid.GetCellCenter(x,y);
            if(gridObject.CanBuild())
            {
                ghostRenderer.color = new Color(1,1,1,0.5f);
            }else{
                ghostRenderer.color = new Color(1,0.2f,0.2f,0.5f);
            }
        }
    }

    private void TryBuildSelectedGameObject()
    {
        if (monsterGrid.GetGridObject(UtilsClass.GetMouseWorldPosition()) != null)
        {
            
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition();

            monsterGrid.GetXY(mousePos, out int x, out int y);
            GridObject gridObject = monsterGrid.GetGridObject(x,y);
            if (gridObject.CanBuild())
            {
                Transform monster = Instantiate(currentBlueprint.GetTransform(), monsterGrid.GetCellCenter(x, y), Quaternion.identity);
                gridObject.SetTransfrom(monster);

                credits -= currentBlueprint.GetCost();
                OnBuilt?.Invoke();
                StopBuilding();
            }
            else
            {
                UtilsClass.CreateWorldTextPopup("Can't bulid here", mousePos);
            }
        }
    }

    public void StartBuilding(KnightBluePrintSO blueprint)
    {
        building = true;
        currentBlueprint = blueprint;
        ghostRenderer.enabled = true;
    }

    public void StopBuilding()
    {
        building = false;
        currentBlueprint = null;
        DisableAndResetGhost();
    }

    private void DisableAndResetGhost()
    {
        ghostRenderer.enabled = false;
        ghost.position = Vector3.zero;
        ghostRenderer.sprite = null;
    }

    public int GetCredits()
    {
        return credits;
    }

    public class GridObject
    {
        private Grid<GridObject> grid;
        private int x;
        private int y;
        private Transform transform;

        public GridObject(Grid<GridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetTransfrom(Transform monster)
        {
            this.transform = monster;
            grid.TriggerGridObjectChanged(x, y);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void ClearTransform()
        {
            transform = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public bool CanBuild()
        {
            if (transform == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return (x + "," + y + "\n" + transform);
        }

    }
}