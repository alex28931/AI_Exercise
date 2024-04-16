using UnityEngine.UIElements;
using UnityEngine;

public class TemplateSpawner : MonoBehaviour
{

    [SerializeField]
    private VisualTreeAsset templateToSpawn;

    private void Start() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        for (int i = 0; i < 20; i++) {
            var instance = templateToSpawn.Instantiate();
            root.Add(instance);
        }
    }

}
