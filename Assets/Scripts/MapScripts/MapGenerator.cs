using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
//[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] int HexHeight, HexWidth;
    [SerializeField] GameObject CellPrefab, hexaGrid;

    [SerializeField] Texture2D MapTexture;
    [SerializeField] int[,] MapTerrain;

    [SerializeField] List<Color> MapColors;
    [SerializeField] List<Material> CellMartial;

    private void Awake()
    {
        GenerateMap();
        Debug.Log("map");
    }

    /// <summary>
    /// Methode to create a 3D hexagonal map with a Image 2D;
    /// </summary>
    void GenerateMap()
    {

        // Taille de la carte en fonction des hexagones
        int mapWidth = MapTexture.width / HexWidth;
        int mapHeight = MapTexture.height / HexHeight;

        MapTerrain = new int[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector2 hexCenter = GetHexCenter(x, y);
                MapTerrain[x, y] = GetHexagonTerrainType((int)hexCenter.x, (int)hexCenter.y);
            }
        }

        SetMap();
    }

    private void SetMap()
    {
        float CellSize = 1;//HexWidth * 0.5f;
        for (int i = 0; i < MapTerrain.GetLength(0); i++)
        {
            for (int y = 0; y < MapTerrain.GetLength(1); y++)
            {
                Vector3 position = HexPositioning(i, y, CellSize);
                GameObject mCell = Instantiate(CellPrefab, position, Quaternion.identity);
                mCell.transform.parent = hexaGrid.transform;

                int terrainType = MapTerrain[i, y];
                if (terrainType >= 0 && terrainType < CellMartial.Count)
                {
                    Renderer cellRenderer = mCell.GetComponent<Renderer>();
                    if (cellRenderer != null)
                    {
                        cellRenderer.material = CellMartial[terrainType];
                    }
                }
                else
                {
                    Debug.LogWarning($"Terrain inconnu à la position ({i}, {y})");
                }
                //SetCell(MapTerrain[i, y],i,y);
            }
        }
    }

    private void instantiateCell(int x, int y)
    {
        //Vector3 position = HexPositioning(x, y, CellSize);
        //GameObject mCell = Instantiate(CellPrefab, position, Quaternion.identity);
        //mCell.transform.parent = hexaGrid.transform;
    }

    /// <summary>
    /// Calcule la position du centre d'un hexagone dans l'image
    /// </summary>
    /// <param name="hexX"></param>
    /// <param name="hexY"></param>
    /// <returns></returns>
    Vector2 GetHexCenter(int hexX, int hexY)
    {
        float offsetX = (hexY % 2 == 0) ? 0 : HexWidth / 2; // Décalage entre lignes d'hexagones
        float centerX = hexX * HexWidth + offsetX + HexWidth / 2;
        float centerY = hexY * (HexHeight * 0.75f) + HexHeight / 2;
        return new Vector2(centerX, centerY);
    }

    /// <summary>
    /// Détermine le type de terrain en fonction de la couleur autour du centre d'un hexagone
    /// </summary>
    /// <param name="centerX"></param>
    /// <param name="centerY"></param>
    /// <returns></returns>
    int GetHexagonTerrainType(int centerX, int centerY)
    {
        // Rayon approximatif de l'hexagone
        int radius = HexWidth / 2;
        Dictionary<int, int> terrainCount = new Dictionary<int, int>();

        for (int x = centerX - radius; x <= centerX + radius; x++)
        {
            for (int y = centerY - radius; y <= centerY + radius; y++)
            {
                if (x >= 0 && x < MapTexture.width && y >= 0 && y < MapTexture.height)
                {
                    if (IsPixelInHexagon(x,y,centerX,centerY,radius))
                    {
                        Color pixelColor = MapTexture.GetPixel(x, y);
                        int terrainType = GetTerrainTypeFromColor(pixelColor);
                        if (terrainType != -1)
                        {
                            if (!terrainCount.ContainsKey(terrainType))
                            {
                                terrainCount[terrainType] = 0;
                            }
                            terrainCount[terrainType]++;
                        }
                    }                   
                }
            }
        }
        if (terrainCount.Count == 0)
            return 1;

        // Retourne la valeur la plus fréquente pour cet hexagone
        return terrainCount.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
    }

    /// <summary>
    /// Check is a pixel is in the Hexagone
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="centerX"></param>
    /// <param name="centerY"></param>
    /// <param name="hexRadius"></param>
    /// <returns></returns>
    bool IsPixelInHexagon(int x, int y, int centerX, int centerY, int hexRadius)
    {
        float dx = Math.Abs(x - centerX) / (float)hexRadius;
        float dy = Math.Abs(y - centerY) / (float)hexRadius;

        return (dx + dy * 2.0f / 3.0f <= 0.95f);
    }


    /// <summary>
    /// Instantiate a Cell in the grid that Correspond to an int
    /// </summary>
    /// <param name="typeOfCell"></param>
    private void SetCell(int typeOfCell,int x,int y)
    {
        switch (typeOfCell)
        {
            case 0: instantiateCell(x,y); Debug.Log("ocean");
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
    /// Return an int with the corresponding color
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private int GetTerrainTypeFromColor(Color color)
    {
        //switch (color)
        //{
        //    case Color c when c == MapColors[0]: return 0;// ocean
        //    case Color c when c == MapColors[1]: return 1;// plaine
        //    case Color c when c == MapColors[2]: return 2;// forest
        //    case Color c when c == MapColors[3]: return 3;// montagne       
        //    case Color c when c == MapColors[4]: return 4;// snow
        //    case Color c when c == MapColors[5]: return 5;// structure
        //    case Color c when c == MapColors[6]: return 6;// lac or river
        //    case Color c when c == MapColors[7]: return 7;// beach
        //    case Color c when c == MapColors[8]: return 8;// swamp
        //    default: return -1;// unknow terrain default
        //}
        for (int i = 0; i < MapColors.Count; i++)
        {
            if (AreColorsSimilar(color, MapColors[i]))
            {
                return i; // Retourne l'index correspondant au terrain
            }
        }
        return -1; // Terrain inconnu si aucune correspondance trouvée
    }

    /// <summary>
    /// Compares two colors with a certain tolerance
    /// </summary>
    private bool AreColorsSimilar(Color c1, Color c2, float tolerance = 0.1f)
    {
        return Mathf.Abs(c1.r - c2.r) < tolerance &&
               Mathf.Abs(c1.g - c2.g) < tolerance &&
               Mathf.Abs(c1.b - c2.b) < tolerance;
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
}
