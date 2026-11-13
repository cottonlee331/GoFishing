using System;
using UnityEngine;

// S5. 피버 게이지, 자동 진입, 지속 시간, 종료 시 게이지 0.
public class FeverSystem : MonoBehaviour
{
    [SerializeField] GameConfigSO config;
    [SerializeField] SizeTableSO sizeTable;

    public float Gauge { get; private set; }
    public bool IsFever { get; private set; }
    public float FeverRemaining { get; private set; }

    public event Action<float> FeverStarted;   // 지속 시간
    public event Action FeverEnded;
    public event Action<float> GaugeChanged;

    // TODO: 성공 포획 시 게이지 증가 (피버 중 제외, 초과분 버림)
    void OnCatchResolved(BiteInfo bite, JudgeResult result) { }

    void Update()
    {
        // TODO: 피버 남은 시간 차감 (실시간)
    }
}
