/*
    This class is used by BattleOutcomeController to update hero's EXP and potentially level hero up.
*/

public class LevelUpService
{
    public void AwardExp(HeroState hero, int expRewarded)
    {
        int newExpTotal = hero.Exp + expRewarded;
        int thresholdId = hero.Level - 1;

        // Level up
        if (newExpTotal >= hero.ExpThresholds[thresholdId])
        {
            // If the hero has already reached the maximum level, keep them stuck on max exp:
            if (hero.Level == hero.ExpThresholds.Count)
            {
                hero.Exp = hero.ExpThresholds[thresholdId];
                return;
            }

            hero.Level += 1;
            hero.Exp = newExpTotal - hero.ExpThresholds[thresholdId];

            CharacterStats statsGrowth = hero.StatGrowthPerLevel[thresholdId];
            hero.Stats.MaxHealth += statsGrowth.MaxHealth;
            hero.Stats.Attack += statsGrowth.Attack;
            hero.Stats.Defense += statsGrowth.Defense;
            hero.Stats.Magic += statsGrowth.Magic;
        }
        else
        {
            hero.Exp = newExpTotal;
        }
    }
}