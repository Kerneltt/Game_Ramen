using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointTracker : MonoBehaviour {

	public GameObject Player;
	public GameObject pointsM;

	private int pointsCount;
	private List<GameObject> playerList;
	private List<float> multiplierList;
    private int maxPlayers = 5;
    private int curPlayers = 1;
    private int maxPoints = 8;
    private int curPoints = 1;
    // Use this for initialization
    void Start () {
		Transform mManager = transform.Find ("MultiplierManager");
		GameObject player = Instantiate (Player);
		pointsCount = 1;
		playerList = new List<GameObject> ();
		multiplierList = new List<float> ();

		multiplierList.Add (1);
		mManager.GetComponent<MultiplierManager> ().setMutipliers (multiplierList);

		player.transform.SetParent(this.transform);
		playerList.Add (player);
		player.GetComponent<Player> ().setMutiplierList (this.multiplierList);
	}

	// Update is called once per frame
	void Update () {

	}

	public void addPlayer()
	{
        if (curPlayers<maxPlayers)
        {
            GameObject player = Instantiate(Player);
            player.transform.SetParent(this.gameObject.transform);
            player.GetComponent<Player>().SetPointsField(pointsCount);
            Debug.Log(pointsCount);
            playerList.Add(player);
            player.GetComponent<Player>().setMutiplierList(this.multiplierList);
            curPlayers++;
        }
		
	}

	public void removePlayer(int index)
	{
		if (playerList.Count > 0)
			Destroy (playerList [playerList.Count - 1]);
	}

	public void addPointsField()
	{
        if (curPoints<maxPoints)
        {
            foreach (RectTransform child in transform)
            {
                if (child.tag == "Points")
                {
                    child.gameObject.GetComponent<PointsManager>().AddPointsText();
                }
                else if (child.tag == "Multiplier")
                {
                    child.gameObject.GetComponent<MultiplierManager>().AddMultiplierField();
                }
                else
                {
                    child.GetComponent<Player>().AddPointsField();
                }
            }
            this.pointsCount += 1;
            curPoints++;
        }
		

	}

	public void UpdateMutiplier() {

		Transform mManager = transform.Find ("MultiplierManager");
		multiplierList.Clear ();

		foreach (RectTransform child in mManager) {
			if (child.tag == "Multiplier") {
				float value;
				float.TryParse (child.GetComponent<InputField> ().text, out value);
				multiplierList.Add (value);
			}
		}

		foreach (GameObject player in playerList) {
			player.GetComponent<Player> ().setMutiplierList (multiplierList);
		}
	}

	public void removePointsField(int index)
	{
		
	}
}
