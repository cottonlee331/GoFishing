using System;
using System.Collections;
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
    //public int sharkCount;
    //public int[] selectedSkillIds;
}

// S5. 포획 점수 = 기본 점수 × 스킬 계수 × 부분 점수 × 피버 배율 + 스킬 9 보너스
// S6. 청상아리 조합 진행도 (기본 소+중+대, 스킬 8은 대+대+중, 같은 어종 불인정).
public class ScoreSystem : MonoBehaviour
{
    [SerializeField] GameConfigSO config;
    [SerializeField] FishTableSO fishTable;
    [SerializeField] SizeTableSO sizeTable;

    [SerializeField] FishingLoop loop;
    [SerializeField] FeverSystem feverSystem;

    public int TotalScore { get; private set; }
    private readonly List<CatchRecord> records = new();
    public IReadOnlyList<CatchRecord> Records => records;

    public bool SharkReady { get; private set; }   // 다음 입질이 청상아리
    public int[] SharkGoal {get; private set;} = new int[3];    // 청상아리 조합 목표 (어종 id 3개)
    public bool[] SharkProgress {get; private set;} = new bool[3];   // 청상아리 조합 진행도
    public float feverMult {get; private set;}

    public event Action<CatchRecord, int> CatchScored;   // 획득 점수 기록, 총점
    public event Action<CatchOutcome, SizeClass> CatchResult;   // 포획 성공 여부
    public event Action<int> SharkProgressCaught; // 청상아리 조합 포획 성공

    private float[] sizePointMult = {1,1,1,1};
    private int currentBonus = 0;
    

    void Awake()
    {
        TotalScore = 0;
        feverMult = config.feverMult;
        SharkReady = false;
    }

    void OnEnable(){
        loop.CatchResolved += OnCatchResolved;
        CatchResult += feverSystem.OnCatchResult;
    }
    void OnDisable() {
        loop.CatchResolved -= OnCatchResolved;
        CatchResult -= feverSystem.OnCatchResult;
    }

    void OnCatchResolved(BiteInfo bite, JudgeResult result)
    {
        // 포획 결과 발행

        CatchOutcome outcome = RollBiteResult(result);
        CatchResult?.Invoke(outcome,bite.size);
        if(outcome != CatchOutcome.Success && outcome != CatchOutcome.Lucky) return;

        // 포획 성공 시 점수 기록
        CatchRecord record = new CatchRecord();
        record.fishId = bite.fishId;
        record.size = bite.size;
        record.duringFever = result.duringFever;
        record.mult = sizePointMult[(int)bite.size];
        if(result.duringFever) record.mult *= feverMult;
        record.add = 0;
        if(result.currentPoint == 1f && outcome!=CatchOutcome.Lucky) record.add = currentBonus;
        // TODO: 스킬 계수, 부분 점수, 피버 배율 적용
        record.points = sizeTable.Get(bite.size).points+fishTable.Get(bite.fishId).basePoints;
        record.points = (int)(record.points*sizePointMult[(int)bite.size]);
        if(result.duringFever){
            record.points = (int)(record.points*feverMult);
        }
        TotalScore += record.points;

        // 청상아리 조합 진행도 갱신
        for(int i = 0; i < SharkGoal.Length; i++){
            if(bite.fishId == SharkGoal[i] && !SharkProgress[i]){
                SharkProgress[i] = true;
                SharkProgressCaught?.Invoke(i);
                for(int j = 0; j < SharkProgress.Length; j++){
                    if(!SharkProgress[j]){
                        SharkReady = false;
                        break;
                    }
                    SharkReady = true;
                }
                if(SharkReady){
                    loop.SetNextBiteShark();
                }
            }
        }
        records.Add(record);
        CatchScored?.Invoke(record, TotalScore);
    }

    void InitSharkGoal(int sharkCombo)
    {
        for(int i = 0; i<SharkGoal.Length; i++){
            SizeClass goalSize = (SizeClass)(sharkCombo%10);
            sharkCombo/=10;

            SharkGoal[i] = fishTable.GetBySize(goalSize)[UnityEngine.Random.Range(0, fishTable.GetBySize(goalSize).Length)].id;
            SharkProgress[i] = false;
        }
        Debug.Log($"SharkGoal: {SharkGoal[0]}, {SharkGoal[1]}, {SharkGoal[2]}");
        SharkReady = false;
    }

    public void InitSkillEffect(Dictionary<EffectKey,float> skillEffects){
        if(skillEffects.ContainsKey(EffectKey.FeverMultAdd)) feverMult += skillEffects[EffectKey.FeverMultAdd];
        
        if(skillEffects.ContainsKey(EffectKey.LPointMult)) sizePointMult[0] = skillEffects[EffectKey.LPointMult];
        if(skillEffects.ContainsKey(EffectKey.MPointMult)) sizePointMult[1] = skillEffects[EffectKey.MPointMult];
        if(skillEffects.ContainsKey(EffectKey.SPointMult)) sizePointMult[2] = skillEffects[EffectKey.SPointMult];

        if(skillEffects.ContainsKey(EffectKey.CurrentBonus)) currentBonus = (int)skillEffects[EffectKey.CurrentBonus];

        int sharkCombo = 12;    // 상어 기본 조합 0,1,2(크기순 L/M/S)
        if(skillEffects.ContainsKey(EffectKey.SharkComboChange)) sharkCombo = (int)skillEffects[EffectKey.SharkComboChange];
        InitSharkGoal(sharkCombo);
    }

    CatchOutcome RollBiteResult(JudgeResult result){
        float probability = config.p0+(config.p2-config.p0)*(result.currentPoint+result.sizePoint)*0.5f;
        if(probability > config.pMax){
            probability = config.pMax;
        }

        int rand = UnityEngine.Random.Range(1,100);
        Debug.Log("확률:"+probability*100+"주사위:"+rand);
        if(rand < probability*100){
            if(result.isLuckey) return CatchOutcome.Lucky;
            return CatchOutcome.Success;
        }
        return CatchOutcome.Fail;
    }

    public RoundResult BuildResult(){
        RoundResult result = new RoundResult();
        result.totalScore = TotalScore;
        result.catches = new List<CatchRecord>(records);
        return result;
    }
}
