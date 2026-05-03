/*
    All server calls from the Client go through this class.
    The ServerApi isn't referenced anywhere else.

    Calls server-side methods located in Server/ServerApi.cs.
*/

using System.Threading.Tasks;
using UnityEngine;

public class ServerGateway
{
    private readonly ServerApi serverAPI;


    // Establishes server connection.
    public ServerGateway()
    {
        new ServerStartup().Initialize();  // This line wouldn't exist on the client side in reality; server would be up and running unrelated to the Client.
        serverAPI = new ServerApi();
    }

   
    // Fetches the full run configuration from the server.
    // Called once per run by MainMenuController after the player clicks the NewGame button.
    public async Task<RunConfigDTO> FetchRunConfig()
    {
        RunConfigDTO result = serverAPI.GetRunConfig();

        if (result == null)
            Debug.LogError("[ServerGateway] FetchRunConfig returned null.");

        return result;
    }

    
    // Asks the server which move the monster plays this turn.
    // The client applies the returned move — the server only decides it.
    // Called by BattleService after the player's action resolves.
    public async Task<MonsterMoveResponse> FetchMonsterMove(BattleStateSnapshot battleState)
    {
        var result = serverAPI.GetMonsterMove(battleState);

        if (result == null)
            Debug.LogError($"[ServerGateway] MonsterAIService returned null for '{battleState.MonsterName}'.");

        return result;
    }
}