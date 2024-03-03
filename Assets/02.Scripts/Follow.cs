using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  플레이어 체력바를 플레이어 따라다니도록 해주는 스크립트
/// </summary>
    public class Follow : MonoBehaviour
    {
        RectTransform rect;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        void FixedUpdate()
        {
        // 월드 좌표를 스크린 좌표로 변경해주는 함수를 사용해 World좌표의 캐릭터와
        // Screen에서 표현되는 UI의 좌표를 컨트롤 함.
            rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        }
    }

