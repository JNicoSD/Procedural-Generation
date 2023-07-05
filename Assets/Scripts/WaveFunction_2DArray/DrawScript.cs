using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DrawScript : MonoBehaviour
{
    //public GameObject parent; // Parent
    ObjectHandler<int>[,] gridCell;
    public ObjectHandler<int>[,] GridCell { get { return gridCell; } }
    public Tiles<int>[] tileSprite;
    public int width, height;
    float halfWidth, halfHeight;
    public float drawSpeed = 0.5f;
    public CheckAdjacent checkAdjacent;
    void Awake()
    {
        halfWidth = (float)(width - 1) / 2;
        halfHeight = (float)(height - 1) / -2;

        DrawGrid();

        /*for(int i = 0; i < width*height; i++)
        {
            PickStartPoint();
            UpdateAdjacentCandidate(1,1,1,1);
        }*/

    }
    void Start()
    {
        StartCoroutine(StartDraw());
    }

    IEnumerator StartDraw()
    {
        for(int i = 0; i < width*height; i++)
        {
            PickStartPoint();
            yield return new WaitForSeconds(drawSpeed);
        }
    }

    public void DrawGrid()
    {
        if(tileSprite.Length > 0)
        {
            gridCell = new ObjectHandler<int>[width, height]; // Set gridPoint size
            
            List<int> candidates = new List<int>(); // create a list of candidates
            int c = 0;
            foreach(Tiles<int> t in tileSprite) // size of candidates is based on the number of sprites
            {
                candidates.Add(c);
                c++;
            }
            // Initialize gridPoints
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    //gridPoint.Add((x,y), new ObjectHandler<int>($"tile x:{x} y:{y}", x, y, test2));
                    
                    // Add grid a cell
                    AddGridCell(out gridCell[x,y], x, y, candidates);
                }
            }
        }
    }

    void AddGridCell(out ObjectHandler<int> cell, int x, int y, List<int> candidates)
    {
        //g = new GameObject(name);
        cell = new ObjectHandler<int>($"tile x:{x} y:{y}", x, y, candidates);
        cell.GetObj().transform.position = new Vector3((float)x - halfWidth, (float)y + halfHeight); // Set Position
        cell.GetObj().AddComponent<SpriteRenderer>(); // For Drawing Sprite
    }

    IDictionary<(int,int), int> lowestEntropy = new Dictionary<(int,int), int>(); // Will contain gridPoints and its indices with the lowest entropy
    int selectedX, selectedY, spriteIndex;
    void PickStartPoint()
    {
        /* Clear lowestEntropy everytime PickStartPoint is called.
        
        */
        lowestEntropy.Clear();
        lowestEntropy.Add((-1, -1), int.MaxValue);

        // Check all gridPoints' candidates
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if(gridCell[x,y].IsFilled() == false) // Exclude checking grids with 0 possibility (probably already filled)
                {                
                    // IF the current checked gridPoint has lower number of candidates, clear the dictionary and assign the new lowestEntropy values
                    if(gridCell[x,y].GetCandidates().Count < lowestEntropy.ElementAt(0).Value)
                    {
                        lowestEntropy.Clear(); // Remove list since it contains higherEntropy
                        lowestEntropy.Add( (x,y), gridCell[x,y].GetCandidates().Count ); // Then replace with the lowerEntropy
                    }
                    // IF the current checked gridPoint has the same number of candidates, add the new values to the lowestEntropy dictionary
                    else if(gridCell[x,y].GetCandidates().Count == lowestEntropy.ElementAt(0).Value)
                    {
                        lowestEntropy.Add( (x,y), gridCell[x,y].GetCandidates().Count ); // Add cell with == entropy
                    }
                }
            }
        }

        // Get a random gridCell from the dictionary containing the ones with the lowest entropy
        (int,int) selectedGridCell = lowestEntropy.ElementAt(Random.Range(0, lowestEntropy.Count)).Key;
        
        selectedX = selectedGridCell.Item1;
        selectedY = selectedGridCell.Item2;

        spriteIndex = gridCell[selectedX,selectedY].GetCandidates()[Random.Range(0, gridCell[selectedX,selectedY].GetCandidates().Count)];

        //Debug.Log($"RAND INT: {randInt} --- SPRITE INDEX: {spriteIndex} --- COUNT: {System.String.Join(", ", gridPoint[_x,_y].GetCandidates())}");
        //int spriteIndex = Random.Range(0,gridPoint.GetCandidates(x,y).Count-1);

        DrawSprite(gridCell[selectedX,selectedY], spriteIndex);
        
        //StartCoroutine(checkAdjacent.StartCheck(_x, _y, width, height));
        checkAdjacent.StartCheck(gridCell, tileSprite, selectedX, selectedY, width, height);
    }

    void DrawSprite(ObjectHandler<int> obj, int index)
    {
        if(index > tileSprite.Length - 1) index = tileSprite.Length - 1;

        obj.AssignSprite(tileSprite[index]);

       // g.GetComponent<SpriteRenderer>().sprite = tileSprite[index].GetSprite();
    }

}