using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMovements : MonoBehaviour
{
    [SerializeField] private List<GameObject> pawns;
    [SerializeField] private List<Vector3> spawnPosition;
    public Vector3 currentPawnPosition, selectedCellPosition;
    public bool canMove, unitSelected;
    public int cout, reserve;

    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] Cell currentCell;


    /// <summary>
    /// Return the distance between two Hexagonal Cell
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    int HexDistance(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x)
              + Mathf.Abs(a.x + a.y - b.x - b.y)
              + Mathf.Abs(a.y - b.y)) / 2;
    }

    /// <summary>
    /// check if the cell is in the tab and not outside
    /// </summary>
    /// <param name="hexCoords"></param>
    /// <returns></returns>
    bool IsCellInBounds(Vector2Int hexCoords)
    {
        // Vérifie si la cellule est dans les limites de la carte
        return hexCoords.x >= 0 && hexCoords.x < mapGenerator.MapTerrain.GetLength(0)/*mapArray*/ && hexCoords.y >= 0 && hexCoords.y < mapGenerator.MapTerrain.GetLength(1)/*mapArray*/;
    }


    Vector2Int[] hexDirections = new Vector2Int[]
    {
            new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
            new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1)
    };

    Vector2Int GetNeighbor(Vector2Int hexCoords, int direction)
    {
        return hexCoords + hexDirections[direction];
    }
    List<Vector2Int> GetAccessibleCells(Vector2Int currentHexCoords, int remainingMovementPoints)
    {
        List<Vector2Int> accessibleCells = new List<Vector2Int>();  // Liste des cellules accessibles
        Queue<(Vector2Int, int)> toExplore = new Queue<(Vector2Int, int)>();  // File d'attente des cellules à explorer, avec la distance restante
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();  // Ensemble des cellules déjà visitées pour éviter les répétitions

        // Ajouter la position actuelle du pion comme point de départ
        toExplore.Enqueue((currentHexCoords, remainingMovementPoints));
        visited.Add(currentHexCoords);

        // Parcours en largeur (BFS) pour trouver toutes les cellules accessibles
        while (toExplore.Count > 0)
        {
            var (currentCell, pointsLeft) = toExplore.Dequeue();

            //if (pointsLeft < 0)
            //    continue;


            // Explorer les voisins si on a encore des points de mouvement
            if (pointsLeft > 0)
            { 
                accessibleCells.Add(currentCell); // Ajouter la cellule actuelle à la liste des accessibles
                for (int i = 0; i < 6; i++)  // Parcours des 6 directions possibles
                {
                    Vector2Int neighbor = GetNeighbor(currentCell, i);
                    Debug.Log("direction : "+ i + ",neighbor:"+  neighbor);
                    if (IsCellInBounds(neighbor) && !visited.Contains(neighbor))  // Si la cellule n'a pas encore été visitée
                    {
                        visited.Add(neighbor);
                        toExplore.Enqueue((neighbor, pointsLeft - 1));  // Ajouter le voisin à explorer avec un point de mouvement en moins
                    }
                }
            }
        }

        return accessibleCells;
    }
    void Start()
    {
        //initialisation of all the pawns
        int i =0;
        if ((pawns != null)&(spawnPosition.Count != 0))
        {
            foreach (var item in pawns)
            {
                item.transform.position = spawnPosition[i];
                currentPawnPosition = item.transform.position;
                i++;
            }
        }

    }

    private GameObject GetObjectUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rayCastHit))
        {
            return rayCastHit.transform.gameObject;
        }
        return null;
    }
    float threshold = 1.0f;
    Cell FindCellFromPosition(Vector3 unitPosition)
    {
        Vector3Int roundedPosition = Vector3Int.RoundToInt(unitPosition);
        if (mapGenerator.CellDictionary.TryGetValue(roundedPosition, out Cell cell))
        {
            return cell;
        }
        //foreach (Cell cell in mapGenerator.mapArray)
        //{
        //    // Vérifie si l'unité est dans cette cellule (en fonction d'une distance ou d'une méthode plus précise)
        //    if (Vector3.Distance(unitPosition, cell.transform.position) < threshold)
        //    {
        //        return cell;
        //    }
        //}
        return null; // Si aucune cellule n'est trouvée
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject clickedObject = GetObjectUnderMouse();

            if (clickedObject != null)
            {
                // Vérifier si c'est une unité
                if (clickedObject.GetComponent<unit>())
                {
                    //SelectPawn(clickedObject);
                    HightlightNeighbor();
                    canMove = true;
                }
                // Vérifier si c'est une cellule
                else if (clickedObject.GetComponent<Cell>())
                {
                    if (canMove)
                    {
                        selectedCellPosition = clickedObject.transform.position;
                        UnitMovement();
                        ClearPreviousHighlights();
                        canMove = false;
                    }
                }
            }
        }
    }
    List<Vector2Int> previouslyHighlightedCells = new List<Vector2Int>();
    private void HightlightNeighbor()
    {
        // Récupérer la position actuelle du pion et les points de mouvement restants
        Vector2Int pionPosition = new Vector2Int ((int)currentPawnPosition.x,(int)currentPawnPosition.z);  // Coordonnées actuelles du pion
        int movementPoints = reserve;  // Points de mouvement restants
        Debug.Log("pionpos : "+ pionPosition);
        // Récupérer toutes les cellules accessibles
        List<Vector2Int> cellsToHighlight = GetAccessibleCells(pionPosition, movementPoints);
        previouslyHighlightedCells = cellsToHighlight;
        // Appliquer le highlight sur chaque cellule
        foreach (Vector2Int cellCoords in cellsToHighlight)
        {
            //Debug.Log(mapGenerator.mapArray[cellCoords.x,cellCoords.y].CellPosition);
            //mapGenerator.mapArray[cellCoords.x, cellCoords.y].Selected = true;
            //mapGenerator.mapArray[cellCoords.x, cellCoords.y].ToggleHighlight(true,Color.red);
        }
    }

    void ClearPreviousHighlights()
    {
        foreach (Vector2Int hexCoords in previouslyHighlightedCells)
        {
            //mapGenerator.mapArray[hexCoords.x, hexCoords.y].ToggleHighlight(false,Color.red);
            //mapGenerator.mapArray[hexCoords.x, hexCoords.y].Selected = false;
        }
        previouslyHighlightedCells.Clear();  // Vider la liste des cellules précédemment surlignées
    }

    public void UnitMovement()
    {
        Vector2Int currentpos = new Vector2Int ((int)currentPawnPosition.x,(int)currentPawnPosition.z) ;
        Vector2Int targetpos = new Vector2Int  ((int)selectedCellPosition.x, (int)selectedCellPosition.z);
        int distance = HexDistance(currentpos,targetpos)/2;
        int FinalCost = cout * distance;
        Debug.Log("distance : "+distance);
        if (reserve >= FinalCost)
        {
            pawns[0].transform.position = selectedCellPosition;//a changer
            reserve -= FinalCost;  
            currentPawnPosition = selectedCellPosition;

            Debug.Log("Pion déplacé. Points restants : " + reserve);
        }
        else
        {
            Debug.Log("Déplacement impossible. Soit trop loin, soit plus assez de points.");
        }
    }
}
