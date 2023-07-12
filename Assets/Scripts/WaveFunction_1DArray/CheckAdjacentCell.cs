using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DebugUI;
public class CheckAdjacentCell : MonoBehaviour
{
    public enum Direction{
        Top = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    List<Cell> cells = new List<Cell>();
    int height, width, gridSize;
    CellSprite.SpriteList[] spriteList;

    public void StartCheckCell(List<Cell> _cells, int _cellIndex, int _height, int _width, CellSprite.SpriteList[] _spriteList)
    {   
        // I did this so it can be accessed by the whole script
        //if(this.cells != null) this.cells.Clear();
        this.cells = _cells;
        //foreach(Cell c in _cells) this.cells.Add(c);

        this.height = _height;
        this.width = _width;
        this.spriteList = _spriteList;

        this.gridSize = _width * _height;

        // The pattern I set is Check TOP -> RIGHT -> DOWN -> LEFT
        // BUT... when checking TOP and DOWN cell, it will also check the ones on its RIGHT and LEFT
        // AND... when checking RIGHT and LEFT cell, it will also check the ones on TOP and DOWN/below of it
        // Check each method to see ^^^^
        CheckTopCell(Direction.Top, _cellIndex);
        
        CheckRightCell(Direction.Right, _cellIndex);
        
        CheckDownCell(Direction.Down, _cellIndex);
        
        CheckLeftCell(Direction.Left, _cellIndex);
    }

    #region Check[Direction]Cell
    /*  maths...
        index >= (height * width) - width ===> indices at the very top
        index >= 0                        ===> indices at the very bottom
        (index + 1) % width == 0          ===> indices at the very right
        index % width == 0                ===> indices at the very left
    */
    public void CheckTopCell(Direction direction, int index)
    {   
        // index < height * width checks if the current index is at the very top
        // IF it is, it does not continue the loop
        int maxSize = height * width;
        for( ; index < maxSize ; index += width)
        {
            // Update Cell. Pass Direction.Top since we are checking the top cell
            // also pass the current Index which will be the basis of the checking (will become previous Cell in UpdateCell)
            UpdateCell(direction, index);

            // ((index + 1) % width != 0) basically checks if it is already on the very right
            // IF it is not, hence the !=, Update the cell on the right
            if((index + 1) % width != 0) UpdateCell(Direction.Right, index + width);

            // (index % width != 0) basically checks if it is already on the very left
            // IF it is not, hence the !=, Update the cell on the left
            if(index % width != 0) UpdateCell(Direction.Left, index + width);
        }
    }
    public void CheckRightCell(Direction direction, int index)
    {
        // (index + 1) % width != 0 checks if the current index is at the very right
        // IF it is, it does not continue the loop
        for( ; (index + 1) % width != 0; index++)
        {
            // Update Cell. Pass Direction.Down since we are checking the top cell
            // also pass the current Index which will be the basis of the checking (will become previous Cell in UpdateCell)
            UpdateCell(direction, index);

            // (index < (height * width) - width) basically checks if it is already at the very top
            // IF it is not, hence the >, Update the cell on the top
            if(index < (height * width) - width) UpdateCell(Direction.Top, index + 1);

            // (index >= width) basically checks if it is already at the very bottom
            // IF it is not, hence the <=, Update the cell below
            if(index >= width) UpdateCell(Direction.Down, index + 1);
        }
    }
    public void CheckDownCell(Direction direction, int index)
    {
        // index >= 0 checks if the current index is at the very bottom
        // IF it is, it does not continue the loop
        for( ; index >= 0 ; index -= width)
        {   
            // Update Cell. Pass Direction.Down since we are checking the top cell
            // also pass the current Index which will be the basis of the checking (will become previous Cell in UpdateCell)
            UpdateCell(direction, index);

            // ((index + 1) % width != 0) basically checks if it is already on the very right
            // IF it is not, hence the !=, Update the cell on the right
            if((index + 1) % width != 0) UpdateCell(Direction.Right, index - width);

            // (index % width != 0) basically checks if it is already on the very left
            // IF it is not, hence the !=, Update the cell on the left
            if(index % width != 0) UpdateCell(Direction.Left, index - width);
        }
    }
    public void CheckLeftCell(Direction direction, int index)
    {   
        // index % width != 0 checks if the current index is at the very left
        // IF it is, it does not continue the loop
        for( ;  index % width != 0; index--)
        {
            // Update Cell. Pass Direction.Left since we are checking the top cell
            // also pass the current Index which will be the basis of the checking (will become previous Cell in UpdateCell)
            UpdateCell(direction, index);

            // (index < (height * width) - width) basically checks if it is already at the very top
            // IF it is not, hence the >, Update the cell on the top
            if(index < (height * width) - width) UpdateCell(Direction.Top, index - 1);

            // (index >= width) basically checks if it is already at the very bottom
            // IF it is not, hence the <=, Update the cell below
            if(index >= width) UpdateCell(Direction.Down, index - 1);
        }
    }
    #endregion

