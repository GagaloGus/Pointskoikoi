using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Info
{
    public static RuleSet_Option[] Get_RuleSets(GameMode gameMode)
    {
        RuleSet_Option[] rules = null;

        switch (gameMode)
        {
            case GameMode.PointThief:
                rules = new RuleSet_Option[] { 
                    RuleSet_Option.MaxRounds,
                    RuleSet_Option.StartPoints
                };
                break;
            case GameMode.Classic:
                rules = new RuleSet_Option[] { 
                    RuleSet_Option.MaxRounds,
                    RuleSet_Option.KoiPointsNeeded
                };
                break;
        }

        return rules;
    }
}
