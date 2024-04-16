using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private InputAction decreaseHP;
    [SerializeField]
    private HealthUpdatedEvent healthUpdatedEvent;

    void Start() {
        decreaseHP.Enable();
        decreaseHP.performed += DecreaseHP;
    }

    private void DecreaseHP(InputAction.CallbackContext obj) {
        healthUpdatedEvent.AddHP(-1);
    }
}
