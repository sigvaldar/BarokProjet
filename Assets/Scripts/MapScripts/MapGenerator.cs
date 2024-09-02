using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] Tilemap Tilemap;
    [SerializeField] float GridHeight, GridWidth;
    [SerializeField] GameObject CellPrefab;
    [SerializeField] float CellSize = 1f;
    private void Awake()
    {
        GenerateMap();
    }
    void GenerateMap() 
    {
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                Vector3 position = HextoWorld(x, y, CellSize);
                Instantiate(CellPrefab,position,Quaternion.identity);
            }
        }
    }

    private Vector3 HextoWorld(int x, int y, float cellSize)
    {
        float width = cellSize * 2;
        float height = Mathf.Sqrt(3) * cellSize;

        float worldX = width * (x * 0.5f * (y & 1));
        float worldZ = height * y * 0.75f;
        float worldY = (int)(UnityEngine.Random.Range(0f, 3f));
        return new Vector3(worldX,worldY,worldZ);
    }
}
