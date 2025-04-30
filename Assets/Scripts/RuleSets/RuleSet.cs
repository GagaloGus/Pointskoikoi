using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum RuleSet_Option { MaxRounds, StartPoints, KoiPointsNeeded }
public class RuleSet : MonoBehaviour
{
    public GameMode gameMode;
    [SerializeField] RuleSet_Option[] ruleSetOptions;

    [Header("Rule panels")]
    [SerializeField] GameObject MaxRounds_Panel;
    [SerializeField] GameObject StartPoints_Panel;
    [SerializeField] GameObject KoiPtsNeeded_Panel;

    private void OnEnable()
    {
        MaxRounds_Panel = Resources.Load<GameObject>("RuleSetPanels/MaxRounds");
        KoiPtsNeeded_Panel = Resources.Load<GameObject>("RuleSetPanels/KoiPointsNeeded");
        StartPoints_Panel = Resources.Load<GameObject>("RuleSetPanels/StartPoints");

        //Obtiene las reglas del modo de juego
        ruleSetOptions = Info.Get_RuleSets(gameMode);

        //Destuye todos los paneles por si acaso
        foreach(Transform t in transform)
            Destroy(t.gameObject);

        //Instancia los paneles respectivos
        foreach(RuleSet_Option rs in ruleSetOptions)
        {
            print($"Spawn {rs}");
            GameObject temp = null;
            switch (rs)
            {
                case RuleSet_Option.MaxRounds:
                    temp = MaxRounds_Panel;
                    break;
                case RuleSet_Option.StartPoints:
                    temp = StartPoints_Panel;
                    break;
                case RuleSet_Option.KoiPointsNeeded:
                    temp = KoiPtsNeeded_Panel;
                    break;
            }
            print($"Object {temp.name}");
            Transform panel = Instantiate(temp).transform;
            panel.SetParent(transform, false);
        }
    }
}