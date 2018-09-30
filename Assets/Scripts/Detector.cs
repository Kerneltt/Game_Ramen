using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {

    public void CheckNumberInDice() {
        Collider[] numberCols = Physics.OverlapBox(transform.position, new Vector3(1000f, 1f, 1000f));

        foreach(Collider col in numberCols)
        {
            if (col.tag == "number")
            {
                Debug.Log("Dice: " + col.transform.root.name + " Number: " + col.name);
            }
        }
    }
}
