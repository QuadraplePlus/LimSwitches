using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleOutputText : MonoBehaviour
{
    [SerializeField] Text myText;

    void Start()
    {
        TextOutputSwitchOn();

        LIMSwitch.onSwitchAction = (isOn) => 
        {
            if (isOn == false)
            {
                TextOutputSwitchOn();               // Function to be executed each time a button is clicked
            }                                       // Invoke corresponding function by action function
            else
            {
                TextOutputSwitchOff();
            }
        };
    }
    public void TextOutputSwitchOn()
    {
        myText.text = "On";
        Debug.Log("온 출력");
    }
    public void TextOutputSwitchOff()
    {
        myText.text = "Off";
        Debug.Log("오프 출력");
    }
}