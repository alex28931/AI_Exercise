using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TriggerType {
    Awake,
    OnEnable,
    OnDisable,
    Start,
    OnTriggerEnter,
    OnTriggerExit,
    Manual
}

public class FlexibleUnityEventTrigger : MonoBehaviour
{

    [SerializeField]
    private TriggerType triggerType;
    [SerializeField]
    private UnityEvent actions;


    public void ManualTrigger () {
        InternalCastActions(TriggerType.Manual);
    }

    private void Awake() {
        InternalCastActions(TriggerType.Awake);
    }

    private void OnEnable() {
        InternalCastActions(TriggerType.OnEnable);
    }

    private void OnDisable() {
        InternalCastActions(TriggerType.OnDisable);
    }

    private void Start() {
        InternalCastActions(TriggerType.Start);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        InternalCastActions(TriggerType.OnTriggerEnter);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        InternalCastActions(TriggerType.OnTriggerExit);
    }

    private void InternalCastActions(TriggerType comingFrom) {
        if (comingFrom != triggerType) return;
        actions?.Invoke();
    }

}
