using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayHandlerScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        GameObject.Find("InstructionsSteer").GetComponent<UnityEngine.UI.Text>().text = "Touch and drag";
        GameObject.Find("InstructionsFire").GetComponent<UnityEngine.UI.Text>().text = "Touch and hold";
        GameObject.Find("InstructionsDeployBomb").GetComponent<UnityEngine.UI.Text>().text = "Double tap";
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
