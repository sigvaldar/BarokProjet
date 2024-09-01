using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMouvement : MonoBehaviour
{

    public GridLayout gridLayout;
    [SerializeField]private Grid grid;
    [SerializeField] private GameObject pion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }       
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rayCastHit))
        {
            return rayCastHit.point;
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
    public void InitializeWidthObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);
 
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
    }
}
