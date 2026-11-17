using System.Collections.Generic;
using UnityEngine;

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

    void Awake()
    {
        // TODO: Instance가 이미 있고 자기 자신이 아니면 Destroy(gameObject) 후 return
        // TODO: Instance = this; DontDestroyOnLoad(gameObject);
    }

    // TODO: Enter Play Mode에서 도메인 리로드를 끈 경우 static Instance 초기화
    //       ([RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)])

    // TODO: SkillSet 씬에서 확정한 스킬 저장 (기존 목록 교체)
    public void SetSelectedSkills(IEnumerable<int> skillIds) { }

    // TODO: 라운드 종료 시 결과 저장
    public void SetLastResult(RoundResult result) { }

    // TODO: 다시하기. 라운드 결과만 비움 (스킬 유지 여부 확인 필요)
    public void ResetForRetry() { }

    // TODO: 타이틀로. 스킬과 결과 모두 비움
    public void ResetAll() { }
}
