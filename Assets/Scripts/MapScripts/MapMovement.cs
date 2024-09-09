using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{

    public GridLayout gridLayout;
    [SerializeField]private Grid grid;
    [SerializeField]private GameObject pion;
    [SerializeField]private Vector3 Selectedcell;
    [SerializeField] private bool canMove;
    //unit possition on the grid;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Selectedcell = SnapCoordinateToGrid(GetMouseWorldPosition());
            UnitMovement();
        }
    }
    public void UnitMovement()
    {
        pion.transform.position = Selectedcell;
    }
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoss;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rayCastHit))
        {
            mousePoss = rayCastHit.point;
            Debug.Log(mousePoss);
            if (rayCastHit.transform.gameObject.GetComponent<Cell>())
            {
                //mousePoss.y = rayCastHit.transform.gameObject.transform.position.y;
                Debug.Log("case toucher");
            }
            return mousePoss;
        }
        else
        {
            return Vector3.zero;
        }
    } 
    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }    
}
