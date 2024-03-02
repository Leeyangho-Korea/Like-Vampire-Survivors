using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타일맵 이동 스크립트 ( 무한 맵 ) 
/// </summary>

public class ReplaceMap : MonoBehaviour
{
    Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    // Area 콜라이더가 타일맵에서 만들어진 타일맵에서 벗어날 때 타일맵 복제.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        // 현재 플레이어 위치와 타일맵 위치
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        // x좌표의 거리 절댓값
        float distanceX = Mathf.Abs(playerPos.x - myPos.x);
        float distanceY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = GameManager.instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (distanceX > distanceY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if( distanceX < distanceY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy": // 적의 위치와 너무 멀어지면 적을 플레이어에 가깝게 위치
                if (coll.enabled)
                {
                    // 플레이어의 이동 방향에 따라 맞은 편에서 등장하도록 이동.
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }

    }
}
