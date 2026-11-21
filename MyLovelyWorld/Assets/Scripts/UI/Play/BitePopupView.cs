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
    [SerializeField] Image[] queueSlots = new Image[2];
    [SerializeField] Sprite[] queueSprite = new Sprite[4];

    [Header("Judge time")]
    [SerializeField] Image sizeIndicator;
    [SerializeField] Sprite[] sizeSprite = new Sprite[3];

    [Header("Judge time")]
    [SerializeField] Slider judgeTimeFill;

    float time;
    float judgetime;
    public JudgeInputQueue judgeQueue{get; private set;}

    void OnEnable()
    {
        // TODO: fishingLoop.BiteStarted, CatchResolved 구독
        // TODO: 큐 Changed 구독 (FishingLoop에 JudgeInputQueue 공개 필요)
        judgeQueue = new JudgeInputQueue();
        //loop.BiteStarted += OnBiteStarted;
        judgeQueue.Changed += OnQueueChanged;
        judgeQueue.Open();
    }

    void OnDisable()
    {
        // TODO: 구독 해제
        //loop.BiteStarted -= OnBiteStarted;
        judgeQueue.Changed -= OnQueueChanged;
        judgeQueue.Close();
    }

    void Update()
    {
        time = loop.GetBiteTimer().Item1;
        judgetime = loop.GetBiteTimer().Item2;
        judgeTimeFill.value = time/judgetime;
        if(time<=0) ConfirmButton();
        // TODO: Biting 중 판정 남은 시간 게이지 (FishingLoop에 남은 시간 공개 필요)
    }

    // TODO: 팝업 표시, 그림자 크기 설정, 큐 칸 비우기, 결과 문구 숨김
    public void OnBiteStarted(BiteInfo bite)
    {
        if ((int)bite.size == 3){
            sizeIndicator.sprite = sizeSprite[0];
            return;
        }
        sizeIndicator.sprite = sizeSprite[(int)bite.size];
    }

    // TODO: 큐 내용 표시 (Peek)
    void OnQueueChanged() {
        JudgeKey? currentKey;
        JudgeKey? sizeKey;
        (currentKey,sizeKey) = judgeQueue.Peek();
        if(currentKey!=null) queueSlots[0].sprite = queueSprite[(int)currentKey+1];
        else queueSlots[0].sprite = queueSprite[0];
        if(sizeKey!=null) queueSlots[1].sprite = queueSprite[(int)sizeKey+1];
        else queueSlots[1].sprite = queueSprite[0];
    }

    public void QueueButton(int key) {
        judgeQueue.Push((JudgeKey)key);
        Debug.Log("물살:"+judgeQueue.Peek().current+" 어종:"+judgeQueue.Peek().size);
    }

    public void ConfirmButton() {
        JudgeContext context = default;
        (context.currentKey, context.sizeKey) = judgeQueue.Peek();
        loop.Judge(context);
    }
}
