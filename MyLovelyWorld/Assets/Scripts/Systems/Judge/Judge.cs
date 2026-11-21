using UnityEngine;
// 판정 입력 (기획서 구현 입력 요약 S4).
public struct JudgeContext
{
    public JudgeKey? currentKey;      // 큐 1번째 = 물살 판단 (비어 있으면 null → 오답)
    public JudgeKey? sizeKey;         // 큐 2번째 = 어종 판단
    public CurrentLevel currentLevel; // 확정 시점의 물살
    public SizeClass sizeClass;
    public  bool duringFever;          // 피버 상태 여부
    public float difficulty;          // sizes.difficulty
    public float forgiveProb;         // 스킬 7 침착 (없으면 0)
}

// 판정 출력.
public struct JudgeResult
{
    public float currentPoint;
    public float sizePoint;
    public bool duringFever;          // 피버 상태 여부
}


// S2. 판정 모듈
public class JudgeModule
{
    public JudgeResult Judge(in JudgeContext context)
    {
        JudgeResult result = default;
        result.duringFever = context.duringFever;

        result.currentPoint = ScoreKey(context.currentKey, (int)context.currentLevel);
        result.sizePoint = ScoreKey(context.sizeKey,(int)context.sizeClass);
        Debug.Log($"물살: {context.currentLevel} 사이즈: {context.sizeClass}");
        Debug.Log($"물살: {result.currentPoint} 사이즈: {result.sizePoint}");
        return result;
    }

    // 입력 키가 정답과 같으면 1, Steady면 0.5, 그 외(미입력 포함)는 0
    private static float ScoreKey(JudgeKey? key, int answer){
        if (key == null) return 0f;
        if ((int)key.Value == answer) return 1f;
        if (key.Value == JudgeKey.Steady) return 0.5f;
        return 0f;
    }
}