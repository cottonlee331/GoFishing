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
    [SerializeField] ToggleGroup root;   // 스킬 리스트 토글그룹
    [SerializeField] GameObject prefab;

    [Header("Detail")]
    [SerializeField] Image detailIcon;
    [SerializeField] TMP_Text detailDescription;
    [SerializeField] Button selectButton;
    [SerializeField] Button releaseButton;

    [Header("Selected")]
    [SerializeField] SelectedDetailPrefab[] selectedDetail;

    readonly List<int> selected = new();
    int focusedSkillId = -1;

    void Start()
    {
        for(int i = 0; i<skillTable.rows.Length; i++){
            GameObject obj = Instantiate(prefab,root.transform);
            obj.GetComponent<SkillToggleUnitPrefab>().InitPrefab(i,root);
            obj.GetComponent<SkillToggleUnitPrefab>().ValueChanged += OnSlotClicked;
        }
        if (GameSession.Instance.SelectedSkillIds.Count > 0)
        {
            foreach(int k in GameSession.Instance.SelectedSkillIds) selected.Add(k);
            selected.Sort();
        }
        DetailSetActive(false);
        RefreshSelectedView();
    }

    // SkillSlot이 클릭되면 호출
    public void OnSlotClicked(int skillId) {
        // TODO: focusedSkillId 갱신, 슬롯 포커스 표시, ShowDetail
        focusedSkillId = skillId;
        ShowDetail(skillId);
    }

    // TODO: 상세 패널 내용, 선택/해제 버튼 활성 상태 갱신
    void ShowDetail(int skillId) {
        Debug.Log(selected);
        DetailSetActive(true);
        var skillInfo = skillTable.Get(skillId);
        detailIcon.sprite = skillInfo.icon;
        detailDescription.text = $"<{skillInfo.displayName}>\n{skillInfo.description}";
        selectButton.gameObject.SetActive(false);
        releaseButton.gameObject.SetActive(true);
        foreach(int k in selected)
        {
            if(k==skillId) return;
        }
        selectButton.gameObject.SetActive(true);
        releaseButton.gameObject.SetActive(false);
    }

    // 선택 버튼 OnClick
    public void OnSelectButton()
    {
        // TODO: 3개 미만이고 미선택이면 추가 → 갱신
        if (selected.Count < MaxSelect)
        {
            foreach(int k in selected)
            {
                if(k == focusedSkillId)
                {
                    Debug.Log("이미 선택된 스킬");
                    return;
                }
            }
            selected.Add(focusedSkillId);
            selected.Sort();
            ShowDetail(focusedSkillId);
            RefreshSelectedView();
        }
        else
        {
            Debug.Log("스킬 갯수 초과");
        }
    }

    // 해제 버튼 OnClick
    public void OnReleaseButton()
    {
        foreach(int k in selected){
            if(k == focusedSkillId)
            {
                selected.Remove(focusedSkillId);
                ShowDetail(focusedSkillId);
                RefreshSelectedView();
                return;
            }
        }
    }

    // TODO: 선택 3칸 아이콘, 슬롯 선택 표시, 버튼 상태 갱신
    void RefreshSelectedView() {
        for(int i = 0; i<selectedDetail.Length; i++)
        {
            selectedDetail[i].gameObject.SetActive(true);
            if(i>=selected.Count) selectedDetail[i].gameObject.SetActive(false);
            else
            {
                selectedDetail[i].SetSkillDetail(selected[i]);
            }
        }
    }

    void DetailSetActive(bool active)
    {
        detailIcon.gameObject.SetActive(active);
        detailDescription.gameObject.SetActive(active);
        selectButton.gameObject.SetActive(active);
        releaseButton.gameObject.SetActive(active);
    }

    // 게임 시작 버튼 OnClick
    public void OnStartButton()
    {
        GameSession.Instance.SetSelectedSkills(selected);
    }
}
