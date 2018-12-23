using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followpos : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position = GetComponentInChildren<Dice>().gameObject.transform.position;
	}
    public void Movepos()
    {
        GetComponentInChildren<Canvas>().gameObject.transform.position= GetComponentInChildren<Dice>().gameObject.transform.position;        
    }

}
