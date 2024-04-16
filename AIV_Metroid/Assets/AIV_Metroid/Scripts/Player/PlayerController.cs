using System;
using System.Collections;
using UnityEngine;

namespace AIV_Metroid_Player {
    public class PlayerController : MonoBehaviour {

        private const string horizontalAnimatorParameter = "HorizontalVelocity";
        private const string verticalAnimatorParameter = "VerticalVelocity";
        private const string hitAnimatorParameter = "Hit";
        private const string deathTriggerAnimatorParameter = "Death";
        private const string isDeadAnimatorParameter = "PlayerDead";

        #region References
        [SerializeField]
        protected Transform playerTransform;
        [SerializeField]
        protected Rigidbody2D playerRigidbody;
        [SerializeField]
        protected Collider2D playerPhysicsCollider;
        [SerializeField]
        protected Transform cameraPositionTransform;
        [SerializeField]
        protected PlayerVisual playerVisual;
        #endregion //References

        #region PrivateAttributes
        private float gravityScale;
        private PlayerAbilityBase[] abilities;
        private Coroutine freezingCoroutine;
        #endregion

        #region ReferenceGetter
        public Transform PlayerTransform {
            get { return playerTransform; }
        }
        public Transform CameraPositionTransform {
            get { return cameraPositionTransform; }
        }
        public Collider2D PlayerPhysicsCollider {
            get { return playerPhysicsCollider;  }
        }
        #endregion

        #region PlayerCollision
        public bool IsGrounded { get; set; }
        public Collider2D LastGroundCollider {
            get;
            set;
        }
        public Action OnGroundLanded;
        public Action OnGroundReleased;
        #endregion

        #region PlayerMovement
        public Vector2 ComputedDirection {
            get;
            set;
        }
        public Action OnWalkStarted;
        public Action OnWalkEnded;
        public Action<float> OnDirectionChanged;
        #endregion

        #region PlayerJump
        public bool IsJumping {
            get;
            set;
        }
        public Action<int> JumpStarted;
        #endregion

        #region PlayerDash
        public bool IsDashing {
            get;
            set;
        }
        public Action OnDashStarted;
        public Action OnDashEnded;
        #endregion

        #region HealthModule
        private bool isDead;
        public Action<DamageContainer> OnDamageTaken;
        public Action OnDeath;
        public bool IsDead {
            get {
                return isDead;
            }
            set {
                isDead = value;
                playerVisual.SetAnimatorParameter(isDeadAnimatorParameter, value);
            }
        }
        #endregion

        #region MonoBehaviour
        private void Awake() {
            abilities = GetComponentsInChildren<PlayerAbilityBase>();
            foreach (var ability in abilities) {
                ability.Init(this, playerVisual);
                //To check saved data if ability is unlocked
                ability.enabled = true;
            }
            gravityScale = playerRigidbody.gravityScale;
            OnDamageTaken += InternalDamageTaken;
            OnDeath += InternalOnDeath;
#if DEBUG
            DebugEvents();
#endif
        }

        private void FixedUpdate() {
            playerVisual.SetAnimatorParameter(horizontalAnimatorParameter,
                Mathf.Abs(playerRigidbody.velocity.x));
            playerVisual.SetAnimatorParameter(verticalAnimatorParameter,
                playerRigidbody.velocity.y);
        }
        #endregion

        #region RigidbodyMethods 
        public Vector2 GetVelocity () {
            return playerRigidbody.velocity;
        }

        public void SetVelocity (Vector2 velocity) {
            playerRigidbody.velocity = velocity;
        }

        public void RemoveGravity () {
            playerRigidbody.gravityScale = 0;
        }

        public void RestoreGravity () {
            playerRigidbody.gravityScale = gravityScale;
        }

        public void SetImpulse (Vector3 impulse) {
            SetVelocity(Vector2.zero);
            playerRigidbody.AddForce(impulse, ForceMode2D.Impulse);
        }
        #endregion

        #region PrivateMethods
        private void DisableInput () {
            foreach( var ability in abilities) {
                ability.OnInputDisabled();
            }
        }

        private void EnableInput () {
            foreach (var ability in abilities) {
                ability.OnInputEnabled();
            }
        }

        private void InternalDamageTaken (DamageContainer damage) {
            freezingCoroutine = StartCoroutine(Freezing(damage.FreezeTime));
            playerVisual.SetAnimatorParameter(hitAnimatorParameter);
        }

        private void InternalOnDeath () {
            DisableInput();
            StopCoroutine(freezingCoroutine);
            playerVisual.SetAnimatorParameter(deathTriggerAnimatorParameter);
        }
        #endregion

        #region Coroutine
        private IEnumerator Freezing (float freezeTime) {
            DisableInput();
            yield return new WaitForSeconds(freezeTime);
            EnableInput();
        }
        #endregion

#if DEBUG
        #region DebugMethods
        private void DebugEvents () {
            OnGroundLanded += () => { Debug.Log("OnGroundLanded"); };
            OnGroundReleased += () => { Debug.Log("OnGroundReleased"); };
            OnDirectionChanged += (rotation) => { Debug.Log("OnDirectionChanged with rotation:" + rotation); };
            OnWalkStarted += () => { Debug.Log("OnWalkStarted"); };
            OnWalkEnded += () => { Debug.Log("OnWalkEnded"); };
            JumpStarted += (number) => { Debug.Log("OnJumpStarted " + number); };
            OnDashStarted += () => { Debug.Log("OnDashStarted"); };
            OnDashEnded += () => { Debug.Log("OnDashEnded"); };
            OnDamageTaken += (DamageContainer dc) => { Debug.Log("OnDamageTaken with damage: " + dc.Damage); };
            OnDeath += () => { Debug.Log("OnDeath"); };
        }
        #endregion
#endif
    }
}
