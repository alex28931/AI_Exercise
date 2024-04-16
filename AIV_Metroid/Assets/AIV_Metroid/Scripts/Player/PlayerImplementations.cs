using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIV_Metroid_Player {
    public class PlayerImplementations : MonoBehaviour {

        [SerializeField]
        private GlobalEventToCast[] onDamageTakenEvents;

        private void OnEnable() {
            Player.Get().onDamageTaken += OnDamageTaken;
        }

        private void OnDisable() {
            Player.Get().onDamageTaken -= OnDamageTaken;
        }

        private void OnDamageTaken (DamageContainer _) {
            foreach(GlobalEventToCast e in onDamageTakenEvents) {
                GlobalEventManager.CastEvent(e.EventToCast, e.Message);
            }
        }

    }
}
