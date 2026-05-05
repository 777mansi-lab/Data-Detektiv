using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuSpilController : MonoBehaviour
{
    public GameObject gameTypePanel;
    public GameObject datatypesPanel;
    public GameObject situationsPanel;
    public GameObject underConstuctionPanel;
    // Start is called before the first frame update
    void Start()
    {
        gameTypePanel.SetActive(true);
        datatypesPanel.SetActive(false);
        situationsPanel.SetActive(false);
        underConstuctionPanel.SetActive(false);
    }

    public void DataType()
    { 
        gameTypePanel.SetActive(false);
        datatypesPanel.SetActive(true); 
    }

    public void Situations()
    {
        gameTypePanel.SetActive(false);
        situationsPanel.SetActive(true);
    }

    public void UnderConstuction()
    {
        underConstuctionPanel.SetActive(true);
    }
    public void CloseConstruction()
    { 
        underConstuctionPanel.SetActive(false); 
    }
}
