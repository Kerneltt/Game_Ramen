using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour {

	public GameObject PointsText;
	public GameObject addPointsButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddPointsText() {
		GameObject points = Instantiate (PointsText);
		points.transform.parent = this.transform;
		//addPointsButton.transform.SetAsLastSibling ();
	}
}
