using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
//[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] Tilemap Tilemap;
    [SerializeField] float GridHeight, GridWidth;
    [SerializeField] GameObject CellPrefab,hexaGrid;
    [SerializeField] float CellSize = 1f;
    [SerializeField] float MapMaxHeight = 0.5f;
     
    public GridLayout gridLayout;
    [SerializeField] private Grid grid;
    private void Awake()
    {
        GenerateMap();
    }
    void GenerateMap() 
    {

        //Tilemap.size = new Vector3Int (20, 20,20);

        Debug.Log(Tilemap.size);
        //Tilemap.FloodFill(Vector3Int.zero,new Tile() { gameObject = CellPrefab});
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                //Vector3Int positionI= new Vector3Int(x, y,0);

                Vector3 position = HextoWorld(x, y, CellSize);
                //position = SnapCoordinateToGrid(position);
                GameObject mCell = Instantiate(CellPrefab, position, Quaternion.identity);
                mCell.transform.parent = hexaGrid.transform;
                //mCell.transform.parent = Tilemap.transform;


                //Tile tile = new Tile();
                //tile.gameObject = CellPrefab;

                //Tilemap.SetTile(positionI, tile);

                //tile.gameObject.GetComponent<Cell>().ChangeHeight(MapMaxHeight);
            }
        }


    }

    //private void ChangeTilePos()
    //{
    //    var b = new BoundsInt(0, 0, 0, 20, 20, 20);
        
    //    foreach (var tile in Tilemap.GetTilesBlock(b)){
    //        ((Tile)tile).gameObject.tr
    //    }
    //}

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
