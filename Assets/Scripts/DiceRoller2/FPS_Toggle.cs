using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_Toggle : DiceManager2
{
    [SerializeField]
    GameObject fps_Text;
    [SerializeField]
    GameObject fps_Counter;
    [SerializeField]
    public bool fps_active;

    // Start is called before the first frame update
    void Start()
    {
        fps_active = false;
        fps_Text.SetActive(false);
        fps_Counter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FPS_Toggler()
    {
        CancelTap();
        if (!fps_active)
        {
            fps_Text.SetActive(false);
            fps_Counter.SetActive(false);
            fps_active = true;
        }
        else if (fps_active)
        {
            fps_Text.SetActive(true);
            fps_Counter.SetActive(true);
            fps_active = false;
        }
    }
}
