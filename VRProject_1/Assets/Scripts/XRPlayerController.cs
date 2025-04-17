using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRPlayerController : MonoBehaviour
{
    public enum XRHandState
    {
        LeftHand, RightHand
    }
    public XRHandState xrHandState = XRHandState.LeftHand; //손의 방향

    public XRController controller; //콘트롤러
    public XRBinding[] bindings;

    public GameObject hitObject; //충돌 대상

    private bool isTrigger; //트리거 동작 신호
    private bool isGrip; //그립 동작 신호
    private bool isXButton; //X 버튼 동작 신호
    private bool isYButton; //Y 버튼 동작 신호
    private bool isAButton; //A 버튼 동작 신호
    private bool isBButton; //B 버튼 동작 신호

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       foreach (var binding in bindings)
        {
            binding.Update(controller.inputDevice);
        }
    }

    //XR콘트롤러 입력 함수 (Device-based)
    void XRControllerInput()
    {
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool trigger))
        {
            //트리거를 눌렀을 경우
            if (trigger)
            {
                //트리거 신호가 true일 경우
                if (isTrigger)
                {
                    Debug.Log("트리거 버튼 누르기");
                    isTrigger = false;
                }
            }
            //트리거를 떼었을 경우
            else
            {
                //트리거 신호가 true일 경우
                if (!isTrigger)
                {
                    Debug.Log("트리거 버튼 떼기");
                    isTrigger = true;
                }
            }
        }
    }
}

public enum XRButton
{
    Trigger, Grip, Primary, PrimaryTouch, Secondary,
    SecondaryTouch, Primary2DAxisClick, Primary2DAxisTouch
}
public enum PressType
{
    Begin, End, Continuous
}

[Serializable]
public class XRBinding
{
    private XRButton button;
    private PressType pressType;
    private UnityEvent onActive;

    bool isPressed;
    bool wasPressed;

    public void Update(InputDevice device)
    {
        device.TryGetFeatureValue(XRStatics.GetFeature(button), out isPressed);
        bool active = false;

        switch(pressType)
        {
            case PressType.Continuous: active = isPressed; break;
            case PressType.Begin: active = isPressed && !wasPressed; break;
            case PressType.End: active = !isPressed && !wasPressed; break;
        }

        if (active) onActive.Invoke();
        wasPressed = isPressed;
    }
}

public static class XRStatics
{
    public static InputFeatureUsage<bool> GetFeature(XRButton button)
    {
        switch (button)
        {
            case XRButton.Trigger: return CommonUsages.triggerButton;
            case XRButton.Grip: return CommonUsages.gripButton;
            case XRButton.Primary: return CommonUsages.primaryButton;
            case XRButton.PrimaryTouch: return CommonUsages.primaryTouch;
            case XRButton.Secondary: return CommonUsages.secondaryButton;
            case XRButton.SecondaryTouch: return CommonUsages.secondaryTouch;
            case XRButton.Primary2DAxisClick: return CommonUsages.primary2DAxisClick;
            case XRButton.Primary2DAxisTouch: return CommonUsages.primary2DAxisTouch;
            default:Debug.LogError("button" + button + "not found");
                return CommonUsages.triggerButton;
        }
    }
}
