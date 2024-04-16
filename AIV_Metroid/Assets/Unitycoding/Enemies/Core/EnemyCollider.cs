using System;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{

    [SerializeField]
    private bool attackOnStay;

    public Action<IDamageble, Vector2> PlayerHitted;
    public Action<IDamageble, Vector2> DamagableHitted;

    protected Collider2D myCollider;
    public Collider2D Collider {
        get {
            if (myCollider == null) {
                myCollider = GetComponent<Collider2D>();
            }
            return myCollider;
        }
    }


    protected void OnTriggerEnter2D(Collider2D other) {
        InternalTrigger(other);
    }

    protected void OnTriggerStay2D(Collider2D other) {
        if (!attackOnStay) return;
        InternalTrigger(other);
    }

    protected void InternalTrigger(Collider2D other) {
        IDamageble damageble = other.GetComponent<IDamageble>();
        if (damageble == null) return;
        if (other.gameObject == gameObject) return;
        Vector2 hitPosition = other.ClosestPoint(transform.position);
        if (other.CompareTag("Player")) {
            PlayerHitted?.Invoke(damageble, hitPosition);
        } else {
            DamagableHitted?.Invoke(damageble, hitPosition);
        }
    }

}
