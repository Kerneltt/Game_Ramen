using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

    public bool locked = false;

    private Rigidbody myRG;

    float upForce = 2f;

	// Use this for initialization
	void Start () {
        myRG = GetComponent<Rigidbody>();
	}

    public void RollDice(){
        if (locked)
            return;

        float randomNum = Random.Range(2f, 10f);

        myRG.AddForce(Vector3.up * randomNum * upForce, ForceMode.VelocityChange);

        Vector3 randomRoll = new Vector3(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f));

        myRG.AddTorque(randomRoll * randomNum, ForceMode.VelocityChange);

    }

    public void LockDice()
    {
        locked = true;
    }

    public void UnlockDice()
    {
        locked = false;
    }

    public void RemoveDice()
    {
        Destroy(transform.root.gameObject);
    }
}
