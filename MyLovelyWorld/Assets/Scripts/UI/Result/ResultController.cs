using TMPro;
using UnityEngine;
using UnityEngine.UI;

// S8. 결과 화면. 총점, 포획 목록, 청상아리, 선택 스킬, 엔딩 등급 표시와 다음 행동.
// 리더보드 등록 흐름은 RegisterFlow(미생성)에서 처리.
public class ResultController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] GameConfigSO config;
    [SerializeField] FishTableSO fishTable;
    [SerializeField] SkillTableSO skillTable;

    [Header("Score / Grade")]
    [SerializeField] TMP_Text totalScoreText;
    [SerializeField] TMP_Text gradeText;
    [SerializeField] TMP_Text newRecordText;

    [Header("Catch grid")]
    [SerializeField] Transform catchGridRoot;
    [SerializeField] GameObject catchCellPrefab;

    [Header("Shark / Skills")]
    [SerializeField] TMP_Text sharkText;
    [SerializeField] Image[] skillIcons = new Image[3];

    void Start()
    {
        // TODO: GameSession.Instance.LastResult 읽기 (null이면 테스트용 처리)
        // TODO: ShowResult → 개인 최고 기록 갱신 → 등록 흐름 시작 (포획 0건이면 생략)
    }

    // TODO: 총점, 포획 그리드(어종별 마릿수), 청상아리 횟수, 스킬 아이콘 표시
    //       (RoundResult에 sharkCount, selectedSkillIds 필드 필요)
    void ShowResult(RoundResult result) { }

    // TODO: 총점 ≥ manseon_threshold 이면 만선
    bool IsManseon(int totalScore) => throw new System.NotImplementedException();

    // 다시하기 버튼 OnClick
    public void OnRetryButton()
    {
        // TODO: GameSession.ResetForRetry → SkillSet 씬
    }

    // 타이틀로 버튼 OnClick
    public void OnTitleButton()
    {
        // TODO: GameSession.ResetAll → Title 씬
    }
}
