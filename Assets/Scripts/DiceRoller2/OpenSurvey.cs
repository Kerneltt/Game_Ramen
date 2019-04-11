using UnityEngine;
using System.Collections;

public class WebInterface : MonoBehaviour
{
    public void OpenWebsite(string url)
    {
            Application.OpenURL(url);
    }
}