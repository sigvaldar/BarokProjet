using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public string OwnerName, OwnerFaction, Region, Country, landtype;
    public bool Own, Revealed;
    public Vector3 CellPosition;
    public List<Ressources> Ressources;
    public List<Unitées> LocalUnit;

    public MapInformation mapInfo;

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
        Volcan,
        Inconnu
    }
    private void OnMouseEnter()
    {
        mapInfo.ShowInformation(this);
        ToggleHighlight(true);
    }
    private void OnMouseExit()
    {
        mapInfo.ShowInformation(this);
        ToggleHighlight(false);
    }
    public void ToggleHighlight(bool highlight)
    {
        var material = this.GetComponent<Renderer>().material;
        var color = UnityEngine.Color.white;
        if (highlight)
        {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color);
        }
        else
        {
                material.DisableKeyword("_EMISSION");
        }
    }
}


