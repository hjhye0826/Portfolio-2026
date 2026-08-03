using UnityEngine;

/// <summary>
/// 씬 오브젝트의 전역 정체성(identity)을 나타내는 키 에셋.
/// 오브젝트(씬)와 데이터(SO)가 같은 키 에셋을 참조해 서로를 안전하게 연결한다.
/// 이름/이동에 무관한 GUID 기반이라 리네임에 깨지지 않는다.
/// </summary>
[CreateAssetMenu(fileName = "ObjectKey", menuName = "Keys/ObjectKey")]
public class ObjectKey : ScriptableObject
{
}
