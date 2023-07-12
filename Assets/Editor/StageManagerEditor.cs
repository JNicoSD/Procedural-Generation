using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor (typeof(StageManager))]
public class StageManagerEditor : Editor
{
    bool isInitialDrawn = false;
    public override void OnInspectorGUI()
    {
        StageManager stageManager = (StageManager)target;

        DrawDefaultInspector();

        if(isInitialDrawn == true)
        {
            if(GUILayout.Button("Generate Map: TOP"))
            {
                stageManager.CallDraw(StageManager.Direction.Top);
            } else if(GUILayout.Button("Generate Map: DOWN"))
            {
                stageManager.CallDraw(StageManager.Direction.Down);
            } else if(GUILayout.Button("Generate Map: RIGHT"))
            {
                stageManager.CallDraw(StageManager.Direction.Right);
            } else if(GUILayout.Button("Generate Map: LEFT"))
            {
                stageManager.CallDraw(StageManager.Direction.Left);
            }
        } else
        {
            if(GUILayout.Button("Generate Initial Map"))
            {
                stageManager.CallInitialDraw();
                Debug.Log("TILE MAP GENERATED");

                isInitialDrawn = true;

                stageManager.baseHeight = stageManager.waveFunction1D_Array.height;
                stageManager.baseWidth = stageManager.waveFunction1D_Array.width;    
                stageManager.baseStartingX = stageManager.waveFunction1D_Array.startingPositionX;
                stageManager.baseStartingY = stageManager.waveFunction1D_Array.startingPositionY;
            }
        }




        if(GUILayout.Button("Remove ALL Generated Tilemap"))
        {
             foreach(GameObject g in stageManager.waveFunction1D_Array.GetTileGroup)
            {
                DestroyImmediate(g);
            }
            stageManager.waveFunction1D_Array.GetCells.Clear();
            stageManager.waveFunction1D_Array.TileGroupIndex = -1;
            stageManager.waveFunction1D_Array.GetTileGroup.Clear(); 
            Debug.Log("TILEMAPS REMOVED");

            isInitialDrawn = false;

            stageManager.waveFunction1D_Array.height = stageManager.baseHeight;
            stageManager.waveFunction1D_Array.width = stageManager.baseWidth;    
            stageManager.waveFunction1D_Array.startingPositionX = stageManager.baseStartingX;
            stageManager.waveFunction1D_Array.startingPositionY = stageManager.baseStartingY;
        } 
    }
}
