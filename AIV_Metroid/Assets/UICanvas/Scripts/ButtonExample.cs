using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ButtonExample : MonoBehaviour
{

    //[SerializeField]
    //private Button button;
    [SerializeField]
    private GameObject changeActivnessOnClicK;
    [SerializeField]
    private UnityEvent magia;
    [SerializeField]
    private InputAction triggerInput;
    [SerializeField]
    private UnityEvent<float> magiaConFloat;

    //private void Awake() {
    //    button.onClick.AddListener(() => { changeActivnessOnClicK.SetActive(!changeActivnessOnClicK.activeSelf); });
    //}


    public void OnClick () {
        changeActivnessOnClicK.SetActive(!changeActivnessOnClicK.activeSelf);
    }

    private void Start() {
        triggerInput.Enable();
        triggerInput.performed += InputCallback;
    }

    void InputCallback (InputAction.CallbackContext context) {
        magia?.Invoke();
    }

    private void Update() {
        magiaConFloat?.Invoke(Time.time % 1);
    }


}
