/*
    The full outcome of performing a single move.
    
    Produced by BattleResolver and passed through BattleService.OnActionResolved 
    to BattleController, which forwards it to BattleView for animation.
*/

public class BattleActionResult
{
    public MoveDTO MoveUsed;
    public string ActorId;
    public int DamageDealt;
    public int HealingDone;
    public ActiveBuff BuffApplied;
    public bool TargetDefeated;
    public bool ActorIsHero => ActorId == "hero";
}