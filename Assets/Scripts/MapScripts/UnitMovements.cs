using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovements : MonoBehaviour
{
    [SerializeField] private List<GameObject> pawns;
    [SerializeField] private List<Vector3> spawnPosition;  
    [SerializeField] public Vector3 currentPawnPosition,selectedCellPosition;
    public bool canMove;
    private int[,] MapTab;
    public int cout,reserve;

    void Start()
    {
        if ((pawns != null)&(spawnPosition.Count != 0))
        {
            foreach (var item in pawns)
            {
                item.transform.position = spawnPosition[item.GetInstanceID()];
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectedCellPosition = GetMouseObjectPosition();
            UnitMovement();
        }
    }
    public void UnitMovement()
    {
        pawns[0].transform.position = selectedCellPosition;
    }

    /// <summary>
    /// Return the position of the clicked element
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetMouseObjectPosition()
    {
        Vector3 mousePoss = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rayCastHit))
        {
            if (rayCastHit.transform.gameObject.GetComponent<unit>())
            {
                //currentPawnPosition = rayCastHit.transform.position;
            }
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
    Vector2Int[] hexDirections = new Vector2Int[]
{
    new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1),
    new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1)
};

    Vector2Int GetNeighbor(Vector2Int hexCoords, int direction)
    {
        return hexCoords + hexDirections[direction];
    }
}
