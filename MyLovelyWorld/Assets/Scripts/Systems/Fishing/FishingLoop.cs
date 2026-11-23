using System;
using System.Collections.Generic;
using UnityEngine;

// S3 + S2 연결. Casting → Waiting → Biting → Resolved 루프를 돌리는 상태 머신.
public class FishingLoop : MonoBehaviour
{
    [SerializeField] GameConfigSO config;
    [SerializeField] SizeTableSO sizeTable;
    [SerializeField] FishTableSO fishTable;

    [Header("Debug (read only)")]
    [SerializeField] FishingState state;
    [SerializeField] float stateTimer;

    public FishingState State => state;
    public BiteInfo CurrentBite { get; private set; }
    public WaveLevel CurrentWave {get; private set;}
    public JudgeInputQueue judgeQueue{get; private set;}

    // Events
    public event Action<BiteInfo> BiteStarted;                    // → S2, UI
    public event Action<BiteInfo, JudgeResult> CatchResolved;     // → S5, S6, UI
    public event Action<WaveLevel> WaveChanged;

    JudgeModule judgeModule;
    BiteGenerator generator;
    bool isShark = false;   // 다음 입질이 청상아리
    bool isFever = false;
    float judgeTime;
    float biteWaitMult;
    float ignoreProb;

    void Awake()
    {
        judgeModule = new JudgeModule(config,ignoreProb);
        judgeTime = config.judgeTimeSec;
        biteWaitMult = 1;
        ignoreProb = 0;
        generator = new BiteGenerator(sizeTable, fishTable);
        judgeQueue = new JudgeInputQueue();
        judgeQueue.Open();
    }

    void Update()
    {
        float dt = Time.unscaledDeltaTime;   // 기획서: timeScale 무관

        switch (state)
        {
            case FishingState.Casting:
                // TODO: 입질 추첨 → Waiting
                SetBite();
                ChangeState(FishingState.Waiting);
                break;
            case FishingState.Waiting:
                // TODO: 대기 시간 차감 → 0이면 BiteStarted 발행 → Biting
                stateTimer-=dt;
                if (stateTimer <= 0){
                    BiteStarted?.Invoke(CurrentBite);
                    ChangeState(FishingState.Biting);
                }
                break;
            case FishingState.Biting:
                stateTimer -= dt;
                if(stateTimer<=0) Judge();
                break;
            case FishingState.Resolved:
                // TODO: 결과 표시 시간 후 → Casting
                stateTimer -= dt;
                if(stateTimer <= 0){
                    ChangeState(FishingState.Casting);
                }
                break;
            case FishingState.Terminated:
                break;
        }
    }

    // TODO: 상태 전환 공통 처리 (stateTimer 초기화 등)
    void ChangeState(FishingState next){
        switch (next){
            case FishingState.Waiting:
                stateTimer = CurrentBite.waitTime;
                break;
            case FishingState.Biting:
                stateTimer = judgeTime;
                judgeQueue.Open();
                break;
            default:
                stateTimer = 0;
                break;

        }
        state = next;
    }

    void SetBite(){
        float mult = biteWaitMult;
        if(isFever) mult *= config.feverWaitMult;
        CurrentBite = generator.Generate(mult,isShark);
        isShark = false;
    }

    public void Judge(){
        if(state != FishingState.Biting) return;

        // 새 Judge Context 셋팅
        JudgeContext context = default;
        context.waveLevel = CurrentWave;
        context.sizeClass = CurrentBite.size;
        (context.waveKey, context.sizeKey) = judgeQueue.Peek();

        CatchResolved?.Invoke(CurrentBite, judgeModule.Judge(context));
        ChangeState(FishingState.Resolved);
        judgeQueue.Close();
    }

    // TODO: 라운드 종료 시 호출. 진행 중 입질을 점수 없이 취소하고 루프 정지 (S1)
    public void CancelAndStop(){
        if(state == FishingState.Biting){
            JudgeResult result = new JudgeResult();
            result.outcome = CatchOutcome.Cancelled;
            CatchResolved?.Invoke(CurrentBite, result);
        }
        ChangeState(FishingState.Terminated);
    }

    // TODO: 라운드 시작 시 호출. 루프 시작
    public void InitSkillEffect(Dictionary<EffectKey,float> skillEffects){
        if(skillEffects.ContainsKey(EffectKey.JudgeTimeAddSec)) judgeTime += skillEffects[EffectKey.JudgeTimeAddSec];
        if(skillEffects.ContainsKey(EffectKey.BiteWaitMult)) biteWaitMult = skillEffects[EffectKey.BiteWaitMult];
        if(skillEffects.ContainsKey(EffectKey.IgnoreWrongKey)) judgeModule = new JudgeModule(config,skillEffects[EffectKey.IgnoreWrongKey]);
        if(skillEffects.ContainsKey(EffectKey.LShareMult)) generator.SetSizeShareMult(SizeClass.Large,skillEffects[EffectKey.LShareMult]);
        if(skillEffects.ContainsKey(EffectKey.MShareMult)) generator.SetSizeShareMult(SizeClass.Medium,skillEffects[EffectKey.MShareMult]);
        if(skillEffects.ContainsKey(EffectKey.SShareMult)) generator.SetSizeShareMult(SizeClass.Small,skillEffects[EffectKey.SShareMult]);
        if(skillEffects.ContainsKey(EffectKey.MLShareMult)) generator.SetSizeShareMult(SizeClass.Medium,SizeClass.Large,skillEffects[EffectKey.MLShareMult]);
    }
    public void Begin(){
        ChangeState(FishingState.Casting);
        OnWaveChange();
    }
    public void OnWaveChange(){
        CurrentWave = (WaveLevel)UnityEngine.Random.Range(0, 3);
        WaveChanged?.Invoke(CurrentWave);
    }
    public void SetNextBiteShark() => isShark = true;
    public float GetBiteTimer() => stateTimer/judgeTime;
    public void SetFever(bool isFever) => this.isFever = isFever;
}
