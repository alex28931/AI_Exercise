using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AIV_Metroid_Player {
    public class PlayerDash : PlayerAbilityBase {

        private const string isDashingAnimatorString = "IsDashing";
        private const string dashAnimatorString = "Dash";

        [SerializeField]
        protected float speed;
        [SerializeField]
        protected float totalSeconds;
        [SerializeField]
        protected int maxAeralDash;

        protected int currentAeralDash;
        protected Coroutine dashCoroutine;

        #region Mono
        private void OnEnable() {
            playerController.OnGroundLanded += OnGroundLanded;
            InputManager.Player.Dash.performed += OnInputPerformed;
        }


        private void OnDisable() {
            playerController.OnGroundLanded -= OnGroundLanded;
            InputManager.Player.Dash.performed -= OnInputPerformed;
        }
        #endregion

        #region Override
        public override void OnInputDisabled() {
            isPrevented = true;
            StopAbility();
        }

        public override void OnInputEnabled() {
            isPrevented = false;
        }

        public override void StopAbility() {
            if (!playerController.IsDashing) return;
            StopCoroutine(dashCoroutine);
            playerController.IsDashing = false;
            playerController.SetVelocity(Vector2.zero);
            playerController.RestoreGravity();
            SetAnimationParameter(false);
        }
        #endregion

        #region Internal
        private bool CanDash () {
            return !isPrevented &&
                !playerController.IsDashing &&
                (playerController.IsGrounded || currentAeralDash < maxAeralDash);
        }

        private void StopDashWithCallback () {
            StopAbility();
            playerController.OnDashEnded?.Invoke();
        }
        #endregion

        public void StartDash () {
            dashCoroutine = StartCoroutine(DashCoroutine());
        }

        #region Callbacks
        private void OnInputPerformed(InputAction.CallbackContext input) {
            if (!input.performed) return;
            if (!CanDash()) return;
            StartDash();
        }

        private void OnGroundLanded() {
            currentAeralDash = 0;
        }
        #endregion

        #region Visual
        private void SetAnimationParameter (bool started) {
            if (started) {
                playerVisual.SetAnimatorParameter(dashAnimatorString);
            }
            playerVisual.SetAnimatorParameter(isDashingAnimatorString, started);
        }
        #endregion

        #region Coroutine 
        private IEnumerator DashCoroutine () {
            playerController.IsDashing = true;
            playerController.RemoveGravity();
            Vector2 direction = playerController.PlayerTransform.right;
            Vector2 dashVelocity = direction.normalized * speed;
            playerController.OnDashStarted?.Invoke();
            if (!playerController.IsGrounded) {
                currentAeralDash++;
            }
            SetAnimationParameter(true);
            yield return new WaitForEndOfFrame();
            playerController.SetVelocity(dashVelocity);
            yield return new WaitForSeconds(totalSeconds);
            StopDashWithCallback();
        }
        #endregion

    }
}
