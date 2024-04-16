using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FC_Param", menuName = "Database/Enemy/FlyingControllerParameters")]
public class FlyingControllerParameters : ScriptableObject
{

    [SerializeField]
    private string parameters1;
    [SerializeField]
    private string parameters2;
    [SerializeField]
    private string parameters3;
    [SerializeField]
    private string parameters4;
}
