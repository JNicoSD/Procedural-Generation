using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DebugUI;
using SimpleRandomizer;

public class WaveFunction1D_Array : MonoBehaviour
{
    public ExtendGridCells extendGridCells;
    
    List<Cell> cells = new List<Cell>();
    public List<Cell> GetCells { get { return cells; } }

    public CellSprite cellSprite;
        
    public int height, width;
    public float startingPositionX, startingPositionY;
    
    [Range(0.1f, 10f)]
    public float renderSpeed = 5.0f;
    public CheckAdjacentCell checkAdjacentCell;
    [HideInInspector] public int gridSize;
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

        //cells = new Cell[gridSize];
        cells = new List<Cell>();

        spriteCandidates.Clear(); // make sure spriteCandidates is empty
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

        //cells = new Cell[gridSize];

        spriteCandidates.Clear(); // make sure spriteCandidates is empty
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

        //extendGridCells.ExtendTop();
    }
    void Update()
    {
        /* ChooseCell();
        if(Input.GetKeyDown(KeyCode.W))
        {
            extendGridCells.ExtendTop();
            startingPositionY = 0;
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            extendGridCells.ExtendRight();
        } */
    }

    List<GameObject> tileGroup = new List<GameObject>();
    public List<GameObject> GetTileGroup { get{ return tileGroup;} }
    int tileGroupIndex = -1;
    public int TileGroupIndex { set { tileGroupIndex = value; } get{ return tileGroupIndex;} }

    public void InsertGridCellsRight(int prevWidth) // Create new cells to the right
    {   
        for(int i = prevWidth; i < gridSize; )
        {

            // Create new cell ( name: , candidates: )
            cells.Insert(i, new Cell($"Cell: new{i}", spriteCandidates));

            // Set tileGroup as the cells parent
            cells[i].GetGameObject.transform.SetParent(tileGroup[tileGroupIndex].transform);
            // Position the cell to the center using halfWidth and halfHeight
            cells[i].GetGameObject.transform.position = new Vector3( cells[i - 1].GetGameObject.transform.position.x + 1,
                                                                     cells[i - 1].GetGameObject.transform.position.y);
            
            // Add a Sprite Renderer to the cell
            cells[i].GetGameObject.AddComponent<SpriteRenderer>();

            i = (i + 1) % width == 0 ? i + prevWidth + 1 : i + 1;   
        } 
    }

    public void InsertGridCellsLeft(int prevWidth, int baseWidth) // Create new cells to the right
    {   
        for(int i = 0, tempIndex = 1; i < gridSize - prevWidth; )
        {
            // Create new cell ( name: , candidates: )
            cells.Insert(i, new Cell($"Cell: new{i}", spriteCandidates));

            // Set tileGroup as the cells parent
            cells[i].GetGameObject.transform.SetParent(tileGroup[tileGroupIndex].transform);

            try{
            cells[i].GetGameObject.transform.position = new Vector3( cells[i + tempIndex].GetGameObject.transform.position.x - baseWidth,
                                                                     cells[i + tempIndex].GetGameObject.transform.position.y);
            } catch{
                Debug.Log($"{cells[150].GetGameObject.name}");
                break;
            }
            // Add a Sprite Renderer to the cell
            cells[i].GetGameObject.AddComponent<SpriteRenderer>();

            //i = ((i + prevWidth) + 1) % width == 0 ? i + prevWidth + 1 : i + 1;
            if((((i + prevWidth) + 1) % width == 0) == false)
            {
                i++;
                tempIndex++;
            } else
            {
                i += prevWidth + 1;
                tempIndex = 1;
            }
        } 
    }

    public void InsertGridCellsAbove(int tempGridSize) // Create new cells above
    {   
        for(int i = tempGridSize; i < gridSize; i++)
        {
            // Create new cell ( name: , candidates: )
            cells.Add(new Cell($"Cell: {i}", spriteCandidates));

            // Set tileGroup as the cells parent
            cells[i].GetGameObject.transform.SetParent(tileGroup[tileGroupIndex].transform);
            // Position the cell to the center using halfWidth and halfHeight
            cells[i].GetGameObject.transform.position = new Vector3( cells[i - width].GetGameObject.transform.position.x,
                                                                     cells[i - width].GetGameObject.transform.position.y + 1);
            
            // Add a Sprite Renderer to the cell
            cells[i].GetGameObject.AddComponent<SpriteRenderer>();
        }
    }

    public void InsertGridCellsBelow(int baseHeight, int tempGridSize) // Create new cells above
    {   
        for(int i = 0; i < tempGridSize; i++)
        {
            // Create new cell ( name: , candidates: )
            cells.Insert(i, new Cell($"Cell: new{i}", spriteCandidates));

            // Set tileGroup as the cells parent
            cells[i].GetGameObject.transform.SetParent(tileGroup[tileGroupIndex].transform);

            // Add a Sprite Renderer to the cell
            cells[i].GetGameObject.AddComponent<SpriteRenderer>();

            // Position the cell to the center using halfWidth and halfHeight
            cells[i].GetGameObject.transform.position = new Vector3(cells[i + i + 1].GetGameObject.transform.position.x,
                                                                    cells[i + i + 1].GetGameObject.transform.position.y - baseHeight);
        }
        // Once everything is created, assign position;
        /* for(int i = 0; i < tempGridSize; i++)
        {
            
        } */
    }
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
            tileGroup[tileGroupIndex].AddComponent<CompositeCollider2D>();
            Rigidbody2D rb = tileGroup[tileGroupIndex].GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;    
            //tileGroup[tileGroupIndex].AddComponent<RigidBody2D>();
            //if(tileGroupIndex > 0) tileGroup[tileGroupIndex-1].SetActive(false); // Hide previous tileGroup
        
            for(int col = 0, i = 0; col < height; col++)
            {
                for(int row = 0; row < width; row++)
                {
                    // Create new cell ( name: , candidates: )
                    //cells[i] = new Cell($"Cell: {i}", spriteCandidates);
                    cells.Add(new Cell($"Cell: {i}", spriteCandidates));

                    // Set tileGroup as the cells parent
                    cells[i].GetGameObject.transform.SetParent(tileGroup[tileGroupIndex].transform);
                    // Position the cell to the center using halfWidth and halfHeight
                    cells[i].GetGameObject.transform.position = new Vector3( ((float)row - halfWidth) + startingPositionX,
                                                                            ((float)col + halfHeight) + startingPositionY);
                    
                    // Add a Sprite Renderer to the cell
                    cells[i].GetGameObject.AddComponent<SpriteRenderer>();
                    i++; // increment i - index of the cell, every loop
                }
            }
        }
    }

