using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierManager : MonoBehaviour {

	public GameObject MultiplierField;
	public GameObject EndField;

	private List<float> listMutipliers;

	// Use this for initialization
	void Start () {
		listMutipliers = new List<float> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddMultiplierField() {
		GameObject newField = Instantiate (MultiplierField);
		newField.transform.SetParent(this.transform);
		newField.GetComponent<InputField> ().text = "1"	;


	}

	private void UpdateData() {
		int i = 0;

		foreach (RectTransform child in transform) {
			if (child.tag == "Mutiplier") {
				child.GetComponent<InputField>().text = listMutipliers[i].ToString();
				i++;
			}
		}
	}
				

	public void setMutipliers(List<float> l) {
		this.listMutipliers = l;
		UpdateData ();	
	}
}
