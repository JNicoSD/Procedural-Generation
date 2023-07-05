using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DebugUI;

public class WaveFunction1D_Array : MonoBehaviour
{
    Cell[] cells;
    [SerializeField] public Cell[] GetCells { get { return cells; } }

    public CellSprite cellSprite;
        
    public int height, width;
    
    [Range(0.1f, 10f)]
    public float renderSpeed = 5.0f;
    public CheckAdjacentCell checkAdjacentCell;
    int gridSize;
    float halfWidth, halfHeight;
    
    List<CellSprite.SpriteList> spriteCandidates = new List<CellSprite.SpriteList>(); // indices of sprite candidates

    // FOR EDITOR SCRIPT
    public void DrawFromEditor()
    {
        renderSpeed = 10f;
        // size of the grid
        gridSize = height * width;
        
        // Will be used to center the grid
        halfWidth =  (float)(width - 1f) / 2f;
        halfHeight = (float)(height - 1f) / -2f;

        cells = new Cell[gridSize];

        // Create list of Sprite Candidates to assign to each grid cells
        foreach(CellSprite.SpriteList sprite in cellSprite.spriteLists) 
        {
            spriteCandidates.Add(sprite);
        }

        // Call DrawGridCells
        DrawGridCells();
    }
    public void StartFromEditor()
    {
        for(int i = 0; i < gridSize; i++)
        {
            // Call Choose cell
            ChooseCell();
        }
    }
    /////////////////////
    void Awake()
    {
        // size of the grid
        gridSize = height * width;
        
        // Will be used to center the grid
        halfWidth =  (float)(width - 1f) / 2f;
        halfHeight = (float)(height - 1f) / -2f;

        cells = new Cell[gridSize];

        // Create list of Sprite Candidates to assign to each grid cells
        foreach(CellSprite.SpriteList sprite in cellSprite.spriteLists) 
        {
            spriteCandidates.Add(sprite);
        }

        // Call DrawGridCells
        DrawGridCells();
    }

    void Start()
    {
        // Used Coroutine just to watch the generation slowly
        StartCoroutine(StartChoose());
    }

    IEnumerator StartChoose()
    {
        for(int i = 0; i < gridSize; i++)
        {
            // Call Choose cell
            ChooseCell();

            // Basically makes the selection and rendering watchable :)
            yield return new WaitForSeconds(Mathf.Lerp(1f, 0f, renderSpeed/10f));
        }
    }

    List<GameObject> tileGroup = new List<GameObject>();
    public List<GameObject> GetTileGroup { get{ return tileGroup;} }
    int tileGroupIndex = -1;
    public int TileGroupIndex { set { tileGroupIndex = value; } get{ return tileGroupIndex;} }
    public void DrawGridCells() // Create cells where a sprite can be placed later on
    {
        if(tileGroup == null) 
        {
            tileGroup = new List<GameObject>(); // NULL checker
            tileGroupIndex = -1;
            tileGroup.Clear();
        }
        else
        {
            tileGroupIndex++; // Increment first every call since we start at -1
            tileGroup.Add(new GameObject($"TILE GROUP: {tileGroup.Count}")); // Will serve as parent object
            tileGroup[tileGroupIndex].transform.position = Vector3.zero; // Makes sure parent object is at Vector3(0,0,0)

            if(tileGroupIndex > 0) tileGroup[tileGroupIndex-1].SetActive(false); // Hide previous tileGroup
        
            for(int col = 0, i = 0; col < height; col++)
            {
                for(int row = 0; row < width; row++)
                {
                    // Create new cell ( name: , candidates: )
                    cells[i] = new Cell($"Cell: {i}", spriteCandidates);

                    // Set tileGroup as the cells parent
                    cells[i].GetGameObject.transform.SetParent(tileGroup[tileGroupIndex].transform);
                    // Position the cell to the center using halfWidth and halfHeight
                    cells[i].GetGameObject.transform.position = new Vector3( (float) row - halfWidth,
                                                                            (float) col + halfHeight);
                    
                    // Add a Sprite Renderer to the cell
                    cells[i].GetGameObject.AddComponent<SpriteRenderer>();
                    i++; // increment i - index of the cell, every loop
                }
            }
        }
    }

    IDictionary<int, int> lowestEntropy = new Dictionary<int, int>(); // list of cells with lowest count of candidates
    int selectedSpriteIndex, selectedCellIndex; // will be used later when randomly selecting sprite and cell
    public void ChooseCell() // Choose Cell to assign a sprite
    {
        // Reset lowestEntropy every call --
        lowestEntropy.Clear();
        lowestEntropy.Add(-1, int.MaxValue);
        // ---------------------------------

        for(int i = 0; i < gridSize; i++) // loop through all cells to find the ones with the lowest candidate count/ entropy
        {
            if(cells[i].IsFilled == false) // make sure that the cell is not yet filled
            {
                if(cells[i].GetCandidates.Count <= lowestEntropy.ElementAt(0).Value) // IF the cell has lower or equal count/ entropy
                {
                    // Clear cell if the entropy/ candidate coutn is not equal (replace the list with new ones)
                    if(cells[i].GetCandidates.Count != lowestEntropy.ElementAt(0).Value) lowestEntropy.Clear(); 
                    
                    // Add the current cell to the list
                    lowestEntropy.Add(i, cells[i].GetCandidates.Count);
                }
            }
        }

        // Get a random index of a cell to choose. This cell is one of the cells with the lowest entropy/candidates count
        selectedCellIndex = lowestEntropy.ElementAt(Random.Range(0, lowestEntropy.Count)).Key;

        // Get a random index of a sprite from the candidates list
        selectedSpriteIndex = Random.Range(0, cells[selectedCellIndex].GetCandidates.Count);

        // Assign a sprite to the cell    
        AssignSpriteToCell(cells[selectedCellIndex], selectedSpriteIndex);

        // Check and adjust the candidates of the adjacent cells
        checkAdjacentCell.StartCheckCell(cells, selectedCellIndex, height, width, cellSprite.spriteLists);
        
        // A Debug script to display the process of elimination

        if(UnityEditor.EditorApplication.isPlaying) DebugDisplay.UpdateCanvas(cells.Select(c => c.GetCandidates).ToList());
    }

    public void AssignSpriteToCell(Cell cell, int spriteIndex)
    {
        //if(spriteIndex > cellSprites.Length - 1) spriteIndex = cellSprites.Length - 1;

        // Assign a sprite to the selected cell
        cell.AssignSprite(cell.GetCandidates[spriteIndex]);
    }
}
