using System;
using UnityEngine;

// S5. 포획 점수 = 기본 점수 × 스킬 계수 × 부분 점수 × 피버 배율 + 스킬 9 보너스
public class ScoreSystem : MonoBehaviour
{
    [SerializeField] GameConfigSO config;
    [SerializeField] FishTableSO fishTable;

    public int TotalScore { get; private set; }
    public event Action<CatchRecord, int> CatchScored;   // 획득 점수 기록, 총점

    // TODO: FishingLoop.CatchResolved 구독, 성공 시 점수 계산과 기록
    void OnCatchResolved(BiteInfo bite, JudgeResult result) { }

    // TODO: RoundResult 생성
    public RoundResult BuildResult() => throw new NotImplementedException();
}
