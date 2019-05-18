﻿using UnityEngine.UI;
using UnityEngine;

public class Dice : MonoBehaviour
{

    DiceTester diceTester;

    public bool locked = false;

    bool rolling = false;

    private Rigidbody myRG;

    float upForce = 2f;

    public Transform[] myNumbers;

    public Text numberDysplay;

    bool killable = false;
    [SerializeField]
    bool isCoin = false;
    [SerializeField]
    GameObject imageLock;
    [SerializeField]
    ParticleSystem endParticle;

    float maxVelocity = 30;

    void Start() {
        myRG = GetComponent<Rigidbody>();

        Text[] texts = transform.root.GetComponentsInChildren<Text>();

        foreach (Text txt in texts)
        {
            if (txt.transform.tag == "DiceNumber")
            {
                numberDysplay = txt;
            }
        }

        diceTester = FindObjectOfType<DiceTester>();
    }
    public void Setkillable(bool kill)
    {
        killable = kill;
    }

    private void Update()
    {
        if (GetComponent<Rigidbody>()==null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            myRG = rb;
        }
        if (rolling)
        {
            if (myRG.velocity.sqrMagnitude < 0.01f && myRG.angularVelocity.sqrMagnitude < 0.01f)
            {
                rolling = false;
                myRG.velocity = Vector3.zero;
                myRG.angularVelocity = Vector3.zero;
                // Debug.Log("checking number");
                //  CheckDiceNumber();
            }
        }
        if (transform.position.y < 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public void RollDice() {
        if (isCoin)
        {
            //GetComponent<Animator>().Play("none");
            switch (Random.Range(0, 2))
            {
                case 0:

                    GetComponent<Animator>().Play("Rotate_1", -1, 0f);
                    print("0");
                    //GetComponent<Animator>().SetBool("repeat", false);
                    break;

                case 1:
                    GetComponent<Animator>().Play("Rotate_2", -1, 0f);
                    print("1");
                    //GetComponent<Animator>().SetBool("repeat", false);
                    break;                    
            }
            rolling = false;
            return;
        }
        if (locked )
                return;
        if (rolling)
        {
            print("is this happening?");
            myRG.AddForce(Vector3.Normalize( myRG.velocity) *100 ,ForceMode.VelocityChange);
        }
        else
        {
            
            //print("rolling");
            rolling = true;
            Invoke("StartDetecting", 2f);

            float randomNum = Random.Range(-10f, 10f);
            if (randomNum <= 0 && randomNum > -5)
            {
                randomNum = -5;
            }
            if (randomNum >= 0 && randomNum < 5)
            {
                randomNum = 5;
            }
            //print(randomNum);
            myRG.AddForce(Vector3.up * (Mathf.Abs(randomNum)) * upForce, ForceMode.VelocityChange);
            myRG.AddForce(Vector3.right * randomNum * upForce, ForceMode.VelocityChange);
            myRG.AddForce(Vector3.forward * randomNum * upForce, ForceMode.VelocityChange);

            Vector3 randomRoll = new Vector3(Random.Range(5f, 10f), Random.Range(5f, 10f), Random.Range(5f, 10f));

            myRG.AddTorque(randomRoll * randomNum, ForceMode.VelocityChange);
        }
        
        
        

        if (myRG.velocity.sqrMagnitude > maxVelocity)
        {
            //smoothness of the slowdown is controlled by the 0.99f, 
            //0.5f is less smooth, 0.9999f is more smooth
            myRG.velocity *= 0.5f;
        }
        //numberDysplay.text = "";
    }

    void CheckDiceNumber()
    {
        Transform numberTop = myNumbers[0];
        float height = 0;
        float maxHeight = -2f;
        foreach (Transform num in myNumbers)
        {
            height = num.position.y;
            if (height > maxHeight)
            {
                numberTop = num;
                maxHeight = num.position.y;
            }
        }
        //Debug.Log("answer: " + numberTop.name);
        //numberDysplay.text = numberTop.name;
    }

    void StartDetecting()
    {
        rolling = true;
    }

    public void LockDice()
    {
        locked = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        imageLock.SetActive(true);
        GetComponentInParent<followpos>().Movepos();
    }

    public void UnlockDice()
    {
        locked = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        imageLock.SetActive(false);
    }

    public void RemoveDice()
    {

        diceTester.RemoveDice(transform.root.gameObject);
        diceTester.ReorderAll();
        Destroy(transform.root.gameObject);

    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Killwall" && killable)
        {
            print("death");
            Destroy(this.transform.parent.gameObject);
        }
    }
    */
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Killwall" && killable)
        {
            PlayParticle();
            print("death");
            Destroy(this.transform.parent.gameObject);
        }
    }

    public bool  getIsCoin()
    { return isCoin; }

    public void PlayParticle()
    {
        if (gameObject.activeInHierarchy)
        {
            ParticleSystem newpart = Instantiate(endParticle);
            newpart.transform.position = gameObject.transform.position;
        }
    }
}
