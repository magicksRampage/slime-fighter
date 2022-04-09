using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIStaminaBar : MonoBehaviour
{

    public int maxValue;

    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetValue(int value)
    {
        text.text = value + "/" + maxValue;
    }
}
