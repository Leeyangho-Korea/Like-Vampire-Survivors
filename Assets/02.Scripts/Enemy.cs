using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터 스크립트
/// 타겟을 찾아 이동.
/// 체력 관리 스크립트
/// </summary>

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    Animator anim;
    bool isLive;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!isLive)
            return;

        // 타겟의 방향
        Vector2 dir = target.position - rigid.position;

        Vector2 nextVec = dir.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }

    // 활성화 됐을 때
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        // Bullet의 데미지만큼 체력 깎임.
        health -= collision.GetComponent<Bullet>().damage;

        if (health >0)
        {
            // 살이있으며 데미지 입은 것에 대한 처리
        }
        else
        {
            // 죽음
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false); // 죽으면 비활성화 _ 활성화될때 모든 값 초기화 됨.
    }
}
