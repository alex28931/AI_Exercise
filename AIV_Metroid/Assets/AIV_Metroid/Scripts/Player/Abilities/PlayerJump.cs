using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AIV_Metroid_Player {
    public class PlayerJump : PlayerAbilityBase {

        private const string isJumpingAnimatorString = "IsJumping";
        private const string startJumpAnimatorString = "Jump";

        #region SerializeField
        [Space]
        [Range(0.5f, 1.5f)]
        [SerializeField]
        protected float jumpTimeEvaluation;
        [SerializeField]
        protected float minVelocity;
        [SerializeField]
        protected float maxVelocity;
        [SerializeField]
        protected AnimationCurve accelerationCurve;
        [SerializeField]
        protected int consecutiveJump;
        [SerializeField]
        protected float consecutiveJumpDelay;
        #endregion

        protected int currentConsecutiveJump;
        protected Coroutine jumpCoroutine;



        #region Mono
        protected void OnEnable() {
            playerController.OnGroundLanded += OnGroundLanded;
            playerController.OnDashStarted += OnDashStarted;
            playerController.OnDashEnded += OnDashEnded;
            InputManager.Player.Jump.performed += OnInputPerform;
        }

        protected void OnDisable() {
            playerController.OnGroundLanded -= OnGroundLanded;
            playerController.OnDashStarted -= OnDashStarted;
            playerController.OnDashEnded -= OnDashEnded;
            InputManager.Player.Jump.performed -= OnInputPerform;
        }
        #endregion

        #region OverridedMethods
        public override void OnInputDisabled() {
            isPrevented = true;
            StopAbility();
        }

        public override void OnInputEnabled() {
            isPrevented = false;
        }

        public override void StopAbility() {
            if (jumpCoroutine != null) {
                StopCoroutine(jumpCoroutine);
            }
            playerController.IsJumping = false;
            StopJumpAnimation();
        }
        #endregion

        #region PublicMethods 
        public void StartJump() {
            if (jumpCoroutine != null) {
                StopCoroutine(jumpCoroutine);
            }
            jumpCoroutine = StartCoroutine(JumpCoroutine());
        }
        #endregion

        #region Callbacks
        protected void OnGroundLanded() {
            currentConsecutiveJump = 0;
        }

        protected void OnInputPerform(InputAction.CallbackContext input) {
            if (!input.performed) return;
            if (!CanJump()) return;
            StartJump();
        }

        protected void OnDashStarted() {
            isPrevented = true;
            StopAbility();
        }

        protected void OnDashEnded () {
            isPrevented = false;
        }
        #endregion

        #region InternalMethods
        protected bool CanJump () {
            return !isPrevented && currentConsecutiveJump < consecutiveJump;
        }
        #endregion

        #region VisualControl
        protected void StartJumpAnimation () {
            playerVisual.SetAnimatorParameter(startJumpAnimatorString);
            playerVisual.SetAnimatorParameter(isJumpingAnimatorString, true);
        }

        protected void StopJumpAnimation () {
            playerVisual.SetAnimatorParameter(isJumpingAnimatorString, false);
        }
        #endregion

        #region Coroutine
        protected IEnumerator JumpCoroutine () {
            currentConsecutiveJump++;
            StartJumpAnimation();
            if (currentConsecutiveJump > 1) {
                float delayingTime = 0;
                float verticalVelocity = playerController.GetVelocity().y;
                WaitForFixedUpdate waitFixedUpdate = new WaitForFixedUpdate();
                while (delayingTime <= consecutiveJumpDelay) {
                    playerController.SetVelocity(new Vector2(playerController.GetVelocity().x, verticalVelocity));
                    delayingTime += Time.fixedDeltaTime;
                    yield return waitFixedUpdate;
                }
            }
            playerController.IsJumping = true;
            playerController.JumpStarted?.Invoke(currentConsecutiveJump);
            if (!InputManager.Player_Jump_Pressed) {
                playerController.SetVelocity(new Vector2(playerController.GetVelocity().x, minVelocity));
                StopAbility();
                yield break; //a come il return di un metodo normale. Interrompe il flusso di esecuzione
            }
            float jumpTime = 0;
            while (jumpTime < jumpTimeEvaluation && 
                InputManager.Player_Jump_Pressed) {
                jumpTime += Time.fixedDeltaTime;
                float evaluatedY = Mathf.Lerp(minVelocity, maxVelocity,
                    accelerationCurve.Evaluate(jumpTime / jumpTimeEvaluation));
                playerController.SetVelocity(new Vector2(playerController.GetVelocity().x, evaluatedY));
                yield return new WaitForFixedUpdate();
            }
            StopAbility();
        }
        #endregion

    }
}
