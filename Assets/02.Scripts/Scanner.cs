using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  �ֺ��� ���� Ž��, �����ϴ� ��ũ��Ʈ
/// </summary>


public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate()
    {
        // ���� ����ĳ��Ʈ.
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        // ���� ����ĳ��Ʈ�� �ɸ� ������Ʈ�� �߿��� RaycastHit ��ü�� Ž��.
        foreach (RaycastHit2D target in targets)
        {
            //���� �� ��ġ = �� ��ũ��Ʈ ��ġ.
            Vector3 myPos = transform.position;
            // Ÿ���� ��ġ = ���� �� ��ġ
            Vector3 targetPos = target.transform.position;
            // Ÿ�ٰ��� �Ÿ����
            float curDiff = Vector3.Distance(myPos, targetPos);

            // ���� Ÿ�ٰ��� �Ÿ��� 100���� ���� ���
            if (curDiff < diff)
            {
                // ������ �Ÿ� = Ÿ�ٰ��� �Ÿ��� ����.
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}

