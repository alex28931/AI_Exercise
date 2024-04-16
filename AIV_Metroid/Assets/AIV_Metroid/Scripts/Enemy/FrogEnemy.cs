using AIV_Metroid_Player;
using UnityEngine;

public class FrogEnemy : MonoBehaviour
{

    [SerializeField]
    private float patrolSpeed;
    [SerializeField]
    private Transform leftPosition;
    [SerializeField]
    private Transform rightPosition;
    [SerializeField]
    private float distanceToFollowPlayer;
    [SerializeField]
    private float distanceToStopFollowPlayer;
    [SerializeField]
    private float distanceToStartAttack;
    [SerializeField]
    private float distanceToStopAttack;
    [SerializeField]
    private float huntSpeed;


    private State SetUpSimplePatrol() {
        SetVelocity2DAction setVelocity = new SetVelocity2DAction(GetComponent<Rigidbody2D>(),
            new Vector2(patrolSpeed, 0), false);
        Simple2DPatrol simplePatrol = new Simple2DPatrol(gameObject, leftPosition, rightPosition);
        AnimatorSetTriggerAction setTrigger = new AnimatorSetTriggerAction(GetComponent<Animator>(), "Run");
        State patrol = new State();
        patrol.SetUpMe(new StateAction[] { setVelocity, simplePatrol, setTrigger });
        return patrol;
    }

    private State SetUpFollowPlayer() {
        State follow = new State();
        SetVelocity2DAction setVelocity = new SetVelocity2DAction(GetComponent<Rigidbody2D>(),
            new Vector2(huntSpeed, 0), false);
        FollowTarget followTarget = new FollowTarget(gameObject, Player.Get().gameObject);
        AnimatorSetTriggerAction setTrigger = new AnimatorSetTriggerAction(GetComponent<Animator>(), "Run");
        follow.SetUpMe(new StateAction[] { setVelocity, followTarget, setTrigger });
        return follow;
    }

    private State SetUpAttackPlayer() {
        State attack = new State();
        SetVelocity2DAction stopAction = new SetVelocity2DAction(GetComponent<Rigidbody2D>(), Vector2.zero, false);
        AnimatorResetTriggerAction resetTrigger = new AnimatorResetTriggerAction(GetComponent<Animator>(), "Run");
        AnimatorSetTriggerAction setTrigger = new AnimatorSetTriggerAction(GetComponent<Animator>(), "Idle");
        attack.SetUpMe(new StateAction[] { stopAction, resetTrigger, setTrigger });
        return attack;
    }

    private Transition StartFollow(State prev, State next) {
        Transition transition = new Transition();
        CheckDistanceCondition distanceCondition = new CheckDistanceCondition(transform, Player.Get().transform, 
            distanceToFollowPlayer, COMPARISON.LESSEQUAL);
        transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
        return transition;
    }

    private Transition StartPatrol(State prev, State next) {
        Transition transition = new Transition();
        ExitTimeCondition exitTimeCondition = new ExitTimeCondition(2f);
        CheckDistanceCondition distanceCondition = new CheckDistanceCondition(transform, Player.Get().transform, 
            distanceToStopFollowPlayer, COMPARISON.GREATEREQUAL);
        transition.SetUpMe(prev, next, new Condition[] { distanceCondition, exitTimeCondition });
        return transition;
    }

    private Transition StartAttack(State prev, State next) {
        Transition transition = new Transition();
        CheckDistanceCondition distanceCondition = new CheckDistanceCondition(transform, Player.Get().transform, 
            distanceToStartAttack, COMPARISON.LESSEQUAL);
        transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
        return transition;
    }

    private Transition StartFollowFromAttack(State prev, State next) {
        Transition transition = new Transition();
        CheckDistanceCondition distanceCondition = new CheckDistanceCondition(transform, 
            Player.Get().transform, distanceToStopAttack, COMPARISON.GREATEREQUAL);
        transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
        return transition;

    }

    private void Start() {
        StateMachine stateMachine = GetComponent<StateMachine>();
        State patrol = SetUpSimplePatrol();
        State follow = SetUpFollowPlayer();
        State attack = SetUpAttackPlayer();
        patrol.SetUpMe(new Transition[] { StartFollow(patrol, follow) });
        follow.SetUpMe(new Transition[] { StartPatrol(follow, patrol), StartAttack(follow, attack) });
        attack.SetUpMe(new Transition[] { StartFollowFromAttack(attack, follow) });
        stateMachine.Init(new State[] { patrol, follow, attack }, patrol);
    }


}
