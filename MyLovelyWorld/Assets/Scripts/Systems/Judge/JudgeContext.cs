// 판정 입력 (기획서 구현 입력 요약 §4).
public struct JudgeContext
{
    public JudgeKey? currentKey;      // 큐 1번째 = 물살 판단 (비어 있으면 null → 오답)
    public JudgeKey? sizeKey;         // 큐 2번째 = 어종 판단
    public CurrentLevel currentLevel; // 확정 시점의 물살
    public SizeClass sizeClass;
    public float difficulty;          // sizes.difficulty
    public float forgiveProb;         // 스킬 7 침착 (없으면 0)
}
