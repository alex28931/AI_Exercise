using UnityEngine;

public class GenericDamager : MonoBehaviour, IDamager
{

    [SerializeField]
    private DamageContainer damage;
    [SerializeField]
    private string damagebleTag;

    private void OnTriggerStay2D (Collider2D other) {
        if (!other.CompareTag(damagebleTag)) return;
        IDamageble damageble = other.GetComponent<IDamageble>();
        if (damageble == null) return;
        damage.SetContactPoint(other.ClosestPoint(transform.position));
        damageble.TakeDamage(damage);
    }

}
