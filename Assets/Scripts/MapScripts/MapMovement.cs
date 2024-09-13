using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    [SerializeField] private GameObject pion;
    [SerializeField] private Vector3 Selectedcell;
    [SerializeField] private bool canMove;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Selectedcell = GetMouseObjectPosition();
            UnitMovement();
        }
    }
    public void UnitMovement()
    {
        pion.transform.position = Selectedcell;
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
}
