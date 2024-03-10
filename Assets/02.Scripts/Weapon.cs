using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {      
        // ���� �������� �ƴ϶�� ���� ����.
        if (!GameManager.instance.isLive)
            return;

        
        switch (id)
        {
            // ������ id �� 0�϶� (��������)
            case 0:
                // �������� ȸ��.
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
                // �� ���� �� ( �� )
            default:
                // �߻� �ֱ� �ð�
                timer += Time.deltaTime;

                // �߻� �ֱⰡ speed���� ���� Ŀ���� ��
                if (timer > speed)
                {
                    // �ֱ� �ð� �ʱ�ȭ
                    timer = 0f;
                    // �߻� �Լ�
                    Fire();
                }
                break;
        }

        // �׽�Ʈ
#if UNITY_EDITOR
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
#endif
    }
    
    // ������ �ڵ�
    public void LevelUp(float damage, int count)
    {
        // ������, ���� �ʱ�ȭ
        this.damage = damage * Character.Damage;
        this.count += count;

        // id 0�� �� ( �������� )
        if (id == 0)
            Batch();

        // player ��ũ��Ʈ ������ �ִ� ������Ʈ�� "ApplyGear"�Լ��� ����Ǵ� ������Ʈ���� �Լ� ����.
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // ���� ����
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // ���� ����
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        // pool prefabs ���̸�ŭ ��ȸ
        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            // �������� Projectile�� ���� ������ �ε����� ���� ��
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                // �� �������� id�� ���� �ε�����
                prefabId = index;
                break;
            }
        }

        // id�� ���� ���� ó��
        switch (id)
        {
            // id�� 0�� �� (���������� ��)
            case 0:
                // ���� ���� ���ǵ� * 150 ( ȸ���ӵ� )
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
                // �� ���� �� (���� ��)
            default:
                // ���� ���� �߻� �ֱ� * 0.5 ( �� ������ �߻�)
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        // ���� ���� ǥ���� ���� ����
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);
        // player ��ũ��Ʈ ������ �ִ� ������Ʈ�� "ApplyGear"�Լ��� ����Ǵ� ������Ʈ���� �Լ� ����.
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // �������� ����
    void Batch()
    {
        // ���� �������� ������ŭ ��ȸ
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            // �����ڽ�(�������� ����)�� �������� index�� ���� ��
            if (index < transform.childCount)
            {
                // bullet�� ���� index�� Ʈ�������� �ӽ�����
                bullet = transform.GetChild(index);
            }
            // ���� �� ( ���� count�� ���� �������Ⱑ ����.)
            else
            {
                // �������� ���� ����.
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                // ���������� �θ������Ʈ �ʱ�ȭ
                bullet.parent = transform;
            }
            // ���� ���� ��ġ �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // ���� ���⸦ 360���� ������ ���� ���� ������ ���缭 ������ �������� ��ġ
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); 
        }
    }

    // �߻�
    void Fire()
    {
        // ���� ��ó�� ���� ���ٸ� �������� ����.
        if (!player.scanner.nearestTarget)
            return;

        // ��ó�� �ִ� ���� ��ġ�� targetPos�� ����.
        Vector3 targetPos = player.scanner.nearestTarget.position;
        // ������ �Ÿ� ���
        Vector3 dir = targetPos - transform.position;
        // ������ �Ÿ����� ����ȭ
        dir = dir.normalized;

        // �Ѿ� ���� ���� (Ǯ�� ����)
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        // �߻�Ǵ� �Ѿ��� ���� ��ġ�� ���� �÷��̾� ��ġ�� �ʱ�ȭ
        bullet.position = transform.position;
        // �߻�Ǵ� �Ѿ��� ȸ�� �ʱ�ȭ
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        // ���� �Ѿ��� �������� ����, Ÿ�ٰ��� �Ÿ����� �ʱ�ȭ
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        // ȿ����
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
