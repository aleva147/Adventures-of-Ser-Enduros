/*
    Sent to the server as the body of GetMonsterMove. 
    Contains all necessary context for any MonsterAI to decide the monster's move. 
*/

using System.Collections.Generic;


[System.Serializable]
public class BattleStateSnapshot
{
    // Hero Info:
    public int HeroCurrentHp;
    public List<ActiveBuff> HeroActiveBuffIds;

    // Monster Info:
    public string MonsterName;
    public int MonsterCurrentHp;
    public List<ActiveBuff> MonsterActiveBuffIds;
    // public List<MoveDTO> MonsterMoves;  // Used the alternative way of sending only monster name and having monster registry on the server.
    // public string AiProfile;

    // General Info:
    public int TurnNumber;  // Some moves should be made only near the start/end of the fight. And some only every n-th turn.
}