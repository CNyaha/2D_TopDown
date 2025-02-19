using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /* Coroutine �̶� �ð� ��� �۾��̳� �񵿱����� �۾��� ó���ϱ� ���� ���Ǵ� �޼��� ������ �ߴ��ϰ� ���߿� �ٽ� ������ �簳�� �� �ִ�.
      yield retunr �� ���� Ư�� �����̳� �ð��� ��ٷȴٰ� �۾��� �簳�� �� �ִ�
    1. yiled return null
       ���� �����ӱ��� ���
    2. yiled return new WaitForSeconds(�ð�(��))
     ������ �ð� ��ŭ ���
    3. yield return new WaitForSecondsRealtime(�ð�)
        ���� �ð��� �������� ���(Time.timeScale)�� ������ ���� �ʴ´�.
    4. yield return new WaitUntil(����)
        ������ ���� �� �� ���� ���
    5. yield return new WaitWhile(����)
        ������ ������ �� �� ���� ���
    6. yield break
       �ڷ�ƾ ������ ��� ����
    
     */
    private Coroutine waveRoutine;

    [SerializeField] private List<GameObject> enemyPrefabs; // ������ �� ������ ����Ʈ

    [SerializeField] List<Rect> spawnAreas; // ���� ������ ���� ����Ʈ

    [SerializeField] private Color gizmoColor = new Color(1, 0, 0, .3f); // ����� ����

    private List<EnemyController> activeEnemies = new List<EnemyController>(); // ���� Ȱ��ȭ�� ����


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
            Debug.LogWarning("Enemy Prefabs �Ǵ� SpawnAreas�� �������� �ʾҾ��ϴ�.");
            return;

        }
        // ������ �� ������ ����
        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // ������ ���� ����
        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        // Rect ���� ������ ���� ��ġ ���
        Vector2 randomPosition = new Vector2(
            Random.Range(randomArea.xMin, randomArea.xMax),
            Random.Range(randomArea.yMin, randomArea.yMax));

        GameObject spawnEnemy = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y), Quaternion.identity);
        EnemyController enemyController = spawnEnemy.GetComponent<EnemyController>();

        activeEnemies.Add(enemyController);

    }
    // ����� �׷� ������ �ð�ȭ (���õ� ��쿡�� ǥ��)
    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        Gizmos.color = gizmoColor;
        foreach (var area in spawnAreas)
        {
            // ��ǥ�� ũ�� �����ֱ�
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
