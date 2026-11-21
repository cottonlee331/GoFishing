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

    float feverGaugeMult = 1;

    public void OnCatchResult(CatchOutcome outcome, SizeClass size)
    {
        if (IsFever) return;
        if (outcome != CatchOutcome.Success) return;

        Gauge += sizeTable.Get(size).gaugeGain*feverGaugeMult;
        GaugeChanged?.Invoke(Gauge);
        if (Gauge >= config.gaugeMax){
            Gauge = config.gaugeMax;
            StartFever();
        }
    }

    void Update()
    {
        if (IsFever){
            FeverRemaining -= Time.unscaledDeltaTime;
            if (FeverRemaining <= 0)
            {
                EndFever();
            }
        }
    }

    void StartFever(){
        IsFever = true;
        FeverRemaining = config.feverDurationSec;
        FeverStarted?.Invoke(FeverRemaining);
    }

    void EndFever(){
        IsFever = false;
        Gauge = 0f;
        FeverEnded?.Invoke();
    }

    public void InitSkillEffect(float feverMult){
        feverGaugeMult = feverMult;
    }
}
