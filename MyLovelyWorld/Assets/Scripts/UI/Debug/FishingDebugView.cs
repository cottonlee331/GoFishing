using TMPro;
using UnityEngine;

// 1단계 루프 확인용. 상태, 남은 시간, 어종을 텍스트로 표시.
public class FishingDebugView : MonoBehaviour
{
    [SerializeField] FishingLoop fishingLoop;
    [SerializeField] TMP_Text label;

    void Update()
    {
        // TODO: fishingLoop.State, CurrentBite 표시
    }
}
