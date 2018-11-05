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
    GameObject loadTemplate;
    [SerializeField]
    GameObject loadButton;
    [SerializeField]
    GameObject loadEraseButton;
    [SerializeField]
    GameObject loadContentPanel;
    private string saveFileName; 
    public int contadorSaves;
    public int playerID;
    public int previousID;
    string[] saveFiles;
    int indexNumber; 
    List<string> filesList=new List<string>();
    // Use this for initialization
    void Start()
    {
        saveFiles = PlayerPrefs.GetString("ResourcesFile").Split('|');
        if (saveFiles.Length == 1){
            if (saveFiles[0].Equals("")){
                contadorSaves = 0; 
                return ;
            }
        }   
        for (int i = 0; i < saveFiles.Length; i++)
        {
            if (!saveFiles[i].Equals("")){
                filesList.Add(saveFiles[i]);
                contadorSaves++;
            }
            
        }

        contadorSaves = saveFiles.Length; 
        UpdateSaves();
    }
    // Update is called once per frame
    void Update()
    {

    }
    // Update save files to show on the load options
    public void UpdateList(){
        contadorSaves = 0; 
        for (int i = 0; i < saveFiles.Length; i++)
        {
            if (!saveFiles[i].Equals("")){
                filesList.Add(saveFiles[i]);
                contadorSaves++;
            }
            
        }
    }

    public void UpdateSaves(){
        foreach(RectTransform child in loadContentPanel.GetComponentInChildren<RectTransform>()){
            Destroy(child.gameObject);
        }
        for (int x = 0; x < saveFiles.Length; x++)
        {
            if(!saveFiles[x].Equals("")){
                GameObject initial = Instantiate(loadTemplate);

                Canvas temp = initial.GetComponent<Canvas>();
                temp.transform.SetParent(loadContentPanel.transform, false); 
                
                Button load = temp.transform.GetChild(0).GetComponent<Button>();
                Button delete = temp.transform.GetChild(1).GetComponent<Button>();
                load.GetComponentInChildren<Text>().text = saveFiles[x];
                load.onClick.AddListener(delegate {IdentifyButton(x); });
                load.onClick.AddListener(delegate {LoadResource(); });

                delete.onClick.AddListener(delegate {DestroyTemplate(saveFiles[x-1]);});
            }
        }
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

        //---------------------------Pruebas para update in rial time no feik de los loads
        string saveFileString = "templateSlot" +contadorSaves;
        string[] remplazo = new string[saveFiles.Length + 1];
        for (int x = 0; x < saveFiles.Length; x++){
            remplazo[x] = saveFiles[x];
        }
        remplazo[(saveFiles.Length)] = saveFileString;
        saveFiles = remplazo;
        
        //------------------------------------------------------------------------------------

        resourcesFiles += "|" + saveFileString; 
        contadorSaves ++; 
        foreach (Resource item in GetComponent<ResourceList>().resourceList)
        {
            resourceInfo += item.gameObject.GetComponentInChildren<Text>().text+";";
            resourceInfo += item.tresholdMax + "|";
        }
        PlayerPrefs.SetString(saveFileString,resourceInfo);
        PlayerPrefs.SetString("ResourcesFile", resourcesFiles);
        UpdateSaves(); 
        
        
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
        string resourceTemplateList = PlayerPrefs.GetString(saveFileName);
        string[] resources = resourceTemplateList.Split(spliter);  
        foreach (string res in resources)
        {
            if (res!="")
            {
                print(res);
                string name = res.Split(spliterName)[0];
                string restresh = res.Split(spliterName)[1];
                GetComponent<ResourceList>().LoadResoruce(int.Parse(restresh), name);
                print(name);
                print(restresh);
            }
            
        }
    }

    public void DeleteResources()
    {
        gameObject.GetComponent<ResourceList>().Clear();
    }

    void IdentifyButton(int index)
    {
        saveFileName = saveFiles[index -1]; 
    }

    void DestroyTemplate(string templateName){
        UpdateList(); 
        filesList.Remove(templateName); 
        string resourceFiles = PlayerPrefs.GetString("ResourceFile");
        string result = resourceFiles.Replace(templateName, ""); 
        PlayerPrefs.SetString("ResourcesFile", result); 

        PlayerPrefs.SetString(templateName, "");
        saveFiles = filesList.ToArray();
        UpdateSaves();
    }
}