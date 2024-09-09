using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInformation : MonoBehaviour
{
    public GameObject PanelInfo,PanelMap;
    public void ShowInformation()
    {
        PanelInfo.SetActive(!PanelInfo.activeInHierarchy);
    }
    public void ShowMap()
    {
        PanelMap.SetActive(!PanelMap.activeInHierarchy);
    }
}
public class Information
{
    public string TestInfo = "tu as cliqué ici";
    public string TestInfo2 = "et maintenant la ";
}
