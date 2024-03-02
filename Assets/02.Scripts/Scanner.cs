using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  주변의 적을 스캔하고 처리하는 스크립트
/// </summary>
public class Scanner : MonoBehaviour
{
    public float scanRange; // 스캔할 반경 ( 원의 반지름 )
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float distance = 100;
        
        foreach (RaycastHit2D target in targets){
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDis = Vector3.Distance(myPos, targetPos);

            // 내가 설정한 거리보다 적과의 거리가 가까우면
            if(curDis < distance)
            {
                distance = curDis;
                result = target.transform; // 가까운 적을 타겟으로 리턴.
            }
        }

        return result;
    }

}
