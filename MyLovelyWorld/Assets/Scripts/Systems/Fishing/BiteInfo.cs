// 한 번의 입질 정보. 캐스팅 시점에 BiteGenerator가 만든다.
public struct BiteInfo
{
    public SizeClass size;
    public int fishId;           // 플레이어에게는 포획 전까지 숨김
    public float waitTime;       // 입질까지 대기 시간(초)
    public float shadowScale;    // 그림자 크기 = shadow_ratio × (1 ± shadow_noise)
    public bool isShark;         // 청상아리 입질 여부 (S6)
}
