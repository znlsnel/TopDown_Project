
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

	[SerializeField] private List<GameObject> enemyPrefabs;
	[SerializeField] private List<Rect> spawnAreas;
	[SerializeField] private Color gizmoColor = new Color(1, 0, 0, 0.3f);
	
	[SerializeField] private float timeBetweenSpawns = 0.2f;
	[SerializeField] private float timeBetweenWaves = 1f;

	private Coroutine waveRoutine;
	private Dictionary<string ,GameObject> enemyPrefabsDict = new Dictionary<string ,GameObject>();
	private List<EnemyController> activeEnemies = new List<EnemyController>();
	private bool enemySpawnComplite;


	GameManager gameManager;
	public void Init(GameManager gameManager)
	{
		this.gameManager = gameManager;
		enemyPrefabsDict = new Dictionary<string, GameObject>();
		foreach (GameObject prefab in enemyPrefabs)
		{
			enemyPrefabsDict[prefab.name] = prefab;
		}
	}

	public void StartWave(int waveCount)
	{
		if (waveCount <= 0)
		{
			gameManager.EndOfWave();
			return;
		}

		if (waveRoutine != null)
			StopCoroutine(waveRoutine);
		waveRoutine = StartCoroutine(SpawnWave(waveCount));
	}

	public void StopWave()
	{
		StopAllCoroutines();
	}

	[SerializeField] private List<GameObject> itemPrefabs;

	private void SpawnRandomEnemy(string prefabName = null)
	{
		if (enemyPrefabs.Count == 0 || spawnAreas.Count == 0)
		{
			Debug.LogWarning("Enemy Prefabs 또는 Spawn Areas가 설정되지 않았습니다.");
			return;
		}

		GameObject randomPrefab;
        if (prefabName == null)
        {
            randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        }
        else
        {
            randomPrefab = enemyPrefabsDict[prefabName];
        }


        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

		Vector2 randomPosition = new Vector2(
				Random.Range(randomArea.xMin, randomArea.xMax),
				Random.Range(randomArea.yMin, randomArea.yMax)
			);

		GameObject spawnedEnemy = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y), Quaternion.identity);
		EnemyController enemyController = spawnedEnemy.GetComponent<EnemyController>();
		enemyController.Init(this, gameManager.player.transform);

		activeEnemies.Add(enemyController);
	}

	public void RemoveEnemyOnDeath(EnemyController enemy)
	{
		activeEnemies.Remove(enemy);
		if (enemySpawnComplite && activeEnemies.Count == 0)
			gameManager.EndOfWave();

		CreateRandomItem(enemy.transform.position);
	}

	public void CreateRandomItem(Vector3 position)
	{
		GameObject item = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Capacity)], position, Quaternion.identity); 
	}

	private void OnDrawGizmosSelected()
	{
		if (spawnAreas == null) return;

		Gizmos.color = gizmoColor;
		foreach (var area in spawnAreas)
		{
			Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
			Vector3 size = new Vector3(area.width, area.height);
			Gizmos.DrawCube(center, size);
		}
	} 

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			StartWave(1);
	}

    public void StartStage(StageInstance stageInstance)
    {
        if (waveRoutine != null)
            StopCoroutine(waveRoutine);

        waveRoutine = StartCoroutine(SpawnStart(stageInstance));
    }

	private IEnumerator SpawnStart(StageInstance stageInstance)
	{
		enemySpawnComplite = false;
		yield return new WaitForSeconds(timeBetweenWaves);

		StageInfo stageInfo = stageInstance.currentStageInfo;
		WaveData waveData = stageInfo.waves[stageInstance.currentWave];

		for (int i = 0; i < waveData.monsters.Length; i++)
		{
			yield return new WaitForSeconds(timeBetweenSpawns);

			MonsterSpawnData monsterSpawnData = waveData.monsters[i];
			for (int j = 0; j < monsterSpawnData.spawnCount; j++)
			{
				SpawnRandomEnemy(monsterSpawnData.monsterType);
			}
		}

		if (waveData.hasBoss)
		{
			yield return new WaitForSeconds(timeBetweenWaves);

			gameManager.MainCameraShake();
			SpawnRandomEnemy(waveData.bossType);	
		}
		
		enemySpawnComplite = true; 
	}


	private IEnumerator SpawnWave(int waveCount)
	{
		enemySpawnComplite = false;
		yield return new WaitForSeconds(timeBetweenWaves);
		for (int i = 0; i < waveCount; i++)
		{
			yield return new WaitForSeconds(timeBetweenSpawns);
			SpawnRandomEnemy();
		}

		enemySpawnComplite = true;
	}
}

