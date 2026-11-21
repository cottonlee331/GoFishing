using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 씬을 넘어 유지되는 데이터. 싱글톤 + DontDestroyOnLoad.
// 라운드 중에만 쓰는 데이터(점수, 큐 등)는 여기 두지 않는다.
public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    // 선택한 스킬 id (최대 3개, 0개 허용 여부 확인 필요)
    readonly List<int> selectedSkillIds = new();
    public IReadOnlyList<int> SelectedSkillIds => selectedSkillIds;

    // 직전 라운드 결과 (Play → Result 전달)
    public RoundResult LastResult { get; private set; }

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ResetAll();
    }

    // TODO: SkillSet 씬에서 확정한 스킬 저장 (기존 목록 교체)
    public void SetSelectedSkills(IEnumerable<int> skillIds) {
        selectedSkillIds.Clear();
        foreach(int k in skillIds) selectedSkillIds.Add(k);
        selectedSkillIds.Sort();
    }

    // TODO: 라운드 종료 시 결과 저장
    public void SetLastResult(RoundResult result)
    {
        LastResult = result;
        SceneManager.LoadScene("3_Result");
    }

    // TODO: 다시하기. 라운드 결과만 비움 (스킬 유지 여부 확인 필요)
    public void ResetForRetry()
    {
        LastResult = null;
    }

    // TODO: 타이틀로. 스킬과 결과 모두 비움
    public void ResetAll()
    {
        selectedSkillIds.Clear();
        LastResult = null;
    }
}
