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
    private UIManager uiManager;
    private CameraShake cameraShake;

    public static bool isFirstLoading = false;
	private void Awake()
	{
		instance = this;
		cameraShake = FindFirstObjectByType<CameraShake>();
		player = FindFirstObjectByType<PlayerController>();
		uiManager = FindFirstObjectByType<UIManager>();
        player.Init(this);

        enemyManager = GetComponentInChildren<EnemyManager>();
        enemyManager.Init(this);

        _playerResourceController = player.GetComponent<ResourceController>();
        _playerResourceController.RemoveHealthCangeEvent(uiManager.ChangePlayerHP);
        _playerResourceController.AddHealthChangeEvent(uiManager.ChangePlayerHP);
	}
    

    private void Start()
    {
        if (!isFirstLoading)
        {
            StartGame();
        }
        else
            isFirstLoading = true;
        
    }

    public void MainCameraShake()
    {
        cameraShake.ShakeCameara(1, 10, 10);
    }

    public void StartGame()
    {
        uiManager.SetPlayGame();
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWaveIndex += 1;
        enemyManager.StartWave(1 + currentWaveIndex / 5);
        uiManager.ChangeWave(currentWaveIndex); 
    }

    public void EndOfWave()
    {
        StartNextWave();
    }

    public void GameOver()
    {
        enemyManager.StopWave();
        uiManager.SetGameOver();
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
