using UnityEngine;
using System.Collections;

public static class Logger {
    public enum Class
    {
        [BooleanEnum(false)]
        GameController,
        [BooleanEnum(true)]
        InGameController,
        [BooleanEnum(true)]
        Player,
        [BooleanEnum(false)]
        InputOrigin,
        [BooleanEnum(false)]
        InputPad,
        [BooleanEnum(false)]
        InputButton,
        [BooleanEnum(true)]
        TitleManager,

    }

    enum Color
    {
        [LabeledEnum("white")]
        White,
        [LabeledEnum("red")]
        Red,
        [LabeledEnum("yellow")]
        Yellow,
        [LabeledEnum("blue")]
        Blue,
        [LabeledEnum("green")]
        Green,
        [LabeledEnum("orange")]
        Orenge,
        [LabeledEnum("aqua")]
        Aqua,
        [LabeledEnum("black")]
        Black,
        [LabeledEnum("brown")]
        Brown,
        [LabeledEnum("fuchsia")]
        Fuchsia,
        [LabeledEnum("teal")]
        Teal,
    }

    public static void Log(string message, Class clazz)
    {
        if (BooleanEnumAttribute.GetFlag(clazz))
            Debug.Log("<color=" + LabeledEnumAttribute.GetLabel((Color)clazz) + ">" + message + "</color>");
    }
}
