// S2. 판정 모듈 인터페이스. 판정 방식이 바뀌어도 루프는 그대로 두기 위해 분리.
public interface IJudgeModule
{
    JudgeResult Judge(in JudgeContext context);
}
