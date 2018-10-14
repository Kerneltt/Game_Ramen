using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour {
    [SerializeField]
    List<GameObject> players;
    private int playerIndex = 1;

    private int pointIndex = 1;
    // Use this for initialization

    public void Showplayer()
    {
        if (playerIndex<players.Count)
        {
            playerIndex++;
            players[playerIndex].SetActive(true);
        }
        
    }

    public void recalculateTotals()
    {
        for (int i = 1; i < players.Count; i++)
        {
            players[i].GetComponent<TotalsManager>().CalcTotal();
        }        
    }


    public void ShowPointField()
    {
        pointIndex++;
        foreach (GameObject player in players)
        {
            player.GetComponent<PointPlayer>().points[pointIndex].SetActive(true);
            player.GetComponent<PointPlayer>().points[pointIndex].GetComponent<InputField>().text="0";
        }
        
    }
}
