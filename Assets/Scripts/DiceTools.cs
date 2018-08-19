using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTools : MonoBehaviour {

	public void Remove()
    {
        transform.GetComponentInChildren<Dice>().RemoveDice();
    }

    public void LockDice()
    {
        transform.GetComponentInChildren<Dice>().LockDice();
    }

    public void UnlockDice()
    {
        transform.GetComponentInChildren<Dice>().UnlockDice();
    }
}
