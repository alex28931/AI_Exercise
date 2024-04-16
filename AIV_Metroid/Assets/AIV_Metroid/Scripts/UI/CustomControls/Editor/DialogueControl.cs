using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;


public class DialogueControl : VisualElement
{
 
    public new class UxmlFactory: UxmlFactory<DialogueControl, UxmlTraits> {

    }

    //public new class UxmlTraits : VisualElement.UxmlTraits {
    //    UxmlUnsignedIntAttributeDescription dialogueIDAttribute = new UxmlUnsignedIntAttributeDescription() {
    //        name = "Dialogue_ID"
    //    };

    //    public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
    //        base.Init(ve, bag, cc);

    //        (ve as DialogueControl).DialogueID = dialogueIDAttribute.GetValueFromBag(bag, cc);
    //    }
    //}

    private const string databaseAsset = "Assets/AIV_Metroid/Database/DialogueSystem/DialogueEntry";
    private const string styleResource = "DialogueUSS";
    private const string containerTemplate = "DialogueContainer";

    private uint dialogueID;
    public uint DialogueID {
        get { return dialogueID; }
        set {
            dialogueID = value;
            foldout.text = "Dialogue " + dialogueID;
            UpdateDialogue();
        }
    }

    private Foldout foldout;
    private VisualElement contentFather;
    private DialogueEntry[] entries;

    public DialogueControl() {
        VisualTreeAsset template = Resources.Load<VisualTreeAsset>(containerTemplate);
        var instance = template.Instantiate();
        foldout = instance.Q<Foldout>("Dialogue");
        contentFather = instance.Q("unity-content");
        hierarchy.Add(instance);
    }

    private void UpdateDialogue () {
        contentFather.Clear();
        entries = GetDialogueEntries();
        for (int i = 0; i < entries.Length; i++) {
            DialogueEntryControl control = new DialogueEntryControl();
            control.Entry = entries[i];
            contentFather.Add(control);
        }
        Button addButton = new Button();
        addButton.text = "+";
        contentFather.Add(addButton);
        addButton.clicked += AddButtonCallback;
    }

    private DialogueEntry[] GetDialogueEntries () {
        string[] allEntriesGUID =
    AssetDatabase.FindAssets("t:DialogueEntry", new string[] { databaseAsset });
        DialogueEntry[] allEntries = new DialogueEntry[allEntriesGUID.Length];
        for (int i = 0; i < allEntries.Length; i++) {
            allEntries[i] = AssetDatabase.LoadAssetAtPath<DialogueEntry>
                (AssetDatabase.GUIDToAssetPath(allEntriesGUID[i]));
        }
        List<DialogueEntry> dialogueEntries = new List<DialogueEntry>();
        foreach (DialogueEntry entry in allEntries) {
            if (entry.Dialogue_ID == DialogueID) dialogueEntries.Add(entry);
        }
        return dialogueEntries.ToArray();
    }

    private void AddButtonCallback () {
        if (entries.Length == 0) {
            DialogueWindow.DialogueEntryAssetFactory(dialogueID, 0, string.Empty, -1);
            UpdateDialogue();
            return;
        }
        DialogueEntry lastEntry = entries[entries.Length - 1];
        uint newEntryID = lastEntry.Entry_ID + 1;
        lastEntry.SetNextEntry_ID((int)newEntryID);
        EditorUtility.SetDirty(lastEntry);
        DialogueWindow.DialogueEntryAssetFactory(dialogueID, newEntryID, string.Empty, -1);
        UpdateDialogue();
    }

}
