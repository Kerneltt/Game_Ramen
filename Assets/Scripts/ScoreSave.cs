using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSave : MonoBehaviour {
    public GameObject[] players;
    public InputField name;
    string score;
    [SerializeField]
    ScoreDispaly display;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Z))
        {
            PlayerPrefs.SetString("ScoreLogs", "");
            print("LogsDeleted");
        }
	}

    public void OnSavePressed()
    {
        if (players[0].activeSelf==true)
        {
            score = "";
            score += name.text + "|" + System.DateTime.Now.ToString();
            foreach (GameObject player in players)
            {
                if (player.activeSelf == true)
                {
                    score += "|" + player.GetComponent<InputField>().text + ": " + player.GetComponent<PointPlayer>().points[8].GetComponent<InputField>().text;
                }
            }
            score = score + "*";
            PlayerPrefs.SetString("ScoreLogs", PlayerPrefs.GetString("ScoreLogs") + score);
            print(PlayerPrefs.GetString("ScoreLogs"));
        }
        display.UpdateScores();
    }
}
