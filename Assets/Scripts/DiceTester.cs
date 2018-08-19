using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTester : MonoBehaviour {

    Dice[] dices;

    Detector detector;

    public GameObject d4;
    public GameObject d6;
    public GameObject d8;
    public GameObject d10;
    public GameObject d12;
    public GameObject d20;
    public GameObject d100;

    public GameObject diceContainer;

    int totalDices = 0;

    float containerXPos = 11;
    float containerYPos = 0;

	
	void Start () {
        detector = FindObjectOfType<Detector>();
	}
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.R))
        {
            detector.CheckNumberInDice();
        }
	}

    public void RollDices()
    {
        dices = FindObjectsOfType<Dice>();

        foreach (Dice diceToRoll in dices)
        {
            diceToRoll.RollDice();
        }
    }

    public void SpawnD4()
    {
        GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).tag == "Respawn")
            {
                Instantiate(d4, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);

                CheckPosition();
            }
        }
    }

    public void SpawnD6()
    {
        GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).tag == "Respawn")
            {
                Instantiate(d6, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);

                CheckPosition();
            }
        }
    }

    public void SpawnD8()
    {
        GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).tag == "Respawn")
            {
                Instantiate(d8, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);

                CheckPosition();
            }
        }
    }

    public void SpawnD10()
    {
        GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).tag == "Respawn")
            {
                Instantiate(d10, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);

                CheckPosition();
            }
        }
    }

    public void SpawnD12()
    {
        GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).tag == "Respawn")
            {
                Instantiate(d12, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);

                CheckPosition();
            }
        }
    }

    public void SpawnD20()
    {
        GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).tag == "Respawn")
            {
                Instantiate(d20, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);

                CheckPosition();
            }
        }
    }

    public void SpawnD100()
    {
        GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).tag == "Respawn")
            {
                Instantiate(d100, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);

                CheckPosition();
            }
        }
    }

    void CheckPosition()
    {
        if (containerXPos >= 100)
        {
            containerXPos = 0f;
            containerYPos += 11f;
        }
        containerXPos += 11f;
    }
}
