using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMouvement : MonoBehaviour
{

    public GridLayout gridLayout;
    [SerializeField]private Grid grid;
    [SerializeField]private GameObject pion;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pion.transform.position = SnapCoordinateToGrid(GetMouseWorldPosition());
        }
    }       
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoss;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rayCastHit))
        {
            mousePoss = rayCastHit.point;
            if (rayCastHit.transform.gameObject.GetComponent<Cell>())
            {
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
