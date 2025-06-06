# **Like Vampire Survivors**

## 소개

이 프로젝트는 Unity 기반의 2D 탑다운 생존 게임.
플레이어는 다양한 무기를 장착하고 몰려오는 몬스터를 처치하며 일정 시간 동안 생존.
경험치를 획득해 레벨업하고, 장비를 업그레이드하며, 조건을 만족하면 새로운 캐릭터도 해금.

## 주요 스크립트 구성

| 분류 | 스크립트명 | 역할 |
|------|------------|------|
| 게임 흐름 | GameManager.cs | 게임 시작, 종료, 승리, 패배, 경험치/레벨 관리 |
| 캐릭터 제어 | Player.cs | 이동, 충돌 판정, 사망 처리 |
| UI 표시 | HUD.cs | 체력, 경험치, 시간, 킬 수 등 표시 |
| 레벨업 시스템 | LevelUp.cs | 아이템 3종 선택 UI, 능력 적용 |
| 게임 결과 | Result.cs | 승리/패배 UI 처리 |
| 오디오 | AudioManager.cs | BGM 및 효과음 처리 |
| 캐릭터 해금 | AchiveManager.cs | 킬 수 또는 생존 시간에 따른 캐릭터 해금 |
| 무기 시스템 | Weapon.cs | 근접/원거리 무기 발사 및 회전 처리 |
| 총알 처리 | Bullet.cs | 데미지 계산, 관통 여부, 충돌 처리 |
| 적 AI | Enemy.cs | 추적, 피격, 사망 처리 및 넉백 |
| 적 탐색 | Scanner.cs | 주변 적 탐지 및 타겟 설정 |
| 손 연출 | Hand.cs | 무기 이미지의 위치 및 방향 조절 |
| 아이템 UI | Item.cs | 아이템 정보 표시 및 클릭 처리 |
| 아이템 데이터 | ItemData.cs | 무기, 장비, 힐 정보 (ScriptableObject) |
| 장비 효과 | Gear.cs | 장비 효과(공격속도, 이동속도) 적용 |
| 적 생성기 | Spawner.cs | 시간에 따른 적 종류 및 속도 변화 |
| 오브젝트 풀링 | PoolManager.cs | 총알/몬스터 등의 재사용 관리 |
| UI 위치 추적 | Follow.cs, FollowPlayer.cs | 체력바 및 UI 요소의 위치 고정 |
| 맵 처리 | ReplaceMap.cs, Reposition.cs | 무한 맵 구현을 위한 타일 위치 갱신 |

## 캐릭터 해금 조건

| 캐릭터 | 해금 조건 |
|--------|-----------|
| Potato | 한 판에 20킬 이상 |
| Bean   | 제한 시간까지 생존 |

## 사용 기술

- ScriptableObject를 활용한 아이템 데이터 구조화
- PlayerPrefs를 통한 로컬 저장 기반 캐릭터 해금 시스템
- Object Pooling 기반 총알 및 적 최적화 관리
- AudioSource 및 AudioHighPassFilter 조합으로 사운드 처리
- Physics2D.Raycast 및 CircleCast를 활용한 타겟 탐색 시스템
- Canvas UI 및 RectTransform으로 동적 HUD 구현
- OnTrigger/OnCollision 기반 충돌 판정 및 넉백 처리
- 무한 타일맵 이동 로직 (Reposition, ReplaceMap)

## 주요 기능 요약

- 근접무기 회전 및 원거리 무기 자동 발사
- 경험치 수집 및 레벨업 시스템
- 자동 적 추적 및 피격 넉백 처리
- 장비 장착으로 능력치 향상
- 오브젝트 풀링 기반 최적화 구조
- 무한 맵 구현
- 캐릭터 해금 시스템

## 개발 환경

- Unity 2021.3.13f1
- Universal Render Pipeline (URP)
- UGUI
- 2D
