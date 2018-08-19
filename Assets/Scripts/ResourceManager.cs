using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public List<ResourceList> PlayerList;
    public float[] avaiablePlayers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject Grid;
    [SerializeField]
    GameObject inputField;
    public int playerID;
    public int previousID;
    string[] saveFiles;
    // Use this for initialization
    void Start()
    {
        saveFiles = PlayerPrefs.GetString("ResourcesFile").Split('|');
    }
    // Update is called once per frame
    void Update()
    {

    }
    // Get player avaible, if able
    public void Newplayer()
    {
        GameObject newplayer=null;
        for (int i = 0; i < 9; i++)

        {
            if (avaiablePlayers[i] > 0)
            {                
                ResourceList rl = Instantiate(player).GetComponent<ResourceList>();
                newplayer = rl.gameObject;
                rl.GetComponent<RectTransform>().SetParent(Grid.GetComponent<RectTransform>());
                string name = inputField.GetComponent<InputField>().text.ToString();
                newplayer.GetComponentInChildren<Text>().text = name;
                rl.SetID(i);

                //Check for any other player, if there is, copy their resource setting
                // If there isnt, just create the new player as it its, without a resource yet. 
                List<Resource> HDResources;
                if (PlayerList.Count > 0)
                {
                  //  ResourceList newKid = Instantiate(PlayerList[0]);
                  //  newKid.ResetResources();
                  //  rl = newKid;

                }

                PlayerList.Add(rl);
                avaiablePlayers[i] = 0;
                break;
            }
        }

    }

    public void GetInfo(int index)
    {
        print("gettingInfo");
        int ind = 0;
        PlayerList[previousID].ChangeOwner(previousID);
        foreach (Resource resource in PlayerList[index].resourceList)
        {
            //assingn the value in te correct object
            resource.quantity = PlayerList[index].resourceValue[ind];
            resource.SetSlider();
            ind++;

        }
    }

    public void SaveResourceInfo()
    {
        string filename =saveFiles+ "Resources" + (saveFiles.Length-1)+"|";
        string resourceInfo="";
        foreach (Resource item in GetComponent<ResourceList>().resourceList)
        {
            resourceInfo += item.gameObject.GetComponentInChildren<Text>().text+";";
            resourceInfo += item.tresholdMax + "|";
        }

        PlayerPrefs.SetString("Resources",resourceInfo);
        PlayerPrefs.SetString("ResourcesFile", filename);
        print(PlayerPrefs.GetString("Resources"));
    }
    public void PrefReset()
    {
        PlayerPrefs.SetString("Resources", "");
    }
    public void LoadResource()
    {
        DeleteResources();
        char spliter = '|';
        char spliterName = ';';
        string[] resources = PlayerPrefs.GetString("Resources").Split(spliter);        
        foreach (string res in resources)
        {
            if (res!="")
            {
                string name = res.Split(spliterName)[0];
                string restresh = res.Split(spliterName)[1];
                print(res);
                GetComponent<ResourceList>().LoadResoruce(int.Parse(restresh), name);
            }
            
        }
    }

    public void DeleteResources()
    {
        gameObject.GetComponent<ResourceList>().Clear();
    }
}
