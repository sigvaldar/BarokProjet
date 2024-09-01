using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    [SerializeField] float GridHeight, GridWidth;
    [SerializeField] float[,,] MapArray;
    // Start is called before the first frame update
    void Start()
    {
        //MapArray = new float[(int)GridHeight, (int)GridWidth,0];
        createGrid();
        Debug.Log(MapArray);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void createGrid()
    {
        for (int y = 0; y < GridHeight; y++)
        {
            for (int i = 0; i < GridWidth; i++)
            {
                //MapArray[y, i] = 0; 
            }
        }
    }
}
