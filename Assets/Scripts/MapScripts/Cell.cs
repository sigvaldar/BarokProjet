using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public string OwnerName, OwnerFaction, Region, Country;
    public bool Own, Revealed;
    public Vector3 CellPosition;
    public LandType landtype;
    public List<Ressources> Ressources;
    public List<Unitées> LocalUnit;

}


