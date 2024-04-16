using UnityEngine;
using ICode;
using AIV_Metroid_Player;

public class EnemyComponent : MonoBehaviour, IDamager
{

    [SerializeField]
    private float maxHP;
    [SerializeField]
    private EnemyCollider[] meleeColliders;
    [SerializeField]
    private EnemyHittableCollider[] bodyColliders;
    [SerializeField]
    private EnemyMovementType movementType;
    [SerializeField]
    private float bodyDamage;
    [SerializeField]
    private float meleeDamage;
    [SerializeField]
    private DamageContainer damageContainer;

    private ICodeBehaviour codeBehaviour;
    private Animator animator;
    private IEnemyMovement movementComponent;

    private float hp;


    private void Awake() {
        codeBehaviour = GetComponent<ICodeBehaviour>();
        animator = GetComponent<Animator>();
        CreateEnemyMovement();
        foreach (EnemyCollider collider in meleeColliders) {
            collider.PlayerHitted += OnPlayerHittedMelee;
        }
        foreach(EnemyCollider collider in bodyColliders) {
            collider.PlayerHitted += OnPlayerHittedBody;
        }
        InitializeAI();
        ResetMe();
    }

    public void ResetMe () {
        hp = maxHP;
    }

    private void OnPlayerHittedMelee (IDamageble player, Vector2 contactPoint) {
        InternalSetPlayerDamage(player, contactPoint, meleeDamage);
    }

    private void OnPlayerHittedBody (IDamageble player, Vector2 contactPoint) {
        InternalSetPlayerDamage(player, contactPoint, bodyDamage);
    }

    private void InternalSetPlayerDamage (IDamageble player, Vector2 contactPoint, float damage) {
        damageContainer.Damage = damage;
        damageContainer.ContactPoint = contactPoint;
        player.TakeDamage(damageContainer);
    }
        
    #region IEnemyMovement
    private void CreateEnemyMovement () {
        switch (movementType) {
            case EnemyMovementType.ground:
                movementComponent = gameObject.AddComponent<GroundMovement>();
                break;
        }
    }

    public void SetMovementSpeed (float movementSpeed) {
        movementComponent.SetMovementSpeed(movementSpeed);
    }

    public void SetInputDirection (Vector2 inputDirection) {
        movementComponent.SetInputDirection(inputDirection);
    }

    public void ReverseInputDirection() {
        movementComponent.ReverseInputDirection();
    }

    public void Jump() {
        movementComponent.Jump();
    }

    public void StopMovement() {
        movementComponent.StopMovement();
    }

    public void SetJumpForce(float jumpForce) {
        movementComponent.SetJumpForce(jumpForce);
    }

    public void SetFaceDirection(bool value) {
        movementComponent.SetFaceDirection(true);
    }

    public void Hitted(Vector2 hitForce, Vector3 sourcePosition) {
        movementComponent.Hitted(hitForce, sourcePosition);
    }

    public void Die(Vector2 dieForce, Vector3 sourcePosition) {
        movementComponent.Die(dieForce, sourcePosition);
    }

    public void Teleport(Vector3 position) {
        movementComponent.Teleport(position);
    }

    public Vector2 InputDirection {
        get {
            return movementComponent.InputDirection;
        }
    }

    public bool IsGrounded {
        get {
            return movementComponent.IsGrounded;
        }
    }

    public bool FaceDirection {
        get {
            return movementComponent.FaceDirection;
        }
    }

    #endregion

    #region ICode
    [SerializeField]
    private StateMachine stateMachine;
    [SerializeField]
    private ExtendedVariable[] stateMachineVariables;

    private const string playerVariableString = "Player";
    private const string startPositionString = "StartPosition";

    private const string hitEventString = "Hit";
    private const string deadEventString = "Dead";

    private void InitializeAI () {
        codeBehaviour.stateMachine = stateMachine;
        codeBehaviour.stateMachine.SetVariable(playerVariableString, Player.Get().gameObject);
        codeBehaviour.stateMachine.SetVariable(startPositionString, transform.position);
        foreach (ExtendedVariable variable in stateMachineVariables) {
            codeBehaviour.stateMachine.SetVariable(variable.VariableName, variable.GetValue());
        }
        codeBehaviour.EnableStateMachine();
    }

    public void SendFSMCustomEvent (string eventName) {
        codeBehaviour.SendEvent(eventName, null);
    }

    private void SendHitFSMEvent () {
        SendFSMCustomEvent(hitEventString);
    }

    private void SendDeadFSMEvent () {
        SendFSMCustomEvent(deadEventString);
    }
    #endregion
}
