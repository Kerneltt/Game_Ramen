using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager2 : MonoBehaviour {
    Dice[] dices;
    float force = 100;
    [SerializeField]
    GameObject[] die;
    [SerializeField]
    LayerMask mask;
    // Use this for initialization
    void Start () {
		
	}

    public void SpawnDice(int dice)
    {
        Instantiate(die[dice]);
    }
	// Update is called once per frame
	void Update () {

        Vector3 dir  = Vector3.zero;
        dir.x = -Input.acceleration.x;
        dir.z = Input.acceleration.z;
        dir.y = Input.acceleration.y;
        if (dir.sqrMagnitude > 5)
        {
            dices = FindObjectsOfType<Dice>();
            dir.Normalize();
            foreach (Dice diceToRoll in dices)
            {
                
                diceToRoll.RollDice();
               // diceToRoll.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
            }
        }

        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit,Mathf.Infinity,mask))
            {
                if (raycastHit.collider.tag=="Dice")
                {
                    Debug.Log("Lock");
                    if (raycastHit.collider.gameObject.GetComponent<Dice>().locked)
                    {
                        raycastHit.collider.gameObject.GetComponent<Dice>().UnlockDice();
                    }
                    else
                    {
                        raycastHit.collider.gameObject.GetComponent<Dice>().LockDice();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            dices = FindObjectsOfType<Dice>();
            print("rolling");
            foreach (Dice diceToRoll in dices)
            {
                
                diceToRoll.RollDice();
            }
        }
	}
}
