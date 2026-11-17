using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// S7. 스킬 선택 화면. 선택 규칙(최대 3개, 중복 불가), 상세 패널, 선택 3칸, 게임 시작.
public class SkillSelectController : MonoBehaviour
{
    const int MaxSelect = 3;

    [SerializeField] SkillTableSO skillTable;

    [Header("Grid")]
    [SerializeField] SkillSlot[] slots;   // 씬에 배치한 슬롯 (분류별 4행)

    [Header("Detail")]
    [SerializeField] Image detailIcon;
    [SerializeField] TMP_Text detailTitle;
    [SerializeField] TMP_Text detailCategory;
    [SerializeField] TMP_Text detailDescription;
    [SerializeField] Button selectButton;
    [SerializeField] Button releaseButton;

    [Header("Selected")]
    [SerializeField] Image[] selectedIcons = new Image[MaxSelect];

    readonly List<int> selected = new();
    int focusedSkillId = -1;

    void Start()
    {
        // TODO: 슬롯마다 Setup(row, this)
        // TODO: 다시하기로 돌아온 경우 GameSession.SelectedSkillIds 복원 (확인 필요)
        // TODO: 상세 패널 초기 상태, 선택 칸 갱신
    }

    // SkillSlot이 클릭되면 호출
    public void OnSlotClicked(int skillId)
    {
        // TODO: focusedSkillId 갱신, 슬롯 포커스 표시, ShowDetail
    }

    // TODO: 상세 패널 내용, 선택/해제 버튼 활성 상태 갱신
    void ShowDetail(int skillId) { }

    // 선택 버튼 OnClick
    public void OnSelectButton()
    {
        // TODO: 3개 미만이고 미선택이면 추가 → 갱신
    }

    // 해제 버튼 OnClick
    public void OnReleaseButton()
    {
        // TODO: 선택돼 있으면 제거 → 갱신
    }

    // TODO: 선택 3칸 아이콘, 슬롯 선택 표시, 버튼 상태 갱신
    void RefreshSelectedView() { }

    // 게임 시작 버튼 OnClick
    public void OnStartButton()
    {
        // TODO: GameSession.SetSelectedSkills(selected) → Play 씬 (3개 미만이어도 시작)
    }
}
