using System;
using UnityEngine;
using UnityEngine.InputSystem;

// S10. 키보드와 화면 버튼 입력을 같은 이벤트로 합친다.
// 입력을 받을지 말지(입질 중인지, 확정 직후 잠금인지)는 FishingLoop가 판단한다.
public class InputRouter : MonoBehaviour
{
    public event Action<JudgeKey> JudgeKeyPressed;
    public event Action ConfirmPressed;

    int lastInputFrame = -1;   // 같은 프레임 다중 입력 방지 (Time.frameCount 비교)

    void Update()
    {
        // TODO: 키보드 판정 키 3개, 낚기 키 검사 (Keyboard.current)
        //       키 배치 확인 필요: A/S/D + Space(기획서) vs 1/2/3 + Enter(와이어프레임)
    }

    // 화면 버튼 OnClick 연결용. 0=강력, 1=무난, 2=섬세 (JudgeKey 순서)
    public void PressJudgeKey(int key)
    {
        // TODO: EmitJudgeKey 호출
    }

    // 화면 버튼 OnClick 연결용 (낚기)
    public void PressConfirm()
    {
        // TODO: EmitConfirm 호출
    }

    // TODO: 이번 프레임에 이미 입력이 처리됐으면 무시, 아니면 기록 후 이벤트 발행
    void EmitJudgeKey(JudgeKey key) { }

    void EmitConfirm() { }
}
