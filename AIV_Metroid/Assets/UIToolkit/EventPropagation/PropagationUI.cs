using UnityEngine.UIElements;
using UnityEngine;

public class PropagationUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("MyButton").clicked += () => Debug.Log("ButtonClicked");

        VisualElement blue = root.Q("Blue");
        blue.userData = Color.blue;
        VisualElement yellow = root.Q("Yellow");
        yellow.userData = Color.yellow;
        VisualElement red = root.Q("Red");
        red.userData = Color.red;

        blue.RegisterCallback<MouseDownEvent>(
            evt => {
                Debug.Log("Blue callback with current phase: " + evt.propagationPhase
                    + " target: " + evt.target);
                Debug.Log((evt.target as VisualElement).userData);
            });

        yellow.RegisterCallback<MouseDownEvent>(
            evt => {
                Debug.Log("Yellow callback with current phase: " + evt.propagationPhase
                    + " target: " + evt.target);
            });

        red.RegisterCallback<MouseDownEvent>(
            evt => {
                Debug.Log("Red callback with current phase: " + evt.propagationPhase
                    + " target: " + evt.target);
            });

    }
}
