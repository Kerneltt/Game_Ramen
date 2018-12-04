using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceList : MonoBehaviour
{
    int id;
    public List<Resource> resourceList;
    public List<int> resourceValue;
    [SerializeField]
    ResourceManager rmanager;
    [SerializeField]
    GameObject grid;
    [SerializeField]
    GameObject treshold;
    [SerializeField]
    GameObject acumulative;
    [SerializeField]
    InputField inpName;
    [SerializeField]
    Toggle istreshold;
    [SerializeField]
    GameObject maxTresh;    

    // Use this for initialization
    void Awake()
    {

    }
    
    private void Start()
    {
 
        rmanager = GameObject.FindObjectOfType<ResourceManager>();
        print("WokeAF");
        resourceList = rmanager.gameObject.GetComponent<ResourceList>().resourceList;
        foreach (Resource resource in resourceList)
        {
            resourceValue.Add(0);
        }
        if (gameObject.name!= "resourceManager")
        {

           // Button b = gameObject.GetComponent<Button>();
          //  b.onClick.AddListener(delegate () { ChangeOwner(id); });
        }
    }

    public void ChangeOwner(int idf)
    {
        int ind = 0;
        print("Saving Values for: " +id);        
        foreach (Resource resource in resourceList)
        {
            //give every value the quantity of every resource
            rmanager.PlayerList[id].resourceValue[ind] = Mathf.RoundToInt(resource.quantity);
            print(resource.quantity);
            ind++;

        }
    }


    public void SetID(int id)
    {
        this.id = id;
    }

    public void GetId()
    {
        rmanager.previousID = rmanager.playerID;
        rmanager.playerID =(id);
        rmanager.GetInfo((id));

    }

    public void CreateResoruce()
    {
        if (inpName.GetComponent<InputField>().text.ToString()=="")
        {
            return;
        }
        string name = inpName.GetComponent<InputField>().text.ToString();
        GameObject newresource;
        if (int.Parse(maxTresh.GetComponent<InputField>().text.ToString()) == 0)
        {
            istreshold.isOn = false;
        }
        if (istreshold.isOn)
        {
            newresource = Instantiate(treshold);            
            newresource.GetComponent<Resource>().SetMxTreshold(int.Parse(maxTresh.GetComponent<InputField>().text.ToString()));           
            print("treshold created");
        }
        else
        {
            newresource = Instantiate(acumulative);
            newresource.GetComponent<Resource>().tresholdMax = 0;
            print("acumulative created");
        }
        newresource.GetComponentInChildren<Text>().text = name;
        resourceList.Add(newresource.GetComponent<Resource>());
        resourceValue.Add(0);
        newresource.transform.SetParent(grid.transform);
        foreach (ResourceList player in rmanager.PlayerList)
        {
            print(player.id);
            player.resourceList=resourceList;
            player.resourceValue.Add(0);
        }
    }
    public void Clear()
    {
        foreach (Resource player in resourceList)
        {            
            Destroy(player.gameObject);
        }
        resourceList.Clear();
    }
    public void LoadResoruce(int maxtreshValue, string name)
    {        
        GameObject newresource;
        if (maxtreshValue > 0)
        {
            newresource = Instantiate(treshold);
            newresource.GetComponent<Resource>().SetMxTreshold(maxtreshValue);
            print("treshold created");
        }
        else
        {
            newresource = Instantiate(acumulative);
            //newresource.GetComponent<Resource>().SetMxTreshold(0);
            print("acumulative created");
        }

        resourceList.Add(newresource.GetComponent<Resource>());
        resourceValue.Add(0);
        newresource.transform.SetParent(grid.transform);
        newresource.GetComponentInChildren<Text>().text = name;
        foreach (ResourceList player in rmanager.PlayerList)
        {
            print(player.id);
            player.resourceList = resourceList;
            player.resourceValue.Add(0);
        }
    }
    public List<Resource> GetResourceList()
    {
        return this.resourceList;
    }

    public void ResetResources()
    {
        foreach (Resource r in resourceList)
        {
            r.ResetQuant();
        }
    }

}