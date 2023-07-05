using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class CheckAdjacent : MonoBehaviour
{   
    //[Flags]
    enum Direction{ // No need for Flags attribute that's why I use 0,1,2,3...
        //None = 0
        Top = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
    int width, height;
    int baseY, baseX;

    ObjectHandler<int>[,] obj;
    Tiles<int>[] tile;
    //List<Action<int, int, Direction>> lookDirection;

    //public DebugCanvas debugCanvas;
    public void StartCheck(ObjectHandler<int>[,] _obj, Tiles<int>[] _tile, int x, int y, int w, int h)
    {
        width = w;
        height = h;
        baseX = x;
        baseY = y;
        obj = _obj;
        tile = _tile;

        // Debug.Log(" --- START --- ");
        /* if(lookDirection == null)
        {
            lookDirection = new List<Action<int, int, Direction>>
            {
                (x, y, direction) => LookDirection(direction, x, y),
                (x, y, direction) => UpdateDirection(direction, x, y)
            };
        } */

        /* foreach(CheckAdjacent.Direction directon in Enum.GetValues(typeof(Direction)))
        {
            Look(x, y, directon);
        } */
        
        for(int i = 0; i < Enum.GetValues(typeof(Direction)).Length; i++)
        {
            LookDirection((Direction)i, x, y);
        }
    }
    void LookDirection(Direction direction, int x, int y)
    {
        if(direction == Direction.Top)
        {
            for( ; y < height ; y++)
            {
                UpdateDirection(direction, x, y);
                UpdateDirection(Direction.Right, x, y+1);
                UpdateDirection(Direction.Left, x, y+1);
            }
        } else if(direction == Direction.Right)
        {
            for( ; x < width ; x++)
            {
                UpdateDirection(direction, x, y);
                UpdateDirection(Direction.Top, x+1, y);
                UpdateDirection(Direction.Down, x+1, y);
            }
        } else if(direction == Direction.Down)
        {
            for( ; y >= 0 ; y--)
            {
                UpdateDirection(direction, x, y);
                UpdateDirection(Direction.Right, x, y-1);
                UpdateDirection(Direction.Left, x, y-1); 
            }
        } else if(direction == Direction.Left)
        {
            for( ; x >= 0 ; x--)
            {
                UpdateDirection(direction, x, y);
                UpdateDirection(Direction.Top, x-1, y);
                UpdateDirection(Direction.Down, x-1, y);
            }
        }
    }
    
    List<int> newCandidates = new List<int>();
    int currX, currY;
    (int, int) ruleDirection;
    void UpdateDirection(Direction direction, int prevX, int prevY)
    {
        newCandidates.Clear();

        if(direction == Direction.Top)
        {
            currX = prevX;
            currY = prevY + 1;
            ruleDirection = (0, 2);
        } else if(direction == Direction.Down)
        {
            currX = prevX;
            currY = prevY - 1;
            ruleDirection = (2, 0);
        } else if(direction == Direction.Right)
        {
            currX = prevX + 1;
            currY = prevY;
            ruleDirection = (1, 3);
        } else if(direction == Direction.Left)
        {
            currX = prevX - 1;
            currY = prevY;
            ruleDirection = (3, 1);
        }
       

        if((currX >= 0 && currX < width) && (currY >= 0 && currY < height) && obj[currX, currY].IsFilled() == false)
        { 
            if(obj[prevX, prevY].GetSpriteHandler() != null) // IF *THERE* IS A SPRITE ASSIGNED
            {
                for(int i = 0; i < tile.Length; i++)
                {
                    /*
                     - Get Sprite rule of the assigned sprite
                     - Compare it to the rule index 2 of the *possible tiles on [Direction]*
                        - IF EQUAL -> ADD it to list newCandidates
                        - IF NOT EQUAL -> DO NOTHING
                    */
                    if(obj[prevX, prevY].GetSpriteHandler().GetRules()[ruleDirection.Item1] == tile[i].GetRules()[ruleDirection.Item2])
                    {
                        newCandidates.Add(i);
                    }
                } 
            } else // IF *NO* SPRITE ASSIGNED
            {
                foreach(var prev in obj[prevX,prevY].GetCandidates()) // Get Candidates of previous obj
                {
                    /*
                     - Get *one of the* possible tile
                     - Get the rules index 1 of the tile
                     - Compare it to the rule index 2 of the *possible tiles on [Direction]*
                        + IF EQUAL -> ADD it to list newCandidates
                        + IF NOT EQUAL -> DO NOTHING
                    */
                    for(int i = 0; i < tile.Length; i++)
                    {
                        if(tile[prev].GetRules()[ruleDirection.Item1] == tile[i].GetRules()[ruleDirection.Item2])
                        {
                            newCandidates.Add(i);
                        }
                    }
                }
            }

            obj[currX, currY].UpdateCandidates(newCandidates); // Update CURRENT obj
            
            //obj[x,y].RemoveCandidates(newCandidates);
        }
    }
    
}
