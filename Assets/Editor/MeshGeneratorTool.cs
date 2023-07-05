using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;

public class MeshGeneratorTool : EditorWindow
{
    public MeshGenerator_Landscape meshGenerator_Landscape = new MeshGenerator_Landscape();
    VisualElement root;
    [MenuItem("Procedural/Mesh Generator Tool")]
    public static void ShowWindow()
    {
        MeshGeneratorTool window = GetWindow<MeshGeneratorTool>();
        window.titleContent = new GUIContent("Mesh Generator");
    }

    public VisualTreeAsset editorUI;

    public void CreateGUI()
    {
        editorUI = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EditorUI/MeshGeneratorToolUI.uxml");

        root = rootVisualElement;
        editorUI.CloneTree(root);

        SetUpButtonHandler();
    }

    public void SetUpButtonHandler()
    {
        Button generateMesh = root.Q<Button>("_generateMesh");
        generateMesh.clicked += GenerateMesh;
    }


    void OnInspectorUpdate()
    {
        if(root.Q<Toggle>("_updateLive").value) meshGenerator_Landscape.CreateShape(root);
    }

    public void GenerateMesh() => meshGenerator_Landscape.CreateShape(root);
}