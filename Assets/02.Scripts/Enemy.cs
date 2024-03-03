using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ������ �����ϴ� ��ũ��Ʈ
/// </summary>
public class Enemy : MonoBehaviour
{
    // �ӵ�
    public float speed;
    // ü��
    public float health;
    // �ִ� ü��
    public float maxHealth;

    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    // ����ִ��� ���� �Ǵ�
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
        // ���� ������ �������� �ƴ϶�� Update ���� X (�� �ڵ��� �Ʒ� ����� ���� X)
        if (!GameManager.instance.isLive)
            return;

        // ���� �� ��ũ��Ʈ�� ���� ���Ͱ� ������� �ʰų�(����)
        // ���� Hit ���ϸ��̼��� �������̶�� �Ʒ� �ڵ�� ���� X
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        // ���� �÷��̾�(target)�� ��ġ���� ������ ���� ��ġ�� ���� ���� ���� ����.
        Vector2 dirVec = target.position - rigid.position;
        // ����ȭ�� ���⺤�Ϳ� �ӵ��� �÷��־� ��� Ÿ���� ���� ���ư� �� �ֵ��� �ϴ� ����.
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        // ���������� Ÿ���� ���� ���ư� �� �ֵ��� ������ �ٵ��� MovePosition ����.
        rigid.MovePosition(rigid.position + nextVec);

        // �߷��� ������ ���� �ʵ��� (0,0)����.
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        // ���� ������ �����ٸ� ���� X
        if (!GameManager.instance.isLive)
            return;

        // ���� ���� �� ��ũ��Ʈ�� ������Ʈ�� �׾��ٸ� ���� x
        if (!isLive)
            return;

        // �� ������Ʈ�� ��ġ���� Ÿ���� ��ġ�� �۴ٸ� ( Ÿ���� �� ������Ʈ���� ���ʿ� �����ϸ�)
        // ��������Ʈ�������� flipX = true
        // �ƴ϶�� flipX = false
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // �� ��ũ��Ʈ�� ������Ʈ�� Ȱ��ȭ �� �� ( �ʱ�ȭ )
    void OnEnable()
    {
        // Ÿ�� ���� ( �÷��̾�� )
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        // ���� �� �ִϸ��̼��� Dead ���� ���� false�� �ʱ�ȭ.
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    // ���� �ʱ�ȭ
    public void Init(SpawnData data)
    {
        // ���� ��ũ��Ʈ�� ���Ϳ� �´� �ִϸ�������Ʈ�ѷ� ����
        anim.runtimeAnimatorController = animCon[data.spriteType];
        // ���� ��ũ��Ʈ�� ���Ϳ� �´� �ӵ�, ü�� �ʱ�ȭ.
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ѿ˿� �������� �ƴϰų� �� ������Ʈ�� ĳ���� �׾��� ��
        // �Ʒ� �ڵ� ���� X
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        // ����ְ�, �Ѿ˿� �¾Ҵٸ� ���� �Ѿ��� ��������ŭ ü�� ����
        health -= collision.GetComponent<Bullet>().damage;

        // KnockBack �ڷ�ƾ ����
        StartCoroutine(KnockBack());

        // ���� ü���� ���Ҵٸ�
        if (health > 0)
        {
            // �´� �ִϸ��̼�, ȿ���� ����
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else // ü���� ���� �ʾҴٸ�
        {
            // �׾��� ���� �� ����.
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            // �÷��̾��� Kill �� ����
            GameManager.instance.kill++;
            // �÷��̾��� Exp ����
            GameManager.instance.GetExp();

            // ���� ������ �������̶��
            if (GameManager.instance.isLive)
                // �״� ȿ���� ����.
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // ���� �ϳ��� ���� ������ ������
        // �÷��̾��� ��ġ
        Vector3 playerPos = GameManager.instance.player.transform.position;
        // �÷��̾��� ��ġ���� �ݴ�Ǵ� ���� ����
        Vector3 dirVec = transform.position - playerPos;
        // �÷��̾��� �ݴ� ��ġ�� 3��ŭ�� ���� �Ͻ������� �־� �˹� ����.
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    // �״� �ִϸ��̼ǿ��� ����.
    void Dead()
    {
        gameObject.SetActive(false);
    }
}