using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /* Coroutine 이란 시간 기반 작업이나 비동기적인 작업을 처리하기 위해 사용되는 메서드 실행을 중단하고 나중에 다시 실행을 재개할 수 있다.
      yield retunr 을 통해 특정 조건이나 시간을 기다렸다가 작업을 재개할 수 있다
    1. yiled return null
       다음 프레임까지 대기
    2. yiled return new WaitForSeconds(시간(초))
     시정된 시간 만큼 대기
    3. yield return new WaitForSecondsRealtime(시간)
        실제 시간을 기준으로 대기(Time.timeScale)의 영향을 받지 않는다.
    4. yield return new WaitUntil(조건)
        조건이 참이 될 때 까지 대기
    5. yield return new WaitWhile(조건)
        조건이 거짓이 될 때 까지 대기
    6. yield break
       코루틴 실행을 즉시 종료
    
     */
    private Coroutine waveRoutine;

    [SerializeField] private List<GameObject> enemyPrefabs; // 생성할 적 프리팹 리스트

    [SerializeField] List<Rect> spawnAreas; // 적을 생성할 영역 리스트

    [SerializeField] private Color gizmoColor = new Color(1, 0, 0, .3f); // 기즈모 색상

    private List<EnemyController> activeEnemies = new List<EnemyController>(); // 현재 활성화된 적들


    private bool enemySpawnComplite;

    [SerializeField] private float timeBetweenSpawn = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;

    public void StartWave(int waveCount)
    {
        if (waveRoutine != null)
        {
            StopCoroutine(waveRoutine);
        }
        waveRoutine = StartCoroutine(SpawnWave(waveCount));
    }

    public void StopWave()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        enemySpawnComplite = false;
        yield return new WaitForSeconds(timeBetweenWaves);

        for (int i = 0; i < waveCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawn);
            SpawnRandomEnemy();
        }

        enemySpawnComplite = true;
    }

    private void SpawnRandomEnemy()
    {
        if (enemyPrefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("Enemy Prefabs 또는 SpawnAreas가 설정되지 않았씁니다.");
            return;

        }
        // 랜덤한 적 프리팹 선택
        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // 랜덤한 영역 선택
        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        // Rect 영역 내부의 랜덤 위치 계산
        Vector2 randomPosition = new Vector2(
            Random.Range(randomArea.xMin, randomArea.xMax),
            Random.Range(randomArea.yMin, randomArea.yMax));

        GameObject spawnEnemy = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y), Quaternion.identity);
        EnemyController enemyController = spawnEnemy.GetComponent<EnemyController>();

        activeEnemies.Add(enemyController);

    }
    // 기즈모를 그려 영역을 시각화 (선택된 경우에만 표시)
    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        Gizmos.color = gizmoColor;
        foreach (var area in spawnAreas)
        {
            // 좌표와 크기 나눠주기
            Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, area.height);

            Gizmos.DrawCube(center, size);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartWave(1);
        }
    }


}
