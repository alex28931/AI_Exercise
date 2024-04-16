using UnityEngine;

public enum GroundEnemyControllerStatus {
    moving,
    hittedJump,
    dying,
    dead
}

public class GroundMovement : MonoBehaviour, IEnemyMovement {

    private float maxYSpeed = -20;
    public float MaxYSpeed {
        set { maxYSpeed = value; }
    }

    #region Private_Attributes
    protected Rigidbody2D myRigidbody;
    private BoxCollider2D myCollider;
    private float movementSpeed;
    private float jumpForce;
    private Vector2 inputDirection;
    public Vector2 InputDirection {
        get { return inputDirection; }
    }

    private Vector2 preHitVelocity;
    private GroundEnemyControllerStatus status;
    #endregion

    #region Properties
    public float MovementSpeed {
        get { return movementSpeed; }
        set {
            if (value < 0) {
                movementSpeed = 0;
                return;
            }
            movementSpeed = value;
        }
    }
    public float JumpForce {
        get { return jumpForce; }
        set {
            if (value < 0) {
                jumpForce = 0;
                return;
            }
            jumpForce = value;
        }
    }
    public bool FaceDirection {
        get;
        set;
    }
    public bool IsGrounded {
        get {
            float extraHeight = Physics2D.defaultContactOffset * 2;
            Vector2 colliderDownPosition = EnemyUtility.GetCornerPosition(AreaCorner.bottomMiddle, myCollider);
            //colliderDownPosition.x = transform.position.x;
            RaycastHit2D raycastHit = Physics2D.Raycast(colliderDownPosition, Vector2.down, extraHeight, EnemyUtility.groundMask);
#if DEBUG
            if (raycastHit.collider == null) {
                Debug.DrawLine(colliderDownPosition, (Vector2)colliderDownPosition + Vector2.down * extraHeight, Color.red);
            } else {
                Debug.DrawLine(colliderDownPosition, (Vector2)colliderDownPosition + Vector2.down * extraHeight, Color.green);
            }
#endif
            return raycastHit.collider != null && EnemyUtility.Approximately(myRigidbody.velocity.y, 0, 0.0001f);
        }
    }
    #endregion

    #region Unity_Game_Loop
    private void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponentInChildren<BoxCollider2D>();
        ResetMe();
    }

    private void FixedUpdate() {
        switch (status) {
            case GroundEnemyControllerStatus.hittedJump:
                HittedJumpStatus();
                break;
            case GroundEnemyControllerStatus.dying:
                Dying();
                break;
        }
        IncreaseFallSpeed();
    }

    protected virtual void IncreaseFallSpeed() {
        if (myRigidbody.velocity.y < -0.01f && !IsGrounded) {
            Vector3 velocity = myRigidbody.velocity;
            velocity.y *= 3;
            if (velocity.y < maxYSpeed) {
                velocity.y = maxYSpeed;
            }
            myRigidbody.velocity = velocity;
        }
    }

    private void HittedJumpStatus() {
        if (!IsGrounded) return;
        RestorePreHitVelocity();
    }

    private void Dying() {
        if (IsGrounded) {
            status = GroundEnemyControllerStatus.dead;
            StopMovement();
        }
    }
    #endregion

    #region PublicMethods
    public void Teleport(Vector3 position) {
        transform.position = position;
    }

    public void Jump() {
        if (!IsGrounded) return;
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
    }

    public void SetInputDirection(Vector2 inputDirection) {
        this.inputDirection = inputDirection.normalized;
        Vector2 performedVelocity = PerformedVelocity(this.inputDirection);
        switch (status) {
            case GroundEnemyControllerStatus.hittedJump:
                preHitVelocity = performedVelocity;
                break;
            case GroundEnemyControllerStatus.moving:
                myRigidbody.velocity = performedVelocity;
                if (FaceDirection) {
                    ChangeYRotation();
                }
                break;
            case GroundEnemyControllerStatus.dead:
                if (inputDirection == Vector2.zero) {
                    myRigidbody.velocity = performedVelocity;
                }
                break;
        }
    }

    protected virtual Vector2 PerformedVelocity(Vector2 direction) {
        return new Vector2(inputDirection.x * movementSpeed, myRigidbody.velocity.y);
    }

    public void ResetMe() {
        FaceDirection = true;
        status = GroundEnemyControllerStatus.moving;
    }

    public void ReverseInputDirection() {
        SetInputDirection(inputDirection * -1);
    }

    public void StopMovement() {
        SetInputDirection(Vector2.zero);
    }
    #endregion

    #region PrivateMethods
    private void ChangeYRotation() {
        if (inputDirection.x == 0 || movementSpeed == 0) return;
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.y = inputDirection.x < 0 ? 180 : 0;
        transform.eulerAngles = eulerRotation;
    }

    public void SetMovementSpeed(float movementSpeed) {
        MovementSpeed = movementSpeed;
        SetInputDirection(inputDirection);
    }

    public void SetJumpForce(float jumpForce) {
        JumpForce = jumpForce;
    }

    public void SetFaceDirection(bool value) {
        FaceDirection = value;
    }

    public void Hitted(Vector2 hitForce, Vector3 sourcePosition) {
        if (hitForce == Vector2.zero) return;
        preHitVelocity = myRigidbody.velocity;
        Vector2 forceDirection;
        forceDirection = sourcePosition.x < transform.position.x ? Vector2.right : Vector2.left;
        forceDirection += Vector2.up;
        myRigidbody.velocity = forceDirection * hitForce;
        status = GroundEnemyControllerStatus.hittedJump;
    }

    public void Die(Vector2 dieForce, Vector3 sourcePosition) {
        dieForce.x *= sourcePosition.x < transform.position.x ? 1 : -1;
        myRigidbody.velocity = dieForce;
        status = GroundEnemyControllerStatus.dying;
    }
    #endregion

    private void RestorePreHitVelocity() {
        status = GroundEnemyControllerStatus.moving;
        myRigidbody.velocity = preHitVelocity;
        if (FaceDirection) {
            ChangeYRotation();
        }
    }
}