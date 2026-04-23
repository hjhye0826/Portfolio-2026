# Tutorial System 설계 문서

## 클래스 구조 개요

```
TutorialManager
└── IEnumerable<Tutorial>

Tutorial
└── List<TutorialAction>

TutorialAction (base)
├── TutorialTouch
├── TutorialWaitTime
└── TutorialDialog
```

---

## TutorialManager

### 책임
- 전체 튜토리얼 목록 보유
- 플레이 시간 기반 시작 조건 확인
- 조건 충족 시 Tutorial 실행

### 변수
| 변수 | 타입 | 설명 |
|------|------|------|
| tutorials | IEnumerable\<Tutorial\> | 게임 내 모든 튜토리얼 |
| currentPlayTime | float | 누적 플레이 시간 |

### 흐름
```
Update()
├── currentPlayTime 누적
└── tutorials 순회
    └── currentPlayTime >= tutorial.triggerTime → tutorial.Start()
```

---

## Tutorial

### 책임
- Step 순차 실행
- 현재 Step의 Action 완료 감지 → 다음 Step 진행
- Action 에러 감지 → 튜토리얼 중단

### 변수
| 변수 | 타입 | 설명 |
|------|------|------|
| triggerTime | float | 튜토리얼 시작 시간 조건 |
| currentActions | List\<TutorialAction\> | 현재 Step의 Action 목록 |
| allStepActions | List\<List\<TutorialAction\>\> | 전체 Step Action 목록 |
| currentStepIndex | int | 현재 Step 인덱스 |

### 흐름
```
Start()
└── 첫 번째 Step의 Action들 일괄 Start()

Update()
└── currentActions 순회
    ├── action.OnProcess() 호출
    ├── action.errorString != "" → 전체 End() 후 튜토리얼 중단
    └── 모든 action.IsCompleted == true → 전체 End() 후 다음 Step으로
```

### Step 진행 규칙
- Step 내 Action들은 **동시 시작, 전부 완료 시 다음 Step**
- Step 간 실행은 **순차**

---

## TutorialAction

### 책임
- 개별 튜토리얼 동작의 라이프사이클 관리
- 완료 및 에러 상태 관리

### 변수
| 변수 | 타입 | 설명 |
|------|------|------|
| IsCompleted | bool | 완료 여부 |
| errorString | string | 에러 내용, "" 이면 정상 |
| tutorialDataId | string | 참조하는 튜토리얼 데이터 ID |

### 메서드
| 메서드 | 형태 | 설명 |
|--------|------|------|
| Start() | virtual | 액션 초기화 |
| OnProcess() | virtual | 매 프레임 동작 처리 |
| Complete() | virtual | IsCompleted = true 세팅, 추가 처리 필요 시 오버라이딩 |
| End() | virtual | 정리 책임, 정상/에러 모두 항상 호출됨 |

### 라이프사이클
```
Start()
OnProcess()
├── 정상 완료 → Complete() → End()
└── 에러 발생 → errorString 세팅 → End()
```

### 서브클래스 구현 가이드
- **단순한 Action** → `OnProcess()`만 구현
- **완료 시 추가 처리 필요** → `Complete()` 오버라이딩 후 `base.Complete()` 호출
- **정리 로직 필요** → `End()` 오버라이딩 후 `base.End()` 호출

---

## TutorialAction 서브클래스

| 클래스 | 설명 |
|--------|------|
| TutorialTouch | 특정 오브젝트 터치/클릭 감지 |
| TutorialWaitTime | 지정 시간 대기 |
| TutorialDialog | 다이얼로그 표시 및 완료 감지 |

---

## 에러 처리 흐름

```
Tutorial.Update()
└── action.errorString != ""
    ├── 현재 Step의 모든 Action End() 호출
    └── 튜토리얼 중단
```

- `errorString`은 Action 내부에서 세팅
- Tutorial은 매 프레임 순회 시 체크
- 에러 발생 시 **같은 Step의 나머지 Action도 반드시 End() 호출** (UI 등 정리 보장)

---

## 확장 고려사항

- 조건 종류가 다양해질 경우 `ITutorialCondition` 인터페이스 분리 고려
- 진행 상태 저장이 필요할 경우 TutorialManager에 저장 로직 추가
- 튜토리얼 데이터가 많아질 경우 ScriptableObject 기반 데이터 드리븐 구조 고려
