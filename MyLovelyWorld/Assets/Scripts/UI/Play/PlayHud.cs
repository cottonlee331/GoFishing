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
    [SerializeField] CurrentSystem current;
    [SerializeField] SkillTableSO skillTable;

    [Header("Timer / Score")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text scoreText;

    [Header("Fever")]
    [SerializeField] Image feverGaugeFill;
    [SerializeField] TMP_Text feverText;

    [Header("Catch log")]
    [SerializeField] Transform catchLogRoot;
    [SerializeField] GameObject catchLogItemPrefab;

    [Header("Shark")]
    [SerializeField] Image[] sharkSlots = new Image[3];

    [Header("Current")]
    [SerializeField] TMP_Text currentText;

    [Header("Equipped skills")]
    [SerializeField] Image[] skillIcons = new Image[3];

    [Header("Ending")]
    [SerializeField] GameObject endingBanner;

    void OnEnable()
    {
        // TODO: 이벤트 구독 (score.CatchScored, fever.GaugeChanged/FeverStarted/FeverEnded,
        //       current.CurrentChanged/CurrentWarning, round.RoundEnded)
    }

    void OnDisable()
    {
        // TODO: OnEnable에서 구독한 이벤트 모두 해제
    }

    void Start()
    {
        // TODO: 장착 스킬 아이콘(GameSession.SelectedSkillIds), 초기값 표시, endingBanner 숨김
    }

    void Update()
    {
        // TODO: 남은 시간 표시 (RoundController에 남은 시간 공개 프로퍼티 필요)
        //       피버 중 타이머가 0이면 0:00 고정 + 피버 남은 시간 표시
    }

    // TODO: 점수 갱신, 포획 목록 항목 추가, 청상아리 칸 갱신
    void OnCatchScored(CatchRecord record, int totalScore) { }

    // TODO: 게이지 비율 표시
    void OnGaugeChanged(float gauge) { }

    // TODO: 피버 표시로 전환
    void OnFeverStarted(float duration) { }

    // TODO: 게이지 표시로 복귀
    void OnFeverEnded() { }

    // TODO: 물살 단계 표시
    void OnCurrentChanged(CurrentLevel level) { }

    // TODO: 물살 변화 예고 깜빡임
    void OnCurrentWarning(float remaining) { }

    // TODO: "오늘 고기잡이 종료!" 표시
    void OnRoundEnded(RoundResult result) { }

    // TODO: 초 → "m:ss"
    static string FormatTime(float seconds) => throw new System.NotImplementedException();
}
