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
    [SerializeField]
    GameObject loadButton;
    [SerializeField]
    GameObject loadContentPanel;
    private string saveFileName; 
    public int playerID;
    public int previousID;
    string[] saveFiles;
    List<string> filesList=new List<string>();
    // Use this for initialization
    void Start()
    {
        saveFiles = PlayerPrefs.GetString("ResourcesFile").Split('|');
        for (int i = 0; i < saveFiles.Length; i++)
        {
            filesList.Add(saveFiles[i]);
        }
        for (int x = 0; x < saveFiles.Length; x++)
        {
            GameObject temp = (GameObject)Instantiate(loadButton);
            temp.transform.SetParent(loadContentPanel.transform, false);
        }
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

        string resourceInfo="";            
        string resourcesFiles = PlayerPrefs.GetString("ResourcesFile");

        if (filesList.Contains(saveFileName))
        {
            print("Nombre de Plantilla existente ! :D");
        }

        else
        {   
            resourcesFiles += "|" +saveFileName;
            string resourceSlotName = "RTslot" + saveFileName;
            foreach (Resource item in GetComponent<ResourceList>().resourceList)
            {
                resourceInfo += item.gameObject.GetComponentInChildren<Text>().text+";";
                resourceInfo += item.tresholdMax + "|";
            }

            PlayerPrefs.SetString(resourceSlotName,resourceInfo);
            PlayerPrefs.SetString("ResourcesFile", resourcesFiles);
            print(PlayerPrefs.GetString(resourceSlotName));
        }


        
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
        string resourceTemplateList = PlayerPrefs.GetString("RTslot"+saveFileName);
        string[] resources = PlayerPrefs.GetString(resourceTemplateList).Split(spliter);      
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
