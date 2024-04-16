using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Pippo", menuName = "Database/Enemy/EnemyParamters")]
public class EnemyParameters : ScriptableObject
{

    [SerializeField]
    private float hp;
    [SerializeField]
    private int enemyLevel;
    [SerializeField]
    private float attack;
    [SerializeField]
    private float exp;
    [SerializeField]
    private float money;

    public float HP {
        get { return hp; }
    }
    public int EnemyLevel {
        get { return enemyLevel; }
    }

    public float Attack {
        get { return attack; }
    }

    public float Exp {
        get { return exp; }
    }

    public float Money {
        get { return money; }
    }

}
