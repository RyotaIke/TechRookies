using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// <para>Dictionary&lt;string,object&gt;型をasスタイルでアクセスできるようにするヘルパークラス。</para>
/// <para>下記のように代入するだけでデータをJsonObj型に変換でき、[]を使った辞書・リストアクセスが実現できます。</para>
/// <para>ex.</para>
/// <para>JsonObj obj =  apiResult as Dictionary&lt;string,object&gt;;</para>
/// <para>string name = obj["cards"][10]["name"];//自動でstringキャスト</para>
/// <para>int id = obj["cards"][10]["id"];//自動でintキャスト</para>
/// <para>左辺値で型を自動変換するので、ある程度型を意識せず記述できます。</para>
/// <para>動的型推測をしている訳ではないので、コンパイル時に型が決まらない記述の仕方をした場合エラーになります。</para>
/// <para>その場合は明示的に型を指定してキャストしてください。</para>
/// </summary>
public class JsonObj
{
	#region メソッド
	/// <summary>
	/// 配列アクセス
	/// </summary>
	/// <param name="pos">index</param>
	/// <returns>JsonObj</returns>
	public JsonObj this[int pos]
	{
		get
		{
			List<object> list = target as List<object>;
			object result = NullObject;
			if ((list == null) || (pos >= list.Count))
			{
				Debug.LogError("invalid index list[" + pos + "]");
			}
			else
			{
				result = list[pos] ?? NullObject;
			}
			return new JsonObj(result);
		}
	}

	/// <summary>
	/// 辞書引きアクセス
	/// </summary>
	/// <param name="key">key</param>
	/// <returns>JsonObj</returns>
	public JsonObj this[string key]
	{
		get
		{
			Dictionary<string, object> dic = target as Dictionary<string, object>;
			object result = NullObject;
			if ((dic == null) || (!dic.ContainsKey(key)))
			{
				Debug.LogError("invalid key dic[" + key + "]");
			}
			else
			{
				result = dic[key] ?? NullObject;
			}
			return new JsonObj(result);
		}
	}

	/// <summary>
	/// キーが存在するかチェック。
	/// </summary>
	/// <param name="key">string</param>
	/// <returns>キーが存在するかどうか:trueで存在</returns>
	public bool ContainsKey(string key)
	{
		Dictionary<string, object> dic = target as Dictionary<string, object>;
		return (dic != null) && (dic.ContainsKey(key));
	}

	/// <summary>
	/// nullチェック
	/// </summary>
	/// <returns>nullかどうか。trueならnull</returns>
	public bool IsNull()
	{
		List<object> listobj = target as List<object>;
		Dictionary<string, object> dicobj = target as Dictionary<string, object>;
		if ((listobj != null))
		{
			return (listobj.Count == 0);
		}
		if ((dicobj != null))
		{
			return (dicobj.Count == 0);
		}
		return target.ToString().ToLower().Equals("null") || (((target as string) != null) && string.IsNullOrEmpty(target as string));
	}

	/// <summary>
	/// リストを取得
	/// </summary>
	/// <returns> List&lt;object&gt; </returns>
	public List<object> ToList()
	{
		return target as List<object>;
	}

