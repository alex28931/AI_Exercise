using UnityEngine;

public class TestEnemy : MonoBehaviour {

    [SerializeField]
    private float timeToWait;
    [SerializeField]
    private string idleTrigger;
    [SerializeField]
    private string moveTrigger;
    [SerializeField]
    private Vector2 moveSpeed;
    

    private State SetUpIdleState(Animator animator, Rigidbody2D rigidbody) {
        State idle = new State();
        AnimatorSetTriggerAction setIdle = new AnimatorSetTriggerAction(animator, idleTrigger);
        SetVelocity2DAction setVelocity = new SetVelocity2DAction(rigidbody, Vector2.zero, false);
        idle.SetUpMe(new StateAction[] { setIdle, setVelocity });
        return idle;
    }

    private State SetUpMoveState(Animator animator, Rigidbody2D rigidbody) {
        State move = new State();
        AnimatorSetTriggerAction setMove = new AnimatorSetTriggerAction(animator, moveTrigger);
        SetVelocity2DAction setVelocity = new SetVelocity2DAction(rigidbody, moveSpeed, true);
        move.SetUpMe(new StateAction[] { setMove, setVelocity });
        return move;
    }

    private Transition IdleToMove(State idle, State move, float timeToWait) {
        Transition transition = new Transition();
        Condition waitTime = new ExitTimeCondition(timeToWait);
        transition.SetUpMe(idle, move, new Condition[] { waitTime });
        return transition;
    }

    private Transition MoveToIdle(State idle, State move, float timeToWait) {
        Transition transition = new Transition();
        Condition waitTime = new ExitTimeCondition(timeToWait);
        transition.SetUpMe(move, idle, new Condition[] { waitTime });
        return transition;
    }

    private void Start() {
        Animator animator = GetComponent<Animator>();
        StateMachine machine = GetComponent<StateMachine>();
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        State idle = SetUpIdleState(animator, rigidbody);
        State move = SetUpMoveState(animator, rigidbody);
        Transition idleToMove = IdleToMove(idle, move, timeToWait);
        Transition moveToIdle = MoveToIdle(idle, move, timeToWait);
        idle.SetUpMe(new Transition[] { idleToMove });
        move.SetUpMe(new Transition[] { moveToIdle });
        machine.Init(new State[] { idle, move }, idle);
    }

}
