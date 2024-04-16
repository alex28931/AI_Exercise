using UnityEngine;
using UnityEngine.Rendering;

namespace AIV_Metroid_Player {
    public class PlayerVisual : MonoBehaviour {

        [SerializeField] protected SortingGroup sortingGroup;
        [SerializeField] protected SpriteRenderer playerMainRenderer;
        [SerializeField] protected Animator playerAnimator;

        #region AnimatorMethods
        public void SetAnimatorParameter (string name) {
            playerAnimator.SetTrigger(Animator.StringToHash(name));
        }

        public void SetAnimatorParameter (string name, bool value) {
            playerAnimator.SetBool(Animator.StringToHash(name), value);
        }

        public void SetAnimatorParameter (string name, float value) {
            playerAnimator.SetFloat(Animator.StringToHash(name), value);
        }

        public void SetAnimatorParameter (string name, int value) {
            playerAnimator.SetInteger(Animator.StringToHash(name), value);
        }

        public void SetAnimatorSpeed (float speed) {
            playerAnimator.speed = speed;
        }
        #endregion //AnimatorMethods

        #region VisualMethods
        public void ChangeSortingLayer (int sortingLayerID) {
            sortingGroup.sortingLayerID = sortingLayerID;
        }

        public void ChangeOrderInLayer (int orderInLayer) {
            sortingGroup.sortingOrder = orderInLayer;
        }

        public void FlipX (bool flipX) {
            playerMainRenderer.flipX = flipX;
        }
        #endregion //VisualMethods

    }
}
