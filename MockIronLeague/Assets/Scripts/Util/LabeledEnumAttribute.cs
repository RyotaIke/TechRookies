using System;
using System.Collections.Generic;

/// <summary>
/// 列挙型のフィールドにラベル文字列を付加するカスタム属性です。
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class LabeledEnumAttribute
    : Attribute
{
    static Dictionary<Enum, string> labelDictionary;

    /// <summary>
    /// ラベル文字列。
    /// </summary>
    private string label;

    /// <summary>
    /// LabeledEnumAttribute クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="label">ラベル文字列</param>
    public LabeledEnumAttribute(string label)
    {
        this.label = label;
    }

    /// <summary>
    /// 属性で指定されたラベル文字列を取得する。
    /// </summary>
    /// <param name="value">ラベル付きフィールド</param>
    /// <returns>ラベル文字列</returns>
    public static string GetLabel(Enum value)
    {
        if (labelDictionary != null && labelDictionary.ContainsKey(value))
            return labelDictionary[value];

        Type enumType = value.GetType();
        string name = Enum.GetName(enumType, value);
        LabeledEnumAttribute[] attrs =
            (LabeledEnumAttribute[])enumType.GetField(name)
            .GetCustomAttributes(typeof(LabeledEnumAttribute), false);

        var ret = attrs[0].label;
        if (labelDictionary == null) labelDictionary = new Dictionary<Enum, string>();
        labelDictionary.Add(value, ret);

        return ret;
    }
}