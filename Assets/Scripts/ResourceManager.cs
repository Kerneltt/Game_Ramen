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
    [SerializeField]
    GameObject resourceName;
    [SerializeField]
    GameObject newPlayerButton;
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
        filesList=new List<string>();
        contadorSaves = 0; 
        for (int i = 0; i < saveFiles.Length; i++)
        {
            if (!saveFiles[i].Equals("")){
                filesList.Add(saveFiles[i]);
                contadorSaves++;
            }
            
        }
    }

    public void CleanResources(){
        string resources = PlayerPrefs.GetString("ResourcesFile");
        string[] characters = new string[resources.Length]; 

        for(int i = 0; i < resources.Length; i++){
            characters[i] = System.Convert.ToString(resources[i]); 
        } 

        int startingIndex = 0; 
        for(int i = 0; i < resources.Length; i++){
            if(characters[i]!="|"){
                startingIndex = i; 
                break; 
            }
        }

        string realrequest = resources.Substring(startingIndex);
        PlayerPrefs.SetString("ResourcesFile", realrequest);


    }

    public void UpdateSaves(){
        CleanResources(); 
        foreach(RectTransform child in loadContentPanel.GetComponentInChildren<RectTransform>()){
            Destroy(child.gameObject);
        }
        for (int x = 0; x < saveFiles.Length; x++)
        {
            if(!saveFiles[x].Equals("")){
                int num = x; 
                GameObject initial = Instantiate(loadTemplate);

                Canvas temp = initial.GetComponent<Canvas>();
                temp.transform.SetParent(loadContentPanel.transform, false); 
                
                Button load = temp.transform.GetChild(0).GetComponent<Button>();
                Button delete = temp.transform.GetChild(1).GetComponent<Button>();
                load.GetComponentInChildren<Text>().text = saveFiles[num];
                load.onClick.AddListener(delegate {IdentifyButton(num); });
                load.onClick.AddListener(delegate {LoadResource(); });

                delete.onClick.AddListener(delegate {DestroyTemplate(saveFiles[num-1]);});
                num = num + 1; 

            }
        }
    }

    // Get player avaible, if able
    public void Newplayer()
    {
        if (inputField.GetComponent<InputField>().text.ToString()=="")
        {
            return;
        }
        GameObject newplayer=null;
        for (int i = 0; i < 10; i++)

        {
            if (avaiablePlayers[i] > 0)
            {                
                ResourceList rl = Instantiate(player).GetComponent<ResourceList>();
                newplayer = rl.gameObject;
                rl.GetComponent<RectTransform>().SetParent(Grid.GetComponent<RectTransform>());
                string name = inputField.GetComponent<InputField>().text.ToString();
                newplayer.GetComponentInChildren<Text>().text = name;
                rl.SetID(i);
                newplayer.GetComponent<Toggle>().group = newplayer.GetComponentInParent<ToggleGroup>();
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
        if (avaiablePlayers[9] == 0)
        {
            newPlayerButton.SetActive(false);
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
        if (resourceName.GetComponent<InputField>().text == "")
        {
            return;
        }
        string resourceInfo="";            
        string resourcesFiles = PlayerPrefs.GetString("ResourcesFile");

        //---------------------------Pruebas para update in rial time no feik de los loads
        string saveFileString = resourceName.GetComponent<InputField>().text.ToString();  
        if(resourcesFiles.Contains(saveFileString)){
            //Si el nombre ingresado ya existia como un template, negar el guardado
            print("TemplateName: " + saveFileString + " existente"); 
        }
        else{
            //Si el nombre ingresado no existe dentro de los templates guardados

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
        resourceName.GetComponent<InputField>().text = "";
        
        
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
                string name = res.Split(spliterName)[0];
                string restresh = res.Split(spliterName)[1];
                GetComponent<ResourceList>().LoadResoruce(int.Parse(restresh), name);
            }
            
        }
    }

    public void DeleteResources()
    {
        gameObject.GetComponent<ResourceList>().Clear();
    }

    void IdentifyButton(int index)
    { 
        saveFileName = saveFiles[index - 1]; 

    }

    void DestroyTemplate(string templateName){
        print("TemplateName: " + templateName); 

        UpdateList(); 
        filesList.Remove(templateName); 

        string resourceFiles = PlayerPrefs.GetString("ResourcesFile");
        print("ResourceFiles Antes: " +  resourceFiles); 
        string source = templateName; 
        string result = resourceFiles.Replace(source, ""); 
        PlayerPrefs.SetString("ResourcesFile", result); 
        CleanResources(); 
        print("Resources Files Despues: "+ PlayerPrefs.GetString("ResourcesFile")); 

        PlayerPrefs.SetString(templateName, "");
        saveFiles = filesList.ToArray();

        for(int i = 0; i < saveFiles.Length; i++){
            print("Elementos restantes: " + saveFiles[i]); 
        }

        UpdateSaves(); 

    }
}