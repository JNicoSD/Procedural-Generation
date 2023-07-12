using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendGridCells : MonoBehaviour
{
    public WaveFunction1D_Array waveFunction1D_Array;
    private int baseHeight = -1, baseWidth = -1;
    private int prevHeight = -1, prevWidth = -1;
    private int tempGridSize = -1;
    public void ExtendTop() // Adds a new grid on top of the existing one
    {
        if(baseHeight == -1) baseHeight = waveFunction1D_Array.height; // Sets how high will be added
        prevHeight = waveFunction1D_Array.height;

        tempGridSize = waveFunction1D_Array.gridSize; // Store previous grid size. To be used later for calculations

        //waveFunction1D_Array.startingPositionY = waveFunction1D_Array.height; // Assign new starting position. In this case, ABOVE the previous grid
        
        
        waveFunction1D_Array.height += baseHeight;

        waveFunction1D_Array.gridSize = waveFunction1D_Array.height * waveFunction1D_Array.width;

        waveFunction1D_Array.InsertGridCellsAbove(tempGridSize);

        for(int i = 0; i < waveFunction1D_Array.width; i++)
        {
            waveFunction1D_Array.checkAdjacentCell.StartCheckCell(  waveFunction1D_Array.GetCells, 
                                                                    tempGridSize - waveFunction1D_Array.width + i, 
                                                                    waveFunction1D_Array.height, 
                                                                    waveFunction1D_Array.width, 
                                                                    waveFunction1D_Array.cellSprite.spriteLists);
        }

        if(Application.isPlaying == false) waveFunction1D_Array.StartFromEditor();
        //waveFunction1D_Array.startingPositionY = 0;

        /* waveFunction1D_Array.startingPositionY += waveFunction1D_Array.height;
        waveFunction1D_Array.DrawFromEditor();
        waveFunction1D_Array.StartFromEditor(); */
    }
    public void ExtendDown()
    {
        if(baseHeight == -1) baseHeight = waveFunction1D_Array.height; // Sets how high will be added
        prevHeight = waveFunction1D_Array.height;


        
        waveFunction1D_Array.height += baseHeight;

        waveFunction1D_Array.gridSize = waveFunction1D_Array.height * waveFunction1D_Array.width;

        tempGridSize = baseHeight * waveFunction1D_Array.width;
        waveFunction1D_Array.InsertGridCellsBelow(baseHeight, tempGridSize);
        

        for(int i = 0; i < waveFunction1D_Array.width; i++)
        {
            waveFunction1D_Array.checkAdjacentCell.StartCheckCell(  waveFunction1D_Array.GetCells, 
                                                                    tempGridSize + i, 
                                                                    waveFunction1D_Array.height, 
                                                                    waveFunction1D_Array.width, 
                                                                    waveFunction1D_Array.cellSprite.spriteLists);
        }

        if(Application.isPlaying == false) waveFunction1D_Array.StartFromEditor();
    }
    public void ExtendRight()
    {
        if(baseWidth == -1) baseWidth = waveFunction1D_Array.width;
        prevWidth = waveFunction1D_Array.width;

        waveFunction1D_Array.width += baseWidth;

        waveFunction1D_Array.gridSize = waveFunction1D_Array.height * waveFunction1D_Array.width;
        waveFunction1D_Array.InsertGridCellsRight(prevWidth);

        for(int i = prevWidth - 1; i < waveFunction1D_Array.gridSize; i += waveFunction1D_Array.width)
        {
            waveFunction1D_Array.checkAdjacentCell.StartCheckCell(  waveFunction1D_Array.GetCells, 
                                                                    i,
                                                                    waveFunction1D_Array.height, 
                                                                    waveFunction1D_Array.width, 
                                                                    waveFunction1D_Array.cellSprite.spriteLists);
        }

        if(Application.isPlaying == false) waveFunction1D_Array.StartFromEditor();
    }
    public void ExtendLeft()
    {
        if(baseWidth == -1) baseWidth = waveFunction1D_Array.width;
        prevWidth = waveFunction1D_Array.width;

        waveFunction1D_Array.width += baseWidth;

        waveFunction1D_Array.gridSize = waveFunction1D_Array.height * waveFunction1D_Array.width;

        tempGridSize = waveFunction1D_Array.height * baseWidth;

        waveFunction1D_Array.InsertGridCellsLeft(prevWidth, baseWidth);

        for(int i = baseWidth; i < waveFunction1D_Array.gridSize; i += waveFunction1D_Array.width)
        {
            waveFunction1D_Array.checkAdjacentCell.StartCheckCell(  waveFunction1D_Array.GetCells, 
                                                                    i,
                                                                    waveFunction1D_Array.height, 
                                                                    waveFunction1D_Array.width, 
                                                                    waveFunction1D_Array.cellSprite.spriteLists);
        }

        if(Application.isPlaying == false) waveFunction1D_Array.StartFromEditor();
    }
}
