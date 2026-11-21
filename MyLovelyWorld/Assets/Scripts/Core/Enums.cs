// 게임 전반에서 공유하는 열거형.
public enum SizeClass { Large, Medium, Small, Shark }          // 대 / 중 / 소 / 상어

public enum CurrentLevel { Strong, Normal, Weak }       // 물살: 강 / 보통 / 약

// 판정 키. 물살 판단일 때 강/보통/약, 어종 판단일 때 대/중/소로 해석한다.
public enum JudgeKey { Power, Steady, Delicate }        // 강력 / 무난 / 섬세

public enum CatchOutcome { Success, Fail, Lucky, Timeout, Cancelled }

public enum FishingState { Casting, Waiting, Biting, Resolved, Terminated }

public enum RoundState { Playing, Ending }

public enum EffectKey {
    None,
    LPointMult, MPointMult, SPointMult,
    FeverMultAdd, FeverGaugeMult, JudgeTimeAddSec, BiteWaitMult,
    LShareMult, MShareMult, SShareMult, MLShareMult,
    IgnoreWrongKey, SharkComboChange, 
    CurrentBonus, CurrentPeriodMult
}
