using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    [SerializeField]
    private DialogueDatabase database;

    private IDisplayer displayer;

    private DialogueEntry currentDisplayedEntry;
    private uint currentDialogueID;


    #region Mono
    private void Awake() {
        displayer = GetComponent<IDisplayer>();
    }

    private void OnEnable() {
        GlobalEventManager.AddListener(GlobalEventIndex.StartDialogue,
            OnStartDialogue);
    }

    private void OnDisable() {
        GlobalEventManager.RemoveListener(GlobalEventIndex.StartDialogue,
            OnStartDialogue);
    }
    #endregion

    #region InterfaceComunication
    private void OpenUI() {
        displayer.Open();
    }

    private void CloseUI () {
        displayer.Close();
    }

    private void DisplayText (string text) {
        displayer.DisplayEntry(text);
    }
    #endregion


    #region Callbacks
    private void OnStartDialogue (GlobalEventArgs message) {
        GlobalEventArgsFactory.StartDialogueParser(message, out uint dialogueID);
        currentDisplayedEntry = database.GetEntry(dialogueID, 0);
        if (!CanEntryBeDisplayed(currentDisplayedEntry)) return; //throw an undisplayable entry event
        StartDialogue();
    }

    private void OnEntryDisplayed () {
        displayer.OnEntryDisplayed -= OnEntryDisplayed;
        if (currentDisplayedEntry.NextEntry_ID == -1) {
            EndDialogue();
            return;
        }

        currentDisplayedEntry = database.GetEntry(currentDialogueID,
            (uint)currentDisplayedEntry.NextEntry_ID);
        if (!CanEntryBeDisplayed(currentDisplayedEntry)) {
            EndDialogue();
            return;
        }
        DisplayEntry();
    }
    #endregion

    private bool CanEntryBeDisplayed (DialogueEntry entry) {
        if (entry == null) return false;
        //logic to know if can be displayed
        return true;
    }

    private void StartDialogue () {
        OpenUI();
        currentDialogueID = currentDisplayedEntry.Dialogue_ID;
        DisplayEntry();
    }

    private void DisplayEntry () {
        displayer.OnEntryDisplayed += OnEntryDisplayed;
        DisplayText(currentDisplayedEntry.Dialogue_Text);
    }

    private void EndDialogue () {
        CloseUI();
        GlobalEventManager.CastEvent(GlobalEventIndex.DialoguePerformed,
            GlobalEventArgsFactory.DialoguePerformedFactory(currentDialogueID));
    }

}
