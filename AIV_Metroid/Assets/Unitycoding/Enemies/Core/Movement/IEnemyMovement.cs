using UnityEngine;

public enum EnemyMovementType {
    ground
}

public interface IEnemyMovement 
{

    void SetMovementSpeed(float movementSpeed);
    void SetInputDirection(Vector2 inputDirection);
    void ReverseInputDirection();
    void Jump();
    void StopMovement();
    void SetJumpForce(float jumpForce);
    void SetFaceDirection(bool value);
    void Hitted(Vector2 hitForce, Vector3 sourcePosition);
    void Die(Vector2 dieForce, Vector3 sourcePosition);
    void Teleport(Vector3 position);

    Vector2 InputDirection {
        get;
    }
    bool IsGrounded {
        get;
    }
    bool FaceDirection {
        get;
    }

}
