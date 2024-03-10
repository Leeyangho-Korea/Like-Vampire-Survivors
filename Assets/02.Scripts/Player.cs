using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ĳ������ �̵�, ������ �Ÿ� ���
/// </summary>

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }
    
    // ĳ���� Ȱ��ȭ ���� �� ( ó�� ������ �� )
    void OnEnable()
    {
        // ���� ĳ������ ���ǵ�, ���� �ʱ�ȭ
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void Update()
    {
        // ���� �������� �ƴ϶�� ���� ����.
        if (!GameManager.instance.isLive)
            return;

        // �¿�, ���� �� �Է¹ޱ�.
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // ���� �������� �ƴ϶�� ���� ����.
        if (!GameManager.instance.isLive)
            return;

        // �Է¹��� ������ �̷���� ���⺤�͸� ����ȭ�� ��, ���ǵ带 �����Ͽ� �̵��� ���� ����.
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        // �ش� �̵����ͷ� ������ٵ� �̿��Ͽ� �̵�.
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        // ���� �������� �ƴ϶�� ���� ����.
        if (!GameManager.instance.isLive)
            return;

        // ������ �� �ִϸ������� "speed" �Ķ������ ���� ����. ( �ִϸ��̼� ���� )
        anim.SetFloat("Speed", inputVec.magnitude);

        // �¿��� ���� 0�� �ƴ� ��
        if (inputVec.x != 0)
        {
            // �������� �� �� ( inputVec.x�� ���� ������ �� ) �̹��� �¿� ����.
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // ���� �������� �ƴ϶�� ���� ����.
        if (!GameManager.instance.isLive)
            return;

        // ���� ü�� =  ���� ü�� - ���� ����� �ð�* 10;
        GameManager.instance.health -= Time.deltaTime * 10;

        // ���� ���� ü���� 0 ���� �۴ٸ�
        if (GameManager.instance.health < 0)
        {
            // ���� ��ũ��Ʈ������Ʈ�� ������ �ִ� ������Ʈ�� �ڽ� ������Ʈ������ŭ ��ȸ
            for (int index = 2; index < transform.childCount; index++)
            {
                // �ڽ� ������Ʈ ��Ȱ��ȭ
                transform.GetChild(index).gameObject.SetActive(false);
            }

            // "Dead"�ִϸ��̼� ���.
            anim.SetTrigger("Dead");
            // GameOver �Լ� ����
            GameManager.instance.GameOver();
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}

