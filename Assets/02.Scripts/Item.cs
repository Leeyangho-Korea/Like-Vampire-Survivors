using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        // 자식오브젝트중 첫번째 인덱스에 있는 이미지를 가져옴. ( 부모 오브젝트의 이미지가 0번 인덱스에 담기기 때문. )
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            // 아이템이 근접무기 또는 총일 경우
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                // textDesc.text = 연사속도 {~~} % 증가로 표현
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
                // 아이템이 글러브 또는 신발일 경우
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                // 이동속도 {~~} % 증가로 표현
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                // 그것도 아닐 경우 해당 아이템의 설명에 적힌 것 표현
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            // 근접 무기 또는 총을 선택했을 때
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                // 해당 무기의 레벨이 0일 경우
                if (level == 0)
                {
                    // 무기 생성
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    // 무기 값 초기화
                    weapon.Init(data);
                }
                else
                {
                    // 무기의 레벨 상승에 따른 데미지 , 레벨 증가
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                // 레벨 업
                level++;
                break;
                // 글러브 또는 신발일 때
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }

                level++;
                break;
                // 힐 팩일때
            case ItemData.ItemType.Heal:
                // 현재 체력을 최대 체력으로 변경
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }
        // 현재 레벨이 최고레벨일 경우 ( 설정해둔 데미지 값의 최대일 때 )
        if (level == data.damages.Length)
        {
            // 해당 버튼은 비활성화
            GetComponent<Button>().interactable = false;
        }
    }
}
