using System;

// 2단계. P = min(p_max, (p0 + (p2 − p0) × s / 2) × d)
public class ProbabilityJudge : IJudgeModule
{
    // TODO: 물살 점수 + 어종 점수(소형+무난 0.5) → s → P → 난수 비교
    public JudgeResult Judge(in JudgeContext context) => throw new NotImplementedException();
}
