using System;
using UnityEngine;

// S4. 물살 3단계. current_period_sec마다 직전과 다른 단계로 변경, 바뀌기 전 예고.
public class CurrentSystem : MonoBehaviour
{
    [SerializeField] GameConfigSO config;

    public CurrentLevel Level { get; private set; }
    public event Action<CurrentLevel> CurrentChanged;
    public event Action<float> CurrentWarning;   // 남은 시간

    // TODO: 라운드 시작 시 무작위 시작값
    public void Begin(float periodMultiplier) { }

    void Update()
    {
        // TODO: 주기 차감, 예고 발행, 단계 변경
    }

    public void Stop() { }
}
