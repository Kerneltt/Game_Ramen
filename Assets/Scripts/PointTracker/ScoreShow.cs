using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreShow : MonoBehaviour {
    Text txtGame,txtDate,txtPlayer1,txtPlayer2,txtPlayer3,txtPlayer4,txtPlayer5;
    string game,date,player1, player2, player3, player4, player5;
    Transform parent, registry;

    // Use this for initialization
    public void AssignUI(Transform p, Transform r, Text g, Text d, Text p1, Text p2, Text p3, Text p4, Text p5)
    {
        parent = p;
        registry = r;
        txtGame = g;
        txtDate = d;
        txtPlayer1 = p1;
        txtPlayer2 = p2;
        txtPlayer3 = p3;
        txtPlayer4 = p4;
        txtPlayer5 = p5;

    }
    public void SetPlayers(string g, string d, string p1, string p2, string p3, string p4, string p5)
    {
        game = g;
        date = d;
        player1 = p1;
        player2 = p2;
        player3 = p3;
        player4 = p4;
        player5 = p5;
    }

    public void ShowRegistry()
    {
        registry.gameObject.SetActive (true);
        txtGame.text = game;
        txtDate.text = date;
        txtPlayer1.text = player1;
        txtPlayer2.text = player2;
        txtPlayer3.text = player3;
        txtPlayer4.text = player4;
        txtPlayer5.text = player5;
        parent.gameObject.SetActive (false);
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
