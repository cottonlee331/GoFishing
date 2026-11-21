using System;
using System.Collections.Generic;

// S7. 선택한 스킬들의 효과를 effect_key별로 집계한다.
// 같은 키를 바꾸는 효과는 곱셈(_mult), 더하는 타입은 덧셈(_add, _pp, _sec, _bonus)으로 겹친다.
public class SkillModifierSet
{
    readonly Dictionary<EffectKey, float> skillEffects = new();

    // TODO: 선택 스킬 id 목록과 테이블로 집계 (effect, cost 둘 다 반영)
    public SkillModifierSet(SkillTableSO table, IReadOnlyList<int> selectedSkillIds) { }

    // TODO: 키가 없으면 1 반환
    public float GetMult(string key) => throw new NotImplementedException();

    // TODO: 키가 없으면 0 반환
    public float GetAdd(string key) => throw new NotImplementedException();

    // TODO: 특정 스킬 장착 여부 (예: 스킬 8 청상아리 조합 변경)
    public bool Has(int skillId) => throw new NotImplementedException();
}
