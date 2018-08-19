using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    [SerializeField]
    GameObject maxTresh;
    [SerializeField]
    GameObject actTresh;
    [SerializeField]
    Slider slider;
    public string RName;
    public Image icon;
    public float tresholdMax;
    public float quantity;
    //si treshold es mayor a 0 usa treshhold

    
    private void Update()
    {
        if (tresholdMax > 0)
        {
            Text text1 = actTresh.GetComponent<Text>();
            quantity = slider.value;
            text1.text = quantity.ToString();
        }
        else
        {
            Text text = actTresh.GetComponent<Text>();
            text.text = "" + quantity;
        }

    }

    public void AddOne()
    {
        quantity = quantity + 1;
    }

    public void AddFive()
    {
        quantity = quantity + 5;
    }

    public void TakeOne()
    {
        if (quantity >= 1)
        {
            quantity = quantity - 1;
        }
        else
        {
            Debug.Log("insufficient Resources for that action");
        }

    }
    public void TakeFive()
    {
        if (quantity >= 5)
        {
            quantity = quantity - 5;
        }
        else
        {
            Debug.Log("insufficient Resources for that action");
        }
    }

    public void ResetQuant()
    {
        this.quantity = 0;
    }
    public void SetSlider()
    {
        if (slider!=null)
        {
            slider.value = quantity;
        }
    }
    public void SetMxTreshold(int max)
    {
        tresholdMax = max;
        slider.maxValue = max;
        Text textTresh = maxTresh.GetComponent<Text>();
        textTresh.text = max.ToString();
        
    }




}
