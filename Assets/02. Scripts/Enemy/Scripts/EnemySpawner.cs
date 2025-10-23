using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private EnemyData[] enemyDatas; // ������ �� ����
    [SerializeField] private Transform[] spawnPoints; // ���� ��ġ��
    [SerializeField] private float spawnInterval = 5f; // ���� ����
    [SerializeField] private int maxEnemies = 10; // ���ÿ� ���� ������ �ִ� ��

    [Header("���̺� ���� (����)")]
    [SerializeField] private bool useWave = false;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float waveDelay = 10f;

    private readonly List<GameObject> activeEnemies = new List<GameObject>();
    private bool spawning = false;

    void Start()
    {
        if (enemyDatas.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawner: EnemyData�� SpawnPoint�� �������� �ʾҽ��ϴ�.");
            return;
        }

        StartCoroutine(useWave ? SpawnWaveRoutine() : SpawnContinuousRoutine());
    }

    IEnumerator SpawnContinuousRoutine()
    {
        spawning = true;
        while (spawning)
        {
            yield return new WaitForSeconds(spawnInterval);
            CleanupDeadEnemies();
            if (activeEnemies.Count < maxEnemies)
                SpawnEnemy();
        }
    }

    IEnumerator SpawnWaveRoutine()
    {
        spawning = true;
        while (spawning)
        {
            CleanupDeadEnemies();

            int spawnCount = Mathf.Min(enemiesPerWave, maxEnemies - activeEnemies.Count);
            for (int i = 0; i < spawnCount; i++)
                SpawnEnemy();

            yield return new WaitForSeconds(waveDelay);
        }
    }

    void SpawnEnemy()
    {
        // ���� �����Ϳ� ��ġ ����
        EnemyData data = enemyDatas[Random.Range(0, enemyDatas.Length)];
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        if (data.prefab == null)
        {
            Debug.LogWarning($"EnemyData {data.name}�� prefab�� ����ֽ��ϴ�.");
            return;
        }

        GameObject enemy = Instantiate(data.prefab, point.position, point.rotation);
        if (enemy.TryGetComponent(out EnemyBase baseComp))
        {
            // ������ ���� ���� (prefab�� ���� �� ���)
            var field = typeof(EnemyBase).GetField("data", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(baseComp, data);
        }

        activeEnemies.Add(enemy);
    }

    void CleanupDeadEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null || !activeEnemies[i].activeInHierarchy)
                activeEnemies.RemoveAt(i);
        }
    }

    // ������ ����׿�
    private void OnDrawGizmos()
    {
        if (spawnPoints == null) return;
        Gizmos.color = Color.green;
        foreach (var p in spawnPoints)
        {
            if (p != null)
                Gizmos.DrawWireSphere(p.position, 0.5f);
        }
    }
}
