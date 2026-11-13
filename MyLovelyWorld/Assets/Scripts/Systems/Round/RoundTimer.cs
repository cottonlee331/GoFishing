using System;

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
