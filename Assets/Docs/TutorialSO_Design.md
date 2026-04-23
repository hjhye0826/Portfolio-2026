# Tutorial ScriptableObject 설계

## TutorialDataSO

튜토리얼 목록 데이터를 관리하는 ScriptableObject

| 필드 | 타입 | 설명 |
|------|------|------|
| id | int | 튜토리얼 고유 ID |
| actionGroupId | int | 참조하는 Action 그룹 ID |
| triggerTime | float | 튜토리얼 시작 시간 조건 |

```
TutorialDataSO
└── List<TutorialData>
    ├── id
    ├── actionGroupId
    └── triggerTime
```

---

## TutorialActionsSO

튜토리얼 Action 목록 데이터를 관리하는 ScriptableObject

| 필드 | 타입 | 설명 |
|------|------|------|
| id | int | Action 고유 ID |
| groupId | int | 소속 그룹 ID (TutorialData.actionGroupId 참조) |
| step | int | 실행 순서 |
| actionType | ActionType | Action 종류 |
| stringValue | string | 문자열 파라미터 (ex. Dialog refId) |
| floatValue | float | 실수 파라미터 (ex. WaitTime duration) |
| intValue | int | 정수 파라미터 |

```
TutorialActionsSO
└── List<TutorialActionData>
    ├── id
    ├── groupId
    ├── step
    ├── actionType
    ├── stringValue
    ├── floatValue
    └── intValue
```

---

## 연결 구조

```
TutorialDataSO.actionGroupId
        ↓
TutorialActionsSO.groupId 로 Action 그룹 조회
        ↓
step 기준으로 정렬 후 순차 실행
```
