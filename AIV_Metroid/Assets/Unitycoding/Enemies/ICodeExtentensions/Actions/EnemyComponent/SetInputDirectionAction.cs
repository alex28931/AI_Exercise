using UnityEngine;
using ICode;
using ICode.Actions;
using Tooltip = ICode.TooltipAttribute;

[Category("EnemyMovement")]
[Tooltip("Set the input direction of the movement")]
public class SetInputDirectionAction : StateAction
{

    [Tooltip("Enemy GameObject")]
    public FsmGameObject gameObject;
    [Tooltip("Speed to set")]
    public FsmVector2 inputDirection;
    [Tooltip("Set the speed at every frame?")]
    public FsmBool everyFrame;

    private EnemyComponent enemyComponent;

    public override void OnEnter() {
        enemyComponent = gameObject.Value.GetComponent<EnemyComponent>();
        if (enemyComponent == null) {
            Finish();
            return;
        }
        InternalSetInpiutDirection();
        if (!everyFrame.Value) Finish();
    }

    public override void OnUpdate() {
        InternalSetInpiutDirection();
    }

    private void InternalSetInpiutDirection() {
        enemyComponent.SetInputDirection(inputDirection.Value);
    }

}
