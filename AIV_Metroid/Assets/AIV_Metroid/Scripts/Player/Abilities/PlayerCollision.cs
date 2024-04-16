using UnityEngine;

namespace AIV_Metroid_Player {
    public class PlayerCollision : PlayerAbilityBase {

        private const string groundAnimatorParameter = "Ground";

        #region InternalClasses
        public enum ColliderPointPosition {
            Center = 0,
            BottomCenter = 1,
            LeftCenter = 2,
            RightCenter = 3
        }

        [System.Serializable]
        protected class CollisionSphereData {

            [SerializeField]
            private ColliderPointPosition pointPosition;
            //TBD problema da risolvere
            [SerializeField]
            private float sphereRadius;
            [SerializeField]
            private Vector2 offset;

            [Space]
            [Header("Editor Only")]
            [SerializeField]
            private Color sphereColor;

            public ColliderPointPosition PointPosition {
                get { return pointPosition; }
            }
            public float SphereRadius {
                get { return sphereRadius; }
            }
            public Vector2 Offset {
                get { return offset; }
            }

            public Color SphereColor {
                get { return sphereColor; }
            }

        }

        #endregion //InternalClasses

        #region SerializedFields 
        [SerializeField]
        protected LayerMask groundLayer;
        [SerializeField]
        protected CollisionSphereData[] spheres;
        #endregion

        #region ProtectedProperties
        protected Collider2D LastGroundCollider {
            get { return playerController.LastGroundCollider; }
            set {
#if DEBUG && LEVEL1
                if (value == null || playerController.LastGroundCollider == value) return;
                Debug.Log("GroundCollider detected: " + value.gameObject.name);
#endif
                playerController.LastGroundCollider = value;
            }
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

        #region Mono
        protected void Update() {
            DetectGroundCollision();
            DetectWallCollision();
        }

        /// <summary>
        /// Will be executed in the editor. 
        /// </summary>
        protected void OnDrawGizmos() {
            if (playerController == null) {
                playerController = GameObject.FindObjectOfType<PlayerController>();
            }
            DrawSphere();
        }
        #endregion

        #region InternalMethods
        protected Vector2 GetCenterPoint (ColliderPointPosition position) {
            CollisionSphereData data = System.Array.Find(spheres, spheres => spheres.PointPosition == position);

            Vector2 positionOffset = data == null ? Vector2.zero : data.Offset;
            Vector2 collisionPoint = Vector2.zero;

            if (playerController.PlayerPhysicsCollider == null) return collisionPoint + positionOffset;
            Vector2 playerCenter = playerController.PlayerPhysicsCollider.bounds.center;
            Vector2 playerExtents = playerController.PlayerPhysicsCollider.bounds.extents;

            switch (position) {
                case ColliderPointPosition.BottomCenter:
                    collisionPoint = new Vector2(playerCenter.x, playerCenter.y - playerExtents.y);
                    break;
                case ColliderPointPosition.LeftCenter:
                    collisionPoint = new Vector2(playerCenter.x - playerExtents.x, playerCenter.y);
                    break;
                case ColliderPointPosition.RightCenter:
                    collisionPoint = new Vector2(playerCenter.x + playerExtents.x, playerCenter.y);
                    break;
                case ColliderPointPosition.Center:
                    collisionPoint = playerCenter;
                    break;
            }
            return collisionPoint + positionOffset;
        }

        protected float GetSphereRadius (ColliderPointPosition position) {
            CollisionSphereData data = System.Array.Find(spheres, spheres => spheres.PointPosition == position);

            if (data == null) {
                return -1;
            }
            return data.SphereRadius;
        }

        protected void DetectGroundCollision () {

            bool wasGrounded = playerController.IsGrounded;
            //Find cast parameters
            Vector2 centerPoint = GetCenterPoint(ColliderPointPosition.BottomCenter);
            float sphereRadius = GetSphereRadius(ColliderPointPosition.BottomCenter);
            //Update controller
            LastGroundCollider = Physics2D.OverlapCircle(centerPoint, sphereRadius, groundLayer);
            playerController.IsGrounded = LastGroundCollider != null;
            //Update visual
            playerVisual.SetAnimatorParameter(groundAnimatorParameter, playerController.IsGrounded);
            //Send event
            if (wasGrounded == playerController.IsGrounded) return;
            if (wasGrounded) {
                playerController.OnGroundReleased?.Invoke();
            } else {
                playerController.OnGroundLanded?.Invoke();
            }
        }

        protected void DetectWallCollision () {

        }

        protected void DrawSphere () {
            foreach(var sphere in spheres) {
                Gizmos.color = sphere.SphereColor;
                Vector2 point = GetCenterPoint(sphere.PointPosition);
                float radius = GetSphereRadius(sphere.PointPosition);
                Gizmos.DrawWireSphere(point, radius);
            }
        }
        #endregion


    }
}
