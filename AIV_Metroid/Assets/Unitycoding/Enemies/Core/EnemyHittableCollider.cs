using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHittableCollider : EnemyCollider, IDamageble
{

    private EnemyComponent enemyComponent;

    private void Awake() {
        enemyComponent = GetComponentInParent<EnemyComponent>();
    }

    public void TakeDamage (DamageContainer container) {
        //enemyComponent e notifica il danno
    }

}
