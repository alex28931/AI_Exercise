using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIV_Metroid_Player {

    public class PlayerImpulse : PlayerAbilityBase {

        #region Mono
        private void OnEnable() {
            playerController.OnDamageTaken += OnDamageTaken;
        }

        private void OnDisable() {
            playerController.OnDamageTaken -= OnDamageTaken;
        }
        #endregion

        #region Override
        public override void OnInputDisabled() {
        }

        public override void OnInputEnabled() {
        }

        public override void StopAbility() {
        }
        #endregion

        #region Callbacks
        private void OnDamageTaken (DamageContainer container) {
            Vector3 impulse = container.DamageImpulse;
            if (container.ContactPoint.x > transform.position.x) {
                impulse.x *= -1;
            }
            StartCoroutine(DelaySetImpulse(impulse));
        }

        private void SetImpulse(Vector3 impulse) {
            playerController.SetImpulse(impulse);
        }
        #endregion

        #region Coroutine
        private IEnumerator DelaySetImpulse (Vector3 impulse) {
            yield return new WaitForEndOfFrame();
            SetImpulse(impulse);
        }
        #endregion
    }
}
