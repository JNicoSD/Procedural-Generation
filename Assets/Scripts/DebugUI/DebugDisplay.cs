using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace DebugUI
{
    [System.Serializable]
    public static class DebugDisplay
    {
        // PRIVATE
        //private static GameObject[,] textObject;
        private static GameObject[,] tiles2DArray;
        private static GameObject[] tiles1DArray;
        private static GameObject debugObject;
        [SerializeField] public static CellSprite[] spriteArray;
        //private static bool tileGridDrawn = false;

        /* public static void DrawCanvas(GameObject tileGrid, float startingPositionX, float startingPositionY, int width, int height, GameObject parentObj)
        {   
            if(tileGridDrawn == false)
            {
                //textObject = new GameObject[width, height];
                tiles2DArray = new GameObject[width, height];

                for(int row = 0; row < height; row++)
                {
                    for(int col = 0; col < width; col++)
                    {
                        // tiles[row, col] = Instantiate(tileGrid, 
                        //                            new Vector3(drawScript.GridCell[row,col].GetObj().transform.position.x, 
                        //                                        drawScript.GridCell[row,col].GetObj().transform.position.y), 
                        //                            Quaternion.identity);
                        //
                        tiles2DArray[row, col] = UnityEngine.Object.Instantiate(tileGrid, new Vector3(row - startingPositionX, col + startingPositionY), Quaternion.identity);
                        tiles2DArray[row, col].transform.SetParent(parentObj.transform);
                        tiles2DArray[row, col].transform.localScale = new Vector3(1,1,1);
                    }
                }
            }
        } */

        //static  spriteObject;

        public static void DrawCanvas(CellSprite spriteList, float startingPositionX, float startingPositionY, int width, int height)
        {   
                Vector2 cellSize = new Vector2(1, 1);
                Vector3 tileScale = new Vector3(0.13f, 0.13f, 0.13f),
                        spriteScale = new Vector3(1, 1, 1);

                /////
                tiles1DArray = new GameObject[width * height];
                GameObject[] spriteObject = new GameObject[spriteList.spriteLists.Length];

                debugObject = new GameObject("++ Debug ++");
                //debugObject.transform.position = new Vector3(0, 0, 5);

                for(int row = 0, i = 0; row < height; row++)
                {
                    for(int col = 0; col < width; col++)
                    {
                        tiles1DArray[i] = new GameObject($"DebugTile: {i}");

                        tiles1DArray[i].transform.position = new Vector3(col - startingPositionX, row + startingPositionY, 1);
                        tiles1DArray[i].transform.localScale = tileScale;

                        tiles1DArray[i].AddComponent<GridLayoutGroup>();

                        tiles1DArray[i].GetComponent<GridLayoutGroup>().cellSize = cellSize;
                        tiles1DArray[i].GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
                        tiles1DArray[i].GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                        tiles1DArray[i].GetComponent<GridLayoutGroup>().constraintCount = Mathf.CeilToInt(Mathf.Sqrt((float)spriteList.spriteLists.Length));

                        tiles1DArray[i].transform.SetParent(debugObject.transform);

                        i++;
                    }
                }

                for(int t = 0; t < tiles1DArray.Length; t++)
                {
                    for(int i = 0; i < spriteObject.Length; i++)
                    {
                        spriteObject[i] = new GameObject($"Sprite {i}");

                        spriteObject[i].AddComponent<SpriteRenderer>().sprite = spriteList.spriteLists[i].GetSprite;
                        spriteObject[i].AddComponent<RectTransform>();

                        spriteObject[i].transform.localPosition = Vector3.zero;
                        spriteObject[i].transform.SetParent(tiles1DArray[t].transform);
                        spriteObject[i].transform.localScale = spriteScale;
                    }
                }
        }

        /* public static void UpdateCanvas(int indexX, int indexY, List<Sprite> candidatesToRemove)
        {
            try{
                for(int i = 0; i < tiles2DArray[indexX, indexY].transform.childCount; i++)
                {
                    for(int k = 0; k < candidatesToRemove.Count; k++)
                    {
                        if(tiles2DArray[indexX, indexY].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite == candidatesToRemove[k])
                        {
                            tiles2DArray[indexX, indexY].transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.Log(ex.GetType() + ": DrawCanvasMatrix is not called. Call it before calling UpdateCanvasArray; Preferably at Start or Awake");
            }
        } */

        public static void UpdateCanvas(List<List<CellSprite.SpriteList>> candidatesList)
        {
            try{
                for(int i = 0; i < tiles1DArray.Length; i++)
                {
                    if(candidatesList[i].Count < 1)
                    {
                        tiles1DArray[i].SetActive(false);
                    } else 
                    {
                        for(int ch = 0; ch < tiles1DArray[i].transform.childCount; ch++)
                        {
                            tiles1DArray[i].transform.GetChild(ch).gameObject.SetActive(false);
                            for(int ca = 0; ca < candidatesList[i].Count; ca++)
                            {
                                if(tiles1DArray[i].transform.GetChild(ch).GetComponent<SpriteRenderer>().sprite == candidatesList[i][ca].GetSprite)
                                {
                                    tiles1DArray[i].transform.GetChild(ch).gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.Log(ex.GetType() + ": DrawCanvas is not called. Call it before calling UpdateCanvas, preferably at Start or Awake");
            }
        }

        public static void UpdateCanvas(int index, List<Sprite> candidates)
        {
            try{
                for(int i = 0; i < tiles1DArray[index].transform.childCount; i++)
                {
                    for(int k = 0; k < candidates.Count; k++)
                    {
                        if(tiles1DArray[index].transform.GetChild(i).GetComponent<SpriteRenderer>().sprite == candidates[k])
                        {
                            tiles1DArray[index].transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.Log(ex.GetType() + ": DrawCanvas is not called. Call it before calling UpdateCanvas, preferably at Start or Awake");
            }
        }
    }
}
