using UnityEngine.UIElements;
using UnityEngine;
using System;

public class PopupWindow : VisualElement
{


    private const string styleResource = "Stylesheet_PopupPanel";
    private const string ussContainer = "popup_container";
    private const string ussPopup = "popup_window";
    private const string ussPopupMsg = "popup_msg";
    private const string ussPopupButton = "popup_button";
    private const string ussCancel = "button_cancel";
    private const string ussConfirm = "button_confirm";
    private const string ussHorContainer = "horizontal_container";

    public new class UxmlFactory : UxmlFactory<PopupWindow, UxmlTraits> {

    }

    public new class UxmlTraits: VisualElement.UxmlTraits {

        UxmlStringAttributeDescription promptAttr = new UxmlStringAttributeDescription() {
            name = "prompt"
        };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
            base.Init(ve, bag, cc);

            (ve as PopupWindow).Prompt = promptAttr.GetValueFromBag(bag, cc);
        }

    }

    string prompt;
    public string Prompt {
        get {
            return prompt;
        }
        set {
            prompt = value;
            UpdatePrompt();
        }
    }

    Label msgLabel;

    public event Action confirmed;
    public event Action cancelled;

    public PopupWindow () {
        styleSheets.Add(Resources.Load<StyleSheet>(styleResource));
        AddToClassList(ussContainer);

        VisualElement window = new VisualElement();
        hierarchy.Add(window);
        window.AddToClassList(ussPopup);

        VisualElement horizontalContainerText = new VisualElement();
        window.Add(horizontalContainerText);
        horizontalContainerText.AddToClassList(ussHorContainer);

        msgLabel = new Label();
        horizontalContainerText.Add(msgLabel);
        msgLabel.AddToClassList(ussPopupMsg);

        VisualElement horizontalContainerButton = new VisualElement();
        window.Add(horizontalContainerButton);
        horizontalContainerButton.AddToClassList(ussHorContainer);

        Button confirmButton = new Button() { text = "CONFIRM" };
        Button cancelButton = new Button() { text = "CANCEL" };
        horizontalContainerButton.Add(confirmButton);
        horizontalContainerButton.Add(cancelButton);

        confirmButton.clicked += () => confirmed?.Invoke();
        cancelButton.clicked += () => cancelled?.Invoke();

        confirmButton.AddToClassList(ussPopupButton);
        confirmButton.AddToClassList(ussConfirm);

        cancelButton.AddToClassList(ussPopupButton);
        cancelButton.AddToClassList(ussCancel);

        msgLabel.text = "Hardocede text messsage";
    }


    private void UpdatePrompt () {
        msgLabel.text = prompt;
    }

}
