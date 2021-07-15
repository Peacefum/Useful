using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour , IDragHandler , IPointerUpHandler, IPointerDownHandler
{
    private Image bgImg;
    private Image joystickimg;
    private Vector3 inputVector;

    // Start is called before the first frame update
    void Start()
    {
        bgImg = GetComponent<Image>();
        joystickimg = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
      //  Debug.Log("드래그중;");
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform,eventData.position,eventData.pressEventCamera,out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, pos.y * 2 - 1, 0);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            joystickimg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3), inputVector.y * (bgImg.rectTransform.sizeDelta.y / 3));
        }


        if (FindObjectOfType<Test_UI>())
            FindObjectOfType<Test_UI>().CurrentGPS() ;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector3.zero;
        joystickimg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float GetHorizontalValue()
    {
        return inputVector.x;
    }
    public float GetVerticalValue()
    {
        return inputVector.y;
    }

}
