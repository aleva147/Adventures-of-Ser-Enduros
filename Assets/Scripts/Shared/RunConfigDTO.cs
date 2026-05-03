/*
    RunConfig didn't need to be a SO for the needs of the current project requirements,
    but it can be handy in the future when the game can have different runs.
*/

using System.Collections.Generic;


[System.Serializable]
public class RunConfigDTO {
    public HeroDTO Hero;
    public List<MonsterDTO> Monsters;
    public List<MoveDTO> AllMoves;
}