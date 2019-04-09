using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    VerticalLayoutGroup vertLayout;
    [SerializeField]
    int leftPadding_iphoneX;
    [SerializeField]
    int leftPadding_16_9;
    [SerializeField]
    int screenHeight;
    [SerializeField]
    int currentLeftPadding;
    /*[SerializeField]
    Vector2 refRes_iPhoneX = new Vector2(360.0f, 780.0f);
    [SerializeField]
    Vector2 Aspect_16_9_iPhone = new Vector2(360.0f, 640.0f);*/

    //iPhoneX height variants
    private int iPhoneX_Height = 2436;
    private int iPhoneXR_Height = 1792;
    private int iPhoneXSMAX_Height = 2688;

    //iPhone 5 & 6 height variants
    private int iPhone5_Height = 1136;
    private int iPhone6_Height = 1334;

    private void Start()
    {
        currentLeftPadding = vertLayout.padding.left;
        screenHeight = Screen.height;

        if (screenHeight == iPhoneX_Height || screenHeight == iPhoneXR_Height || screenHeight == iPhoneXSMAX_Height)
        {
            vertLayout.padding.left = 20;
        }
        else
        {
            vertLayout.padding.left = 20;
            vertLayout.padding.right = 5;
        }
        Canvas.ForceUpdateCanvases();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
