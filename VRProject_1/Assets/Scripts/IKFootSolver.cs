using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default; //터레인 레이어 마스크
    [SerializeField] Transform body = default; //몸통
    [SerializeField] IKFootSolver otherFoot = default; //반대 발
    [SerializeField] float speed = 1; //이동 속도
    [SerializeField] float stepDistance = 4; //스텝 간격
    [SerializeField] float stepLength = 4; //스텝 거리
    [SerializeField] float stepHeight = 1; //스텝 높이
    [SerializeField] Vector3 footOffset = default; //발 오프셋
    [SerializeField] Vector3 footRotOffset; //발 회전 오프셋
    float footSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp;

    private void Start()
    {
        footSpacing = transform.localPosition.x;
        currentPosition = newPosition = oldPosition = body.forward * stepLength;
        currentNormal = newNormal = oldNormal = transform.up;
    }

    // Update is called once per frame

    void Update()
    {
        transform.position = currentPosition + footOffset;
        var newRot = footRotOffset + (body.forward * 90.0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRot), 0.1f);

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = info.point + (body.forward * stepLength * direction) + footOffset;
                newNormal = info.normal;
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
    }



    public bool IsMoving()
    {
        return lerp < 1;
    }



}
