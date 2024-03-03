using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터의 로직을 관리하는 스크립트
/// </summary>
public class Enemy : MonoBehaviour
{
    // 속도
    public float speed;
    // 체력
    public float health;
    // 최대 체력
    public float maxHealth;

    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    // 살아있는지 여부 판단
    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        // 현재 게임이 실행중이 아니라면 Update 실행 X (이 코드의 아래 내용들 실행 X)
        if (!GameManager.instance.isLive)
            return;

        // 만약 이 스크립트를 가진 몬스터가 살아있지 않거나(죽음)
        // 현재 Hit 에니메이션이 실행중이라면 아래 코드들 실행 X
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        // 현재 플레이어(target)의 위치에서 몬스터의 현재 위치를 빼서 방향 벡터 설정.
        Vector2 dirVec = target.position - rigid.position;
        // 정규화된 방향벡터에 속도를 늘려주어 계속 타겟을 향해 나아갈 수 있도록 하는 벡터.
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        // 지속적으로 타겟을 향해 나아갈 수 있도록 리지드 바디의 MovePosition 실행.
        rigid.MovePosition(rigid.position + nextVec);

        // 중력의 영향을 받지 않도록 (0,0)고정.
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        // 만약 게임이 끝났다면 실행 X
        if (!GameManager.instance.isLive)
            return;

        // 만약 현재 이 스크립트의 오브젝트가 죽었다면 실행 x
        if (!isLive)
            return;

        // 이 오브젝트의 위치보다 타겟의 위치가 작다면 ( 타겟이 이 오브젝트보다 왼쪽에 존재하면)
        // 스프라이트렌더러의 flipX = true
        // 아니라면 flipX = false
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // 이 스크립트의 오브젝트가 활성화 될 때 ( 초기화 )
    void OnEnable()
    {
        // 타겟 설정 ( 플레이어로 )
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        // 죽을 때 애니메이션이 Dead 였던 것을 false로 초기화.
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    // 몬스터 초기화
    public void Init(SpawnData data)
    {
        // 현재 스크립트의 몬스터에 맞는 애니메이터컨트롤러 설정
        anim.runtimeAnimatorController = animCon[data.spriteType];
        // 현재 스크립트의 몬스터에 맞는 속도, 체력 초기화.
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알에 맞은것이 아니거나 이 오브젝트의 캐릭이 죽었을 때
        // 아래 코드 실행 X
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        // 살아있고, 총알에 맞았다면 맞은 총알의 데미지만큼 체력 감소
        health -= collision.GetComponent<Bullet>().damage;

        // KnockBack 코루틴 실행
        StartCoroutine(KnockBack());

        // 만약 체력이 남았다면
        if (health > 0)
        {
            // 맞는 애니메이션, 효과음 실행
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else // 체력이 남지 않았다면
        {
            // 죽었을 때의 값 설정.
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            // 플레이어의 Kill 수 증가
            GameManager.instance.kill++;
            // 플레이어의 Exp 증가
            GameManager.instance.GetExp();

            // 아직 게임이 실행중이라면
            if (GameManager.instance.isLive)
                // 죽는 효과음 실행.
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        // 플레이어의 위치
        Vector3 playerPos = GameManager.instance.player.transform.position;
        // 플레이어의 위치에서 반대되는 방향 벡터
        Vector3 dirVec = transform.position - playerPos;
        // 플레이어의 반대 위치로 3만큼의 힘을 일시적으로 주어 넉백 연출.
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    // 죽는 애니메이션에서 실행.
    void Dead()
    {
        gameObject.SetActive(false);
    }
}