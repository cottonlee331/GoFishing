using UnityEngine;
// 판정 입력 (기획서 구현 입력 요약 S4).
public struct JudgeContext
{
    public JudgeKey? waveKey;      // 큐 1번째 = 물살 판단 (비어 있으면 null → 오답)
    public JudgeKey? sizeKey;         // 큐 2번째 = 어종 판단
    public WaveLevel waveLevel; // 확정 시점의 물살
    public SizeClass sizeClass;
    public  bool duringFever;          // 피버 상태 여부
}

// 판정 출력.
public struct JudgeResult
{
    public float wavePoint;
    public float sizePoint;
    public bool duringFever;          // 피버 상태 여부
    public CatchOutcome outcome;
}


// S2. 판정 모듈
public class JudgeModule
{
    readonly GameConfigSO config;
    readonly float ignoreProb = 0;

    public JudgeModule(GameConfigSO config, float ignoreProb)
    {
        this.config = config;
        this.ignoreProb = ignoreProb;
    }
    public JudgeResult Judge(in JudgeContext context)
    {
        JudgeResult result = new JudgeResult();

        result.wavePoint = ScoreKey(context.waveKey, (int)context.waveLevel);
        result.sizePoint = ScoreKey(context.sizeKey,(int)context.sizeClass);
        if(result.wavePoint*result.sizePoint<1 && UnityEngine.Random.Range(0,100)<ignoreProb*100) result.outcome = CatchOutcome.Lucky;
        else result.outcome = RollBiteResult(result.wavePoint,result.sizePoint);
        Debug.Log($"물살: {context.waveLevel} 사이즈: {context.sizeClass}");
        Debug.Log($"물살: {result.wavePoint} 사이즈: {result.sizePoint}");
        return result;
    }

    // 입력 키가 정답과 같으면 1, Steady면 0.5, 그 외(미입력 포함)는 0
    private static float ScoreKey(JudgeKey? key, int answer){
        if (key == null) return 0f;
        if ((int)key.Value == answer) return 1f;
        if (key.Value == JudgeKey.Steady) return 0.5f;
        return 0f;
    }

    private CatchOutcome RollBiteResult(float wavePoint, float sizePoint){
        float probability = config.p0+(config.p2-config.p0)*(wavePoint+sizePoint)*0.5f;
        if(probability > config.pMax){
            probability = config.pMax;
        }

        int rand = Random.Range(1,100);
        Debug.Log("확률:"+probability*100+"주사위:"+rand);
        if(rand < probability*100) return CatchOutcome.Success;
        else return CatchOutcome.Fail;
    }
}