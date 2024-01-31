using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainbowTextColor : MonoBehaviour
{
    public Text text;
    public float R;
    public float G;
    public float B;

    private void Update()
    {
        if (R <= 255 && G == 255 && B == 0) R--;
        if (R == 0 && G == 255 && B >= 0) B++;
        if (R == 0 && G <= 255 && B == 255)  G--;
        if (R >= 0 && G == 0 && B == 255) R++;
        if (R == 255 && G == 0 && B <= 255)B--;
        if (R == 255 && G >= 0 && B == 0)G++;

        text.color = new Color(R/255f,G/255f,B/255f);
    }

}


