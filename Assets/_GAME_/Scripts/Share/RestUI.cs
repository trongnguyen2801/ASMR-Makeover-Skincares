using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestUI : MonoBehaviour
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        ResUi();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResUi()
    {
        float scrWidth = Screen.width;
        float scrHeight = Screen.height;
        float tile = scrHeight / scrWidth;
        Debug.Log("width=" + scrWidth + " Height= " + scrHeight);
        if (tile >= 1920f / 1080f)
        {
            canvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 0;

        }
        else
        {
            canvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
        }
    }
}
