using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDispaly : MonoBehaviour {
    [SerializeField]
    GameObject ScoreParent;
    [SerializeField]
    GameObject scorePrefab;
    string[] scorestring;
    [SerializeField]
    Text txtGame,txtDate,txtP1,txtP2,txtP3,txtP4,txtP5;
    [SerializeField]
    Transform displayScores, buttonsPanel;
    // Use this for initialization
    void Start () {
        UpdateScores();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateScores()
    {        
        foreach (Transform children in ScoreParent.GetComponentInChildren<Transform>())
        {
            Destroy(children.gameObject);
        }
        scorestring = PlayerPrefs.GetString("ScoreLogs").Split('*');
        foreach (string registry in scorestring)
        {
            string[] substring = registry.Split('|');
            if (registry!="")
            {
                GameObject temp;
                temp = Instantiate(scorePrefab);                
                temp.GetComponent<ScoreShow>().AssignUI(buttonsPanel,displayScores,txtGame,txtDate,txtP1,txtP2,txtP3,txtP4,txtP5);
                temp.GetComponentInChildren<Button>().gameObject.GetComponentInChildren<Text>().text = substring[0]+ "\n"+ substring[1];
                switch (substring.Length)
                {
                    case 3:
                        temp.GetComponent<ScoreShow>().SetPlayers(substring[0], substring[1], substring[2], "", "", "", "");
                        break;
                    case 4:
                        temp.GetComponent<ScoreShow>().SetPlayers(substring[0], substring[1], substring[2], substring[3], "", "", "");
                        break;
                    case 5:
                        temp.GetComponent<ScoreShow>().SetPlayers(substring[0], substring[1], substring[2], substring[3], substring[4], "", "");
                        break;
                    case 6:
                        temp.GetComponent<ScoreShow>().SetPlayers(substring[0], substring[1], substring[2], substring[3], substring[4], substring[5], "");
                        break;
                    case 7:
                        temp.GetComponent<ScoreShow>().SetPlayers(substring[0], substring[1], substring[2], substring[3], substring[4], substring[5], substring[6]);
                        break;
                    default:
                        break;
                }
                print(PlayerPrefs.GetString("ScoreLogs"));
                temp.transform.SetParent(ScoreParent.transform);
            }            
        }
    }
}
