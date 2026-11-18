using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 입질 팝업. 그림자, 큐 2칸, 판정 시간 게이지, 성공/실패 표시.
// 판정 버튼 4개의 OnClick은 InputRouter.PressJudgeKey / PressConfirm에 연결한다.
public class BitePopupView : MonoBehaviour
{
    [SerializeField] FishingLoop loop;

    [Header("Popup")]
    [SerializeField] GameObject root;

    [Header("Queue (0 = 물살, 1 = 크기)")]
    [SerializeField] TMP_Text[] queueSlots = new TMP_Text[2];

    [Header("Judge time")]
    [SerializeField] Slider judgeTimeFill;

    [Header("Result")]
    [SerializeField] TMP_Text resultText;

    float time;
    float judgetime;

    void OnEnable()
    {
        // TODO: fishingLoop.BiteStarted, CatchResolved 구독
        // TODO: 큐 Changed 구독 (FishingLoop에 JudgeInputQueue 공개 필요)
        loop.BiteStarted += OnBiteStarted;
    }

    void OnDisable()
    {
        // TODO: 구독 해제
    }

    void Update()
    {
        time = loop.GetBiteTimer().Item1;
        judgetime = loop.GetBiteTimer().Item2;
        judgeTimeFill.value = time/judgetime;
        // TODO: Biting 중 판정 남은 시간 게이지 (FishingLoop에 남은 시간 공개 필요)
    }

    // TODO: 팝업 표시, 그림자 크기 설정, 큐 칸 비우기, 결과 문구 숨김
    void OnBiteStarted(BiteInfo bite) { }

    // TODO: 큐 내용 표시 (Peek)
    void OnQueueChanged() { }

    // TODO: 성공/실패/TIMEOUT 표시. Cancelled면 바로 숨김
    void OnCatchResolved(BiteInfo bite, JudgeResult result) { }

    public void BiteButton(int key) => loop.JudgeButton((JudgeKey)key);
    public void ConfirmButton() => loop.ConfirmButton();

}
