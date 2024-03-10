using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 캐릭터의 이동, 적과의 거리 계산
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
    
    // 캐릭터 활성화 됐을 때 ( 처음 시작할 때 )
    void OnEnable()
    {
        // 현재 캐릭터의 스피드, 외형 초기화
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void Update()
    {
        // 게임 진행중이 아니라면 실행 안함.
        if (!GameManager.instance.isLive)
            return;

        // 좌우, 상하 값 입력받기.
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // 게임 진행중이 아니라면 실행 안함.
        if (!GameManager.instance.isLive)
            return;

        // 입력받은 값으로 이루어진 방향벡터를 정규화한 후, 스피드를 가산하여 이동할 벡터 설정.
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        // 해당 이동벡터로 리지드바디를 이용하여 이동.
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        // 게임 진행중이 아니라면 실행 안함.
        if (!GameManager.instance.isLive)
            return;

        // 움직일 때 애니메이터의 "speed" 파라미터의 값을 세팅. ( 애니메이션 변경 )
        anim.SetFloat("Speed", inputVec.magnitude);

        // 좌우의 값이 0이 아닐 때
        if (inputVec.x != 0)
        {
            // 왼쪽으로 갈 때 ( inputVec.x의 값이 음수일 때 ) 이미지 좌우 반전.
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // 게임 진행중이 아니라면 실행 안함.
        if (!GameManager.instance.isLive)
            return;

        // 현재 체력 =  현재 체력 - 게임 진행된 시간* 10;
        GameManager.instance.health -= Time.deltaTime * 10;

        // 만약 현재 체력이 0 보닥 작다면
        if (GameManager.instance.health < 0)
        {
            // 현재 스크립트컴포넌트를 가지고 있는 오브젝트의 자식 오브젝트개수만큼 순회
            for (int index = 2; index < transform.childCount; index++)
            {
                // 자식 오브젝트 비활성화
                transform.GetChild(index).gameObject.SetActive(false);
            }

            // "Dead"애니메이션 출력.
            anim.SetTrigger("Dead");
            // GameOver 함수 실행
            GameManager.instance.GameOver();
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}

