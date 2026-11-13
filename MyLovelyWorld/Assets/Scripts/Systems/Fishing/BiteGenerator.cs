using System;

// S3. 입질 추첨 로직. MonoBehaviour가 아닌 일반 C# 클래스 (테스트 용이).
public class BiteGenerator
{
    readonly SizeTableSO sizeTable;
    readonly FishTableSO fishTable;
    readonly GameConfigSO config;
    readonly Random rng;

    public BiteGenerator(SizeTableSO sizeTable, FishTableSO fishTable, GameConfigSO config, int? seed = null)
    {
        this.sizeTable = sizeTable;
        this.fishTable = fishTable;
        this.config = config;
        rng = seed.HasValue ? new Random(seed.Value) : new Random();
    }

    // TODO: 아래 단계를 순서대로 호출해 BiteInfo 하나를 만든다.
    public BiteInfo Generate(float waitMultiplier) => throw new NotImplementedException();

    // TODO: sizes.share 비율로 크기 추첨 (스킬 6 → 8 → 10 보정은 나중에)
    SizeClass DrawSize() => throw new NotImplementedException();

    // TODO: 해당 크기 어종 중 weight 비율로 추첨
    int DrawFish(SizeClass size) => throw new NotImplementedException();

    // TODO: wait_min ~ wait_max 랜덤 × waitMultiplier (스킬, 피버 배율)
    float DrawWaitTime(SizeClass size, float waitMultiplier) => throw new NotImplementedException();

    // TODO: shadow_ratio × (1 ± shadow_noise)
    float CalcShadowScale(SizeClass size) => throw new NotImplementedException();
}
