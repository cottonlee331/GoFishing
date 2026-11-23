using System;
using System.Collections.Generic;
using System.Linq;

// 한 번의 입질 정보. 캐스팅 시점에 BiteGenerator가 만든다.
public struct BiteInfo
{
    public SizeClass size;
    public int fishId;           // 플레이어에게는 포획 전까지 숨김
    public float waitTime;       // 입질까지 대기 시간(초)
}

// S3. 입질 추첨 로직. MonoBehaviour가 아닌 일반 C# 클래스 (테스트 용이).
public class BiteGenerator
{
    readonly SizeTableSO sizeTable;
    readonly FishTableSO fishTable;
    readonly Random rng;

    private int[] sizeShare;


    public BiteGenerator(SizeTableSO sizeTable, FishTableSO fishTable, int? seed = null)
    {
        this.sizeTable = sizeTable;
        this.fishTable = fishTable;
        sizeShare = new int[sizeTable.rows.Length];
        for(int i = 0; i<sizeTable.rows.Length; i++){
            sizeShare[i] = sizeTable.Get((SizeClass)i).share;
        }
        rng = seed.HasValue ? new Random(seed.Value) : new Random();
    }

    // TODO: 아래 단계를 순서대로 호출해 BiteInfo 하나를 만든다.
    public BiteInfo Generate() => Generate(1f,false);
    public BiteInfo Generate(float waitMultiplier, bool isShark){
                UnityEngine.Debug.Log($"ShareInfo: {sizeShare[0]}/{sizeShare[1]}/{sizeShare[2]}");
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
        foreach (var share in sizeShare) total += share;

        int random = rng.Next(total);
        for (int i = 0; i<sizeShare.Length; i++)
        {
            random -= sizeShare[i];
            if (random < 0)
            {
                return (SizeClass)i;
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

    public void SetSizeShareMult(SizeClass size, float mult){
        int id = (int)size;
        int tmp = sizeShare[id];
        sizeShare[id] = (int)(sizeShare[id]*mult);
        tmp = (sizeShare[id]-tmp)/(sizeShare.Length-2); // 출현률이 바뀌는 사이즈, 상어 제외
        for(int i = 0; i<sizeShare.Length; i++){
            if((SizeClass)i == SizeClass.Shark || i==id) continue;
            sizeShare[i]-=tmp;
            if(sizeShare[i]<=0)sizeShare[i] = 1;
        }

    }
    public void SetSizeShareMult(SizeClass size1, SizeClass size2, float mult){
        int id1 = (int)size1;
        int id2 = (int)size2;
        int tmp = sizeShare[id1]+sizeShare[id2];
        sizeShare[id1] = (int)(sizeShare[id1]*mult);
        sizeShare[id2] = (int)(sizeShare[id2]*mult);
        tmp = (sizeShare[id1]+sizeShare[id2]-tmp)/(sizeShare.Length-3);
        for(int i = 0; i<sizeShare.Length; i++){
            if((SizeClass)i == SizeClass.Shark || i==id1 || i==id2) continue;
            sizeShare[i]-=tmp;
            if(sizeShare[i]<=0)sizeShare[i] = 1;
        }
    }
}
