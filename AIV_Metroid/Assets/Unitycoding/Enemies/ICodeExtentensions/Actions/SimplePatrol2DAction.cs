using UnityEngine;
using ICode;
using ICode.Actions;
using Tooltip = ICode.TooltipAttribute;

[Category("EnemyPatrol")]
[Tooltip("Set Enemy Patrol")]
public class SimplePatrol2DAction : StateAction
{
    [Tooltip("Enemy GameObject")]
    public FsmGameObject Owner;
    [Tooltip("Speed to set")]
    public FsmFloat PatrolSpeed;
    [Tooltip("Left Position")]
    public FsmGameObject LeftPosition;
    [Tooltip("Right Position")]
    public FsmGameObject RightPosition;

    private EnemyComponent enemyComponent;

    private Transform currentTransformToReach;

    public override void OnEnter()
    {
        enemyComponent = Owner.Value.GetComponent<EnemyComponent>();
        if (enemyComponent == null)
        {
            Finish();
            return;
        }
        currentTransformToReach = Owner.Value.transform.right.x > 0 ?
            RightPosition.Value.transform : LeftPosition.Value.transform;
        InternalSetVelocity();
    }

    public override void OnUpdate()
    {
        InternalSetVelocity();
        Vector3 positionToReachLocal = Owner.Value.transform.
            InverseTransformPoint(currentTransformToReach.position);
        if (positionToReachLocal.x < 0)
        {
            Switch();
        }
    }

    private void Switch()
    {
        currentTransformToReach = currentTransformToReach == LeftPosition.Value.transform ? RightPosition.Value.transform : LeftPosition.Value.transform;
        enemyComponent.ReverseInputDirection();
    }


    private void InternalSetVelocity()
    {
        Vector2 direction = currentTransformToReach == RightPosition.Value.transform ? Vector2.right : Vector2.left;
        enemyComponent.SetMovementSpeed(PatrolSpeed.Value);
        enemyComponent.SetInputDirection(direction);
    }
}
