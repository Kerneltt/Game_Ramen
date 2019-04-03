using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    CanvasScaler canvasScaler;
    [SerializeField]
    RectTransform fitRectTransform;
    [SerializeField]
    Vector2 refRes_iPhoneX = new Vector2(360.0f, 780.0f);
    [SerializeField]
    Vector2 Aspect_16_9_iPhone = new Vector2(360.0f, 640.0f);

    //iPhoneX height variants
    private int iPhoneX_Height = 2436;
    private int iPhoneXR_Height = 1792;
    private int iPhoneXSMAX_Height = 2688;

    //iPhone 5 & 6 height variants
    private int iPhone5_Height = 1136;
    private int iPhone6_Height = 1334;

    private void Awake()
    {

    }

    private void Start()
    {
        Canvas.ForceUpdateCanvases();

        if (Screen.height == iPhoneX_Height || Screen.height == iPhoneXR_Height || Screen.height == iPhoneXSMAX_Height)
        {
            //Set canvas scaler reference resolution
            canvasScaler.referenceResolution = refRes_iPhoneX;

            //Set Fit Content Rect Transform Values
            fitRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            fitRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            //fitRectTransform.sizeDelta = refRes_iPhoneX;
            fitRectTransform.rect.Set(0, 0, refRes_iPhoneX.x, refRes_iPhoneX.y);
            Debug.Log("Fit Content RectTransform = " + fitRectTransform.rect);

        }
        else if (Screen.height == iPhone5_Height || Screen.height == iPhone6_Height)
        {
            //Set canvas scaler reference resolution
            canvasScaler.referenceResolution = Aspect_16_9_iPhone;

            //Set Fit Content Rect Transform Values
            fitRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            fitRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            fitRectTransform.sizeDelta = Aspect_16_9_iPhone;
            fitRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            fitRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {

    }
}
