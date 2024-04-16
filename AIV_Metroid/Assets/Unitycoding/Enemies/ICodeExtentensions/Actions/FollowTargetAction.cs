using UnityEngine;
using ICode;
using ICode.Actions;
using Tooltip = ICode.TooltipAttribute;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

[Category("EnemyFollow")]
[Tooltip("Set Enemy Follow")]
public class FollowTargetAction : StateAction
{
    [Tooltip("Enemy GameObject")]
    public FsmGameObject Owner;
    [Tooltip("Speed to set")]
    public FsmFloat FollowSpeed;
    [Tooltip("Target to set")]
    public FsmGameObject Target;
    private EnemyComponent enemyComponent;

    public override void OnUpdate()
    {
        enemyComponent = Owner.Value.GetComponent<EnemyComponent>();
        if (enemyComponent == null)
        {
            Finish();
            return;
        }
        if (Target.Value.transform.position.x > Owner.Value.transform.position.x)
        {
            InternalSetVelocity(Vector2.right);
        }
        else
        {
            InternalSetVelocity(Vector2.left);
        }
    }

    private void InternalSetVelocity(Vector2 direction)
    {
        enemyComponent.SetMovementSpeed(FollowSpeed.Value);
        enemyComponent.SetInputDirection(direction);
    }
}