    List<CellSprite.SpriteList> newCandidates = new List<CellSprite.SpriteList>(); // will serve as the list of the new candidates to assign to the cell
    int currIndex; // will be used to get the current cell
    
    /*
        ruleDirection is used to compare the indices of rules:
              
         +-------+
         |   0   |
         | 1   3 |
         |   2   |
         +-------+
            
        So we compare:
            0 -> 2 when the base tile is using its TOP side
            1 -> 3 when the base tile is using its RIGHT side
            2 -> 0 when the base tile is using its DOWN side
            3 -> 1 when the base tile is using its LEFT side
    */
    (int, int) ruleDirection; 
    void UpdateCell(Direction direction, int prevIndex)
    {
        // Clear new candidates every call to make sure nothing is added unintentionally
        newCandidates.Clear();

        // Depending on the direction given, we assign a value to currIndex
        if(direction == Direction.Top)
        {
            currIndex = prevIndex + width; // + width to get check the one ABOVE (TOP) the passed Index (which is now prevIndex)
            ruleDirection = (0, 2); // 0 -> 2 when the base tile is using its TOP side
        } else if(direction == Direction.Down)
        {
            currIndex = prevIndex - width; // - width to get check the one BELOW (DOWN) the passed Index (which is now prevIndex)
            ruleDirection = (2, 0); // 2 -> 0 when the base tile is using its DOWN side
        } else if(direction == Direction.Right)
        {
            currIndex = prevIndex + 1; // +1 to get check the one on the RIGHT of the passed Index (which is now prevIndex)
            ruleDirection = (1, 3); // 1 -> 3 when the base tile is using its RIGHT side
        } else if(direction == Direction.Left)
        {
            currIndex = prevIndex - 1; // -1 to get check the one on the LEFT of the passed Index (which is now prevIndex)
            ruleDirection = (3, 1); // 3 -> 1 when the base tile is using its LEFT side
        }


        if((currIndex >= 0 && currIndex < gridSize) == false) // This makes sure that the current index is not out of range 
        { 
            // do nothing...
        } else if(cells[currIndex].IsFilled == false) // Also skips checking the current if it is already filled (basically, it has already been adjusted before)
        {
            if(cells[prevIndex].GetSprites != null) // IF PREVIOUS INDEX *HAS* A SPRITE ASSIGNED
            {
                    for(int i = 0; i < spriteList.Length; i++) // loop base on cell sprite length
                    {
                        /*
                            - Get Sprite rule of the assigned sprite
                            - Compare it to the rule index: [depending on direction] of the *possible tiles on [Direction]*
                                - IF EQUAL -> ADD it to list newCandidates
                                - IF NOT EQUAL -> DO NOTHING
                        */
                        if(cells[prevIndex].GetSprites.GetSpriteRules[ruleDirection.Item1] == spriteList[i].GetSpriteRules[ruleDirection.Item2]) // This basically compares if the two rules are equal
                        {
                            newCandidates.Add(spriteList[i]);
                        }
                    }
            } else // IF PREVIOUS INDEX HAS *NO* SPRITE ASSIGNED
            {
                foreach(CellSprite.SpriteList sprite in cells[prevIndex].GetCandidates) // loop through all cell candidates of the previous cell/Index
                {
                    for(int i = 0; i < spriteList.Length; i++) // loop base on cell sprite length
                    {
                        /*
                            - Get Sprite rule of the current sprite
                            - Compare it to the rule index: [depending on direction] of the *possible tiles on [Direction]*
                                - IF EQUAL -> ADD it to list newCandidates
                                - IF NOT EQUAL -> DO NOTHING
                        */
                        if(sprite.GetSpriteRules[ruleDirection.Item1] == spriteList[i].GetSpriteRules[ruleDirection.Item2]) // This basically compares if the two rules are equal
                        {
                            newCandidates.Add(spriteList[i]); 
                        }
                    }
                }
            }
            cells[currIndex].UpdateCandidates(newCandidates); // Update the cell candidates of the current cell
        }
        
    }

}
