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


    /// <summary>
    /// Return the distance between twoo Hexagonal Cell
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
    /// Return the position of the clicked element if it's a cell
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetMouseObjectPosition()
    {
        Vector3 mousePoss = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rayCastHit))
        {
            if (rayCastHit.transform.gameObject.GetComponent<Cell>())
            {
                mousePoss = rayCastHit.transform.gameObject.transform.position;
                mousePoss.y += 1f;
            }
            return mousePoss;
        }
        else
        {
            return Vector3.zero;
        }
    }

    /// <summary>
    /// Return if the gameobject is a unit or not
    /// </summary>
    /// <returns></returns>
    private bool CheckGameobject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rayCastHit))
        {
            if (rayCastHit.transform.gameObject.GetComponent<unit>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// check if the cell is in the tab and not outside
    /// </summary>
    /// <param name="hexCoords"></param>
    /// <returns></returns>
    bool IsCellInBounds(Vector2Int hexCoords)
    {
        // Vérifie si la cellule est dans les limites de la carte
        return hexCoords.x >= 0 && hexCoords.x < mapGenerator.mapArray.GetLength(0) && hexCoords.y >= 0 && hexCoords.y < mapGenerator.mapArray.GetLength(1);
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

            if (pointsLeft < 0)
                continue;

            // Ajouter la cellule actuelle à la liste des accessibles
            accessibleCells.Add(currentCell);

            // Explorer les voisins si on a encore des points de mouvement
            if (pointsLeft > 0)
            {
                for (int i = 0; i < 6; i++)  // Parcours des 6 directions possibles
                {
                    Vector2Int neighbor = GetNeighbor(currentCell, i);

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
                i++;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canMove)
            {
                selectedCellPosition = GetMouseObjectPosition();
                UnitMovement();
                canMove = false;
                ClearPreviousHighlights();
            }
            unitSelected = CheckGameobject();
            if (unitSelected)
            {
                canMove = true;
                HightlightNeighbor();
            }
        }
    }
    List<Vector2Int> previouslyHighlightedCells = new List<Vector2Int>();
    private void HightlightNeighbor()
    {
        // Récupérer la position actuelle du pion et les points de mouvement restants
        Vector2Int pionPosition = new Vector2Int ((int)currentPawnPosition.x,(int)currentPawnPosition.z);  // Coordonnées actuelles du pion
        int movementPoints = reserve;  // Points de mouvement restants

        // Récupérer toutes les cellules accessibles
        List<Vector2Int> cellsToHighlight = GetAccessibleCells(pionPosition, movementPoints);
        previouslyHighlightedCells = cellsToHighlight;
        // Appliquer le highlight sur chaque cellule
        foreach (Vector2Int cellCoords in cellsToHighlight)
        {
            mapGenerator.mapArray[cellCoords.x, cellCoords.y].Selected = true;
            mapGenerator.mapArray[cellCoords.x, cellCoords.y].ToggleHighlight(true,Color.green);
        }
    }

    void ClearPreviousHighlights()
    {
        foreach (Vector2Int hexCoords in previouslyHighlightedCells)
        {
            mapGenerator.mapArray[hexCoords.x, hexCoords.y].ToggleHighlight(false,Color.green);
            mapGenerator.mapArray[hexCoords.x, hexCoords.y].Selected = false;
        }
        previouslyHighlightedCells.Clear();  // Vider la liste des cellules précédemment surlignées
    }

    public void UnitMovement()
    {
        Vector2Int currentpos = new Vector2Int ((int)currentPawnPosition.x,(int)currentPawnPosition.z) ;
        Vector2Int targetpos = new Vector2Int  ((int)selectedCellPosition.x, (int)selectedCellPosition.z);
        int distance = HexDistance(currentpos,targetpos);
        int FinalCost = cout * distance;
        Debug.Log(distance);
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
