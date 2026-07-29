# CLAUDE.md

## Overview
Unity 3D 퍼즐 인터랙션 프로젝트

---

## Decision Rules
### Type Usage
* 지역 변수는 기본적으로 `var` 사용
* 타입이 명확하지 않을 때만 명시적 타입 사용

---

### Field Exposure
* 외부 설정이 필요한 값만 `[SerializeField]` 사용
* 런타임 변경 값은 private 유지

---

### Responsibility
* 하나의 클래스는 하나의 역할만 담당
* 상태 관리와 동작 로직을 분리

---

### Collection Usage
* 단순 순회는 `foreach` 사용
* 인덱스 접근이 필요한 경우만 `for` 사용
* 외부 노출 컬렉션은 수정 불가능하게 유지

---

### Method Design
* 메서드는 하나의 동작만 수행
* 조건 분기가 많아지면 메서드 분리
* 외부 호출 메서드와 내부 로직 메서드 분리

---

### Initialization
* 초기화 로직은 명확한 진입점에서 수행
* Awake / Start 중 하나만 선택해서 사용
* 초기화 순서가 중요한 경우 명시적으로 분리

---

### Boolean Expression
* 조건은 긍정형으로 작성
* 이중 부정 사용 금지

---

### Null Handling
* null 가능성이 있는 값은 명시적으로 체크
* null 상태를 정상 흐름으로 사용하지 않는다

---

## Naming
* Interface: `I` prefix
* Abstract: `Base` prefix
* ScriptableObject: `SO` suffix
* Private: `_camelCase`
* Public: `PascalCase`
* Event: `OnXXX`

---

## Constraints
* Magic Number 사용 금지
* 모든 제어문에 중괄호 사용
