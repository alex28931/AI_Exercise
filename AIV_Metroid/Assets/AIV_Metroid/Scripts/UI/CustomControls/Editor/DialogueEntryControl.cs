using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class DialogueEntryControl : VisualElement
{

    public new class UxmlFactory : UxmlFactory<DialogueEntryControl,
        UxmlTraits> { }

    private const string containerTemplate = "EntryTemplate";

    private DialogueEntry entry;
    public DialogueEntry Entry {
        get { return entry; }
        set {
            if (value == null) return;
            entry = value;
            UpdateValue();
        }
    }

    UnsignedIntegerField id;
    TextField textField;
    Button applyButton;

    public DialogueEntryControl () {
        VisualTreeAsset entryTemplate = Resources.Load<VisualTreeAsset>(containerTemplate);

        var instance = entryTemplate.Instantiate();

        id = instance.Q<UnsignedIntegerField>("Entry_ID");
        textField = instance.Q<TextField>("Entry_Text");
        applyButton = instance.Q<Button>("Entry_Apply");
        applyButton.clicked += ApplyButtonCallback;
        hierarchy.Add(instance);
    }


    private void UpdateValue () {
        id.value = entry.Entry_ID;
        textField.SetValueWithoutNotify(entry.Dialogue_Text);
    }

    private void ApplyButtonCallback() {
        if (entry == null) return;
        entry.SetText(textField.text);
        EditorUtility.SetDirty(entry);
        AssetDatabase.SaveAssets();
    }

}
