using UnityEngine;
using UnityEngine.SceneManagement;

class SimulationInitializer : MonoBehaviour
{
    public static SimulationInitializer Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadSimulationScene()
    {
        SceneManager.LoadScene("ElevatorSimulationScene", LoadSceneMode.Single);
        SceneManager.sceneLoaded += SimulationInitialization;
    }

    public void SimulationInitialization(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded!");

        PlayerController player = new PlayerController();
        ElevatorSimulation elevator = new ElevatorSimulation();
        elevator.ElevatorPosChanged += player.OnElevatorPositionChanged;
        elevator.ElevatorStopMoving += player.OnElevatorAchiveFloor;

        DoorManager doorMng = new DoorManager();
        player.PlayerPositionChanged += doorMng.OnPlayerPositionChanged;
        player.PlayerStateChanged += doorMng.OnPlayerStateChanged;
        elevator.ElematorLeaveFloor += doorMng.OnElevatorStartMoving;
        elevator.ElevatorStopMoving += doorMng.OnElevatorStopMoving;
        

        UIManager uiMng = UIManager.Instance;
        uiMng.Initialize();
        player.PlayerPositionChanged += uiMng.OnPlayerPositionChanged;
        player.PlayerStateChanged += uiMng.OnPlayerStateChanged;
        elevator.ElevatorPosChanged += uiMng.OnElevatorPositionChanged;
        doorMng.DoorIsOpen += uiMng.OnDoorStateChanged;

        ButtonManager btnMng = new ButtonManager();
        btnMng.InitializeControllers();
        btnMng.ConnectWithPresenter(uiMng);
        btnMng.ConnectWithElevator(elevator);
        btnMng.ConnectWithPlayer(player);

        player.ResetSimulationWithRandomValues();
        elevator.ResetSimulationWithRandomValues();

        RequestGenerator reqGenerator = new RequestGenerator();
        reqGenerator.Initialize();
        reqGenerator.SetElevatorSimulation(elevator);

        SimulationExecuterCtr executor = SimulationExecuterCtr.Instance;
        executor.AddToExecution(elevator);
        executor.AddToExecution(reqGenerator);
    }
}

