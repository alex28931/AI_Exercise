using UnityEngine;
using UnityEngine.InputSystem;

namespace AIV_Metroid_Player {

    public class PlayerMovement : PlayerAbilityBase {

        private const float turnSpeedOffset = 0.05f;

        #region SerializedField
        [SerializeField]
        protected float speedGrounded;
        [SerializeField]
        protected float speedNotGrounded;
        #endregion

        protected InputAction moveAction;
        protected float previousYRotation;
        protected bool wasWalking;

        #region Override
        public override void OnInputDisabled() {
            isPrevented = true;
            playerController.ComputedDirection = Vector2.zero;
            StopAbility();
        }

        public override void OnInputEnabled() {
            isPrevented = false;
        }

        public override void StopAbility() {
            SetSpeed(0);
        }
        #endregion

        #region Mono
        protected void OnEnable() {
            moveAction = InputManager.Player.Move;
            wasWalking = false;
            previousYRotation = playerController.PlayerTransform.localEulerAngles.y;
            playerController.OnDashStarted += OnDashStarted;
            playerController.OnDashEnded += OnDashEnded;
        }

        protected void OnDisable() {
            playerController.OnDashStarted -= OnDashStarted;
            playerController.OnDashEnded -= OnDashEnded;
        }

        protected void Update() {
            if (!CanMove()) return;
            FillDirectionFromInput();
            Move();
            Turn();
            HandleEvents();
        }
        #endregion

        #region InternalMethods
        protected void FillDirectionFromInput() {
            playerController.ComputedDirection = moveAction.ReadValue<Vector2>();
        }

        protected void Move() {
            float speed = playerController.IsGrounded ? speedGrounded : speedNotGrounded;
            speed *= playerController.ComputedDirection.x;
            SetSpeed(speed);
        }

        protected bool CanMove () {
            return !isPrevented;
        }

        protected void Turn () {
            Vector2 velocity = playerController.GetVelocity();

            Vector3 eulerRotation = playerController.PlayerTransform.localEulerAngles;
            if (velocity.x < -turnSpeedOffset) {
                eulerRotation.y = 180;
            } else if (velocity.x > turnSpeedOffset) {
                eulerRotation.y = 0;
            }
            playerController.PlayerTransform.localEulerAngles = eulerRotation;
        }

        protected void HandleEvents () {
            bool isWalking = Mathf.Abs(playerController.GetVelocity().x) > turnSpeedOffset;
            if (wasWalking && !isWalking) playerController.OnWalkEnded?.Invoke();
            if (!wasWalking && isWalking) playerController.OnWalkStarted?.Invoke();
            float yRotation = playerController.PlayerTransform.localEulerAngles.y;
            if (yRotation != previousYRotation) {
                playerController.OnDirectionChanged?.Invoke(yRotation);
            }
            wasWalking = isWalking;
            previousYRotation = yRotation;
        }

        protected void SetSpeed (float speed) {
            Vector2 velocity = playerController.GetVelocity();
            velocity.x = speed;
            playerController.SetVelocity(velocity);
        }
        #endregion

        #region Callbacks
        protected void OnDashStarted () {
            isPrevented = true;
        }
        protected void OnDashEnded () {
            isPrevented = false;
        }
        #endregion
    }

}
