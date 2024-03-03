using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터 정보에 관해 초기화 하고 값을 리턴해주는 스크립트
/// </summary>
public class Character : MonoBehaviour
{
   
    public static float Speed
    {
        // PlayerID가 0이라면 1.1, 아니라면 1을 반환
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    public static float WeaponSpeed
    {
        // PlayerID가 1이라면 1.1, 아니라면 1을 반환
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }

    public static float WeaponRate
    {
        // PlayerID가 1이라면 0.9, 아니라면 1을 반환
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
    }

    public static float Damage
    {
        // PlayerID가 2라면 1.2, 아니라면 1을 반환
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }

    public static int Count
    {
        // PlayerID가 3이라면 1, 아니라면 0을 반환
        get { return GameManager.instance.playerId == 3 ? 1 : 0; }
    }
}

