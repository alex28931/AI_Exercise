using UnityEngine;
using ICode;
using ICode.Actions;
using Tooltip = ICode.TooltipAttribute;

[Category("EnemyMovement")]
[Tooltip("Set the speed of the movement")]
public class SetMovementSpeedAction : StateAction
{

    [Tooltip("Enemy GameObject")]
    public FsmGameObject gameObject;
    [Tooltip("Speed to set")]
    public FsmFloat movementSpeed;
    [Tooltip("Set the speed at every frame?")]
    public FsmBool everyFrame;

    private EnemyComponent enemyComponent;

    public override void OnEnter() {
        enemyComponent = gameObject.Value.GetComponent<EnemyComponent>();
        if (enemyComponent == null) {
            Finish();
            return;
        }
        InternalSetMovementSpeed();
        if (!everyFrame.Value) Finish();
    }

    public override void OnUpdate() {
        InternalSetMovementSpeed();
    }

    private void InternalSetMovementSpeed () {
        enemyComponent.SetMovementSpeed(movementSpeed.Value);
    }

}
