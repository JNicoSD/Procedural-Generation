using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(WaveFunction1D_Array))]
public class WaveFunction1D_ArrayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WaveFunction1D_Array waveFunction1D_Array = (WaveFunction1D_Array)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Generate TileMap"))
        {
            waveFunction1D_Array.DrawFromEditor();
            waveFunction1D_Array.StartFromEditor();
            Debug.Log("TILE MAP GENERATED");
        }

         if(GUILayout.Button("Remove ALL Generated Tilemap"))
        {
             foreach(GameObject g in waveFunction1D_Array.GetTileGroup)
            {
                DestroyImmediate(g);
            }
            waveFunction1D_Array.TileGroupIndex = -1;
            waveFunction1D_Array.GetTileGroup.Clear(); 
            Debug.Log("TILEMAPS REMOVED");
        } 
    }
}
