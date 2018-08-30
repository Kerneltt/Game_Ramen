using UnityEngine.UI;
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

	void Start () {
        myRG = GetComponent<Rigidbody>();

        Text[] texts = transform.root.GetComponentsInChildren<Text>();

        foreach(Text txt in texts)
        {
            if(txt.transform.tag == "DiceNumber")
            {
                numberDysplay = txt;
            }
        }

        diceTester = FindObjectOfType<DiceTester>();
	}

    private void Update()
    {
        if(rolling)
        {
            if(myRG.velocity.sqrMagnitude < 0.01f && myRG.angularVelocity.sqrMagnitude < 0.01f)
            {
                rolling = false;
                myRG.velocity = Vector3.zero;
                myRG.angularVelocity = Vector3.zero;
                Debug.Log("checking number");
                CheckDiceNumber();
            }
        }
    }

    public void RollDice(){
        if (locked)
            return;

        Invoke("StartDetecting", 2f);

        float randomNum = Random.Range(2f, 10f);

        myRG.AddForce(Vector3.up * randomNum * upForce, ForceMode.VelocityChange);

        Vector3 randomRoll = new Vector3(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f));

        myRG.AddTorque(randomRoll * randomNum, ForceMode.VelocityChange);

        numberDysplay.text = "";
    }

    void CheckDiceNumber() 
    {
        Transform numberTop = myNumbers[0];
        float height = 0;
        float maxHeight = -2f;
        foreach(Transform num in myNumbers)
        {
            height = num.position.y;
            if(height > maxHeight)
            {
                numberTop = num;
                maxHeight = num.position.y;
            }
        }
        Debug.Log("answer: " + numberTop.name);
        numberDysplay.text = numberTop.name;
    }

    void StartDetecting()
    {
        rolling = true; 
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
        diceTester.RemoveDice(transform.root.gameObject);
        diceTester.ReorderAll();
        Destroy(transform.root.gameObject);
    }
}
