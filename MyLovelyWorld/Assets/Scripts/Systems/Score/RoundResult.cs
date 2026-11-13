using System.Collections.Generic;

// 라운드 결과. Result 씬으로 넘긴다.
public class RoundResult
{
    public int totalScore;
    public List<CatchRecord> catches = new List<CatchRecord>();
    public int sharkCount;
    public int[] selectedSkillIds;
}
