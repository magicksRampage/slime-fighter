using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeResult : MonoBehaviour
{
    float timeStart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        Text text = GetComponent<Text>();
        text.text = " " + Mathf.FloorToInt(Time.timeSinceLevelLoad / 60) + "m " + Mathf.FloorToInt(Time.timeSinceLevelLoad % 60) + "s";

    }
}
