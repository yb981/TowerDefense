using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class GridBuildingSystem : MonoBehaviour
{
    public event Action OnBuilt;

    [Serializable]
    public class GridBluePrint
    {
        public Transform origin; 
        public int width;
        public int height;
        public float cellSize;
        public FieldType type;
    }

    public enum FieldType
    {
        unit,
        building,
        all,
    }

    [SerializeField] List<GridBluePrint> gridBluePrints = new List<GridBluePrint>();

    [Header("Other")]
    [SerializeField] private Transform ghost;

    private List<Grid<GridObject>> grids = new List<Grid<GridObject>>();
    private bool building = false;
    private KnightBluePrintSO currentBlueprint;
    
    private SpriteRenderer ghostRenderer;

    void Awake()
    {
        foreach (GridBluePrint bluePrint in gridBluePrints)
        {
            grids.Add(new Grid<GridObject>(bluePrint.width, bluePrint.height, bluePrint.cellSize, bluePrint.origin.position, bluePrint.type, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y)));
        }
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
            
            GhostBuildingDisplayAllGrids();

            if (Input.GetMouseButtonDown(0))
            {
                TryBuildSelectedGameObjectAllGrids();
            }
        }
    }

    private void GhostBuildingDisplayAllGrids()
    {
        foreach (Grid<GridObject> grid in grids)
        {
            GhostBuildingDisplay(grid);
        }
    }

    private void GhostBuildingDisplay(Grid<GridObject> grid)
    {
        if(currentBlueprint == null) 
            return;

        if(!CorrectBuildType(grid)) 
            return;

        Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
        grid.GetXY(mousePos, out int x, out int y);
        GridObject gridObject = grid.GetGridObject(x,y);
        if(gridObject != null)
        {
            ghostRenderer.sprite = currentBlueprint.GetTransform().GetComponentInChildren<SpriteRenderer>().sprite;
            ghost.transform.position = grid.GetCellCenter(x,y);
            if(gridObject.CanBuild())
            {
                ghostRenderer.color = new Color(1,1,1,0.5f);
            }else{
                ghostRenderer.color = new Color(1,0.2f,0.2f,0.5f);
            }
        }
    }

    private void TryBuildSelectedGameObjectAllGrids()
    {
        foreach (Grid<GridObject> grid in grids)
        {
            TryBuildSelectedGameObject(grid);
        }
    }

    private void TryBuildSelectedGameObject(Grid<GridObject> grid)
    {
        if(!CorrectBuildType(grid))
            return;

        if (grid.GetGridObject(UtilsClass.GetMouseWorldPosition()) != null)
        {       
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition();

            grid.GetXY(mousePos, out int x, out int y);
            GridObject gridObject = grid.GetGridObject(x,y);
            if (gridObject.CanBuild())
            {
                Transform monster = Instantiate(currentBlueprint.GetTransform(), grid.GetCellCenter(x, y), Quaternion.identity);
                gridObject.SetTransfrom(monster);

                DoPayment();
                
                OnBuilt?.Invoke();
                StopBuilding();
            }
            else
            {
                UtilsClass.CreateWorldTextPopup("Can't bulid here", mousePos);
            }
        }
    }

    private bool CorrectBuildType(Grid<GridObject> grid)
    {
        if(currentBlueprint == null)
            return false;

        if(currentBlueprint.GetBuildType() != FieldType.all && currentBlueprint.GetBuildType() != grid.GetBuildType())
        {
            return false;
        }else{
            return true;
        }
        
    }

    private void DoPayment()
    {
        int oldCredits = PlayerStats.Instance.GetCredits();
        oldCredits -= currentBlueprint.GetCost();
        PlayerStats.Instance.SetCredits(oldCredits);
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

        public void SetTransfrom(Transform building)
        {
            this.transform = building;
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