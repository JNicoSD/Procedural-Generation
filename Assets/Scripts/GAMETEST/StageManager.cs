using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public enum Direction
    {
        Top,
        Right,
        Down,
        Left
    }
    public WaveFunction1D_Array waveFunction1D_Array;
    public ExtendGridCells extendGridCells;

    [HideInInspector]public int baseHeight, baseWidth;
    [HideInInspector]public float baseStartingX, baseStartingY;

    Direction direction;
    
    public void CallInitialDraw()
    {
        waveFunction1D_Array.startingPositionX = 0;
        waveFunction1D_Array.startingPositionY = 0;

        waveFunction1D_Array.DrawFromEditor();
        waveFunction1D_Array.StartFromEditor();

    }

    public void CallDraw(Direction direction)
    {
        if(direction == Direction.Top) 
        {
            //waveFunction1D_Array.startingPositionY += waveFunction1D_Array.height;
            extendGridCells.ExtendTop();
        } else if(direction == Direction.Right) 
        {
            extendGridCells.ExtendRight();
        } else if(direction == Direction.Down) 
        {
            extendGridCells.ExtendDown();
        } else {
            extendGridCells.ExtendLeft();
        }

        //waveFunction1D_Array.DrawFromEditor();
        //waveFunction1D_Array.StartFromEditor();
    }
}
