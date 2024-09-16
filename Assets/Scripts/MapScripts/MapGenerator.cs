using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
//[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] float GridHeight, GridWidth;
    [SerializeField] GameObject CellPrefab, hexaGrid;

    [SerializeField] Texture2D MapTexture;
    [SerializeField] int[,] MapTerrain,test;
    int mapterraintype;

    [SerializeField] List<Color> MapColors;
    [SerializeField] List<Material> CellMartial;

    //[SerializeField] float CellSize = 1f;
    //[SerializeField] Tilemap Tilemap;
    //public GridLayout gridLayout;
    //[SerializeField] private Grid grid;
    private void Awake()
    {
        GenerateMap();
        Debug.Log("map");
    }
    /// <summary>
    /// Methode to create a 3d hexagonal map 
    /// </summary>
    void GenerateMap()
    {
        ReadMap();
        SetMapTab();

        for (int i = 0; i < test.GetLength(0); i++)
        {
            for (int y = 0; y < test.GetLength(1); y++)
            {
                SetCell(test[i,y]);
            }
        }

        //for (int y = 0; y < GridHeight; y++)
        //{
        //    for (int x = 0; x < GridWidth; x++)
        //    {
        //        Vector3 position = HexPositioning(x, y, CellSize);
        //        GameObject mCell = Instantiate(CellPrefab, position, Quaternion.identity);
        //        mCell.transform.parent = hexaGrid.transform;
        //    }
        //}
    }

    private void ReadMap()
    {
        MapTerrain = new int[MapTexture.width, MapTexture.height];
        for (int x = 0; x < MapTexture.width; x ++)
        {
            for (int y = 0; y < MapTexture.height; y ++)
            {
                Color pixelColor = MapTexture.GetPixel(x, y);
                MapTerrain[x, y] = GetTerrainTypeFromColor(pixelColor);
                Debug.Log(MapTerrain[x,y]);
            }
        }
    }

    private void SetMapTab()
    {
        test = DownsampleTerrainMap(MapTerrain,10);
    }

    private void SetCell(int typeOfCell)
    {
        switch (typeOfCell)
        {
            case 0: Debug.Log("ocean");
                break;
            case 1:
                Debug.Log("plaine");
                break;
            case 2:
                Debug.Log("forest");
                break;
            case 3:
                Debug.Log("mountagne");
                break;
            case 4:
                Debug.Log("snow");
                break;
            case 5:
                Debug.Log("structure");
                break;
            case 6:
                Debug.Log("river");
                break;
            case 7:
                Debug.Log("beach");
                break;
            case 8:
                Debug.Log("swamp");
                break;
            default: Debug.Log("none");
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private int GetTerrainTypeFromColor(Color color)
    {
        switch (color)
        {
            case Color c when c == MapColors[0]: return 0;// ocean
            case Color c when c == MapColors[1]: return 1;// plaine
            case Color c when c == MapColors[2]: return 2;// forest
            case Color c when c == MapColors[3]: return 3;// montagne       
            case Color c when c == MapColors[4]: return 4;// snow
            case Color c when c == MapColors[5]: return 5;// structure
            case Color c when c == MapColors[6]: return 6;// lac or river
            case Color c when c == MapColors[7]: return 7;// beach
            case Color c when c == MapColors[8]: return 8;// swamp
            default: return -1;// unknow terrain default
        }
    }
    
    /// <summary>
    /// Return the position of an hexagonal cell on the map
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="cellSize"></param>
    /// <returns></returns>
    private Vector3 HexPositioning(int x,int y, float cellSize)
    {
        float width = MathF.Sqrt(3) * cellSize;
        float height = 2 * cellSize;

        float worldX = width * (x + 0.5f * (y & 1));
        float worldZ = height * y * 0.75f;
        float worldY = (UnityEngine.Random.Range(0f, 1f));
        return new Vector3(worldX, worldY, worldZ);
    }

    int[,] DownsampleTerrainMap(int[,] originalMap, int blockSize)
    {
        int originalWidth = originalMap.GetLength(0);
        int originalHeight = originalMap.GetLength(1);

        int newWidth = originalWidth / blockSize;
        int newHeight = originalHeight / blockSize;

        int[,] downsampledMap = new int[newWidth, newHeight];

        for (int x = 0; x < newWidth; x++)
        {
            for (int y = 0; y < newHeight; y++)
            {
                downsampledMap[x, y] = GetDominantTerrainType(originalMap, x * blockSize, y * blockSize, blockSize);
            }
        }

        return downsampledMap;
    }

    // Fonction pour obtenir la valeur dominante dans un bloc de pixels
    int GetDominantTerrainType(int[,] map, int startX, int startY, int blockSize)
    {
        Dictionary<int, int> terrainCount = new Dictionary<int, int>();

        for (int x = startX; x < startX + blockSize; x++)
        {
            for (int y = startY; y < startY + blockSize; y++)
            {
                int terrainType = map[x, y];
                if (!terrainCount.ContainsKey(terrainType))
                {
                    terrainCount[terrainType] = 0;
                }
                terrainCount[terrainType]++;
            }
        }

        // Retourne la valeur avec la fréquence la plus élevée
        return terrainCount.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
    }


    /*void GenerateMap() //old
    {

        //Tilemap.size = new Vector3Int (20, 20,20);

        Debug.Log(Tilemap.size);
        //Tilemap.FloodFill(Vector3Int.zero,new Tile() { gameObject = CellPrefab});
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                //Vector3Int positionI= new Vector3Int(x, y,0);

                Vector3 position = HexPositioning(x, y, CellSize);
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
    }*/


    //public Vector3 SnapCoordinateToGrid(Vector3 position)
    //{
    //    Vector3Int cellPos = gridLayout.WorldToCell(position);
    //    position = grid.GetCellCenterWorld(cellPos);
    //    return position;
    //}
}
