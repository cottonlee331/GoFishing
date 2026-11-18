using System;
using System.Collections.Generic;
using UnityEngine;

// 포획 1건 기록.
public struct CatchRecord
{
    public int fishId;
    public SizeClass size;
    public int points;
    public bool duringFever;
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

    public event Action<CatchRecord, int> CatchScored;   // 획득 점수 기록, 총점
    public event Action<CatchOutcome, SizeClass> CatchResult;   // 포획 성공 여부

    void Awake()
    {
        TotalScore = 0;
        SharkReady = false;
        InitSharkGoal();
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
        if(outcome != CatchOutcome.Success) return;

        // 포획 성공 시 점수 기록
        CatchRecord record = new CatchRecord();
        record.fishId = bite.fishId;
        record.size = bite.size;
        record.duringFever = result.duringFever;
        // TODO: 스킬 계수, 부분 점수, 피버 배율 적용
        record.points = sizeTable.Get(bite.size).points+fishTable.Get(bite.fishId).basePoints;
        if(result.duringFever){
            record.points = (int)(record.points*config.feverMult);
        }
        TotalScore += record.points;

        // 청상아리 조합 진행도 갱신
        for(int i = 0; i < SharkGoal.Length; i++){
            if(bite.fishId == SharkGoal[i] && !SharkProgress[i]){
                SharkProgress[i] = true;
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

    void InitSharkGoal()
    {
        // 스킬에 의한 조합 변경은 추후 구현
        SharkGoal[0] = fishTable.GetBySize(SizeClass.Large)[UnityEngine.Random.Range(0, fishTable.GetBySize(SizeClass.Large).Length)].id;
        SharkGoal[1] = fishTable.GetBySize(SizeClass.Medium)[UnityEngine.Random.Range(0, fishTable.GetBySize(SizeClass.Medium).Length)].id;
        SharkGoal[2] = fishTable.GetBySize(SizeClass.Small)[UnityEngine.Random.Range(0, fishTable.GetBySize(SizeClass.Small).Length)].id;
        SharkProgress[0] = false;
        SharkProgress[1] = false;
        SharkProgress[2] = false;
        SharkReady = false;
    }

    CatchOutcome RollBiteResult(JudgeResult result){
        float probability = config.p0+(config.p2-config.p0)*(result.currentPoint+result.sizePoint)*0.5f;
        if(probability > config.pMax){
            probability = config.pMax;
        }

        int rand = UnityEngine.Random.Range(1,100);
        Debug.Log("확률:"+probability*100+"주사위:"+rand);
        if(rand < probability*100){
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
