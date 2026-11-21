using UnityEngine;

public class PCMovement : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.15f;   // 흔들리는 높이 (유닛)
    [SerializeField] private float frequency = 1.5f;    // 초당 흔들림 속도
    [SerializeField] private float phaseOffset = 0f;    // 시작 위상 (여러 개 쓸 때 어긋나게)

    private Vector3 startLocalPos;

    private void Awake()
    {
        startLocalPos = transform.localPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency + phaseOffset) * amplitude;
        transform.localPosition = startLocalPos + new Vector3(0f, offset, 0f);
    }
}