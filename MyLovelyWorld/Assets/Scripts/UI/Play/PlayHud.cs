using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// S10. 플레이 화면 HUD. 타이머, 점수, 피버, 포획 목록, 청상아리, 장착 스킬, 물살, Ending 문구.
// 시스템 값을 읽기만 하고 수정하지 않는다.
public class PlayHud : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] RoundController round;
    [SerializeField] ScoreSystem score;
    [SerializeField] FeverSystem fever;
    [SerializeField] FishingLoop loop;
    [SerializeField] FishTableSO fishTable;
    [SerializeField] SkillTableSO skillTable;
    [SerializeField] GameConfigSO config;

    [Header("Timer / Score")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text scoreText;

    [Header("Fever")]
    [SerializeField] Slider feverGaugeFill;
    [SerializeField] TMP_Text feverText;

    [Header("Bite Popup")]
    [SerializeField] GameObject bitePopup;
    [SerializeField] GameObject resultPopup;
    [SerializeField] Sprite[] resultSprite = new Sprite[3]; // 0은 성공, 1은 실패, 2는 스킬성공

    [Header("Catch log")]
    [SerializeField] Transform catchLogRoot;
    [SerializeField] GameObject catchLogItemPrefab;

    [Header("Shark")]
    [SerializeField] Image[] sharkSlots = new Image[3];
    [SerializeField] Image[] sharkCheck = new Image[3];

    [Header("Current")]
    [SerializeField] SpriteRenderer[] currentObj;
    [SerializeField] Sprite[] currentSprite;

    [Header("Equipped skills")]
    [SerializeField] SelectedDetailPrefab[] skillDetail;

    [Header("Ending")]
    [SerializeField] GameObject endingBanner;

    private float resultTimer=0;

    void OnEnable(){
        score.CatchScored += OnCatchScored;
        score.CatchResult += OnCatchResult;
        score.SharkProgressCaught += OnSharkProgressCaught;
        fever.GaugeChanged += OnGaugeChanged;
        fever.FeverStarted += OnFeverStarted;
        fever.FeverEnded += OnFeverEnded;
        loop.CurrentChanged += OnCurrentChanged;
        loop.BiteStarted+= OnBiteStarted;
        loop.CatchResolved += OnCatchResolved;
        round.RoundStarted += OnRoundStarted;
        round.RoundEnded += OnRoundEnded;
    }

    void OnDisable(){
        score.CatchScored -= OnCatchScored;
        score.SharkProgressCaught -= OnSharkProgressCaught;
        fever.GaugeChanged -= OnGaugeChanged;
        fever.FeverStarted -= OnFeverStarted;
        fever.FeverEnded -= OnFeverEnded;
        loop.CurrentChanged -= OnCurrentChanged;
        round.RoundEnded -= OnRoundEnded;
    }

    void Start()
    {
        // TODO: 장착 스킬 아이콘(GameSession.SelectedSkillIds), 초기값 표시, endingBanner 숨김
        foreach(Transform child in catchLogRoot) Destroy(child.gameObject);
        timerText.text = FormatTime(0);
        scoreText.text = "0 점";
        feverGaugeFill.value = 0;
        bitePopup.SetActive(false);

        for(int i = 0; i<skillDetail.Length; i++)
        {
            skillDetail[i].gameObject.SetActive(true);
            if(i>=GameSession.Instance.SelectedSkillIds.Count) skillDetail[i].gameObject.SetActive(false);
            else
            {
                skillDetail[i].SetSkillDetail(GameSession.Instance.SelectedSkillIds[i]);
            }
        }
    }

    void Update(){
        timerText.text = FormatTime(round.GetRoundTime());
        if(resultTimer>0){
            resultTimer-=Time.unscaledDeltaTime;
            if(resultTimer<=0){
                resultPopup.SetActive(false);
            }
        }
    }

    // TODO: 점수 갱신, 포획 목록 항목 추가, 청상아리 칸 갱신
    void OnCatchScored(CatchRecord record, int totalScore) {
        scoreText.text = totalScore.ToString()+" 점";
        GameObject obj = Instantiate(catchLogItemPrefab,catchLogRoot);
        obj.GetComponent<ScoreUnitPrefab>().InitPrefab(record.fishId,record.mult,record.add);
    }

    void OnCatchResult(CatchOutcome outcome, SizeClass size){
        int result = 1;
        if(outcome == CatchOutcome.Success) result = 0;
        else if(outcome == CatchOutcome.Lucky) result = 2;
        resultTimer = config.resultTimeSec;
        resultPopup.SetActive(true);
        resultPopup.GetComponent<Image>().sprite = resultSprite[result];
    }

    void OnSharkProgressCaught(int slotNum){
        sharkCheck[slotNum].gameObject.SetActive(true);
    }

    // TODO: 게이지 비율 표시
    void OnGaugeChanged(float gauge) {
        feverGaugeFill.value = gauge/config.gaugeMax;
    }

    // TODO: 피버 표시로 전환
    void OnFeverStarted(float duration) { }

    // TODO: 게이지 표시로 복귀
    void OnFeverEnded() { }

    void OnBiteStarted(BiteInfo bite){
        bitePopup.SetActive(true);
        bitePopup.GetComponent<BitePopupView>().OnBiteStarted(bite);
    }

    void OnCatchResolved(BiteInfo bite, JudgeResult judge)
    {
        if(loop.State==FishingState.Terminated)return;
        bitePopup.SetActive(false);
    }
    // TODO: 물살 단계 표시
    void OnCurrentChanged(CurrentLevel level) {
        Debug.Log($"CurrentChange:{level}");
        for(int i = 0; i<currentObj.Length; i++)
        {
            currentObj[i].sprite = currentSprite[(int)level];
        }
    }

    // TODO: 물살 변화 예고 깜빡임
    void OnCurrentWarning(float remaining) { }

    void OnRoundStarted()
    {
        sharkSlots[0].sprite = fishTable.Get(score.SharkGoal[0]).icon;
        sharkSlots[1].sprite = fishTable.Get(score.SharkGoal[1]).icon;
        sharkSlots[2].sprite = fishTable.Get(score.SharkGoal[2]).icon;
        sharkCheck[0].gameObject.SetActive(false);
        sharkCheck[1].gameObject.SetActive(false);
        sharkCheck[2].gameObject.SetActive(false);
    }
    // TODO: "오늘 고기잡이 종료!" 표시
    void OnRoundEnded(RoundResult result) { }

    // TODO: 초 → "m:ss"
    static string FormatTime(float seconds){
        string timeString = "";
        int min = (int)seconds/60;
        int sec = (int)seconds%60;
        if(min<10) timeString+="0";
        timeString+=min.ToString()+":";
        if(sec<10) timeString+="0";
        timeString+=sec.ToString();
        return timeString;
    }
}
