using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTester : MonoBehaviour {

    Dice[] dices;

    public GameObject d4;
    public GameObject d6;
    public GameObject d8;
    public GameObject d10;
    public GameObject d12;
    public GameObject d20;
    public GameObject d100;

    public GameObject diceContainer;

    List<GameObject> diceContainers = new List<GameObject>();

    int totalDices = 0;

    float containerXPos = 11;
    float containerYPos = 3;

    float columnMulti = 3;

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
        if (diceContainers.Count<9)
        {
            GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (go.transform.GetChild(i).tag == "Respawn")
                {
                    GameObject diceGo = Instantiate(d4, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);
                    CheckPosition();
                }
            }
            diceContainers.Add(go);
        }
    }

    public void SpawnD6()
    {
        if (diceContainers.Count < 9)
        {
            GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (go.transform.GetChild(i).tag == "Respawn")
                {
                    GameObject diceGo = Instantiate(d6, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);
                    CheckPosition();
                }
            }
            diceContainers.Add(go);
        }
    }

    public void SpawnD8()
    {
        if (diceContainers.Count < 9)
        {
            GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (go.transform.GetChild(i).tag == "Respawn")
                {
                    GameObject diceGo = Instantiate(d8, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);
                    CheckPosition();
                }
            }
            diceContainers.Add(go);
        }
    }

    public void SpawnD10()
    {
        if (diceContainers.Count < 9)
        {
            GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (go.transform.GetChild(i).tag == "Respawn")
                {
                    GameObject diceGo = Instantiate(d10, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);
                    CheckPosition();
                }
            }
            diceContainers.Add(go);
        }
    }

    public void SpawnD12()
    {
        if (diceContainers.Count < 9)
        {
            GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (go.transform.GetChild(i).tag == "Respawn")
                {
                    GameObject diceGo = Instantiate(d12, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);
                    CheckPosition();
                }
            }
            diceContainers.Add(go);
        }
    }

    public void SpawnD20()
    {
        if (diceContainers.Count < 9)
        {
            GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (go.transform.GetChild(i).tag == "Respawn")
                {
                    GameObject diceGo = Instantiate(d20, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);
                    CheckPosition();
                }
            }
            diceContainers.Add(go);
        }
    }

    public void SpawnD100()
    {
        if (diceContainers.Count < 9)
        {
            GameObject go = Instantiate(diceContainer, new Vector3(containerXPos, 0f, containerYPos), Quaternion.identity);
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (go.transform.GetChild(i).tag == "Respawn")
                {
                    GameObject diceGo = Instantiate(d100, go.transform.GetChild(i).transform.position, Quaternion.identity, go.transform);
                    CheckPosition();
                }
            }
            diceContainers.Add(go);
        }
    }

    void CheckPosition()
    {
        if (containerXPos >= 30)
        {
            containerXPos = 0f;
            containerYPos += 11f;
        }
        containerXPos += 11f;

        if(diceContainers.Count / 2f > columnMulti-1)
        {
            columnMulti += 1f;
            Vector3 newPos = new Vector3(transform.position.x,
                                         transform.position.y + 10f,
                                         transform.position.z + 3f);
            transform.position = newPos;
        }
    }

    public void RemoveDice(GameObject removedContainer)
    {
        diceContainers.Remove(removedContainer);
        if(columnMulti - 1 >= 3)
        {
            if (diceContainers.Count / 2f < columnMulti - 1)
            {
                columnMulti -= 1f;
                Vector3 newPos = new Vector3(transform.position.x,
                                             transform.position.y - 10f,
                                             transform.position.z - 3f);
                transform.position = newPos;
            } 
        }
    }

    public void ReorderAll()
    {
        containerXPos = 11f;
        containerYPos = 0f;
        foreach(GameObject containerGO in diceContainers)
        {
            containerGO.transform.position = new Vector3(containerXPos, 0f, containerYPos);
            CheckPosition();
        }
    }
}
