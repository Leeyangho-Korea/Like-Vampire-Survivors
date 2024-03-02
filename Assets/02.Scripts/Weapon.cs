using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기의 개수, 속도, 데미지 등을 관리
/// </summary>


public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage; // 무기 데미지
    public int count; // 무기 개수
    public float speed; // 무기 속도

    float timer;
    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed  *  Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        // ..TestCode..
        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Set();
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150; 
                Set();
                    break;
            default:
                speed = 0.3f; // 값이 적을수록 빠른 주기로 발사
                break;
        }
    }

    void Set()
    {
        for(int i = 0; i < count; i ++)
        {
           Transform bullet;

            if(i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }


            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count ;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector2.zero); // -1 은 적을 관통
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
