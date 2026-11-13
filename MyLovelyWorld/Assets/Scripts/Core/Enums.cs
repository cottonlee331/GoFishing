// 게임 전반에서 공유하는 열거형.
public enum SizeClass { Small, Medium, Large }          // 소 / 중 / 대

public enum CurrentLevel { Strong, Normal, Weak }       // 물살: 강 / 보통 / 약

// 판정 키. 물살 판단일 때 강/보통/약, 어종 판단일 때 대/중/소로 해석한다.
public enum JudgeKey { Power, Steady, Delicate }        // 강력 / 무난 / 섬세

public enum CatchOutcome { Success, Fail, Timeout, Cancelled }

public enum FishingState { Casting, Waiting, Biting, Resolved }

public enum RoundState { Playing, Ending }
