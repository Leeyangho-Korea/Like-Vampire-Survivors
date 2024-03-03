using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ĳ���� ������ ���� �ʱ�ȭ �ϰ� ���� �������ִ� ��ũ��Ʈ
/// </summary>
public class Character : MonoBehaviour
{
   
    public static float Speed
    {
        // PlayerID�� 0�̶�� 1.1, �ƴ϶�� 1�� ��ȯ
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    public static float WeaponSpeed
    {
        // PlayerID�� 1�̶�� 1.1, �ƴ϶�� 1�� ��ȯ
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }

    public static float WeaponRate
    {
        // PlayerID�� 1�̶�� 0.9, �ƴ϶�� 1�� ��ȯ
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
    }

    public static float Damage
    {
        // PlayerID�� 2��� 1.2, �ƴ϶�� 1�� ��ȯ
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }

    public static int Count
    {
        // PlayerID�� 3�̶�� 1, �ƴ϶�� 0�� ��ȯ
        get { return GameManager.instance.playerId == 3 ? 1 : 0; }
    }
}

