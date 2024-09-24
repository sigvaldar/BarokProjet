using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cell;

public class MapInformation : MonoBehaviour
{
    public GameObject PanelInfo,PanelMap;
    public TextMeshProUGUI Text;

    /// <summary>
    /// Show all the information of a Cell
    /// </summary>
    /// <param name="checkedCell"></param>
    public void ShowInformation(Cell checkedCell)
    {
        PanelInfo.SetActive(!PanelInfo.activeInHierarchy);
        Text.text = "";
        Text.text = checkedCell.OwnerName + " " + checkedCell.OwnerFaction +
            " " + " " + checkedCell.Region + " " + checkedCell.Country + " " + checkedCell.landtype +
            " " +"\n"+ "pos : " + checkedCell.CellPosition;
    }
    public void ShowMap()
    {
        PanelMap.SetActive(!PanelMap.activeInHierarchy);
    }
}


