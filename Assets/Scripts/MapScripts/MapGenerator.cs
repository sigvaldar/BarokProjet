using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] Tilemap Tilemap;
    [SerializeField] Tile tile;
    [SerializeField] float GridHeight, GridWidth;
    [SerializeField] GameObject CellPrefab;
    [SerializeField] float CellSize = 1f;
     
    public GridLayout gridLayout;
    [SerializeField] private Grid grid;
    private void Awake()
    {
        GenerateMap();
    }
    void GenerateMap() 
    {

        //Tilemap.size = new Vector3Int (20, 20,0);

        Debug.Log(Tilemap.size);
        //Tilemap.FloodFill(Vector3Int.zero,new Tile() { gameObject = CellPrefab});
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                Vector3Int postion = new Vector3Int(x, y,(int) (UnityEngine.Random.Range(0f, 2f)));
                //Vector3 position = HextoWorld(x, y, CellSize);
                //position = SnapCoordinateToGrid(position);
                //Debug.Log(position);
                //GameObject mCell = Instantiate(CellPrefab, position, Quaternion.identity);
                //mCell.transform.parent = Tilemap.transform;
                Tilemap.SetTile(postion, new Tile() { gameObject = CellPrefab });

            }
        }
    }

    private Vector3 HextoWorld(int x, int y, float cellSize)
    {
        float width = cellSize * 2;
        float height = Mathf.Sqrt(3) * cellSize;

        float worldX = width * (x + 0.5f * (y & 1));
        float worldZ = height * y * 0.75f;
        float worldY = (UnityEngine.Random.Range(0f, 1f));
        return new Vector3(worldX,worldY,worldZ);
    }
    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }
}
