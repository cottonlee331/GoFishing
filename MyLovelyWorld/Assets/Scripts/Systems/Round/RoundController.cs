using System;
using UnityEngine;

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

    void Start()
    {
        // TODO: 라운드 시작 (타이머, 물살, 루프 시작)
    }

    void Update()
    {
        // TODO: 타이머 Tick, 종료 조건 검사 (타이머 0 && 피버 아님)
    }

    // TODO: 진행 중 입질 취소 → Ending 연출 → Result 씬으로
    void EndRound() { }
}
