using System;
using UnityEngine;

// S6. 청상아리 조합 진행도 (기본 소+중+대, 스킬 8은 대+대+중, 같은 어종 불인정).
public class SharkSystem : MonoBehaviour
{
    [SerializeField] FishTableSO fishTable;
    public bool SharkReady { get; private set; }   // 다음 입질이 청상아리
    public int[] SharkGoal {get; private set;} = new int[3];    // 청상아리 조합 목표 (어종 id 3개)
    public bool[] SharkProgress {get; private set;} = new bool[3];   // 청상아리 조합 진행도
    public event Action<int> SharkProgressCaught; // 청상아리 조합 포획 성공

    void Awake()
    {
        SharkReady = false;
        InitSharkGoal(12);
    }

    public void UpdateSharkProgress(int fishId)
    {
        // 청상아리 조합 진행도 갱신
        for(int i = 0; i < SharkGoal.Length; i++){
            if(fishId == SharkGoal[i] && !SharkProgress[i]){
                SharkProgress[i] = true;
                SharkProgressCaught?.Invoke(i);
                for(int j = 0; j < SharkProgress.Length; j++){
                    if(!SharkProgress[j]){
                        SharkReady = false;
                        break;
                    }
                    SharkReady = true;
                }
            }
        }
    }

    void InitSharkGoal(int sharkCombo)
    {
        for(int i = 0; i<SharkGoal.Length; i++){
            SizeClass goalSize = (SizeClass)(sharkCombo%10);
            sharkCombo/=10;

            SharkGoal[i] = fishTable.GetBySize(goalSize)[UnityEngine.Random.Range(0, fishTable.GetBySize(goalSize).Length)].id;
            SharkProgress[i] = false;
        }
        Debug.Log($"SharkGoal: {SharkGoal[0]}, {SharkGoal[1]}, {SharkGoal[2]}");
        SharkReady = false;
    }

    public void SetSharkDone()
    {
        SharkReady = false;
    }

    public void InitSkillEffect(int value)
    {
        InitSharkGoal(value);
    }
}
