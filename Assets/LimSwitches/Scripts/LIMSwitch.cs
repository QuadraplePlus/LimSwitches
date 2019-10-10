using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LIMSwitch : MonoBehaviour
{
    //스위치 온 오프 여부 확인용
    public bool isOn;
    // 0 ~ 3 속도 조절을 할 수 있는 스크롤 생성, 스위치의 이동시간 (최대 3)
    [Range(0,3)]
    public float moveDuration = 3f;
    //컬러 기본값
    public Color handleColor = Color.white;
    public Color onSwitchColor = Color.green;
    public Color offSwitchColor = Color.gray; 

    const float totalHandleMoveLength = 72;    //좌 우 최대 이동 할 수 있는 거리
    const float halfMoveLength = totalHandleMoveLength / 2;  //최대 이동거리의 절반 ?

    Image handleImage;                  // 핸들 이미지
    Image backgroundImage;              // 배경 이미지
    RectTransform handleRectTransform;  //스위치 핸들의 RectTransform

    //코루틴 함수
    Coroutine moveHandleCoroutine;      // 핸들 이동 코루틴
    Coroutine backgroundColorChangeCoroutine; //배경색 변경 코루틴
    
    // Start is called before the first frame update
    void Start()
    {
        // Handle 오브젝트 초기화 
        GameObject handleObfect = transform.Find("Handle").gameObject;
        handleRectTransform = handleObfect.GetComponent<RectTransform>();
        //핸들 이미지
        handleImage = GetComponent<Image>();
        handleImage.color = handleColor;

        //배경 이미지
        backgroundImage = GetComponent<Image>();
        backgroundImage.color = offSwitchColor;

        if (isOn)
        {
            handleRectTransform.anchoredPosition = new Vector2(halfMoveLength ,0);
        }
        else
        {
            handleRectTransform.anchoredPosition = new Vector2(-halfMoveLength, 0);
        }
    }

    public void OnClickSwitch()
    {
        //서로 반대의 동작을 하도록 만듦
        isOn = !isOn;

        Vector2 fromPos = handleRectTransform.anchoredPosition;
        Vector2 toPos = (isOn) ? new Vector2(halfMoveLength, 0) : new Vector2(-halfMoveLength, 0);
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
        Color toColor = (isOn) ? onSwitchColor : offSwitchColor;
        if (backgroundColorChangeCoroutine != null)
        {
            StopCoroutine(backgroundColorChangeCoroutine);
            backgroundColorChangeCoroutine = null;
        }
        backgroundColorChangeCoroutine = 
            StartCoroutine(changeBackgroundColor(fromColor,toColor,duration));
    }
    /// <summary>
    /// 핸들을 이동하기 위한 함수 
    /// </summary>
    /// <param name="fromPos">핸들의 시작위치</param>
    /// <param name="toPos">핸들의 목적위치</param>
    /// <param name="duartion">핸들이 이동할 시간</param>
    /// <returns>없음</returns>
    IEnumerator MoveHandle(Vector2 fromPos, Vector2 toPos, float duartion)
    {
        float currTime = 0f;
        while (currTime < duartion)
        { 
            float t = currTime / duartion;
            Vector2 newPos = Vector2.Lerp(fromPos, toPos, t);
            handleRectTransform.anchoredPosition = newPos;

            currTime += Time.deltaTime;
            yield return null;
        }
    }
    /// <summary>
    /// 스위치 배경색 변경 하는 코루틴 함수
    /// </summary>
    /// <param name="fromColor">처음 시작 색상</param>
    /// <param name="toColor">변경될 색상</param>
    /// <param name="duration">변경되는 시간</param>
    /// <returns>없음</returns>
    IEnumerator changeBackgroundColor(Color fromColor, Color toColor, float duration)
    {
        float currTime = 0f;
        while (currTime < duration)
        {
            float t = currTime / duration;
            Color newColor = Color.Lerp(fromColor, toColor, t);
            backgroundImage.color = newColor;

            currTime += Time.deltaTime;
            yield return null;
        }
    }
}
