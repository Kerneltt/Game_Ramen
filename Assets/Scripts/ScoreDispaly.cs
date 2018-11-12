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
                temp.GetComponentInChildren<Button>().gameObject.GetComponentInChildren<Text>().text = substring[0]+ "\n"+ substring[1];
                temp.transform.SetParent(ScoreParent.transform);
            }            
        }
    }
}
