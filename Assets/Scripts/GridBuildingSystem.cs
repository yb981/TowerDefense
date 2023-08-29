using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public event Action OnBuilt;
    public static EventHandler OnStartBuilding;
    public static event Action OnEndBuilding;
    public static event EventHandler<OnNewSubTileEventArgs> OnNewSubTile;
    public class OnNewSubTileEventArgs : EventArgs {
        public Tilemap tileTilemap;
        public Vector2Int pos;
        public int buildHeight;
        public FieldType fieldType;
    };

    public enum FieldType
    {
        none,
        unit,
        building,
        all,
    }

    [SerializeField] private GameObject gridHighlightPrefab;
    public static GameObject GridHighlightPrefab { get; private set; }

    [Header("BuildGrid")]
    [SerializeField] private int cellSize = 1;
    private int width;
    private int height;


    [Header("Other")]
    [SerializeField] private Transform ghost;

    private TileEffects  tileEffects;
    private TileGrid tileGrid;
    private Grid<SubTileGridObject> grid;
    private bool building = false;
    private MinionBluePrintSO currentBlueprint;

    private SpriteRenderer ghostRenderer;

    void Awake()
    {
        GridHighlightPrefab = gridHighlightPrefab;

        SetupGrid();
    }

    private void Start()
    {
        LevelManager.instance.OnLevelPhaseBuild += LevelManager_OnLevelPhaseBuild;
        LevelManager.instance.OnLevelPhasePlay += LevelManager_OnLevelPhasePlay;

        ghostRenderer = ghost.GetComponent<SpriteRenderer>();
        ghostRenderer.enabled = false;

        tileEffects = FindObjectOfType<TileEffects>();
    }

    private void SetupGrid()
    {
        tileGrid = GetComponent<TileGrid>();

        int tileGridCellSize = tileGrid.GetCellSize();
        width = tileGrid.GetWidth() * (tileGridCellSize / cellSize);
        height = tileGrid.GetHeight() * (tileGridCellSize / cellSize);

        grid = new Grid<SubTileGridObject>(width, height, cellSize, transform.position, (Grid<SubTileGridObject> g, int x, int y) => new SubTileGridObject(g, x, y));
    }

    private void LevelManager_OnLevelPhaseBuild()
    {
        //building = true;
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
        if (currentBlueprint == null)
            return;

        Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
        grid.GetXY(mousePos, out int x, out int y);
        SubTileGridObject gridObject = grid.GetGridObject(x, y);
        if (gridObject != null)
        {
            ghostRenderer.sprite = currentBlueprint.GetTransform().GetComponentInChildren<SpriteRenderer>().sprite;
            ghost.transform.position = grid.GetCellCenter(x, y);
            if (gridObject.CanBuild(currentBlueprint.GetBuildType()))
            {
                ghostRenderer.color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                ghostRenderer.color = new Color(1, 0.2f, 0.2f, 0.5f);
            }
        }
    }

    private void TryBuildSelectedGameObject()
    {
        if (grid.GetGridObject(UtilsClass.GetMouseWorldPosition()) != null)
        {
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition();

            grid.GetXY(mousePos, out int x, out int y);
            SubTileGridObject gridObject = grid.GetGridObject(x, y);
            if (gridObject.CanBuild(currentBlueprint.GetBuildType()))
            {
                Build(x, y);
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

    private void Build(int x, int y)
    {
        SubTileGridObject gridObject = grid.GetGridObject(x, y);
        Transform unitOrTower = Instantiate(currentBlueprint.GetTransform(), grid.GetCellCenter(x, y), Quaternion.identity);
        tileEffects.ApplyTileBonus(unitOrTower,gridObject.GetTileEffect());
        gridObject.SetTransfrom(unitOrTower);
    }

    private void DoPayment()
    {
        int oldCredits = PlayerStats.Instance.GetCredits();
        oldCredits -= currentBlueprint.GetCost();
        PlayerStats.Instance.SetCredits(oldCredits);
    }

    public void StartBuilding(MinionBluePrintSO blueprint)
    {
        building = true;
        currentBlueprint = blueprint;
        ghostRenderer.enabled = true;
        OnStartBuilding?.Invoke(this, EventArgs.Empty);
    }

    public void StopBuilding()
    {
        OnEndBuilding?.Invoke();
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

    public MinionBluePrintSO GetCurrentBluePrint()
    {
        return currentBlueprint;
    }

    public void SetTypeOfCell(FieldType type, int x, int y)
    {
        grid.GetGridObject(x, y).SetType(type);
    }

    public void SetSubTileGroundLevel(Tilemap tilemap, int height, int x, int y)
    {
        grid.GetGridObject(x,y).SetSubTileGroundLevel(height);
        OnNewSubTile?.Invoke(this, new OnNewSubTileEventArgs(){
            tileTilemap = tilemap,
            pos = new Vector2Int(x,y),
            fieldType = grid.GetGridObject(x,y).GetFieldType(),
            buildHeight = height
        });
    }

    public void SetMainEffectOfCell(MainTileEffect effect, int x, int y)
    {
        grid.GetGridObject(x, y).SetTileEffect(effect);
    }

    public int GetCellSize()
    {
        return cellSize;
    }

    public class SubTileGridObject
    {
        private Grid<SubTileGridObject> grid;
        private int x;
        private int y;
        private Transform transform;
        private GameObject highlight;
        private FieldType type = FieldType.none;
        private MainTileEffect tileEffect;
        private int groundLevel;

        public SubTileGridObject(Grid<SubTileGridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;

            // Setup Highlight Object
            highlight = Instantiate(GridBuildingSystem.GridHighlightPrefab, grid.GetCellCenter(x, y), Quaternion.identity);
            highlight.name = highlight.name + x + "," + y;
            highlight.transform.SetParent(FindObjectOfType<GridBuildingSystem>().transform);
            highlight.SetActive(false);

            GridBuildingSystem.OnStartBuilding += GridBuildingSystem_OnStartBuilding;
            GridBuildingSystem.OnEndBuilding += GridBuildingSystem_OnEndBuilding;
        }

        private void GridBuildingSystem_OnStartBuilding(object sender, EventArgs e)
        {
            GridBuildingSystem gridBuildingSystem = (GridBuildingSystem)sender;
            if (transform == null && gridBuildingSystem.GetCurrentBluePrint().GetBuildType() == type)
            {
                highlight.SetActive(true);
            }
        }

        private void GridBuildingSystem_OnEndBuilding()
        {
            highlight.SetActive(false);
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

        public void SetType(FieldType type)
        {
            this.type = type;
        }

        public FieldType GetFieldType()
        {
            return type;
        }

        public void ClearTransform()
        {
            transform = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public bool CanBuild(FieldType type)
        {
            if (transform == null && this.type == type)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetTileEffect(MainTileEffect effect)
        {
            tileEffect = effect;
        }

        public MainTileEffect GetTileEffect()
        {
            return tileEffect;
        }

        public void SetSubTileGroundLevel(int height)
        {
            groundLevel = height;
        }

        public override string ToString()
        {
            return (x + "," + y + "\n" + transform);
        }
    }
}