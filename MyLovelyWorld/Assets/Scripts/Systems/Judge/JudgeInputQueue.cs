using System;
using System.Diagnostics;

// 2단계. 판정 키 2칸 FIFO 큐. 입질 중에만 입력을 받는다.
public class JudgeInputQueue
{
    JudgeKey?[] queue = new JudgeKey?[2];
    public event Action Changed;   // UI 갱신용

    // TODO: 입질 시작 시 비우고 입력 허용
    public void Open() { Array.Clear(queue, 0, queue.Length); Changed?.Invoke(); }

    // TODO: 확정 또는 취소 시 입력 차단
    public void Close() { Array.Clear(queue, 0, queue.Length); Changed?.Invoke(); }

    // TODO: 가득 차 있으면 가장 오래된 입력을 밀어냄
    public void Push(JudgeKey key) {
        if(queue[0] == null) queue[0] = key;
        else if(queue[1] == null) queue[1] = key;
        else{
            queue[1] = queue[0];
            queue[0] = key;
        }
        Changed?.Invoke();
    }

    // TODO: 1번째(물살), 2번째(어종) 반환. 빈 칸은 null
    public (JudgeKey? current, JudgeKey? size) Peek() => (queue[0], queue[1]);
}
