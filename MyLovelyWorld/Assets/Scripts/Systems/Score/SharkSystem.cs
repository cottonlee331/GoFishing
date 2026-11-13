using System;
using UnityEngine;

// S6. 청상아리 조합 진행도 (기본 소+중+대, 스킬 8은 대+대+중, 같은 어종 불인정).
public class SharkSystem : MonoBehaviour
{
    public bool SharkReady { get; private set; }   // 다음 입질이 청상아리
    public event Action SharkProgressChanged;

    // TODO: 성공 포획 시 진행도 갱신, 조합 완성 시 SharkReady
    void OnCatchResolved(BiteInfo bite, JudgeResult result) { }
}