#region ChooseCell
    WeightedRandom wRand = new WeightedRandom(); 
    IDictionary<int, float> lowestEntropy = new Dictionary<int, float>(); // data of cells with lowest count of candidates
    
    IDictionary<CellSprite.SpriteList, float> currentCandidates; // data of current candidates, will be used for weighted random
    CellSprite.SpriteList selectedSprite; // will hold the selected CellSprite.SpriteList of the weighted random
    
    int selectedCellIndex; // will hold the selected cell of the randomizer
    
    public void ChooseCell() // Choose Cell to assign a sprite
    {
        currentCandidates = new Dictionary<CellSprite.SpriteList, float>();
        // Reset lowestEntropy every call --
        lowestEntropy.Clear();
        lowestEntropy.Add(-1, float.MaxValue);
        // ---------------------------------

        #region DEPRECATED - OLD ENTROPY COMPARISON (BASE ON CANDIDATE COUNT)
        /* for(int i = 0; i < gridSize; i++) // loop through all cells to find the ones with the lowest candidate count/ entropy
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
        } */
        #endregion
        
        float entropy;

        for(int i = 0; i < gridSize; i++) // loop through all cells to find the ones with the lowest entropy
        {
            if(cells[i].IsFilled == false) // make sure that the cell is not yet filled
            {
                entropy = EntropyCalculator(cells[i].GetCandidates);
                if(entropy <= lowestEntropy.ElementAt(0).Value) // IF the cell has lower or equal entropy
                {
                    // Clear cell if the entropy is not equal (replace the list with new ones)
                    if(entropy != lowestEntropy.ElementAt(0).Value) lowestEntropy.Clear(); 
                    
                    // Add the current cell to the list
                    lowestEntropy.Add(i, entropy);
                }
            }
        }

        if(lowestEntropy.Count > 0 && lowestEntropy.ElementAt(0).Key != -1) // Prevents Error
        {
            // Get a random index of a cell to choose. This cell is one of the cells with the lowest entropy/candidates count
            selectedCellIndex = lowestEntropy.ElementAt(UnityEngine.Random.Range(0, lowestEntropy.Count)).Key;
            
            // Get a random CellSprite.SpriteList from the current candidates list
            currentCandidates = cells[selectedCellIndex].GetCandidates.ToDictionary(o => o, o => o.GetWeight); // create a dictionary that contains the CellSprite.SpriteList, and Weight
            selectedSprite = wRand.WeightedRandomKey<CellSprite.SpriteList>(currentCandidates); // Randomize based on weight value
            
            // Assign a sprite to the cell    
            AssignSpriteToCell(cells[selectedCellIndex], selectedSprite);

            // Check and adjust the candidates of the adjacent cells
            checkAdjacentCell.StartCheckCell(cells, selectedCellIndex, height, width, cellSprite.spriteLists);
            
            // A Debug script to display the process of elimination
            if(UnityEditor.EditorApplication.isPlaying) DebugDisplay.UpdateCanvas(cells.Select(c => c.GetCandidates).ToList());
        }
    }
#endregion

    public void AssignSpriteToCell(Cell cell, CellSprite.SpriteList _selectedSprite)
    {
        //if(spriteIndex > cellSprites.Length - 1) spriteIndex = cellSprites.Length - 1;

        // Assign a sprite to the selected cell
        cell.AssignSprite(_selectedSprite);
        if(_selectedSprite.hasCollider == true) cell.GetGameObject.AddComponent<BoxCollider2D>().usedByComposite = true;
    }

    private float EntropyCalculator(List<CellSprite.SpriteList> cs)
    {
        float entropy = 0;
        float totalSum = 0;
        // h(x) = summation of p(x) * log2(px)
        foreach(CellSprite.SpriteList c in cs)
        {
            totalSum += c.GetWeight;
        }
        
        foreach(CellSprite.SpriteList c in cs)
        {
            entropy -= (c.GetWeight/totalSum) * Mathf.Log((c.GetWeight/totalSum), 2);
        }
        return entropy;
    }
}
