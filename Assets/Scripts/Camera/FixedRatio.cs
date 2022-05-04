using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRatio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float targetAspect = 1024.0f/900.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;

        float scaleHeight = windowAspect / targetAspect;


        Camera camera = GetComponent<Camera>();
        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;

        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
        // Screen.SetResolution(1920, 1080, false);
    }
}
