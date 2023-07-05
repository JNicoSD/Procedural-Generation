using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

public class MeshGenerator_Landscape
{   
    public Material mat;
    Mesh mesh;
    GameObject landscape;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    int xSize, zSize;
    Renderer renderer;

    float perlinX, perlinZ, maxHeight;
    float scale, frequency, persistence, lacunarity, amplitude;
    float octaves = 4;

    int levelOfDetail;
    public void CreateShape(VisualElement root)
    {
        if(landscape == null)
        {
            landscape = new GameObject("Landscape");
            landscape.AddComponent<MeshFilter>();
            landscape.AddComponent<MeshRenderer>();
            //landscape.GetComponent<MeshRenderer>().material = _material;
            renderer = landscape.GetComponent<MeshRenderer>();
            mesh = new Mesh();
        }
        levelOfDetail = root.Q<SliderInt>("_levelOfDetail").value > 0 ? (int)(root.Q<SliderInt>("_levelOfDetail").value * 2) : 1; // multiply by 2 if higher than 1
        int verticesPerLine = (xSize - 1) / levelOfDetail + 1;


        landscape.GetComponent<MeshFilter>().mesh = mesh;

        SetLandSize(root);
        SetLandMaterial(root);

        perlinX = root.Q<Slider>("_perlinX").value; 
        perlinZ = root.Q<Slider>("_perlinZ").value;
        maxHeight = root.Q<Slider>("_maxHeight").value;

        scale = root.Q<Slider>("_scale").value;
        frequency = root.Q<Slider>("_frequency").value;
        persistence = root.Q<Slider>("_persistence").value;
        lacunarity = root.Q<Slider>("_lacunarity").value;
        amplitude = root.Q<Slider>("_amplitude").value;

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                //float y = Mathf.PerlinNoise(x * perlinX, z * perlinZ) * maxHeight;
                float y = Noise.GenerateNoise(x, z, octaves, scale, frequency, persistence, lacunarity, amplitude) * maxHeight;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[vertices.Length * 6];

        int vert = 0;
        int t = 0;

        for(int z = 0; z < zSize; z+=levelOfDetail) // Manages Z-axis of the triangle
        {
            for(int x = 0; x < xSize; x+=levelOfDetail) // Manages X-axis of the triangle
            {
                triangles[t + 0] = vert;
                triangles[t + 1] = vert + ((xSize + 1) * levelOfDetail);
                triangles[t + 2] = vert + levelOfDetail;
                triangles[t + 3] = vert + levelOfDetail; 
                triangles[t + 4] = vert + ((xSize + 1) * levelOfDetail);
                triangles[t + 5] = vert + ((xSize + 1) * levelOfDetail) + (1 * levelOfDetail);
                vert+=levelOfDetail;
                t += 6; // Add 6 to make sure not to overwrite the implemented triangles
            }
            vert = vert + (root.Q<SliderInt>("_levelOfDetail").value * (xSize + 1) + 1);
        }

        uvs = new Vector2[vertices.Length];
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                i++;
            }
        }
        UpdateMesh();
    }

    public void SetLandSize(VisualElement root)
    {
        xSize = root.Q<IntegerField>("_xSize").value;
        zSize = root.Q<IntegerField>("_zSize").value;
    }

    public void SetLandMaterial(VisualElement root)
    {
        mat = root.Q<ObjectField>("_material").value as Material;

        mat.mainTexture = GenerateTexture(root);

        landscape.GetComponent<MeshRenderer>().material = mat; 
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }

    int width = 256;
    int height = 256;
    Texture2D GenerateTexture(VisualElement root)
    {   
        width = xSize;
        height = zSize;

        Texture2D texture = new Texture2D(width, height);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y, root);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y, VisualElement root)
    {   
        /*
        for(int o = 0; o < octaves; o++)
        {
            float xCoord = (float)x / scale * frequency;
            float yCoord = (float)y / scale * frequency;

            float sample = Mathf.PerlinNoise(xCoord, yCoord);
            noiseValue += sample * amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }*/

        float noiseValue = Noise.GenerateNoise(x, y, octaves, scale, frequency, persistence, lacunarity, amplitude);   

        return new Color(noiseValue, noiseValue, noiseValue);
    }
}
