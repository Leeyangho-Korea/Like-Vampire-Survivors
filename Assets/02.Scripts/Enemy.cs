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
    Collider2D coll;
    Animator anim;
    bool isLive;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    WaitForFixedUpdate wait;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
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


        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriteRenderer.sortingOrder = 2;
        anim.SetBool("Dead", false);

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
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        // Bullet의 데미지만큼 체력 깎임.
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());
        if (health >0)
        {
            // 살이있으며 데미지 입은 것에 대한 처리
            if (health > 0)
            {
                anim.SetTrigger("Hit");
            }
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriteRenderer.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }
    
    //몬스터가 뒤로 밀리는 처리.
    IEnumerator KnockBack()
    {
        yield return wait; // 물리프레임 1 프레임 쉬기.
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);


    }

    void Dead()
    {
        gameObject.SetActive(false); // 죽으면 비활성화 _ 활성화될때 모든 값 초기화 됨.
    }
}