	/// <summary>
	/// List&lt;JsonObj&gt;形式で取得
	/// </summary>
	/// <returns>List&lt;JsonObj&gt;</returns>
	public List<JsonObj> ToJsonObjList()
	{
		List<JsonObj> output = new List<JsonObj>();
		List<object> converter = target as List<object>;
		if (converter != null)
		{
			for (int i = 0; i < converter.Count; ++i)
			{
				output.Add(new JsonObj(converter[i]));
			}
		}
		return output;
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>boolean</returns>
	public bool ToBoolean() { return this; }

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>int</returns>
	public int ToInt() { return this; }

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>long</returns>
	public long ToLong() { return this; }

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>uint</returns>
	public uint ToUInt() { return this; }

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>ulong</returns>
	public ulong ToULong() { return this; }

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>string</returns>
	public override string ToString() { return this; }

	/// <summary>
	/// Add the specified key and value.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	public void Add(string key, object value)
	{
		Dictionary<string, object> dic = target as Dictionary<string, object>;
		if (dic == null)
		{
			Debug.LogError("FATAL: Could not add data(" + key + "," + value + ") to invalid dictionary");
		}
		else
		{
			dic.Add(key, value);
		}
	}

	/// <summary>
	/// Set the specified key and value.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	public void Set(string key, object value)
	{
		Dictionary<string, object> dic = target as Dictionary<string, object>;
		if (dic == null)
		{
			Debug.LogError("FATAL: Could not add data(" + key + "," + value + ") to invalid dictionary");
		}
		else
		{
			dic[key] = value;
		}
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>bool</returns>
	public static implicit operator bool(JsonObj obj)
	{
		bool result = false;
		try
		{
			int tempInt = 0;
			bool tempBool = false;
			if ((obj.target as string != null) && !Boolean.TryParse((string)obj, out tempBool))
			{
				result = int.TryParse((string)obj, out tempInt) && Convert.ToBoolean(Convert.ToInt32(obj.target));
			}
			else
			{
				result = Convert.ToBoolean(obj.target);
			}
		}
		catch (Exception e)
		{
			Debug.LogError(e.ToString());
		}
		return result;
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>string</returns>
	public static implicit operator string(JsonObj obj)
	{
		string result = "";
		try
		{
			result = Convert.ToString(obj.target);
		}
		catch (Exception e)
		{
			Debug.LogError(e.ToString());
		}
		return result;
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>int</returns>
	public static implicit operator int(JsonObj obj)
	{
		int result = 0;
		try
		{
			result = Convert.ToInt32(obj.target);
		}
		catch (Exception e)
		{
			Debug.LogError(e.ToString());
		}
		return result;
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>long</returns>
	public static implicit operator long(JsonObj obj)
	{
		long result = 0;
		try
		{
			result = Convert.ToInt64(obj.target);
		}
		catch (Exception e)
		{
			Debug.LogError(e.ToString());
		}
		return result;
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>uint</returns>
	public static implicit operator uint(JsonObj obj)
	{
		uint result = 0;
		try
		{
			result = Convert.ToUInt32(obj.target);
		}
		catch (Exception e)
		{
			Debug.LogError(e.ToString());
		}
		return result;
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>ulong</returns>
	public static implicit operator ulong(JsonObj obj)
	{
		ulong result = 0u;
		try
		{
			result = Convert.ToUInt64(obj.target);
		}
		catch (Exception e)
		{
			Debug.LogError(e.ToString());
		}
		return result;
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>float</returns>
	public static implicit operator float(JsonObj obj)
	{
		double result = 0.0;
		try
		{
			result = Convert.ToDouble(obj.target);
		}
		catch (Exception e)
		{
			Debug.LogError(e.ToString());
		}
		return (float)result;
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns>double</returns>
	public static implicit operator double(JsonObj obj)
	{
		double result = 0.0;
		try
		{
			result = Convert.ToDouble(obj.target);
		}
		catch (Exception e)
		{
			Debug.LogError(e.ToString());
		}
		return result;
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns></returns>
	public static implicit operator List<JsonObj>(JsonObj target)
	{
		return target.ToJsonObjList();
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns></returns>
	public static implicit operator List<object>(JsonObj target)
	{
		return target.target as List<object>;
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns></returns>
	public static implicit operator Dictionary<string, object>(JsonObj target)
	{
		return target.target as Dictionary<string, object>;
	}

	/// <summary>
	/// 型変換
	/// </summary>
	/// <returns></returns>
	public static implicit operator JsonObj(Dictionary<string, object> target)
	{
		return new JsonObj(target);
	}

	/// <summary>
	/// Dictionaryの中身をjson stringで返す。
	/// </summary>
	/// <returns></returns>
	public string Dump()
	{
		if (target == null) return "(null)";
		return MiniJSON.Json.Serialize(target);
	}

	/// <summary>
	/// Dictionaryの中身をjsonっぽく表示
	/// </summary>
	public void Print()
	{
		Debug.Log(Dump());
	}

	/// <summary>
	/// ログ書き出し
	/// </summary>
	/// <param name="filename">Filename.</param>
	/// <param name="prefix">Prefix.</param>
	/// <param name="suffix">Suffix.</param>
	public void WriteLog(string filename, string prefix = "", string suffix = "")
	{
		System.IO.StreamWriter sw;
		System.IO.FileInfo fi;
		fi = new System.IO.FileInfo(filename);
		sw = fi.AppendText();
		sw.Write(prefix + Dump() + suffix);
		sw.Flush();
		sw.Close();
	}


	#endregion

	#region コンストラクタ

	/// <summary>
	/// コンストラクタ 
	/// </summary>
	private JsonObj() { }

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="target">object</param>
	private JsonObj(object target)
	{
		this.target = target ?? NullObject;
	}

	#endregion

	#region メンバ変数/static nullオブジェクト

	/// <summary>
	/// 変数
	/// </summary>
	private object target;

	/// <summary>
	/// nullオブジェクト
	/// </summary>
	public static readonly object NullObject = "null";

	#endregion

	/// <summary>
	/// 各演算子をオーバーロードして2項演算子の左側に来た時に正しくキャストされるようにしました。
	/// </summary>
	/// <returns></returns>
	#region operator オーバーロード
	public static double operator +(JsonObj lho, double rho) { return (double)lho + rho; }
	public static double operator -(JsonObj lho, double rho) { return (double)lho - rho; }
	public static double operator /(JsonObj lho, double rho) { return (double)lho / rho; }
	public static double operator *(JsonObj lho, double rho) { return (double)lho * rho; }
	public static float operator +(JsonObj lho, float rho) { return (float)lho + rho; }
	public static float operator -(JsonObj lho, float rho) { return (float)lho - rho; }
	public static float operator /(JsonObj lho, float rho) { return (float)lho / rho; }
	public static float operator *(JsonObj lho, float rho) { return (float)lho * rho; }
	public static int operator +(JsonObj lho, int rho) { return (int)lho + rho; }
	public static int operator -(JsonObj lho, int rho) { return (int)lho - rho; }
	public static int operator /(JsonObj lho, int rho) { return (int)lho / rho; }
	public static int operator *(JsonObj lho, int rho) { return (int)lho * rho; }
	public static uint operator +(JsonObj lho, uint rho) { return (uint)lho + rho; }
	public static uint operator -(JsonObj lho, uint rho) { return (uint)lho - rho; }
	public static uint operator /(JsonObj lho, uint rho) { return (uint)lho / rho; }
	public static uint operator *(JsonObj lho, uint rho) { return (uint)lho * rho; }
	public static long operator +(JsonObj lho, long rho) { return (long)lho + rho; }
	public static long operator -(JsonObj lho, long rho) { return (long)lho - rho; }
	public static long operator /(JsonObj lho, long rho) { return (long)lho / rho; }
	public static long operator *(JsonObj lho, long rho) { return (long)lho * rho; }
	public static ulong operator +(JsonObj lho, ulong rho) { return (ulong)lho + rho; }
	public static ulong operator -(JsonObj lho, ulong rho) { return (ulong)lho - rho; }
	public static ulong operator /(JsonObj lho, ulong rho) { return (ulong)lho / rho; }
	public static ulong operator *(JsonObj lho, ulong rho) { return (ulong)lho * rho; }
	public static string operator +(JsonObj lho, string rho) { return (string)lho + rho; }
	#endregion
}
