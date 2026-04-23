# Interaction System 설계 문서

## 프로젝트 컨텍스트
- Unity 3D 포트폴리오용 퍼즐 인터랙션 시스템
- 참고 레퍼런스: 원신 스타일의 월드 기믹
- 환경 오브젝트 퍼즐 메커니즘 (압력판, 스위치, 회전 기믹, 연쇄 트리거 등)

## 코드 규약
- 지역변수는 `var` 사용
- Favor Composition over Inheritance 설계 원칙 준수
- 코드 설계와 구조의 확장성 우선

---

## 파일 구조

```
📁 Assets/Scripts/Interaction
 ├── InteractableBase.cs     ← 모든 기믹의 베이스 (IInteractable 흡수 + Layer 자동 세팅)
 ├── IFocusable.cs
 ├── InteractionController.cs
 │
 └── 📁 UI
      ├── InteractionSelectionUI.cs
      └── InteractionListItem.cs
```

---

## 플레이어 오브젝트 구조

```
📦 Player
 ├── PlayerController        (이동)
 ├── CameraController        (카메라)
 └── InteractionController   (인터랙션 탐지/처리) ← Player에 부착

📦 Managers (추후 추가 예정)
 └── InteractionManager      (퍼즐 상태/연쇄 트리거 전역 관리)
```

---

## 스크립트별 상세 설계

### `InteractableBase.cs`
모든 퍼즐 기믹이 상속받는 추상 베이스 클래스

- `IInteractable`을 별도 인터페이스로 두지 않고 베이스 클래스로 흡수
- 모든 기믹 공통 동작인 Layer 세팅을 Awake에서 자동 처리
- 상속이 적절한 이유: Layer 세팅은 모든 기믹의 공통 동작이므로 한 곳에서 관리하는 것이 올바른 상속 사용

```csharp
public abstract class InteractableBase : MonoBehaviour
{
    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable"); // 모든 기믹 공통
        OnAwake();
    }

    protected virtual void OnAwake() { } // 각 기믹의 Awake 로직은 여기서

    public abstract void OnInteract(GameObject interactor); // 인터랙션 실행
    public abstract bool CanInteract { get; }               // 인터랙션 가능 여부
    public abstract string InteractionPrompt { get; }       // UI에 표시할 오브젝트 이름
}
```

---

### `IFocusable.cs`
포커스 반응이 필요한 기믹만 선택적으로 구현 (ISP 원칙)

- `OnFocus/OnDefocus`는 모든 기믹이 필요한 게 아니므로 인터페이스로 분리
- Composition over Inheritance를 적용한 지점

```csharp
public interface IFocusable
{
    void OnFocus();    // 포커스 진입 시 (압력판 테두리 등)
    void OnDefocus();  // 포커스 해제 시 (원복)
}
```

**구현 예시:**
```csharp
// 압력판 - 포커스 반응 필요 → IFocusable 추가 구현
public class PressurePlate : InteractableBase, IFocusable { }

// 스위치 - 포커스 반응 불필요 → InteractableBase만 상속
public class Switch : InteractableBase { }
```

---

### `InteractionController.cs`
Player에 부착. 탐지/인터랙션 처리 담당

**상태 관리:**
```
None   : 범위 내 오브젝트 없음 → UI 숨김
Single : 범위 내 오브젝트 1개  → UI 숨김, 자동 포커스
Multi  : 범위 내 오브젝트 2개+ → UI 표시, 휠 입력 활성화
```

**주요 함수:**

| 함수 | 역할 |
|---|---|
| `OverlapSphere()` | 매 프레임 반경 내 InteractableBase 탐색 (Interactable Layer 기준) |
| `SortByDistance()` | 후보 리스트 거리순 정렬 |
| `RefreshCandidates()` | 후보 갱신, 이전 타겟 유지 시도 / 실패 시 index 0 리셋 |
| `SetIndex(int)` | 선택 index 변경 - **단일 진실 공급원** → OnDefocus(이전) / OnFocus(신규) 처리 → UI Refresh 호출 |
| `HandleWheel()` | 휠 입력 → SetIndex() 순환 (Multi 상태에서만) |
| `HandleInteract()` | F키 → currentTarget.OnInteract() 호출 |
| `UpdateState()` | None / Single / Multi 상태 전환 → UI 활성/비활성 제어 |

**핵심 설계 포인트:**
- `transform.position` 기준으로 OverlapSphere 탐색 (별도 Transform 참조 불필요)
- `SetIndex()`가 모든 상태 변경의 단일 진입점
- IFocusable 체크: `if (target is IFocusable focusable) focusable.OnFocus()`
- 후보 갱신 시 이전 타겟 유지 로직 필수 (경계에서 포커스 튀는 현상 방지)

---

### `InteractionSelectionUI.cs`
Multi 상태일 때 후보 리스트 패널 관리

| 함수 | 역할 |
|---|---|
| `Show(List<InteractableBase>)` | 패널 활성화, ListItem 동적 생성 |
| `Hide()` | 패널 비활성화, ListItem 전체 제거 |
| `Refresh(int index)` | 선택된 index 하이라이트 갱신 |

---

### `InteractionListItem.cs`
개별 버튼 UI 컴포넌트

| 함수 | 역할 |
|---|---|
| `Init(label, index, onHover, onClick)` | 버튼 초기화, 콜백 주입 (onHover/onClick 기본값 null) |
| `SetHighlight(bool)` | 하이라이트 on/off (외부에서 제어) |
| `OnPointerEnter()` | onHover 콜백 invoke (null이면 무시) |
| `OnPointerClick()` | onClick 콜백 invoke (null이면 무시) |

**확장 포인트:**
- 현재는 마우스 미지원으로 onHover/onClick = null
- 추후 마우스 모드 추가 시 Init() 호출부에 콜백만 주입하면 확장 완료
- InteractionController, UI 구조 변경 불필요

---

## 입력 정의

| 입력 | 동작 |
|---|---|
| `F키` | 현재 포커스된 대상 인터랙션 실행 |
| `마우스 휠` | Multi 상태에서 선택 index 순환 |

---

## UI 동작 정의

- **Single 상태**: 리스트 UI 숨김, 가장 가까운 오브젝트 자동 포커스
- **Multi 상태**: 화면 고정 패널에 후보 오브젝트 이름 리스트 출력 (거리순)
  - 선택된 항목 하이라이트
  - 휠 Up → 이전 항목 / 휠 Down → 다음 항목
