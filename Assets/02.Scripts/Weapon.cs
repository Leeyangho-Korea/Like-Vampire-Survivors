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
        // 게임 진행중이 아니라면 실행 안함.
        if (!GameManager.instance.isLive)
            return;

        
        switch (id)
        {
            // 무기의 id 가 0일때 (근접무기)
            case 0:
                // 근접무기 회전.
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
                // 그 외일 때 ( 총 )
            default:
                // 발사 주기 시간
                timer += Time.deltaTime;

                // 발사 주기가 speed보다 값이 커졌을 때
                if (timer > speed)
                {
                    // 주기 시간 초기화
                    timer = 0f;
                    // 발사 함수
                    Fire();
                }
                break;
        }

        // 테스트
#if UNITY_EDITOR
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
#endif
    }
    
    // 레벨업 코드
    public void LevelUp(float damage, int count)
    {
        // 데미지, 개수 초기화
        this.damage = damage * Character.Damage;
        this.count += count;

        // id 0일 때 ( 근접무기 )
        if (id == 0)
            Batch();

        // player 스크립트 가지고 있는 오브젝트에 "ApplyGear"함수가 적용되는 오브젝트에서 함수 실행.
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // 공통 세팅
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // 개별 세팅
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        // pool prefabs 길이만큼 순회
        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            // 아이템의 Projectile이 현재 프리팹 인덱스와 같을 때
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                // 그 아이템의 id를 현재 인덱스로
                prefabId = index;
                break;
            }
        }

        // id의 값에 따라 처리
        switch (id)
        {
            // id가 0일 때 (근접무기일 때)
            case 0:
                // 현재 무기 스피드 * 150 ( 회전속도 )
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
                // 그 외일 때 (총일 때)
            default:
                // 현재 무기 발사 주기 * 0.5 ( 더 빠르게 발사)
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        // 손의 무기 표현을 위한 변수
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);
        // player 스크립트 가지고 있는 오브젝트에 "ApplyGear"함수가 적용되는 오브젝트에서 함수 실행.
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // 근접무기 세팅
    void Batch()
    {
        // 현재 근접무기 개수만큼 순회
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            // 현재자식(근접무기 개별)의 개수보다 index가 적을 때
            if (index < transform.childCount)
            {
                // bullet에 현재 index의 트랜스폼을 임시저장
                bullet = transform.GetChild(index);
            }
            // 많을 때 ( 현재 count에 비해 근접무기가 적음.)
            else
            {
                // 근접무기 새로 생성.
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                // 근접무기의 부모오브젝트 초기화
                bullet.parent = transform;
            }
            // 무기 현재 위치 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 현재 무기를 360도에 나눠서 현재 무기 개수에 맞춰서 일정한 간격으로 배치
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); 
        }
    }

    // 발사
    void Fire()
    {
        // 현재 근처에 적이 없다면 실행하지 않음.
        if (!player.scanner.nearestTarget)
            return;

        // 근처에 있는 적의 위치를 targetPos로 저장.
        Vector3 targetPos = player.scanner.nearestTarget.position;
        // 적과의 거리 계산
        Vector3 dir = targetPos - transform.position;
        // 적과의 거리벡터 정규화
        dir = dir.normalized;

        // 총알 새로 생성 (풀링 관리)
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        // 발사되는 총알의 시작 위치를 현재 플레이어 위치로 초기화
        bullet.position = transform.position;
        // 발사되는 총알의 회전 초기화
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        // 현재 총알의 데미지와 개수, 타겟과의 거리벡터 초기화
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        // 효과음
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
