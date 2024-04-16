using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponentExample : MonoBehaviour
{

    [SerializeField]
    private EnemyParameters parameters;
    [SerializeField]
    private TextAsset testAsset;
    

    public float MaxHP {
        get { return parameters.HP; }
    }

    private float currentHP;

    void Start () {
        Init();
    }

    public void Init() {

    }

}
