using System;

// 2단계. 판정 키 2칸 FIFO 큐. 입질 중에만 입력을 받는다.
public class JudgeInputQueue
{
    public event Action Changed;   // UI 갱신용

    // TODO: 입질 시작 시 비우고 입력 허용
    public void Open() { }

    // TODO: 확정 또는 취소 시 입력 차단
    public void Close() { }

    // TODO: 가득 차 있으면 가장 오래된 입력을 밀어냄
    public void Push(JudgeKey key) { }

    // TODO: 1번째(물살), 2번째(어종) 반환. 빈 칸은 null
    public (JudgeKey? current, JudgeKey? size) Peek() => throw new NotImplementedException();
}
