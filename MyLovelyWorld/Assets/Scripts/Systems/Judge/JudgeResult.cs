// 판정 출력.
public struct JudgeResult
{
    public CatchOutcome outcome;
    public bool isPartial;        // 소형 + 무난
    public float probability;     // 계산된 성공 확률 (로그용)
}
