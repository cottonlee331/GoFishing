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

    public int TotalScore { get; private set; }
    private readonly List<CatchRecord> records = new();
    public IReadOnlyList<CatchRecord> Records => records;
    public bool SharkReady { get; private set; }   // 다음 입질이 청상아리
    public int[] SharkGoal {get; private set;} = new int[3];    // 청상아리 조합 목표 (어종 id 3개)
    public bool[] SharkProgress {get; private set;} = new bool[3];   // 청상아리 조합 진행도
    public event Action<CatchRecord, int> CatchScored;   // 획득 점수 기록, 총점

    void Awake()
    {
        TotalScore = 0;
        SharkReady = false;
        InitSharkGoal();
    }

    void OnEnable() => loop.CatchResolved += OnCatchResolved;
    void OnDisable() => loop.CatchResolved -= OnCatchResolved;

    void OnCatchResolved(BiteInfo bite, JudgeResult result)
    {
        if (result.outcome != CatchOutcome.Success) return;
        CatchRecord record = new CatchRecord();
        record.fishId = bite.fishId;
        record.size = bite.size;
        record.duringFever = result.duringFever;
        // TODO: 스킬 계수, 부분 점수, 피버 배율 적용
        record.points = sizeTable.Get(bite.size).points+fishTable.Get(bite.fishId).basePoints;
        TotalScore += record.points;
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
            }
        }
        // TODO: 피버 게이지 추가
        records.Add(record);
        CatchScored?.Invoke(record, TotalScore);
    }

    void InitSharkGoal()
    {
        // 스킬에 의한 조합 변경은 추후 구현
        SharkGoal[0] = fishTable.GetBySize(SizeClass.Large)[UnityEngine.Random.Range(0, fishTable.GetBySize(SizeClass.Large).Length)].id;
        SharkGoal[1] = fishTable.GetBySize(SizeClass.Medium)[UnityEngine.Random.Range(0, fishTable.GetBySize(SizeClass.Medium).Length)].id;
        SharkGoal[2] = fishTable.GetBySize(SizeClass.Small)[UnityEngine.Random.Range(0, fishTable.GetBySize(SizeClass.Small).Length)].id;
        SharkReady = false;
    }

    // TODO: RoundResult 생성
    public RoundResult BuildResult() => throw new NotImplementedException();
}
