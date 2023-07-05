using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DebugUI;

public class ManagerScript_1DArray : MonoBehaviour
{
    public WaveFunction1D_Array createGrid;
    public CellSprite gamePrefab;
    
    void Awake()
    {
        DebugDisplay.DrawCanvas(gamePrefab, ((float)createGrid.width - 1f) / 2f, ((float)createGrid.height - 1f) / -2f, createGrid.width, createGrid.height);
    }

    /* public void DrawDebugFromEditor()
    {
        DebugDisplay.DrawCanvas(gamePrefab, ((float)createGrid.width - 1f) / 2f, ((float)createGrid.height - 1f) / -2f, createGrid.width, createGrid.height);
    } */
}
