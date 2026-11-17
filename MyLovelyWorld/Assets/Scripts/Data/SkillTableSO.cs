using System;
using UnityEngine;

// skills.csv 대응.
[CreateAssetMenu(menuName = "Fishing/Skill Table", fileName = "SkillTable")]
public class SkillTableSO : ScriptableObject
{
    [Serializable]
    public class Row
    {
        public int id;
        public string displayName;
        public string category;          // TODO: Enums.cs에 SkillCategory 추가 후 교체 (분류 기준: 기획서 본문 표 vs skills.csv 확인 필요)
        [TextArea] public string description;
        public Sprite icon;
        public string effectKey;         // 예: large_points_mult, wait_mult, judge_time_add_sec
        public float effectValue;
        public string costKey;           // 부수 효과 키. 없으면 빈 문자열
        public float costValue;          // 스킬 8의 shark_combo(LLM)는 숫자가 아님. 처리 방식 결정 필요
    }

    public Row[] rows;

    // TODO: id로 Row 찾기
    public Row Get(int id) => throw new NotImplementedException();
}
