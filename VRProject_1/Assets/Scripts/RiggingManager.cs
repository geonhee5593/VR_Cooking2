using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiggingManager : MonoBehaviour
{
    public Transform leftHandIK; //왼손IK
    public Transform rightHandIK; //오른손 IK
    public Transform headIK; //머리 IK

    public Transform leftHandController; //왼손 콘트롤러
    public Transform rightHandContoller; //오른손 콘트롤러
    public Transform hmd; //헤드셋

    public Vector3[] leftOffset; //왼손 오프셋
    public Vector3[] rightOffset; //오른손 오프셋
    public Vector3[] headOffset; //머리 오프셋

    public float smootValue = 0.1f; //스무스 값
    public float modelHeight = 1.6f; //모델 키


    private void LateUpdate()
    {
        MappingHandTransform(leftHandIK, leftHandController, true);
        MappingHandTransform(rightHandIK, rightHandContoller, false);
        MappingBodyTransform(headIK, hmd);
        MappingHeadTransform(headIK, hmd);
    }

    //위치를 맵핑하는 함수(IK정보, 콘트롤러, 손의 방향)
    void MappingHandTransform(Transform ik, Transform controller, bool isLeft)
    {
        //offset이 왼손인지? 참->leftOffset, 거짓->rightOffset
        var offset = isLeft ? leftOffset : rightOffset;

        ik.position = controller.TransformPoint(offset[0]);
        ik.rotation = controller.rotation * Quaternion.Euler(offset[1]);
    }

    //몸통 맵핑 함수(IK정보, 헤드셋)
    void MappingBodyTransform(Transform ik, Transform hmd)
    {
        transform.position = new Vector3(hmd.position.x, hmd.position.y - modelHeight, hmd.position.z);
        float yaw = hmd.eulerAngles.y;
        var targetRotation = new Vector3(transform.eulerAngles.x, yaw, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), smootValue);
    }

    //머리 맵핑 함수(IK정보, 헤드셋)
    void MappingHeadTransform(Transform ik, Transform hmd)
    {
        ik.position = hmd.TransformPoint(headOffset[0]);
        ik.rotation = hmd.rotation * Quaternion.Euler(headOffset[1]);
    }
}
