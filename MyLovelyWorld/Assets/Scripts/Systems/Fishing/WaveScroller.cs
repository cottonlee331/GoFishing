using UnityEngine;

public class WaveScroller : MonoBehaviour
{
    [SerializeField] private Transform[] tiles;          // 3개
    [SerializeField] private SpriteRenderer sampleRenderer; // 너비 측정용 (tiles[0]의 SpriteRenderer)

    [Header("가로 흐름")]
    [SerializeField] private float speed = 1f;           // 초당 이동 거리 (유닛)
    [SerializeField] private bool moveLeft = true;

    [Header("상하 흔들림")]
    [SerializeField] private float bobAmplitude = 0.1f;  // 흔들리는 높이
    [SerializeField] private float bobFrequency = 1.5f;  // 초당 흔들림 속도
    [SerializeField] private float phaseOffset = 0.8f;   // 타일마다 위상 차이

    private float tileWidth;
    private float baseY;

    private void Start()
    {
        tileWidth = sampleRenderer.bounds.size.x;
        baseY = tiles[0].position.y;

        // 가로로 나란히 배치 (tiles[0] 위치를 시작점으로)
        float startX = tiles[0].position.x;
        for (int i = 0; i < tiles.Length; i++)
            tiles[i].position = new Vector3(startX + tileWidth * i, baseY, tiles[i].position.z);
    }

    private void Update()
    {
        float dir = moveLeft ? -1f : 1f;
        float moveX = speed * Time.deltaTime * dir;

        for (int i = 0; i < tiles.Length; i++)
        {
            var pos = tiles[i].position;
            pos.x += moveX;
            pos.y = baseY + Mathf.Sin(Time.time * bobFrequency + phaseOffset * i) * bobAmplitude;
            tiles[i].position = pos;
        }

        Wrap();
    }

    // 화면 밖으로 나간 타일을 반대쪽 끝으로 이동
    private void Wrap()
    {
        float totalWidth = tileWidth * tiles.Length;
        float rootX = transform.position.x;

        foreach (var t in tiles)
        {
            var pos = t.position;
            if (moveLeft && pos.x <= rootX - tileWidth)
                pos.x += totalWidth;
            else if (!moveLeft && pos.x >= rootX + totalWidth - tileWidth)
                pos.x -= totalWidth;
            t.position = pos;
        }
    }
}