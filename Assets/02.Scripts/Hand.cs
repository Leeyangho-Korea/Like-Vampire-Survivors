using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 가능한 무기에 따라 플레이어 이미지 연출을 관리하는 스크립트
/// </summary>

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        // 캐릭터가 왼쪽 또는 오른쪽을 볼 때 이미지 처리
        // isLeft == true => ( 왼손 - 근접무기 )
        if (isLeft)
        { 
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        // isLef == false => ( 오른손 - 원거리무기 )
        else
        { 
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}

