using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기의 데미지와 관통 여부, 적과의 거리를 초기화하는 스크립트
/// </summary>
public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        
        if(per > -1)
        {
            rigid.velocity = dir * 15; // 속력 곱해주어서 총알 속도 정하기
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1) // per가 -1 인경우 관통 안함.(근접무기)
            return;

        per--;
        if(per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false); // 오브젝트 풀링을 위해 비활성화 처리
        }
    }
}
