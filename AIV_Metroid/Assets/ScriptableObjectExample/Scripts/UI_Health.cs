using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Health : MonoBehaviour
{

    [SerializeField]
    private HealthUpdatedEvent healthUpdatedEvent;

    private void OnEnable() {
        healthUpdatedEvent.HealthUpdated += HealthUpdated;
    }

    private void OnDisable() {
        healthUpdatedEvent.HealthUpdated -= HealthUpdated;
    }

    private void HealthUpdated() {
        Debug.Log("La UI si aggiorna con il valore di maxHP: " + healthUpdatedEvent.MaxHP +
            " e il valore di currentHP: " + healthUpdatedEvent.CurrentHP);
    }
}
