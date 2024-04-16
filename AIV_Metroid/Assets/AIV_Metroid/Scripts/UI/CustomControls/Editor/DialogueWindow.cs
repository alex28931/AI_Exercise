using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class DialogueWindow : EditorWindow
{
    #region const
    public const string DialogueEntryPath = "Assets/AIV_Metroid/Database/DialogueSystem/DialogueEntry/";

    private const string dialogueIDColumnName = "DIALOGUE_ID";
    private const string dialogueEntryColumnName = "DIALOGUE_ENTRY";
    private const string dialogueTextColumnName = "DIALOGUE_TEXT";
    private const string dialogueNextEntryColumnName = "NEXT_ENTRY";
    #endregion

    #region Static
    public static string GetDialogueEntryName (uint dialogueID, uint entryID) {
        return "Dialogue_" + dialogueID + "_" + entryID;
    }

    public static DialogueEntry DialogueEntryAssetFactory (uint dialogueID, uint entryID, string entryText, int nextEntryID) {
        DialogueEntry entry = DialogueEntry.Facotry(dialogueID, entryID, entryText, nextEntryID);
        AssetDatabase.CreateAsset(entry, DialogueEntryPath + GetDialogueEntryName(dialogueID, entryID) + ".asset");
        AssetDatabase.SaveAssets();
        return entry;
    }

    public static DialogueEntry DialogueEntryAssetFactory (Dictionary<string, object> entry, int line) {
        uint dialogueID;
        try {
            dialogueID = uint.Parse(entry[dialogueIDColumnName].ToString());
        } catch {
            throw new System.Exception("Il dialogue id nella linea: " + (line + 1).ToString() + " non è un numero intero senza segno");
        }
        uint entry_ID;
        try {
            entry_ID = uint.Parse(entry[dialogueEntryColumnName].ToString());
        } catch {
            throw new System.Exception("L'entry id nella linea " + (line + 1).ToString() + " non è un intero senza segno");
        }
        string entry_Text = entry[dialogueTextColumnName].ToString();
        int next_ID;
        try {
            next_ID = int.Parse(entry[dialogueNextEntryColumnName].ToString());
        } catch {
            throw new System.Exception("Il next entry id nella linea " + (line + 1).ToString() + " non è un intero");
        }
        return DialogueEntryAssetFactory(dialogueID, entry_ID, entry_Text, next_ID);
    }
    #endregion


    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("AIV_Metroid/DialgueWindow")]
    public static void ShowWindow()
    {
        DialogueWindow wnd = GetWindow<DialogueWindow>();
        wnd.titleContent = new GUIContent("DialogueWindow");
    }

    private ScrollView scrollView;
    private ObjectField csvAsset;

    public void CreateGUI()
    {

        VisualElement root = rootVisualElement;

        VisualElement template = m_VisualTreeAsset.Instantiate();
        root.Add(template);
        template.style.flexGrow = 1;
        scrollView = template.Q("DialogueListView").Q<ScrollView>();
        csvAsset = template.Q("UpPart").Q<ObjectField>();
        template.Q("UpPart").Q<Button>("ImportCSV").clicked += WrapDialogues;
        UpdateDialogues();
    }

    private void UpdateDialogues () {
        scrollView.Clear();
        string[] allEntriesGUID = AssetDatabase.FindAssets("t:DialogueEntry", new string[] { DialogueEntryPath });
        DialogueEntry[] allEntries = new DialogueEntry[allEntriesGUID.Length];
        for (int i = 0; i < allEntries.Length; i++) {
            allEntries[i] = AssetDatabase.LoadAssetAtPath<DialogueEntry>
                (AssetDatabase.GUIDToAssetPath(allEntriesGUID[i]));
        }
        uint lastDialogueID = allEntries[allEntries.Length - 1].Dialogue_ID;
        for (uint i = 0; i <= lastDialogueID; i++) {
            DialogueControl dialogueControl = new DialogueControl();
            dialogueControl.DialogueID = i;
            scrollView.Add(dialogueControl);
        }
        VisualElement separator = new VisualElement();
        separator.style.height = 20;
        separator.style.backgroundColor = Color.white;
        scrollView.Add(separator);
        Button button = new Button();
        button.text = "CREATE NEW DIALOGUE";
        button.clicked += (() => CreateNewDialogueCallback(lastDialogueID + 1));
        scrollView.Add(button);
    }

    private void CreateNewDialogueCallback(uint id) {
        DialogueEntryAssetFactory(id, 0, string.Empty, -1);
        UpdateDialogues();
    }

    private void WrapDialogues () {
        if (csvAsset.value == null) return;
        List<Dictionary<string, object>> parsedCSV = CSVReader.Read(csvAsset.value as TextAsset);
        for (int i = 0; i < parsedCSV.Count; i++) {
            DialogueEntryAssetFactory(parsedCSV[i], i);
        }
        UpdateDialogues();
    }
}
