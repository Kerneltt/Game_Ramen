using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager2 : MonoBehaviour {
    Dice[] dices;
    float force = 100;
    [SerializeField]
    GameObject[] die;
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    GameObject collorpickerIMG;
    [SerializeField]
    Texture2D collors;
    [SerializeField]
    GameObject collorpickerButton;
    // Use this for initialization
    void Start () {
		
	}

    public void Setcollor()
    {
        RectTransform rt = collorpickerIMG.GetComponent<RectTransform>();        
        float width = rt.rect.width;
        float height = rt.rect.height;
        Touch touch = Input.GetTouch(0);        
        print("X: " + (collorpickerIMG.transform.InverseTransformPoint(touch.position).x));
        print("Y:" +  (collorpickerIMG.transform.InverseTransformPoint(touch.position).y));
        print("X trans: " + ((((collorpickerIMG.transform.InverseTransformPoint(touch.position).x +width/2)* collors.width) + (width / 2)) / width));
        print("Y trans:" + (((((collorpickerIMG.transform.InverseTransformPoint(touch.position).y +height/2)* collors.height) + (height / 2)) / height)));
        int x = (int)((((collorpickerIMG.transform.InverseTransformPoint(touch.position).x + width / 2) * collors.width) + (width / 2)) / width);
        int y = (int)((((collorpickerIMG.transform.InverseTransformPoint(touch.position).y + height / 2) * collors.height) + (height / 2)) / height);
        collorpickerButton.GetComponent<Image>().color = collors.GetPixel(x, y);
        //print(collors.GetPixel(x, y));


    }

    public void SpawnDice(int dice)
    {
        GameObject newdice=  Instantiate(die[dice]);
        newdice.GetComponent<Renderer>().material.color = collorpickerButton.GetComponent<Image>().color;
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
