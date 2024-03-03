using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����(�Ѿ�) ���� ��ũ��Ʈ
/// </summary>

public class Bullet : MonoBehaviour
{
    // ������ ������
    public float damage;
    // ������ ���� ���� Ƚ��
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // ���� �ʱ�ȭ
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // �������Ⱑ �ƴ϶��
        if (per >= 0)
        {
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // �ε��� �ݶ��̴� ����� ���� �ƴϰų� ��������(per == -100)��� �ش� �Լ��� �������� �ʴ´�.
        if (!collision.CompareTag("Enemy") || per == -100) 
            return;

        per--; // ������ ���� ������ ���� ���� Ƚ�� ����.

        // ���� ���� Ƚ���� 0���� ���ٸ� (������ ���� Ƚ����ŭ ���� �����ߴٸ�)
        if (per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    // ���� ��� ���� �����ϰ� ���� Ƚ���� ���Ƽ� ��� ���󰣴ٸ�
    // ���� �ִ� ���� ����� �� ������Ʈ ��Ȱ��ȭ�ؼ� ����.
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}
