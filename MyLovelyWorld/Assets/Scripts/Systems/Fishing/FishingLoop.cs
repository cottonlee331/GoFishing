using System;
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

    public event Action<BiteInfo> BiteStarted;                    // → S2, UI
    public event Action<BiteInfo, JudgeResult> CatchResolved;     // → S5, S6, UI

    BiteGenerator generator;
    IJudgeModule judge;

    void Awake()
    {
        // TODO: generator 생성, judge = new InstantSuccessJudge() (2단계에서 교체)
        BiteStarted+=BiteStartedUI;
    }

    void Update()
    {
        float dt = Time.unscaledDeltaTime;   // 기획서: timeScale 무관

        switch (state)
        {
            case FishingState.Casting:
                // TODO: 입질 추첨 → Waiting
                SetCasting();
                ChangeState(FishingState.Waiting);
                break;
            case FishingState.Waiting:
                // TODO: 대기 시간 차감 → 0이면 BiteStarted 발행 → Biting
                stateTimer-=dt;
                if (stateTimer <= 0){
                    stateTimer = 0;
                    BiteStarted?.Invoke(CurrentBite);
                    ChangeState(FishingState.Biting);
                }
                break;
            case FishingState.Biting:
                // TODO: 낚기 입력 시 judge로 판정 / judge_time_sec 초과 시 TIMEOUT → Resolved
                ChangeState(FishingState.Resolved);
                break;
            case FishingState.Resolved:
                // TODO: 결과 표시 시간 후 → Casting
                ChangeState(FishingState.Casting);
                break;
        }
    }

    // TODO: 상태 전환 공통 처리 (stateTimer 초기화 등)
    void ChangeState(FishingState next){
        stateTimer = (next == FishingState.Waiting) ? CurrentBite.waitTime : 0;
        state = next;
    }

    void SetCasting(){
        
    }

    void BiteStartedUI(BiteInfo info)
    {
        
    }

    // TODO: 라운드 종료 시 호출. 진행 중 입질을 점수 없이 취소하고 루프 정지 (S1)
    public void CancelAndStop() { }

    // TODO: 라운드 시작 시 호출. 루프 시작
    public void Begin() { }
}
