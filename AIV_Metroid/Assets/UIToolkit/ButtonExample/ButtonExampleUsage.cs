using UnityEngine.UIElements;
using UnityEngine;

public class ButtonExampleUsage : MonoBehaviour
{

    private float onOpenValue;
    private float currentValue;

    private void Start() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button myButton = root.Q<Button>("MyButton");
        myButton.text = "Testo Cambiato";
        myButton.clicked += OnButtonClicked;

        Slider mySlider = root.Q<Slider>("MySlider");
        mySlider.RegisterValueChangedCallback(OnSliderChanged);
    }

    private void OnEnable() {
        onOpenValue = PlayerPrefs.GetFloat("MusicVolume", 100);
        currentValue = onOpenValue;
    }

    private void OnDisable() {
        if (onOpenValue == currentValue) return;
        PlayerPrefs.SetFloat("MusicValue", currentValue);
    }

    private void OnButtonClicked () {
        Debug.Log("Il pulsante è stato premuto");
    }

    private void OnSliderChanged (ChangeEvent<float> evt) {
        currentValue = evt.newValue;
        Debug.Log("Il valore è passato da: " + evt.previousValue + " a: " + evt.newValue);
    }

}
