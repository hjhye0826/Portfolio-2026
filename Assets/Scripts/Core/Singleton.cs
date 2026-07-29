/// <summary>
/// POCO(일반 C# 클래스) 기반 싱글톤 베이스.
/// 외부 SDK 래퍼 등 Unity 라이프사이클이 필요 없는 매니저용.
/// 첫 접근 시 Lazy 초기화로 인스턴스 생성.
/// </summary>
public abstract class Singleton<T> where T : Singleton<T>, new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }

    protected Singleton() { }
}
