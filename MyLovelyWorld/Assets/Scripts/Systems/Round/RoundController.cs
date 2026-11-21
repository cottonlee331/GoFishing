using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// S1. timeScale과 무관한 라운드 카운트다운.
public class RoundTimer
{
    public float Remaining { get; private set; }
    public float WaveRemaining { get; private set; }   // 물살 변화까지 남은 시간
    public bool IsExpired => Remaining <= 0f;
    public event Action Expired;
    public event Action Wave;

    float WaveSeconds;

    public void Init(float seconds, float waveSeconds){
        Remaining = seconds;
        WaveRemaining = waveSeconds;
        WaveSeconds = waveSeconds;
    }

    // TODO: 실시간 dt 차감, 0 도달 시 Expired 한 번 발행
    public void Tick(float unscaledDt){
        //if (IsExpired) return;
        Remaining -= unscaledDt;
        WaveRemaining -= unscaledDt;
        if (Remaining <= 0f){
            Remaining = 0f;
            Expired?.Invoke();
        }
        if(WaveRemaining <= 0f){
            Wave?.Invoke();
            WaveRemaining = WaveSeconds;
        }
    }
}

// S1. Play 씬의 라운드 상태(Playing → Ending) 관리. 다른 시스템을 시작·종료시킨다.
public class RoundController : MonoBehaviour
{
    [SerializeField] GameConfigSO config;
    [SerializeField] SkillTableSO skillTable;
    [SerializeField] FishingLoop fishingLoop;
    [SerializeField] FeverSystem feverSystem;
    [SerializeField] ScoreSystem scoreSystem;

    [Header("Debug (read only)")]
    [SerializeField] RoundState state;

    public event Action RoundStarted;
    public event Action<RoundResult> RoundEnded;

    public RoundTimer timer{get; private set;}

    private Dictionary<EffectKey,float> skillEffects;
    private float waveMult = 1;


    void OnEnable()
    {
        timer = new RoundTimer();
        timer.Expired += EndRound;
        timer.Wave += fishingLoop.OnWaveChange;
        feverSystem.FeverEnded += EndRound;
    }
    void Start()
    {
        StartRound();
    }

    void Update(){
        timer.Tick(Time.unscaledDeltaTime);
    }

    void StartRound(){
        SetSkillEffects();
        if(skillEffects.ContainsKey(EffectKey.CurrentPeriodMult)) waveMult = skillEffects[EffectKey.CurrentPeriodMult];
        timer.Init(config.roundTimeSec, config.currentPeriodSec*waveMult);
        scoreSystem.InitSkillEffect(skillEffects);
        fishingLoop.InitSkillEffect(skillEffects);
        if(skillEffects.ContainsKey(EffectKey.FeverGaugeMult))feverSystem.InitSkillEffect(skillEffects[EffectKey.FeverGaugeMult]);
        fishingLoop.Begin();
        RoundStarted?.Invoke();
    }

    // TODO: 진행 중 입질 취소 → Ending 연출 → Result 씬으로
    void EndRound(){
        if(!timer.IsExpired) return;   // 타이머 만료 전이면 종료 무시
        if(feverSystem.IsFever) return;   // 피버 중이면 종료 지연
        fishingLoop.CancelAndStop();
        RoundResult result = scoreSystem.BuildResult();
        GameSession.Instance.SetLastResult(result);
        RoundEnded?.Invoke(result);
    }

    void SetSkillEffects(){
        skillEffects = new Dictionary<EffectKey, float>();
        foreach(int id in GameSession.Instance.SelectedSkillIds){
            var skill = skillTable.Get(id);
            AddSkillEffect(skill.effectKey,skill.effectValue);
            AddSkillEffect(skill.costKey,skill.costValue);
        }
    }

    void AddSkillEffect(EffectKey key, float val){
        if(skillEffects.ContainsKey(key)) skillEffects[key] += val;
        else skillEffects.Add(key,val);
    }

    public float GetRoundTime(){
        return timer.Remaining;
    }
}
