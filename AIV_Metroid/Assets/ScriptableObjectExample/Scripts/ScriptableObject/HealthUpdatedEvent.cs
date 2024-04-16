using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthUpdatedEventPorcata", menuName = "Dont/HealtUpdatedEvent")]
public class HealthUpdatedEvent : ScriptableObject
{
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float currentHP;

    public float MaxHP {
        get { return maxHP; }
    }
    public float CurrentHP {
        get { return currentHP; }
    }

    public Action HealthUpdated;

    public void AddHP (float hpToAdd) {
        if (hpToAdd == 0) return;
        currentHP += hpToAdd; //clamp tra 0 e maxHP che non facciamo
        HealthUpdated?.Invoke();
    }

}
