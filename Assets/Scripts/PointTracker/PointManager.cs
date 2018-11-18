using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour {
    [SerializeField]
    List<GameObject> players;
    private int playerIndex = 2;

    private int pointIndex = 1;
    // Use this for initialization

    public void ShowPlayer()
    {
        if (playerIndex<players.Count)
        {            
            players[playerIndex].SetActive(true);
            playerIndex++;
        }       
    }
    public void ClearAll()
    {
        foreach (GameObject player in players)
        {
            if (player.name!="Points" && player.name != "Modifier")
            {
                foreach (Transform slot in player.GetComponentInChildren<Transform>())
                {
                    if (slot.GetComponent<InputField>() != null)
                    {
                        slot.GetComponent<InputField>().text = "0";
                    }
                }
            }           
        }
    }
    public void HidePlayer()
    {
        if (playerIndex > 2)
        {
            playerIndex--;
            players[playerIndex].SetActive(false);            
        }
    }

    public void recalculateTotals()
    {
        for (int i = 2; i < players.Count; i++)
        {
            players[i].GetComponent<TotalsManager>().CalcTotal();
        }        
    }


    public void ShowPointField()
    {
        if (pointIndex<players[0].GetComponent<PointPlayer>().points.Count)
        {            
            foreach (GameObject player in players)
            {
                player.GetComponent<PointPlayer>().points[pointIndex].SetActive(true);
                player.GetComponent<PointPlayer>().points[pointIndex].GetComponent<InputField>().text = "0";
            }
            pointIndex++;
        }                
    }

    public void HidePointField()
    {
        if (pointIndex > 1)
        {
            pointIndex--;
            foreach (GameObject player in players)
            {
                player.GetComponent<PointPlayer>().points[pointIndex].SetActive(false);
                player.GetComponent<PointPlayer>().points[pointIndex].GetComponent<InputField>().text = "0";
            }            
        }
    }
}
