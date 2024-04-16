using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{

    private static Inputs input;

    static InputManager () {
        input = new Inputs();
        //TEMPORARY SUPER TBD MA RIMARRA' COSI' FINO ALLA FINE DELLO SVILUPPO. UN CLASSICO
        input.Player.Enable();
    }

    public static Inputs.PlayerActions Player {
        get { return input.Player; }
    }

    public static Inputs.UIActions UI {
        get { return input.UI; }
    }

    public static float Player_Horizontal {
        get { return input.Player.Move.ReadValue<Vector2>().x; }
    }
    public static bool Player_Jump_Pressed {
        get { return input.Player.Jump.IsPressed(); }
    }

    public static void EnablePlayerMap (bool enable) {
        if (enable) {
            input.Player.Enable();
        } else {
            input.Player.Disable();
        }
    }

    public static void EnableUIMap (bool enable) {
        if (enable) {
            input.UI.Enable();
        } else {
            input.UI.Disable();
        }
    }

}
