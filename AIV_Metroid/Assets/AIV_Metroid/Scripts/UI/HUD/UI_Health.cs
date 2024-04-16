using UnityEngine.UIElements;
using UnityEngine;
using NotserializableEventManager;

namespace AIV_Metroid_UI {
    public class UI_Health : MonoBehaviour {

        private PointBarControl healthBar;

        private void Awake() {
            healthBar = GetComponent<UIDocument>().rootVisualElement.Q<PointBarControl>("HealthBar");
        }

        private void OnEnable() {
            //Player.Get().healthUpdate += OnHealthUpdate;
            //GlobalEventSystem.AddListener(EventName.PlayerHealthUpdated, OnHealthUpdate);
            GlobalEventManager.AddListener(GlobalEventIndex.PlayerHealthUpdated, OnHealthUpdate);

        }

        private void OnDisable() {
            //Player.Get().healthUpdate -= OnHealthUpdate;
            //GlobalEventSystem.RemoveListener(EventName.PlayerHealthUpdated, OnHealthUpdate);
            GlobalEventManager.RemoveListener(GlobalEventIndex.PlayerHealthUpdated, OnHealthUpdate);
        }

        private void Start() {
            //OnHealthUpdate(Player.Get().MaxHP, Player.Get().CurrentHP);
        }

        private void OnHealthUpdate (GlobalEventArgs message/*EventArgs message*//*int maxHP, int currentHP*/) {
            //EventArgsFactory.PlayerHealthUpdatedParser(message, out int maxHP, out int currentHP);
            GlobalEventArgsFactory.PlayerHealthUpdatedParser(message, out float maxHP, out float currentHP);
            if(healthBar.MaxPoints != maxHP) {
                healthBar.MaxPoints = (int)maxHP;
            }
            if (healthBar.FullPoints != currentHP) {
                healthBar.FullPoints = (int)currentHP;
            }
        }

    }

}
