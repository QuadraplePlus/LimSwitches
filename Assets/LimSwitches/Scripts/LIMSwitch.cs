using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LIMSwitch : MonoBehaviour
{
    // Scroll generation with 0 to 3 speed adjustment, travel time of switch (maximum 3)
    //Optimal value: 0.1 ~ 0.7
    [Range(0,3)]
    public float moveDuration = 3f;
    //컬러 기본값
    public Color handleColor;             //Switch handle color
    public Color onBackgroundColor;       //Color value when 'On'
    public Color offBackgroundColor;       //Color value when 'Off'

    private float totalHandleMoveLength;    //Maximum length x - axis of the background.
    private float halfMoveLength;           //The distance the handle will move
    private float handleRadius;             //Radius of handle

    private float backGrundRadius;          //Radius of the background
    private float radiusValue;              //The difference between the arc of the steering wheel and the arc of the background.
    const String handleName = "Handle";

    Image handleImage;                      // Handle Image
    Image backgroundImage;                  // Background image
    RectTransform handleRectTransform;      //RectTransform of Switch Handle

    //코루틴 함수
    Coroutine moveHandleCoroutine;               // Handle moving corutine
    Coroutine backgroundColorChangeCoroutine;    // Background color change corutine
    public static Action<bool> onSwitchAction;   // Signal to be passed according to switch on/off
    bool isOn;                                   // To check the operation of the switch
    void Start()    
    {
        //Calling up components of field declared variables  
        GameObject handleObject = transform.Find(handleName).gameObject;
        handleRectTransform = handleObject.GetComponent<RectTransform>();
        handleRadius = handleObject.GetComponent<RectTransform>().sizeDelta.x;
        totalHandleMoveLength = GetComponent<RectTransform>().sizeDelta.x;
        backGrundRadius = GetComponent<RectTransform>().sizeDelta.y;

        radiusValue = (backGrundRadius - handleRadius);
        halfMoveLength = ((totalHandleMoveLength - handleRadius) - radiusValue) / 2;

        handleImage = handleObject.GetComponent<Image>();
        handleImage.color = handleColor;

        backgroundImage = GetComponent<Image>();
        backgroundImage.color = onBackgroundColor;

        if (isOn)
        {
            handleRectTransform.anchoredPosition = new Vector2(-halfMoveLength ,0);
        }
        else
        {
            handleRectTransform.anchoredPosition = new Vector2(halfMoveLength, 0);
        }
    }
    public void OnClickSwitch()
    {
        //Toggle every click , The two perform different actions.
        isOn = !isOn;

        Vector2 fromPos = handleRectTransform.anchoredPosition;
        Vector2 toPos = (isOn) ? new Vector2(-halfMoveLength, 0) : new Vector2(halfMoveLength, 0);
        Vector2 dis = toPos - fromPos;

        float ratio = Mathf.Abs(dis.x) / totalHandleMoveLength;
        float duration = moveDuration * ratio;

        //handlemoveCoroutine
        if (moveHandleCoroutine != null)
        {
            StopCoroutine(moveHandleCoroutine);
            moveHandleCoroutine = null;
        }
        moveHandleCoroutine = 
            StartCoroutine(MoveHandle(fromPos,toPos,duration));

        //backgruondChangeColorCouroutine
        Color fromColor = backgroundImage.color;
        Color toColor = (isOn) ? offBackgroundColor : onBackgroundColor;
        if (backgroundColorChangeCoroutine != null)
        {
            StopCoroutine(backgroundColorChangeCoroutine);
            backgroundColorChangeCoroutine = null;
        }
        backgroundColorChangeCoroutine = 
            StartCoroutine(ChangeBackgroundColor(fromColor,toColor,duration));

        if (isOn)                   //Important!!!
        {                           //Execute an action function where you can invoke a function from another script.
            onSwitchAction(isOn);   //'if' the switch is 'on' and 'else' switch is 'off'.
        }                           //Check 'ExampleOutputText.cs'
        else
        {
            onSwitchAction(isOn);
        }
    }
    /// <summary>
    /// Function for moving the handle
    /// </summary>
    /// <param name="fromPos">Starting position of handle</param>
    /// <param name="toPos">The destination of the handle</param>
    /// <param name="duartion">Time for the handle to move</param>
    /// <returns>none</returns>
    IEnumerator MoveHandle(Vector2 fromPos, Vector2 toPos, float duartion)
    {
        float currTime = 0f;
        while (currTime < duartion)
        {
            float t = currTime / duartion;
            float delayT = t / 3;
            Vector2 newPos = Vector2.Lerp(fromPos, toPos, t + delayT);
            handleRectTransform.anchoredPosition = newPos;

            currTime += Time.deltaTime;
            yield return null;
        }
    }
    /// <summary>
    /// Function to change switch background color
    /// </summary>
    /// <param name="fromColor">First Start Color</param>
    /// <param name="toColor">Color to change</param>
    /// <param name="duration">Changing time</param>
    /// <returns>none</returns>
    IEnumerator ChangeBackgroundColor(Color fromColor, Color toColor, float duration)
    {      
        float currTime = 0f;
        while (currTime < duration)
        {    
            float t = currTime / duration;
            float delayT = t / 3;
            Color newColor = Color.Lerp(fromColor, toColor, t + delayT);
            backgroundImage.color = newColor;

            currTime += Time.deltaTime;
            yield return null;
        }
    }
}