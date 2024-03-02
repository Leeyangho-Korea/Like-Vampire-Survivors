using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  �ֺ��� ���� ��ĵ�ϰ� ó���ϴ� ��ũ��Ʈ
/// </summary>
public class Scanner : MonoBehaviour
{
    public float scanRange; // ��ĵ�� �ݰ� ( ���� ������ )
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

            // ���� ������ �Ÿ����� ������ �Ÿ��� ������
            if(curDis < distance)
            {
                distance = curDis;
                result = target.transform; // ����� ���� Ÿ������ ����.
            }
        }

        return result;
    }

}
