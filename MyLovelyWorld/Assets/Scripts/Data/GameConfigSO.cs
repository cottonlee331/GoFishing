using UnityEngine;

// config.csv 대응. 지금은 인스펙터에서 직접 입력하고, CSV 변환기는 나중에 붙인다.
[CreateAssetMenu(menuName = "Fishing/Game Config", fileName = "GameConfig")]
public class GameConfigSO : ScriptableObject
{
    [Header("Round")]
    public float roundTimeSec = 150f;

    [Header("Judge")]
    public float judgeTimeSec = 4.0f;
    public float confirmLockSec = 0.3f;
    public float p0 = 0.30f;
    public float p2 = 0.90f;
    public float pMax = 1f;
    public float partialScore = 0.5f;

    [Header("Bite")]
    public float shadowNoise = 0.15f;
    public float resultTimeSec = 1.0f;   // TODO: Resolved 유지 시간으로 쓸지 확인 필요

    [Header("Current")]
    public float currentPeriodSec = 30f;
    //public float currentWarnSec = 0.5f;

    [Header("Fever")]
    public float gaugeMax = 100f;
    public float feverDurationSec = 20f;
    public float feverMult = 2.0f;
    public float feverWaitMult = 0.6f;

    [Header("Shark / Ending")]
    public int manseonThreshold = 600;
}
