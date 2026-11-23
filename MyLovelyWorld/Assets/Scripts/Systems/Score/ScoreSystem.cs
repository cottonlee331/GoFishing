using System.Collections.Generic;
using UnityEngine;

// 포획 1건 기록.
public struct CatchRecord
{
    public int fishId;
    public SizeClass size;
    public int points;
    public bool duringFever;
    public float mult;
    public int add;
}

// 라운드 결과. Result 씬으로 넘긴다.
public class RoundResult
{
    public int totalScore;
    public List<CatchRecord> catches;
}

// S5. 포획 점수 = 기본 점수 × 스킬 계수 × 부분 점수 × 피버 배율 + 스킬 9 보너스
// S6. 청상아리 조합 진행도 (기본 소+중+대, 스킬 8은 대+대+중, 같은 어종 불인정).
public class ScoreSystem : MonoBehaviour
{
    [SerializeField] GameConfigSO config;
    [SerializeField] FishTableSO fishTable;
    [SerializeField] SizeTableSO sizeTable;

    public int TotalScore { get; private set; }
    private readonly List<CatchRecord> records = new();
    public IReadOnlyList<CatchRecord> Records => records;

    public float feverMult {get; private set;}

    private float[] sizePointMult = {1,1,1,1};
    private int currentBonus = 0;
    

    void Awake()
    {
        TotalScore = 0;
        feverMult = config.feverMult;
    }

    public CatchRecord CalcScore(BiteInfo bite, JudgeResult result){
        // 포획 정보 기록
        CatchRecord record = new CatchRecord();
        record.fishId = bite.fishId;
        record.size = bite.size;
        record.duringFever = result.duringFever;
        record.mult = sizePointMult[(int)bite.size];
        if(result.duringFever) record.mult *= feverMult;
        record.add = 0;
        if(result.wavePoint == 1f && result.outcome!=CatchOutcome.Lucky) record.add = currentBonus;
        
        // 점수 계산
        record.points = sizeTable.Get(bite.size).points+fishTable.Get(bite.fishId).basePoints;
        record.points = (int)(record.points*sizePointMult[(int)bite.size]);
        if(result.duringFever){
            record.points = (int)(record.points*feverMult);
        }
        TotalScore += record.points;

        records.Add(record);
        return record;
    }

    public void InitSkillEffect(Dictionary<EffectKey,float> skillEffects){
        if(skillEffects.ContainsKey(EffectKey.FeverMultAdd)) feverMult += skillEffects[EffectKey.FeverMultAdd];
        
        if(skillEffects.ContainsKey(EffectKey.LPointMult)) sizePointMult[0] = skillEffects[EffectKey.LPointMult];
        if(skillEffects.ContainsKey(EffectKey.MPointMult)) sizePointMult[1] = skillEffects[EffectKey.MPointMult];
        if(skillEffects.ContainsKey(EffectKey.SPointMult)) sizePointMult[2] = skillEffects[EffectKey.SPointMult];

        if(skillEffects.ContainsKey(EffectKey.CurrentBonus)) currentBonus = (int)skillEffects[EffectKey.CurrentBonus];
    }
    public RoundResult BuildResult(){
        RoundResult result = new RoundResult();
        result.totalScore = TotalScore;
        result.catches = new List<CatchRecord>(records);
        return result;
    }
}
