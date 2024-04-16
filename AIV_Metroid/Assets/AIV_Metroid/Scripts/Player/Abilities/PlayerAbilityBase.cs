using UnityEngine;

namespace AIV_Metroid_Player {
    public abstract class PlayerAbilityBase : MonoBehaviour {

        #region References
        protected PlayerController playerController;
        protected PlayerVisual playerVisual;
        #endregion //References

        #region ProtectedMembers
        protected bool isPrevented;
        #endregion //ProtectedMembers

        #region AbstractMembers
        public abstract void OnInputDisabled();
        public abstract void OnInputEnabled();
        public abstract void StopAbility();
        #endregion //AbstractMembers

        #region VirtualMembers
        public virtual void Init (PlayerController playerController, PlayerVisual playerVisual) {
            this.playerController = playerController;
            this.playerVisual = playerVisual;
        }
        #endregion //VirtualMembers
        
    }
}
