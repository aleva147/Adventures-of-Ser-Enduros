/*
    Singleton MonoBehaviour that survives all scene transitions.
        
    Owns every service whose state must outlive a single scene and that is its only purpose.
    Scene controllers always access these services strictly through the GameManager.Instance.
*/

using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton:
    public static GameManager Instance { get; private set; }

    // Services:
    public ServerGateway Gateway { get; private set; }
    public RunStateService RunState { get; private set; }
    public MoveInventoryService MoveInventory { get; private set; }
    public LevelUpService LevelUp { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        EstablishServerConnection();
    }


    // Instantiates classes used for sending requests to the server.
    private void EstablishServerConnection()
    {
        Gateway = new ServerGateway();
    }


    // Called by MainMenuController after loading RunConfig from the Server.
    public void Initialize(RunConfigDTO runConfig)
    {
        RunState      = new RunStateService(runConfig);
        MoveInventory = new MoveInventoryService();
        LevelUp       = new LevelUpService();
    }
}