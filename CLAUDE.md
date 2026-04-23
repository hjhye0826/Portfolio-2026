# CLAUDE.md

## Project Overview
Unity 3D 포트폴리오용 환경 퍼즐 인터랙션 시스템.
- **Render Pipeline**: Built-in

---

## 구조
- Assets/Resources/ : 각종 리소스타 프리팹, 하위 폴더 확인 후 적절한 위치에 생성
- Assets/Scripts/ : 스크립트 파일, 하위 폴더 확인 후 적절한 위치에 생성

## Coding Style

### 네이밍 컨벤션

| 대상 | 규칙 | 예시 |
|---|---|---|
| 인터페이스 | `I` 접두사 | `IPuzzleElement` |
| 추상 클래스 | `Base` 접두사 | `BaseTrigger` |
| ScriptableObject | `SO` 접미사 | `PuzzleConfigSO` |
| private 필드 | `_camelCase` | `_isActivated` |
| UnityEvent 필드 | `On` 접두사 | `OnActivated` |
| 지역 변수 | `var` 사용 | `var plate = GetComponent<PressurePlate>()` |

### 필드 선언
- private 필드 + `[SerializeField]` 사용. public 필드 지양

---

## Work Rules
### 코드 생성 시 주의사항
- Magic Number 금지 — 상수 또는 SerializeField로 노출
