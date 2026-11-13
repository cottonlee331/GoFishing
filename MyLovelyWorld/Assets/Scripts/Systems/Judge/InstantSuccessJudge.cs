using System;

// 1단계 임시 판정: 낚기를 누르면 무조건 성공. 2단계에서 ProbabilityJudge로 교체.
public class InstantSuccessJudge : IJudgeModule
{
    // TODO: outcome = Success 반환
    public JudgeResult Judge(in JudgeContext context) => throw new NotImplementedException();
}
