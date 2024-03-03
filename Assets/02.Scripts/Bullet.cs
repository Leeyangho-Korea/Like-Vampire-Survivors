using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기(총알) 로직 스크립트
/// </summary>

public class Bullet : MonoBehaviour
{
    // 무기의 데미지
    public float damage;
    // 무기의 관통 가능 횟수
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // 무기 초기화
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // 근접무기가 아니라면
        if (per >= 0)
        {
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 콜라이더 대상이 적이 아니거나 근접무기(per == -100)라면 해당 함수를 실행하지 않는다.
        if (!collision.CompareTag("Enemy") || per == -100) 
            return;

        per--; // 적에게 닿을 때마다 관통 가능 횟수 차감.

        // 관통 가능 횟수가 0보다 적다면 (가능한 관통 횟수만큼 몬스터 관통했다면)
        if (per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    // 만약 모두 적을 관통하고도 관통 횟수가 남아서 계속 날라간다면
    // 현재 있는 맵의 벗어났을 때 오브젝트 비활성화해서 관리.
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}
