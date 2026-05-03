/*
    This class acts as a server startup simulation.
    It initializes server-side data that should be the same for all clients and that exists unrelated to client connections.  
    
    In our case, this data is only a map that pairs MonsterNames to MonsterSOs 
    (used by MonsterMoveEndpoint.cs to access the right monster data based on the provided MonsterName).

    In our simulation, the client calls ServerStartup when a new game starts (concretely, from the ServerGateway.cs).
    But obviously, in reality, that call would be removed and the ServerStartup would already be executed by the Server itself before the game opens up.
*/

using UnityEngine;


public class ServerStartup
{
    public void Initialize()
    {
        MonsterSO[] allMonsters = Resources.LoadAll<MonsterSO>("Monsters");
        MonsterRegistry.Initialize(allMonsters);
    }
}