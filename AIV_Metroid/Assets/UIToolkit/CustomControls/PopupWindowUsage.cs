using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PopupWindowUsage : MonoBehaviour
{

    [SerializeField]
    private InputAction quitButton;

    PopupWindow popupWindow;

    private void OnEnable() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        popupWindow = new PopupWindow();
        popupWindow.visible = false;
        root.Add(popupWindow);

        quitButton.Enable();
        quitButton.performed += OnQuitButtonPerformed;
    }

    private void OnQuitButtonPerformed(InputAction.CallbackContext obj) {
        if (popupWindow.visible) {
            OnQuitCancelled();
        } else {
            popupWindow.visible = true;
            popupWindow.Prompt = "Do you really want to quit?";
            popupWindow.confirmed += OnQuitConfirmed;
            popupWindow.cancelled += OnQuitCancelled;
        }
    }

    private void OnQuitConfirmed () {
        Debug.Log("OnQuitConfirmed");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
   
    }

    private void OnQuitCancelled () {
        popupWindow.visible = false;
        popupWindow.confirmed -= OnQuitConfirmed;
        popupWindow.cancelled -= OnQuitCancelled;
    }
}
