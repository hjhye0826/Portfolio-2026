using System.Collections;
using UnityEngine;

/// <summary>
/// 구멍을 덮는 슬라이드 덮개. 좌/우 두 패널이 양옆에서 나와 가운데서 만나 닫힌다.
/// 레버(IToggleable)로 원격 조작된다. IsOpen=true 는 '구멍이 열림(패널 후퇴)'을 의미한다.
/// 직접 F 대상이 아니며(CanInteract=false), 자신의 ObjectKey로 레지스트리에만 등록된다.
/// 패널은 로컬 X로만 미끄러진다: 좌 패널은 -X(왼쪽), 우 패널은 +X(오른쪽)로 물러나 열린다.
/// 씬에서는 패널을 '닫힘' 위치(구멍 중앙)에 배치해 두면 된다.
/// </summary>
public class SlideGate : InteractableBase, IToggleable
{
    [SerializeField] private Transform _leftPanel;
    [SerializeField] private Transform _rightPanel;
    [SerializeField] private float _openSlide = 4f;   // 각 패널이 열릴 때 바깥으로 물러나는 거리
    [SerializeField] private float _speed = 8f;        // 이동 속도(유닛/초)
    [SerializeField] private bool _startOpen = true;   // 시작 시 구멍이 열려 있음(블록이 그냥 떨어짐)

    private Vector3 _leftClosed, _leftOpen, _rightClosed, _rightOpen;
    private bool _isOpen;
    private Coroutine _anim;

    public bool IsOpen => _isOpen;

    // 레버로만 조작 → F 후보에서 제외
    public override bool CanInteract => false;

    protected override void OnAwake()
    {
        _leftClosed = _leftPanel.localPosition;
        _rightClosed = _rightPanel.localPosition;
        _leftOpen = _leftClosed + Vector3.left * _openSlide;
        _rightOpen = _rightClosed + Vector3.right * _openSlide;

        _isOpen = _startOpen;
        _leftPanel.localPosition = _isOpen ? _leftOpen : _leftClosed;
        _rightPanel.localPosition = _isOpen ? _rightOpen : _rightClosed;
    }

    public void Toggle() => SetOpen(!_isOpen);
    public void Open() => SetOpen(true);
    public void Close() => SetOpen(false);

    public void SetOpen(bool open)
    {
        _isOpen = open;

        if (_anim != null) StopCoroutine(_anim);

        Vector3 lt = _isOpen ? _leftOpen : _leftClosed;
        Vector3 rt = _isOpen ? _rightOpen : _rightClosed;

        if (isActiveAndEnabled)
        {
            _anim = StartCoroutine(Animate(lt, rt));
        }
        else
        {
            _leftPanel.localPosition = lt;
            _rightPanel.localPosition = rt;
        }
    }

    private IEnumerator Animate(Vector3 lt, Vector3 rt)
    {
        while ((_leftPanel.localPosition - lt).sqrMagnitude > 0.0000001f ||
               (_rightPanel.localPosition - rt).sqrMagnitude > 0.0000001f)
        {
            _leftPanel.localPosition =
                Vector3.MoveTowards(_leftPanel.localPosition, lt, _speed * Time.deltaTime);
            _rightPanel.localPosition =
                Vector3.MoveTowards(_rightPanel.localPosition, rt, _speed * Time.deltaTime);
            yield return null;
        }

        _leftPanel.localPosition = lt;
        _rightPanel.localPosition = rt;
        _anim = null;
    }
}
