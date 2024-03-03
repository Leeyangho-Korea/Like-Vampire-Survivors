using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어의 이동과 애니메이션 스크립트
/// </summary>

public class Player : MonoBehaviour
{
    public Vector2 inputVec = Vector2.zero; // 입력받는 좌표를 담을 변수 초기화.
    public Scanner scanner;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    public float speed = 0; // 캐릭터 이동 속도

    private void Awake()
    {
        // 변수 초기화
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void Start()
    {
        
    }


     void FixedUpdate()
    {
        // 받은 값에 의한 벡터 정규화. Time.fixedDeltaTime = 물리 프레임 1개 소비된 시간
        Vector2 tempVec = inputVec.normalized * speed * Time.fixedDeltaTime;

        // 플레이어 이동.
        rigid.MovePosition(rigid.position + tempVec);
    }

    private void LateUpdate()
    {
        // 정지 상태가 아닐 때
        if(inputVec.x != 0)  {
            // 스프라이트렌더러의 flip기능을 사용해서 좌우 반전
            spriteRenderer.flipX = inputVec.x < 0;
        }

        // 입력 값에 의해 애니메이션 컨트롤.
        anim.SetFloat("Speed", inputVec.magnitude);


      //  anim.SetTrigger("Dead");
    }

    // InputSystem을 사용한 이동
    void OnMove(InputValue _value)
    {
        // 인풋시스템의 값을 Vector2 형태로 불러오기.
        inputVec = _value.Get<Vector2>();
    }
}
