/*
    This class acts as a simplified server simulation.
    
    In reality, client and server would exchange serialized data 
    and structural classes like RunConfigSO and BattleStateSnapshot would have to be mirrored on both ends.

    But for the sake of simplicity, in this simulation both the client and the server have access to Shared DTOs.
*/

public class ServerApi
{
    private readonly RunConfigEndpoint _runConfigEndpoint = new();
    private readonly MonsterMoveEndpoint _monsterMoveEndpoint = new();


    public RunConfigDTO GetRunConfig() => _runConfigEndpoint.HandleRequest();
    public MonsterMoveResponse GetMonsterMove(BattleStateSnapshot state) => _monsterMoveEndpoint.HandleRequest(state);
}