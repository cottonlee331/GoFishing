using System;
using UnityEngine;

// S1. timeScale과 무관한 라운드 카운트다운.
public class RoundTimer
{
    public float Remaining { get; private set; }
    public bool IsExpired => Remaining <= 0f;
    public event Action Expired;

    // TODO: round_time_sec로 시작
    public void Start(float seconds) { }

    // TODO: 실시간 dt 차감, 0 도달 시 Expired 한 번 발행
    public void Tick(float unscaledDt) { }
}

// S1. Play 씬의 라운드 상태(Playing → Ending) 관리. 다른 시스템을 시작·종료시킨다.
public class RoundController : MonoBehaviour
{
    [SerializeField] GameConfigSO config;
    [SerializeField] FishingLoop fishingLoop;
    [SerializeField] FeverSystem feverSystem;

    [Header("Debug (read only)")]
    [SerializeField] RoundState state;

    public event Action RoundStarted;
    public event Action<RoundResult> RoundEnded;

    RoundTimer timer;

    void OnEnable()
    {
        timer = new RoundTimer();
        timer.Expired += EndRound;
    }
    void Start()
    {
        // TODO: 라운드 시작 (타이머, 물살, 루프 시작)
        RoundStarted?.Invoke();
    }

    void Update()
    {
        // TODO: 타이머 Tick, 종료 조건 검사 (타이머 0 && 피버 아님)
    }

    // TODO: 진행 중 입질 취소 → Ending 연출 → Result 씬으로
    void EndRound()
    {
        RoundResult result = new RoundResult();
        result.totalScore = 0; // TODO: 점수 합산
        RoundEnded?.Invoke(result);
    }
}
