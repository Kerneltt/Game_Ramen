using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject totalField;
	public GameObject PointsField;


	private List<GameObject> pfList = new List<GameObject> ();
	private int pointsCount;
	private float total;
	private List<float> mutiplierList;

    public Player()
    {
		pointsCount = 1;
    }
	// Use this for initialization
	void Start () {
		total = 0;
		mutiplierList = new List<float> ();
		pointsCount = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddPointsField()
    {
		GameObject pointsField = Instantiate (PointsField);
        pointsField.transform.SetParent(transform);
		pfList.Add (pointsField);

		totalField.transform.SetAsLastSibling ();
    }

	public void SetPointsField(int x) {


		for (int i = pointsCount; i < x; i++) {
			GameObject points = Instantiate (PointsField);
			points.transform.SetParent(this.transform);
			pfList.Add (points);

		}
		this.pointsCount = x;
		totalField.transform.SetAsLastSibling ();
	}


    public void RemovePointsField(int index)
    {
		if (pfList.Count > 0) {
			Destroy (pfList [pfList.Count - 1]);
		}
    }

    public float getTotal()
    {
        return this.total;
    }

	public void setMutiplierList(List<float> l) {
		mutiplierList = l;
		calculateTotal ();
	}

    public void calculateTotal()
    {
		int i = 0;
        this.total = 0;

		foreach (RectTransform child in transform) {
			if (child.tag == "PointsField") {
				int value;
				int.TryParse (child.GetComponent<InputField> ().text, out value);
				this.total = mutiplierList[i] * (this.total + value);
				i++;
			}
		}

		totalField.GetComponent<InputField>().text = this.total.ToString();
    }
}
