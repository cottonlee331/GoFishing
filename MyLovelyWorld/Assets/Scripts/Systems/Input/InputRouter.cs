using System;
using UnityEngine;

// S10. 키보드와 화면 버튼 입력을 같은 이벤트로 합친다.
// 입력을 받을지 말지(입질 중인지, 확정 직후 잠금인지)는 FishingLoop가 판단한다.
public class InputRouter : MonoBehaviour
{
    [SerializeField] FishingLoop loop;

    int lastInputFrame = -1;   // 같은 프레임 다중 입력 방지 (Time.frameCount 비교)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) EmitConfirm();

        if (Input.GetKeyDown(KeyCode.A)) EmitJudgeKey(JudgeKey.Power);
        if (Input.GetKeyDown(KeyCode.S)) EmitJudgeKey(JudgeKey.Steady);
        if (Input.GetKeyDown(KeyCode.D)) EmitJudgeKey(JudgeKey.Delicate);
    }

    // 화면 버튼 OnClick 연결용. 0=강력, 1=무난, 2=섬세 (JudgeKey 순서)
    public void QueueButton(int key) {
        EmitJudgeKey((JudgeKey)key);
    }

    // 화면 버튼 OnClick 연결용 (낚기)
    public void PressConfirm() {
        EmitConfirm();
    }

    // TODO: 이번 프레임에 이미 입력이 처리됐으면 무시, 아니면 기록 후 이벤트 발행
    void EmitJudgeKey(JudgeKey key)
    {
        if(lastInputFrame == Time.frameCount) return;
        loop.judgeQueue.Push(key);
        lastInputFrame = Time.frameCount;
    }

    void EmitConfirm()
    {
        loop.Judge();
    }
}
