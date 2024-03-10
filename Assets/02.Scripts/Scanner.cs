using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  주변의 적을 탐색, 추적하는 스크립트
/// </summary>


public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate()
    {
        // 원형 레이캐스트.
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        // 원형 레이캐스트에 걸린 오브젝트들 중에서 RaycastHit 객체를 탐색.
        foreach (RaycastHit2D target in targets)
        {
            //현재 내 위치 = 이 스크립트 위치.
            Vector3 myPos = transform.position;
            // 타겟의 위치 = 현재 내 위치
            Vector3 targetPos = target.transform.position;
            // 타겟과의 거리계산
            float curDiff = Vector3.Distance(myPos, targetPos);

            // 만약 타겟과의 거리가 100보다 작을 경우
            if (curDiff < diff)
            {
                // 적과의 거리 = 타겟과의 거리로 설정.
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}

