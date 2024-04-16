using System.Collections;
using UnityEngine;
using System;
using NotserializableEventManager;

namespace AIV_Metroid_Player {
    public class Player : MonoBehaviour, IDamageble {


        #region SerializeFields
        [SerializeField]
        private HealthModule healthModule;
        [SerializeField]
        private float damageInvTime;
        #endregion

        #region PrivateMembers
        private Coroutine invCoroutine;
        #endregion
        ////TEMP
        //#region ExternalEvents 
        //public Action<int, int> healthUpdate;

        //public int CurrentHP {
        //    get { return (int)healthModule.CurrentHP; }
        //}

        //public int MaxHP {
        //    get { return (int)healthModule.MaxHP; }
        //}
        //#endregion

        #region StaticMembers
        private static Player instance;

        public static Player Get () {
            if (instance != null) return instance;
            instance = GameObject.FindObjectOfType<Player>();
            return instance;
        }
        #endregion //StaticMembers

        #region PlayerReferences
        [SerializeField]
        private PlayerController playerController;
        [SerializeField]
        private PlayerVisual playerVisual;
        #endregion //PlayerReferences

        #region PublicEvents
        public Action<DamageContainer> onDamageTaken;
        #endregion

        #region MonoCallbacks
        private void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            if (instance != this) return;
            ResetHealth();
            healthModule.OnDamageTaken += InternalOnDamageTaken;
            healthModule.OnDeath += InternalOnDeath;
        }

        #endregion

        #region HealthModule
        public void ResetHealth () {
            healthModule.Reset();
            NotifyHealthUpdatedGlobal();
            playerController.IsDead = false;
        }

        public void TakeDamage(DamageContainer damage) {
            healthModule.TakeDamage(damage);
        }

        public void InternalOnDamageTaken(DamageContainer container) {
            //healthUpdate?.Invoke(MaxHP, CurrentHP);
            NotifyHealthUpdatedGlobal();
            onDamageTaken?.Invoke(container);
            playerController.OnDamageTaken?.Invoke(container);
            SetInvulnearble(damageInvTime);
        }

        public void InternalOnDeath() {
            playerController.IsDead = true;
            playerController.OnDeath?.Invoke();
        }
        #endregion

        #region PrivateMethods
        private void SetInvulnearble (float invTime) {
            if (invCoroutine != null) {
                StopCoroutine(invCoroutine);
            }
            invCoroutine = StartCoroutine(InvulnerabilityCoroutine(invTime));
        }

        private void NotifyHealthUpdatedGlobal () {
            //GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated,
            //    EventArgsFactory.PlayerHealthUpdatedFactory((int)healthModule.MaxHP, (int)healthModule.CurrentHP));
            GlobalEventManager.CastEvent(GlobalEventIndex.PlayerHealthUpdated,
                GlobalEventArgsFactory.PlayerHealthUpdatedFactory(healthModule.MaxHP, healthModule.CurrentHP));
        }
        #endregion

        #region Coroutine
        private IEnumerator InvulnerabilityCoroutine (float invTime) {
            healthModule.SetInvulnerable(true);
            yield return new WaitForSeconds(invTime);
            healthModule.SetInvulnerable(false);
        }
        #endregion

    }
}
