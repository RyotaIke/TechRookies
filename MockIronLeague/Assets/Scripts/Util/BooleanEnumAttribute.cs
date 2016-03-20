using System;
using System.Collections.Generic;

/// <summary>
/// 列挙型のフィールドにラベル文字列を付加するカスタム属性です。
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class BooleanEnumAttribute
    : Attribute
{

    static Dictionary<Enum, bool> flagDictionary;

    /// <summary>
    /// ラベル文字列。
    /// </summary>
    private bool flag;

    /// <summary>
    /// LabeledEnumAttribute クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="flag">ラベル文字列</param>
    public BooleanEnumAttribute(bool flag)
    {
        this.flag = flag;
    }

    /// <summary>
    /// 属性で指定されたラベル文字列を取得する。
    /// </summary>
    /// <param name="value">ラベル付きフィールド</param>
    /// <returns>ラベル文字列</returns>
    public static bool GetFlag(Enum value)
    {
        if (flagDictionary != null && flagDictionary.ContainsKey(value))
            return flagDictionary[value];

        Type enumType = value.GetType();
        string name = Enum.GetName(enumType, value);
        BooleanEnumAttribute[] attrs =
            (BooleanEnumAttribute[])enumType.GetField(name)
            .GetCustomAttributes(typeof(BooleanEnumAttribute), false);

        var ret = attrs[0].flag;
        if (flagDictionary == null) flagDictionary = new Dictionary<Enum, bool>();
        flagDictionary.Add(value, ret);

        return ret;
    }
}