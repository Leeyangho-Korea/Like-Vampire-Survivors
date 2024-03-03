using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장착 아이템에 따른 효과 정리 스크립트
/// </summary>
public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        // 공통 세팅
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // 아이템 별로 가지고 있는 타입과 데미지로 초기화.
        type = data.itemType;
        rate = data.damages[0];
        //  정보 적용.
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            // Glove 타입일경우
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            // Sho 타입일 경우
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp()
    {
        // 현재 가능한 무기 정보들 가져오기
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                // 무기 id 가 0일 경우 (근접무기)
                case 0:
                    // 1.5배 속도 증가
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                    // 그  외 일 경우 (총알)
                default:
                    // 총알 연사 속도 증가 ( 총알 발사 주기를 짧게 )
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }

    // 속도 증가
    void SpeedUp()
    {
        // 현재 캐릭터 속도 증가
        float speed = 3 * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
