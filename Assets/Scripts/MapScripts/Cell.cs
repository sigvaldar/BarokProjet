using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public string OwnerName, OwnerFaction, Region, Country;
    public int  RegionID;
    public bool Own, Revealed;
    public Vector3 CellPossition;
    public LandType landtype;
    public List<Ressources> Ressources;
    public List<Unitées> LocalUnit;

    //public GameObject obCell;
    
    //public void ChangeHeight(float heightMax = 1f)
    //{
    //    obCell.transform.position = new(transform.position.x,Random.Range(0f, heightMax), transform.position.z);
    //}
}

