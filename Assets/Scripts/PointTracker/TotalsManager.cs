using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalsManager : MonoBehaviour {
    [SerializeField]
    GameObject ModifierList;
    [SerializeField]
    InputField totalField;
    float total=0;
    // Use this for initialization
    public void CalcTotal()
    {
        total = 0;
        for (int i = 0; i < ModifierList.GetComponent<PointPlayer>().points.Count; i++)
        {
            string value= gameObject.GetComponent<PointPlayer>().points[i].GetComponent<InputField>().text;
            string multyplier = ModifierList.GetComponent<PointPlayer>().points[i].GetComponent<InputField>().text;
            total += float.Parse(value) * float.Parse(multyplier);
        }
        totalField.text= total.ToString();
    }
}
