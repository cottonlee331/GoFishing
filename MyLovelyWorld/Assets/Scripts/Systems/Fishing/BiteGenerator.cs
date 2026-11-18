using System;
using System.Linq;

// 한 번의 입질 정보. 캐스팅 시점에 BiteGenerator가 만든다.
public struct BiteInfo
{
    public SizeClass size;
    public int fishId;           // 플레이어에게는 포획 전까지 숨김
    public float waitTime;       // 입질까지 대기 시간(초)
    //public float shadowScale;    // 그림자 크기 = shadow_ratio × (1 ± shadow_noise)
}

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
    public BiteInfo Generate() => Generate(1f,false);
    public BiteInfo Generate(float waitMultiplier,bool isShark){
        BiteInfo biteInfo = default;
        if(isShark){
            biteInfo.size = SizeClass.Shark;
            biteInfo.fishId = fishTable.GetBySize(SizeClass.Shark).First().id;
            biteInfo.waitTime = DrawWaitTime(biteInfo.size, waitMultiplier);
            return biteInfo;
        }
        biteInfo.size = DrawSize();
        biteInfo.fishId = DrawFish(biteInfo.size);
        biteInfo.waitTime = DrawWaitTime(biteInfo.size, waitMultiplier);
        //biteInfo.shadowScale = CalcShadowScale(biteInfo.size);
        return biteInfo;
    }

    // TODO: sizes.share 비율로 크기 추첨 (스킬 6 → 8 → 10 보정은 나중에)
    SizeClass DrawSize(){
        int total = 0;
        foreach (var row in sizeTable.rows) total += row.share;

        int random = rng.Next(total);
        foreach (var row in sizeTable.rows)
        {
            random -= row.share;
            if (random < 0)
            {
                return row.size;
            }
        }

        throw new InvalidOperationException("도달 불가: 가중치 선택 실패");
    }

    // TODO: 해당 크기 어종 중 weight 비율로 추첨
    int DrawFish(SizeClass size){
        FishTableSO.Row[] fishes = fishTable.GetBySize(size);
        int total = fishes.Sum(row => row.weight);
        int random = rng.Next(total);
        foreach (var fish in fishes)
        {
            random -= fish.weight;
            if (random < 0)
            {
                return fish.id;
            }
        }
        throw new InvalidOperationException("도달 불가: 어종 선택 실패");
    }

    // TODO: wait_min ~ wait_max 랜덤 × waitMultiplier (스킬, 피버 배율)
    float DrawWaitTime(SizeClass size, float waitMultiplier){
        SizeTableSO.Row row = sizeTable.Get(size);
        float waitTime = (float)(row.waitMin + rng.NextDouble() * (row.waitMax - row.waitMin));
        return waitTime * waitMultiplier;
    }

    // TODO: shadow_ratio × (1 ± shadow_noise)
    //float CalcShadowScale(SizeClass size) => throw new NotImplementedException();
}
