using UnityEngine;
using UnityEngine.UI;

// 스킬 그리드의 슬롯 하나. 아이콘 표시와 클릭 전달만 한다.
public class SkillSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] GameObject focusFrame;      // 현재 보고 있는 슬롯 (와이어프레임의 빨간 테두리)
    [SerializeField] GameObject selectedMark;    // 선택된 스킬 표시

    public int SkillId { get; private set; } = -1;

    SkillSelectController owner;

    // TODO: 스킬 정보와 소유 컨트롤러 저장, 아이콘 설정. row가 null이면 빈 슬롯 처리
    public void Setup(SkillTableSO.Row row, SkillSelectController owner) { }

    // TODO: focusFrame 표시
    public void SetFocused(bool focused) { }

    // TODO: selectedMark 표시
    public void SetSelected(bool isSelected) { }

    // 슬롯 Button OnClick
    public void OnClick()
    {
        // TODO: 빈 슬롯이 아니면 owner.OnSlotClicked(SkillId)
    }
}
