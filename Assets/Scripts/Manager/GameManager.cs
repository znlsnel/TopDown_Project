using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player { get; private set; }
    private ResourceController _playerResourceController;

    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private int currentStageIndex = 0;

    private EnemyManager enemyManager;
    private CameraShake cameraShake;
	private void Awake()
	{
		instance = this;
		cameraShake = FindFirstObjectByType<CameraShake>();
		player = FindFirstObjectByType<PlayerController>();
        player.Init(this);

        enemyManager = GetComponentInChildren<EnemyManager>();
        enemyManager.Init(this);
        MainCameraShake();
	}

    public void MainCameraShake()
    {
        cameraShake.ShakeCameara(1, 10, 10);
    }

    public void StartGame()
    {
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWaveIndex += 1;
        enemyManager.StartWave(1 + currentWaveIndex / 5);
    }

    public void EndOfWave()
    {
        StartNextWave();
    }

    public void GameOver()
    {
        enemyManager.StopWave();
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame(); 
        }
    }
    public void StartStage()
    {
        // StageInfo stageInfo = GetStageInfo(currentStageIndex);

        // if (stageInfo == null)
        // {
        //     Debug.Log("스테이지 정보가 없습니다.");
        //     return;
        // }

       // uiManager.ChangeWave(currentStageIndex + 1);

       // enemyManager.StartStage(stageInfo.waves[currentWaveIndex]);
    }

    // private StageInfo GetStageInfo(int stageKey)
    // {
    //     foreach (var stage in StageData.Stages)
    //     {
    //         //
    //     }
    // }

}
