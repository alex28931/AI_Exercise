using UnityEngine;

public class FollowTarget : StateAction
{

    private GameObject owner;
    private GameObject target;


    private Rigidbody2D rigidbody;

    public FollowTarget(GameObject owner, GameObject target) {
        this.owner = owner;
        this.target = target;
        rigidbody = owner.GetComponent<Rigidbody2D>();
    }

    public override void OnUpdate () {
        if (target.transform.position.x > owner.transform.position.x) {
            InternalSetVelocity(Vector2.right);
            owner.transform.eulerAngles = Vector3.zero;
        } else {
            InternalSetVelocity(Vector2.left);
            owner.transform.eulerAngles = Vector3.up * 180;
        }
    }

    private void InternalSetVelocity(Vector2 direction) {
        rigidbody.velocity = direction * Mathf.Abs(rigidbody.velocity.x);
    }

}
