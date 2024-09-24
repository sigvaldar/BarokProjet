using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public string OwnerName, OwnerFaction, Region, Country, landtype;
    public bool Own, Revealed,Selected;
    public Vector3 CellPosition;
    public List<Ressources> Ressources;
    public List<unit> LocalUnit;

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
        if (!Selected)
        {
            //mapInfo.ShowInformation(this);
            ToggleHighlight(true, UnityEngine.Color.white);
        }

    }
    private void OnMouseExit()
    {
        if (!Selected)
        {
            //mapInfo.ShowInformation(this);
            ToggleHighlight(false, UnityEngine.Color.white);
        }
    }

    /// <summary>
    /// hightlight the gameObject with a color
    /// </summary>
    /// <param name="highlight"></param>
    /// <param name="color"></param>
    public void ToggleHighlight(bool highlight, UnityEngine.Color color)
    {
        var material = GetComponent<Renderer>().material;
        //var color = UnityEngine.Color.white;
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


