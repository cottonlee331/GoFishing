using System;
using UnityEngine;

// S3 + S2 연결. Casting → Waiting → Biting → Resolved 루프를 돌리는 상태 머신.
public class FishingLoop : MonoBehaviour
{
    [SerializeField] GameConfigSO config;
    [SerializeField] SizeTableSO sizeTable;
    [SerializeField] FishTableSO fishTable;
    [SerializeField] FeverSystem feverSystem;

    [Header("Debug (read only)")]
    [SerializeField] FishingState state;
    [SerializeField] float stateTimer;

    public FishingState State => state;
    public BiteInfo CurrentBite { get; private set; }

    public CurrentLevel CurrentWave {get; private set;}

    public event Action<BiteInfo> BiteStarted;                    // → S2, UI
    public event Action<BiteInfo, JudgeResult> CatchResolved;     // → S5, S6, UI
    public event Action<CurrentLevel> CurrentChanged;

    JudgeModule judge;
    JudgeInputQueue judgeQueue;
    BiteGenerator generator;
    bool isShark = false;   // 다음 입질이 청상아리

    void Awake()
    {
        judge = new JudgeModule();
        judgeQueue = new JudgeInputQueue();
        generator = new BiteGenerator(sizeTable, fishTable, config);
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
                // TODO: 낚기 입력 시 judge로 판정 / judge_time_sec 초과 시 TIMEOUT → Resolved
                stateTimer -= dt;
                if (stateTimer <= 0){
                    Judge();
                }
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
                stateTimer = config.judgeTimeSec;
                judgeQueue.Open();
                // 스킬에 의한 연장 가능성은 추후 구현
                break;
            case FishingState.Resolved:
                stateTimer = config.resultTimeSec;
                judgeQueue.Close();
                // 결과 저장 및 UI 추가 필요
                break;
            default:
                stateTimer = 0;
                break;

        }
        state = next;
    }

    void SetBite(){
        float waitMultiplier = 1f;
        if(feverSystem.IsFever) waitMultiplier *= config.feverWaitMult;
        // TODO: 스킬로 인한 waitMultiplier 보정은 추후 구현
        CurrentBite = generator.Generate(waitMultiplier,isShark);
        isShark = false;
    }

    void Judge(){
        if(state != FishingState.Biting) return;
        Debug.Log("Judge Confirm");
        JudgeContext context = default; 
        (context.currentKey, context.sizeKey) = judgeQueue.Peek();
        context.currentLevel = CurrentWave;
        context.duringFever = feverSystem.IsFever;
        CatchResolved?.Invoke(CurrentBite, judge.Judge(context));
        ChangeState(FishingState.Resolved);
    }

    // TODO: 라운드 종료 시 호출. 진행 중 입질을 점수 없이 취소하고 루프 정지 (S1)
    public void CancelAndStop()
    {
        if(state == FishingState.Biting){
            CatchResolved?.Invoke(CurrentBite, new JudgeResult());
        }
        ChangeState(FishingState.Terminated);
    }

    // TODO: 라운드 시작 시 호출. 루프 시작
    public void Begin()
    {
        ChangeState(FishingState.Casting);
        CurrentWave = CurrentLevel.Normal;
        OnWaveChange();
    }
    public void OnWaveChange(){
        CurrentWave = (CurrentLevel)UnityEngine.Random.Range(0, 3);
        CurrentChanged?.Invoke(CurrentWave);
    }
    public void SetNextBiteShark(){
        isShark = true;
    }

    public (float,float) GetBiteTimer(){
        return (stateTimer,config.judgeTimeSec);
    }

    public void JudgeButton(JudgeKey key){
        judgeQueue.Push(key);
        Debug.Log("물살:"+judgeQueue.Peek().current+" 어종:"+judgeQueue.Peek().size);
    }

    public void ConfirmButton()
    {
        Judge();
    }
}
