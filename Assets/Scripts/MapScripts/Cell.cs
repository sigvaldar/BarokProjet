using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;


public class Cell : MonoBehaviour
{
    public string OwnerName, OwnerFaction, Region, Country, landtype;
    public bool Own, Revealed;
    public Vector3 CellPosition;
    public List<Ressources> Ressources;
    public List<Unitées> LocalUnit;

    public GameObject PanelInfo;
    public TextMeshProUGUI Text;

    public enum LandType
    {
        Ocean,
        Plaine,
        Foret,
        Montagne,
        Neige,
        Structure,
        Riviere,
        Plage,
        Marais,
        Inconnu
    }
    private void OnMouseEnter()
    {
        PanelInfo.SetActive(true);
        ShowCellInfo();
    }
    private void OnMouseExit()
    {
        PanelInfo.SetActive(false);
    }

    private void ShowCellInfo()
    {
        Text.text = "";
        Text.text = OwnerName+" "+OwnerFaction+" "+" "+Region+" "+Country+" "+landtype;
    }
}


