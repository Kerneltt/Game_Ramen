using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMyDice : MonoBehaviour
{
    [SerializeField]
    DiceManager2 diceManager;
    [SerializeField]
    Material diceMat;
    [SerializeField]
    Color currentColor;


    // Start is called before the first frame update
    void Start()
    {
        currentColor = diceManager.diceColor;
        diceMat.color = currentColor;
    }

    // Update is called once per frame
    void Update()
    {
        currentColor = diceManager.diceColor;
        ColorMe();
    }

    public void ColorMe()
    {
        diceMat.color = currentColor;
    }
}
