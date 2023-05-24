using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000825 RID: 2085
public class iTween : MonoBehaviour
{
	// Token: 0x06002C5C RID: 11356 RVA: 0x000E0F8C File Offset: 0x000DF18C
	public static void Init(GameObject target)
	{
		iTween.MoveBy(target, Vector3.zero, 0f);
	}

	// Token: 0x06002C5D RID: 11357 RVA: 0x000E0FA0 File Offset: 0x000DF1A0
	public static void CameraFadeFrom(float amount, float time)
	{
		if (iTween.cameraFade)
		{
			iTween.CameraFadeFrom(iTween.Hash(new object[] { "amount", amount, "time", time }));
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	// Token: 0x06002C5E RID: 11358 RVA: 0x000E1000 File Offset: 0x000DF200
	public static void CameraFadeFrom(Hashtable args)
	{
		if (iTween.cameraFade)
		{
			iTween.ColorFrom(iTween.cameraFade, args);
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x000E102C File Offset: 0x000DF22C
	public static void CameraFadeTo(float amount, float time)
	{
		if (iTween.cameraFade)
		{
			iTween.CameraFadeTo(iTween.Hash(new object[] { "amount", amount, "time", time }));
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x000E108C File Offset: 0x000DF28C
	public static void CameraFadeTo(Hashtable args)
	{
		if (iTween.cameraFade)
		{
			iTween.ColorTo(iTween.cameraFade, args);
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x000E10B8 File Offset: 0x000DF2B8
	public static void ValueTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (!args.Contains("onupdate") || !args.Contains("from") || !args.Contains("to"))
		{
			Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
			return;
		}
		args["type"] = "value";
		if (args["from"].GetType() == typeof(Vector2))
		{
			args["method"] = "vector2";
		}
		else if (args["from"].GetType() == typeof(Vector3))
		{
			args["method"] = "vector3";
		}
		else if (args["from"].GetType() == typeof(Rect))
		{
			args["method"] = "rect";
		}
		else if (args["from"].GetType() == typeof(float))
		{
			args["method"] = "float";
		}
		else
		{
			if (args["from"].GetType() != typeof(Color))
			{
				Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
				return;
			}
			args["method"] = "color";
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		iTween.Launch(target, args);
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x000E1250 File Offset: 0x000DF450
	public static void FadeFrom(GameObject target, float alpha, float time)
	{
		iTween.FadeFrom(target, iTween.Hash(new object[] { "alpha", alpha, "time", time }));
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x000E1288 File Offset: 0x000DF488
	public static void FadeFrom(GameObject target, Hashtable args)
	{
		iTween.ColorFrom(target, args);
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x000E1294 File Offset: 0x000DF494
	public static void FadeTo(GameObject target, float alpha, float time)
	{
		iTween.FadeTo(target, iTween.Hash(new object[] { "alpha", alpha, "time", time }));
	}

	// Token: 0x06002C65 RID: 11365 RVA: 0x000E12CC File Offset: 0x000DF4CC
	public static void FadeTo(GameObject target, Hashtable args)
	{
		iTween.ColorTo(target, args);
	}

	// Token: 0x06002C66 RID: 11366 RVA: 0x000E12D8 File Offset: 0x000DF4D8
	public static void ColorFrom(GameObject target, Color color, float time)
	{
		iTween.ColorFrom(target, iTween.Hash(new object[] { "color", color, "time", time }));
	}

	// Token: 0x06002C67 RID: 11367 RVA: 0x000E1310 File Offset: 0x000DF510
	public static void ColorFrom(GameObject target, Hashtable args)
	{
		Color color = default(Color);
		Color color2 = default(Color);
		args = iTween.CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					Hashtable hashtable = (Hashtable)args.Clone();
					hashtable["ischild"] = true;
					iTween.ColorFrom(transform.gameObject, hashtable);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			color = (color2 = target.GetComponent<GUITexture>().color);
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			color = (color2 = target.GetComponent<GUIText>().material.color);
		}
		else if (target.GetComponent<Renderer>())
		{
			color = (color2 = target.GetComponent<Renderer>().material.color);
		}
		else if (target.GetComponent<Light>())
		{
			color = (color2 = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			color = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				color.r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				color.g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				color.b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				color.a = (float)args["a"];
			}
		}
		if (args.Contains("amount"))
		{
			color.a = (float)args["amount"];
			args.Remove("amount");
		}
		else if (args.Contains("alpha"))
		{
			color.a = (float)args["alpha"];
			args.Remove("alpha");
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			target.GetComponent<GUITexture>().color = color;
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			target.GetComponent<GUIText>().material.color = color;
		}
		else if (target.GetComponent<Renderer>())
		{
			target.GetComponent<Renderer>().material.color = color;
		}
		else if (target.GetComponent<Light>())
		{
			target.GetComponent<Light>().color = color;
		}
		args["color"] = color2;
		args["type"] = "color";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C68 RID: 11368 RVA: 0x000E16A0 File Offset: 0x000DF8A0
	public static void ColorTo(GameObject target, Color color, float time)
	{
		iTween.ColorTo(target, iTween.Hash(new object[] { "color", color, "time", time }));
	}

	// Token: 0x06002C69 RID: 11369 RVA: 0x000E16D8 File Offset: 0x000DF8D8
	public static void ColorTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					Hashtable hashtable = (Hashtable)args.Clone();
					hashtable["ischild"] = true;
					iTween.ColorTo(transform.gameObject, hashtable);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		args["type"] = "color";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C6A RID: 11370 RVA: 0x000E17D8 File Offset: 0x000DF9D8
	public static void AudioFrom(GameObject target, float volume, float pitch, float time)
	{
		iTween.AudioFrom(target, iTween.Hash(new object[] { "volume", volume, "pitch", pitch, "time", time }));
	}

	// Token: 0x06002C6B RID: 11371 RVA: 0x000E182C File Offset: 0x000DFA2C
	public static void AudioFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent(typeof(AudioSource)))
			{
				Debug.LogError("iTween Error: AudioFrom requires an AudioSource.");
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		Vector2 vector;
		Vector2 vector2;
		vector.x = (vector2.x = audioSource.volume);
		vector.y = (vector2.y = audioSource.pitch);
		if (args.Contains("volume"))
		{
			vector2.x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			vector2.y = (float)args["pitch"];
		}
		audioSource.volume = vector2.x;
		audioSource.pitch = vector2.y;
		args["volume"] = vector.x;
		args["pitch"] = vector.y;
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C6C RID: 11372 RVA: 0x000E19A8 File Offset: 0x000DFBA8
	public static void AudioTo(GameObject target, float volume, float pitch, float time)
	{
		iTween.AudioTo(target, iTween.Hash(new object[] { "volume", volume, "pitch", pitch, "time", time }));
	}

	// Token: 0x06002C6D RID: 11373 RVA: 0x000E19FC File Offset: 0x000DFBFC
	public static void AudioTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", iTween.EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C6E RID: 11374 RVA: 0x000E1A5C File Offset: 0x000DFC5C
	public static void Stab(GameObject target, AudioClip audioclip, float delay)
	{
		iTween.Stab(target, iTween.Hash(new object[] { "audioclip", audioclip, "delay", delay }));
	}

	// Token: 0x06002C6F RID: 11375 RVA: 0x000E1A8C File Offset: 0x000DFC8C
	public static void Stab(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "stab";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x000E1AB0 File Offset: 0x000DFCB0
	public static void LookFrom(GameObject target, Vector3 looktarget, float time)
	{
		iTween.LookFrom(target, iTween.Hash(new object[] { "looktarget", looktarget, "time", time }));
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x000E1AE8 File Offset: 0x000DFCE8
	public static void LookFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		Vector3 eulerAngles = target.transform.eulerAngles;
		if (args["looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = target.transform;
			Transform transform2 = (Transform)args["looktarget"];
			Vector3? vector = (Vector3?)args["up"];
			transform.LookAt(transform2, (vector == null) ? iTween.Defaults.up : vector.Value);
		}
		else if (args["looktarget"].GetType() == typeof(Vector3))
		{
			Transform transform3 = target.transform;
			Vector3 vector2 = (Vector3)args["looktarget"];
			Vector3? vector3 = (Vector3?)args["up"];
			transform3.LookAt(vector2, (vector3 == null) ? iTween.Defaults.up : vector3.Value);
		}
		if (args.Contains("axis"))
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			string text = (string)args["axis"];
			if (text != null)
			{
				if (!(text == "x"))
				{
					if (!(text == "y"))
					{
						if (text == "z")
						{
							eulerAngles2.x = eulerAngles.x;
							eulerAngles2.y = eulerAngles.y;
						}
					}
					else
					{
						eulerAngles2.x = eulerAngles.x;
						eulerAngles2.z = eulerAngles.z;
					}
				}
				else
				{
					eulerAngles2.y = eulerAngles.y;
					eulerAngles2.z = eulerAngles.z;
				}
			}
			target.transform.eulerAngles = eulerAngles2;
		}
		args["rotation"] = eulerAngles;
		args["type"] = "rotate";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C72 RID: 11378 RVA: 0x000E1CF4 File Offset: 0x000DFEF4
	public static void LookTo(GameObject target, Vector3 looktarget, float time)
	{
		iTween.LookTo(target, iTween.Hash(new object[] { "looktarget", looktarget, "time", time }));
	}

	// Token: 0x06002C73 RID: 11379 RVA: 0x000E1D2C File Offset: 0x000DFF2C
	public static void LookTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("looktarget") && args["looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["looktarget"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		args["type"] = "look";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C74 RID: 11380 RVA: 0x000E1E2C File Offset: 0x000E002C
	public static void MoveTo(GameObject target, Vector3 position, float time)
	{
		iTween.MoveTo(target, iTween.Hash(new object[] { "position", position, "time", time }));
	}

	// Token: 0x06002C75 RID: 11381 RVA: 0x000E1E64 File Offset: 0x000E0064
	public static void MoveTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("position") && args["position"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["position"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "move";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C76 RID: 11382 RVA: 0x000E1FA4 File Offset: 0x000E01A4
	public static void MoveFrom(GameObject target, Vector3 position, float time)
	{
		iTween.MoveFrom(target, iTween.Hash(new object[] { "position", position, "time", time }));
	}

	// Token: 0x06002C77 RID: 11383 RVA: 0x000E1FDC File Offset: 0x000E01DC
	public static void MoveFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		if (args.Contains("path"))
		{
			Vector3[] array2;
			if (args["path"].GetType() == typeof(Vector3[]))
			{
				Vector3[] array = (Vector3[])args["path"];
				array2 = new Vector3[array.Length];
				Array.Copy(array, array2, array.Length);
			}
			else
			{
				Transform[] array3 = (Transform[])args["path"];
				array2 = new Vector3[array3.Length];
				for (int i = 0; i < array3.Length; i++)
				{
					array2[i] = array3[i].position;
				}
			}
			if (array2[array2.Length - 1] != target.transform.position)
			{
				Vector3[] array4 = new Vector3[array2.Length + 1];
				Array.Copy(array2, array4, array2.Length);
				if (flag)
				{
					array4[array4.Length - 1] = target.transform.localPosition;
					target.transform.localPosition = array4[0];
				}
				else
				{
					array4[array4.Length - 1] = target.transform.position;
					target.transform.position = array4[0];
				}
				args["path"] = array4;
			}
			else
			{
				if (flag)
				{
					target.transform.localPosition = array2[0];
				}
				else
				{
					target.transform.position = array2[0];
				}
				args["path"] = array2;
			}
		}
		else
		{
			Vector3 vector2;
			Vector3 vector;
			if (flag)
			{
				vector = (vector2 = target.transform.localPosition);
			}
			else
			{
				vector = (vector2 = target.transform.position);
			}
			if (args.Contains("position"))
			{
				if (args["position"].GetType() == typeof(Transform))
				{
					Transform transform = (Transform)args["position"];
					vector = transform.position;
				}
				else if (args["position"].GetType() == typeof(Vector3))
				{
					vector = (Vector3)args["position"];
				}
			}
			else
			{
				if (args.Contains("x"))
				{
					vector.x = (float)args["x"];
				}
				if (args.Contains("y"))
				{
					vector.y = (float)args["y"];
				}
				if (args.Contains("z"))
				{
					vector.z = (float)args["z"];
				}
			}
			if (flag)
			{
				target.transform.localPosition = vector;
			}
			else
			{
				target.transform.position = vector;
			}
			args["position"] = vector2;
		}
		args["type"] = "move";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C78 RID: 11384 RVA: 0x000E2348 File Offset: 0x000E0548
	public static void MoveAdd(GameObject target, Vector3 amount, float time)
	{
		iTween.MoveAdd(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C79 RID: 11385 RVA: 0x000E2380 File Offset: 0x000E0580
	public static void MoveAdd(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "move";
		args["method"] = "add";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C7A RID: 11386 RVA: 0x000E23B4 File Offset: 0x000E05B4
	public static void MoveBy(GameObject target, Vector3 amount, float time)
	{
		iTween.MoveBy(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x000E23EC File Offset: 0x000E05EC
	public static void MoveBy(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "move";
		args["method"] = "by";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C7C RID: 11388 RVA: 0x000E2420 File Offset: 0x000E0620
	public static void ScaleTo(GameObject target, Vector3 scale, float time)
	{
		iTween.ScaleTo(target, iTween.Hash(new object[] { "scale", scale, "time", time }));
	}

	// Token: 0x06002C7D RID: 11389 RVA: 0x000E2458 File Offset: 0x000E0658
	public static void ScaleTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("scale") && args["scale"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["scale"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "scale";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C7E RID: 11390 RVA: 0x000E2598 File Offset: 0x000E0798
	public static void ScaleFrom(GameObject target, Vector3 scale, float time)
	{
		iTween.ScaleFrom(target, iTween.Hash(new object[] { "scale", scale, "time", time }));
	}

	// Token: 0x06002C7F RID: 11391 RVA: 0x000E25D0 File Offset: 0x000E07D0
	public static void ScaleFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		Vector3 localScale;
		Vector3 vector = (localScale = target.transform.localScale);
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				vector = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				vector = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				vector.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				vector.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				vector.z = (float)args["z"];
			}
		}
		target.transform.localScale = vector;
		args["scale"] = localScale;
		args["type"] = "scale";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C80 RID: 11392 RVA: 0x000E2730 File Offset: 0x000E0930
	public static void ScaleAdd(GameObject target, Vector3 amount, float time)
	{
		iTween.ScaleAdd(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C81 RID: 11393 RVA: 0x000E2768 File Offset: 0x000E0968
	public static void ScaleAdd(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "add";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x000E279C File Offset: 0x000E099C
	public static void ScaleBy(GameObject target, Vector3 amount, float time)
	{
		iTween.ScaleBy(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x000E27D4 File Offset: 0x000E09D4
	public static void ScaleBy(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "by";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x000E2808 File Offset: 0x000E0A08
	public static void RotateTo(GameObject target, Vector3 rotation, float time)
	{
		iTween.RotateTo(target, iTween.Hash(new object[] { "rotation", rotation, "time", time }));
	}

	// Token: 0x06002C85 RID: 11397 RVA: 0x000E2840 File Offset: 0x000E0A40
	public static void RotateTo(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		if (args.Contains("rotation") && args["rotation"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["rotation"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "rotate";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C86 RID: 11398 RVA: 0x000E2980 File Offset: 0x000E0B80
	public static void RotateFrom(GameObject target, Vector3 rotation, float time)
	{
		iTween.RotateFrom(target, iTween.Hash(new object[] { "rotation", rotation, "time", time }));
	}

	// Token: 0x06002C87 RID: 11399 RVA: 0x000E29B8 File Offset: 0x000E0BB8
	public static void RotateFrom(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		Vector3 vector2;
		Vector3 vector;
		if (flag)
		{
			vector = (vector2 = target.transform.localEulerAngles);
		}
		else
		{
			vector = (vector2 = target.transform.eulerAngles);
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				vector = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				vector = (Vector3)args["rotation"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				vector.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				vector.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				vector.z = (float)args["z"];
			}
		}
		if (flag)
		{
			target.transform.localEulerAngles = vector;
		}
		else
		{
			target.transform.eulerAngles = vector;
		}
		args["rotation"] = vector2;
		args["type"] = "rotate";
		args["method"] = "to";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C88 RID: 11400 RVA: 0x000E2B74 File Offset: 0x000E0D74
	public static void RotateAdd(GameObject target, Vector3 amount, float time)
	{
		iTween.RotateAdd(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C89 RID: 11401 RVA: 0x000E2BAC File Offset: 0x000E0DAC
	public static void RotateAdd(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "add";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C8A RID: 11402 RVA: 0x000E2BE0 File Offset: 0x000E0DE0
	public static void RotateBy(GameObject target, Vector3 amount, float time)
	{
		iTween.RotateBy(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C8B RID: 11403 RVA: 0x000E2C18 File Offset: 0x000E0E18
	public static void RotateBy(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "by";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C8C RID: 11404 RVA: 0x000E2C4C File Offset: 0x000E0E4C
	public static void ShakePosition(GameObject target, Vector3 amount, float time)
	{
		iTween.ShakePosition(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C8D RID: 11405 RVA: 0x000E2C84 File Offset: 0x000E0E84
	public static void ShakePosition(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "position";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C8E RID: 11406 RVA: 0x000E2CB8 File Offset: 0x000E0EB8
	public static void ShakeScale(GameObject target, Vector3 amount, float time)
	{
		iTween.ShakeScale(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C8F RID: 11407 RVA: 0x000E2CF0 File Offset: 0x000E0EF0
	public static void ShakeScale(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "scale";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C90 RID: 11408 RVA: 0x000E2D24 File Offset: 0x000E0F24
	public static void ShakeRotation(GameObject target, Vector3 amount, float time)
	{
		iTween.ShakeRotation(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C91 RID: 11409 RVA: 0x000E2D5C File Offset: 0x000E0F5C
	public static void ShakeRotation(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "rotation";
		iTween.Launch(target, args);
	}

	// Token: 0x06002C92 RID: 11410 RVA: 0x000E2D90 File Offset: 0x000E0F90
	public static void PunchPosition(GameObject target, Vector3 amount, float time)
	{
		iTween.PunchPosition(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C93 RID: 11411 RVA: 0x000E2DC8 File Offset: 0x000E0FC8
	public static void PunchPosition(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "position";
		args["easetype"] = iTween.EaseType.punch;
		iTween.Launch(target, args);
	}

	// Token: 0x06002C94 RID: 11412 RVA: 0x000E2E18 File Offset: 0x000E1018
	public static void PunchRotation(GameObject target, Vector3 amount, float time)
	{
		iTween.PunchRotation(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C95 RID: 11413 RVA: 0x000E2E50 File Offset: 0x000E1050
	public static void PunchRotation(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "rotation";
		args["easetype"] = iTween.EaseType.punch;
		iTween.Launch(target, args);
	}

	// Token: 0x06002C96 RID: 11414 RVA: 0x000E2EA0 File Offset: 0x000E10A0
	public static void PunchScale(GameObject target, Vector3 amount, float time)
	{
		iTween.PunchScale(target, iTween.Hash(new object[] { "amount", amount, "time", time }));
	}

	// Token: 0x06002C97 RID: 11415 RVA: 0x000E2ED8 File Offset: 0x000E10D8
	public static void PunchScale(GameObject target, Hashtable args)
	{
		args = iTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "scale";
		args["easetype"] = iTween.EaseType.punch;
		iTween.Launch(target, args);
	}

	// Token: 0x06002C98 RID: 11416 RVA: 0x000E2F28 File Offset: 0x000E1128
	private void GenerateTargets()
	{
		string text = this.type;
		switch (text)
		{
		case "value":
		{
			string text2 = this.method;
			if (text2 != null)
			{
				if (!(text2 == "float"))
				{
					if (!(text2 == "vector2"))
					{
						if (!(text2 == "vector3"))
						{
							if (!(text2 == "color"))
							{
								if (text2 == "rect")
								{
									this.GenerateRectTargets();
									this.apply = new iTween.ApplyTween(this.ApplyRectTargets);
								}
							}
							else
							{
								this.GenerateColorTargets();
								this.apply = new iTween.ApplyTween(this.ApplyColorTargets);
							}
						}
						else
						{
							this.GenerateVector3Targets();
							this.apply = new iTween.ApplyTween(this.ApplyVector3Targets);
						}
					}
					else
					{
						this.GenerateVector2Targets();
						this.apply = new iTween.ApplyTween(this.ApplyVector2Targets);
					}
				}
				else
				{
					this.GenerateFloatTargets();
					this.apply = new iTween.ApplyTween(this.ApplyFloatTargets);
				}
			}
			break;
		}
		case "color":
		{
			string text3 = this.method;
			if (text3 != null)
			{
				if (text3 == "to")
				{
					this.GenerateColorToTargets();
					this.apply = new iTween.ApplyTween(this.ApplyColorToTargets);
				}
			}
			break;
		}
		case "audio":
		{
			string text4 = this.method;
			if (text4 != null)
			{
				if (text4 == "to")
				{
					this.GenerateAudioToTargets();
					this.apply = new iTween.ApplyTween(this.ApplyAudioToTargets);
				}
			}
			break;
		}
		case "move":
		{
			string text5 = this.method;
			if (text5 != null)
			{
				if (!(text5 == "to"))
				{
					if (text5 == "by" || text5 == "add")
					{
						this.GenerateMoveByTargets();
						this.apply = new iTween.ApplyTween(this.ApplyMoveByTargets);
					}
				}
				else if (this.tweenArguments.Contains("path"))
				{
					this.GenerateMoveToPathTargets();
					this.apply = new iTween.ApplyTween(this.ApplyMoveToPathTargets);
				}
				else
				{
					this.GenerateMoveToTargets();
					this.apply = new iTween.ApplyTween(this.ApplyMoveToTargets);
				}
			}
			break;
		}
		case "scale":
		{
			string text6 = this.method;
			if (text6 != null)
			{
				if (!(text6 == "to"))
				{
					if (!(text6 == "by"))
					{
						if (text6 == "add")
						{
							this.GenerateScaleAddTargets();
							this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
						}
					}
					else
					{
						this.GenerateScaleByTargets();
						this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
					}
				}
				else
				{
					this.GenerateScaleToTargets();
					this.apply = new iTween.ApplyTween(this.ApplyScaleToTargets);
				}
			}
			break;
		}
		case "rotate":
		{
			string text7 = this.method;
			if (text7 != null)
			{
				if (!(text7 == "to"))
				{
					if (!(text7 == "add"))
					{
						if (text7 == "by")
						{
							this.GenerateRotateByTargets();
							this.apply = new iTween.ApplyTween(this.ApplyRotateAddTargets);
						}
					}
					else
					{
						this.GenerateRotateAddTargets();
						this.apply = new iTween.ApplyTween(this.ApplyRotateAddTargets);
					}
				}
				else
				{
					this.GenerateRotateToTargets();
					this.apply = new iTween.ApplyTween(this.ApplyRotateToTargets);
				}
			}
			break;
		}
		case "shake":
		{
			string text8 = this.method;
			if (text8 != null)
			{
				if (!(text8 == "position"))
				{
					if (!(text8 == "scale"))
					{
						if (text8 == "rotation")
						{
							this.GenerateShakeRotationTargets();
							this.apply = new iTween.ApplyTween(this.ApplyShakeRotationTargets);
						}
					}
					else
					{
						this.GenerateShakeScaleTargets();
						this.apply = new iTween.ApplyTween(this.ApplyShakeScaleTargets);
					}
				}
				else
				{
					this.GenerateShakePositionTargets();
					this.apply = new iTween.ApplyTween(this.ApplyShakePositionTargets);
				}
			}
			break;
		}
		case "punch":
		{
			string text9 = this.method;
			if (text9 != null)
			{
				if (!(text9 == "position"))
				{
					if (!(text9 == "rotation"))
					{
						if (text9 == "scale")
						{
							this.GeneratePunchScaleTargets();
							this.apply = new iTween.ApplyTween(this.ApplyPunchScaleTargets);
						}
					}
					else
					{
						this.GeneratePunchRotationTargets();
						this.apply = new iTween.ApplyTween(this.ApplyPunchRotationTargets);
					}
				}
				else
				{
					this.GeneratePunchPositionTargets();
					this.apply = new iTween.ApplyTween(this.ApplyPunchPositionTargets);
				}
			}
			break;
		}
		case "look":
		{
			string text10 = this.method;
			if (text10 != null)
			{
				if (text10 == "to")
				{
					this.GenerateLookToTargets();
					this.apply = new iTween.ApplyTween(this.ApplyLookToTargets);
				}
			}
			break;
		}
		case "stab":
			this.GenerateStabTargets();
			this.apply = new iTween.ApplyTween(this.ApplyStabTargets);
			break;
		}
	}

	// Token: 0x06002C99 RID: 11417 RVA: 0x000E3548 File Offset: 0x000E1748
	private void GenerateRectTargets()
	{
		this.rects = new Rect[3];
		this.rects[0] = (Rect)this.tweenArguments["from"];
		this.rects[1] = (Rect)this.tweenArguments["to"];
	}

	// Token: 0x06002C9A RID: 11418 RVA: 0x000E35B0 File Offset: 0x000E17B0
	private void GenerateColorTargets()
	{
		this.colors = new Color[1, 3];
		this.colors[0, 0] = (Color)this.tweenArguments["from"];
		this.colors[0, 1] = (Color)this.tweenArguments["to"];
	}

	// Token: 0x06002C9B RID: 11419 RVA: 0x000E3618 File Offset: 0x000E1818
	private void GenerateVector3Targets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (Vector3)this.tweenArguments["from"];
		this.vector3s[1] = (Vector3)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002C9C RID: 11420 RVA: 0x000E36DC File Offset: 0x000E18DC
	private void GenerateVector2Targets()
	{
		this.vector2s = new Vector2[3];
		this.vector2s[0] = (Vector2)this.tweenArguments["from"];
		this.vector2s[1] = (Vector2)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			Vector3 vector = new Vector3(this.vector2s[0].x, this.vector2s[0].y, 0f);
			Vector3 vector2 = new Vector3(this.vector2s[1].x, this.vector2s[1].y, 0f);
			float num = Math.Abs(Vector3.Distance(vector, vector2));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x000E37DC File Offset: 0x000E19DC
	private void GenerateFloatTargets()
	{
		this.floats = new float[3];
		this.floats[0] = (float)this.tweenArguments["from"];
		this.floats[1] = (float)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(this.floats[0] - this.floats[1]);
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002C9E RID: 11422 RVA: 0x000E3878 File Offset: 0x000E1A78
	private void GenerateColorToTargets()
	{
		if (base.GetComponent(typeof(GUITexture)))
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.GetComponent<GUITexture>().color);
		}
		else if (base.GetComponent(typeof(GUIText)))
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.GetComponent<GUIText>().material.color);
		}
		else if (base.GetComponent<Renderer>())
		{
			this.colors = new Color[base.GetComponent<Renderer>().materials.Length, 3];
			for (int i = 0; i < base.GetComponent<Renderer>().materials.Length; i++)
			{
				this.colors[i, 0] = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
				this.colors[i, 1] = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
			}
		}
		else if (base.GetComponent<Light>())
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.GetComponent<Light>().color);
		}
		else
		{
			this.colors = new Color[1, 3];
		}
		if (this.tweenArguments.Contains("color"))
		{
			for (int j = 0; j < this.colors.GetLength(0); j++)
			{
				this.colors[j, 1] = (Color)this.tweenArguments["color"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("r"))
			{
				for (int k = 0; k < this.colors.GetLength(0); k++)
				{
					this.colors[k, 1].r = (float)this.tweenArguments["r"];
				}
			}
			if (this.tweenArguments.Contains("g"))
			{
				for (int l = 0; l < this.colors.GetLength(0); l++)
				{
					this.colors[l, 1].g = (float)this.tweenArguments["g"];
				}
			}
			if (this.tweenArguments.Contains("b"))
			{
				for (int m = 0; m < this.colors.GetLength(0); m++)
				{
					this.colors[m, 1].b = (float)this.tweenArguments["b"];
				}
			}
			if (this.tweenArguments.Contains("a"))
			{
				for (int n = 0; n < this.colors.GetLength(0); n++)
				{
					this.colors[n, 1].a = (float)this.tweenArguments["a"];
				}
			}
		}
		if (this.tweenArguments.Contains("amount"))
		{
			for (int num = 0; num < this.colors.GetLength(0); num++)
			{
				this.colors[num, 1].a = (float)this.tweenArguments["amount"];
			}
		}
		else if (this.tweenArguments.Contains("alpha"))
		{
			for (int num2 = 0; num2 < this.colors.GetLength(0); num2++)
			{
				this.colors[num2, 1].a = (float)this.tweenArguments["alpha"];
			}
		}
	}

	// Token: 0x06002C9F RID: 11423 RVA: 0x000E3CF0 File Offset: 0x000E1EF0
	private void GenerateAudioToTargets()
	{
		this.vector2s = new Vector2[3];
		if (this.tweenArguments.Contains("audiosource"))
		{
			this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
		}
		else if (base.GetComponent(typeof(AudioSource)))
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		else
		{
			Debug.LogError("iTween Error: AudioTo requires an AudioSource.");
			this.Dispose();
		}
		this.vector2s[0] = (this.vector2s[1] = new Vector2(this.audioSource.volume, this.audioSource.pitch));
		if (this.tweenArguments.Contains("volume"))
		{
			this.vector2s[1].x = (float)this.tweenArguments["volume"];
		}
		if (this.tweenArguments.Contains("pitch"))
		{
			this.vector2s[1].y = (float)this.tweenArguments["pitch"];
		}
	}

	// Token: 0x06002CA0 RID: 11424 RVA: 0x000E3E30 File Offset: 0x000E2030
	private void GenerateStabTargets()
	{
		if (this.tweenArguments.Contains("audiosource"))
		{
			this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
		}
		else if (base.GetComponent(typeof(AudioSource)))
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		else
		{
			base.gameObject.AddComponent(typeof(AudioSource));
			this.audioSource = base.GetComponent<AudioSource>();
			this.audioSource.playOnAwake = false;
		}
		this.audioSource.clip = (AudioClip)this.tweenArguments["audioclip"];
		if (this.tweenArguments.Contains("pitch"))
		{
			this.audioSource.pitch = (float)this.tweenArguments["pitch"];
		}
		if (this.tweenArguments.Contains("volume"))
		{
			this.audioSource.volume = (float)this.tweenArguments["volume"];
		}
		this.time = this.audioSource.clip.length / this.audioSource.pitch;
	}

	// Token: 0x06002CA1 RID: 11425 RVA: 0x000E3F78 File Offset: 0x000E2178
	private void GenerateLookToTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.eulerAngles;
		if (this.tweenArguments.Contains("looktarget"))
		{
			if (this.tweenArguments["looktarget"].GetType() == typeof(Transform))
			{
				Transform transform = base.transform;
				Transform transform2 = (Transform)this.tweenArguments["looktarget"];
				Vector3? vector = (Vector3?)this.tweenArguments["up"];
				transform.LookAt(transform2, (vector == null) ? iTween.Defaults.up : vector.Value);
			}
			else if (this.tweenArguments["looktarget"].GetType() == typeof(Vector3))
			{
				Transform transform3 = base.transform;
				Vector3 vector2 = (Vector3)this.tweenArguments["looktarget"];
				Vector3? vector3 = (Vector3?)this.tweenArguments["up"];
				transform3.LookAt(vector2, (vector3 == null) ? iTween.Defaults.up : vector3.Value);
			}
		}
		else
		{
			Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!");
			this.Dispose();
		}
		this.vector3s[1] = base.transform.eulerAngles;
		base.transform.eulerAngles = this.vector3s[0];
		if (this.tweenArguments.Contains("axis"))
		{
			string text = (string)this.tweenArguments["axis"];
			if (text != null)
			{
				if (!(text == "x"))
				{
					if (!(text == "y"))
					{
						if (text == "z")
						{
							this.vector3s[1].x = this.vector3s[0].x;
							this.vector3s[1].y = this.vector3s[0].y;
						}
					}
					else
					{
						this.vector3s[1].x = this.vector3s[0].x;
						this.vector3s[1].z = this.vector3s[0].z;
					}
				}
				else
				{
					this.vector3s[1].y = this.vector3s[0].y;
					this.vector3s[1].z = this.vector3s[0].z;
				}
			}
		}
		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002CA2 RID: 11426 RVA: 0x000E4340 File Offset: 0x000E2540
	private void GenerateMoveToPathTargets()
	{
		Vector3[] array2;
		if (this.tweenArguments["path"].GetType() == typeof(Vector3[]))
		{
			Vector3[] array = (Vector3[])this.tweenArguments["path"];
			if (array.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				this.Dispose();
			}
			array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
		}
		else
		{
			Transform[] array3 = (Transform[])this.tweenArguments["path"];
			if (array3.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				this.Dispose();
			}
			array2 = new Vector3[array3.Length];
			for (int i = 0; i < array3.Length; i++)
			{
				array2[i] = array3[i].position;
			}
		}
		bool flag;
		int num;
		if (base.transform.position != array2[0])
		{
			if (!this.tweenArguments.Contains("movetopath") || (bool)this.tweenArguments["movetopath"])
			{
				flag = true;
				num = 3;
			}
			else
			{
				flag = false;
				num = 2;
			}
		}
		else
		{
			flag = false;
			num = 2;
		}
		this.vector3s = new Vector3[array2.Length + num];
		if (flag)
		{
			this.vector3s[1] = base.transform.position;
			num = 2;
		}
		else
		{
			num = 1;
		}
		Array.Copy(array2, 0, this.vector3s, num, array2.Length);
		this.vector3s[0] = this.vector3s[1] + (this.vector3s[1] - this.vector3s[2]);
		this.vector3s[this.vector3s.Length - 1] = this.vector3s[this.vector3s.Length - 2] + (this.vector3s[this.vector3s.Length - 2] - this.vector3s[this.vector3s.Length - 3]);
		if (this.vector3s[1] == this.vector3s[this.vector3s.Length - 2])
		{
			Vector3[] array4 = new Vector3[this.vector3s.Length];
			Array.Copy(this.vector3s, array4, this.vector3s.Length);
			array4[0] = array4[array4.Length - 3];
			array4[array4.Length - 1] = array4[2];
			this.vector3s = new Vector3[array4.Length];
			Array.Copy(array4, this.vector3s, array4.Length);
		}
		this.path = new iTween.CRSpline(this.vector3s);
		if (this.tweenArguments.Contains("speed"))
		{
			float num2 = iTween.PathLength(this.vector3s);
			this.time = num2 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002CA3 RID: 11427 RVA: 0x000E46A0 File Offset: 0x000E28A0
	private void GenerateMoveToTargets()
	{
		this.vector3s = new Vector3[3];
		if (this.isLocal)
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.localPosition);
		}
		else
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.position);
		}
		if (this.tweenArguments.Contains("position"))
		{
			if (this.tweenArguments["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["position"];
				this.vector3s[1] = transform.position;
			}
			else if (this.tweenArguments["position"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["position"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			this.tweenArguments["looktarget"] = this.vector3s[1];
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002CA4 RID: 11428 RVA: 0x000E4948 File Offset: 0x000E2B48
	private void GenerateMoveByTargets()
	{
		this.vector3s = new Vector3[6];
		this.vector3s[4] = base.transform.eulerAngles;
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.position));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = this.vector3s[0] + (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = this.vector3s[0].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = this.vector3s[0].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = this.vector3s[0].z + (float)this.tweenArguments["z"];
			}
		}
		base.transform.Translate(this.vector3s[1], this.space);
		this.vector3s[5] = base.transform.position;
		base.transform.position = this.vector3s[0];
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			this.tweenArguments["looktarget"] = this.vector3s[1];
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002CA5 RID: 11429 RVA: 0x000E4C0C File Offset: 0x000E2E0C
	private void GenerateScaleToTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("scale"))
		{
			if (this.tweenArguments["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["scale"];
				this.vector3s[1] = transform.localScale;
			}
			else if (this.tweenArguments["scale"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["scale"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002CA6 RID: 11430 RVA: 0x000E4E20 File Offset: 0x000E3020
	private void GenerateScaleByTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = Vector3.Scale(this.vector3s[1], (Vector3)this.tweenArguments["amount"]);
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x * (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y * (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z * (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002CA7 RID: 11431 RVA: 0x000E4FE4 File Offset: 0x000E31E4
	private void GenerateScaleAddTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002CA8 RID: 11432 RVA: 0x000E51A0 File Offset: 0x000E33A0
	private void GenerateRotateToTargets()
	{
		this.vector3s = new Vector3[3];
		if (this.isLocal)
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.localEulerAngles);
		}
		else
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.eulerAngles);
		}
		if (this.tweenArguments.Contains("rotation"))
		{
			if (this.tweenArguments["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["rotation"];
				this.vector3s[1] = transform.eulerAngles;
			}
			else if (this.tweenArguments["rotation"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["rotation"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002CA9 RID: 11433 RVA: 0x000E5490 File Offset: 0x000E3690
	private void GenerateRotateAddTargets()
	{
		this.vector3s = new Vector3[5];
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.eulerAngles));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002CAA RID: 11434 RVA: 0x000E5660 File Offset: 0x000E3860
	private void GenerateRotateByTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.eulerAngles));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += Vector3.Scale((Vector3)this.tweenArguments["amount"], new Vector3(360f, 360f, 360f));
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + 360f * (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + 360f * (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + 360f * (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06002CAB RID: 11435 RVA: 0x000E5858 File Offset: 0x000E3A58
	private void GenerateShakePositionTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[3] = base.transform.eulerAngles;
		this.vector3s[0] = base.transform.position;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06002CAC RID: 11436 RVA: 0x000E599C File Offset: 0x000E3B9C
	private void GenerateShakeScaleTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.localScale;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06002CAD RID: 11437 RVA: 0x000E5AC4 File Offset: 0x000E3CC4
	private void GenerateShakeRotationTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.eulerAngles;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06002CAE RID: 11438 RVA: 0x000E5BEC File Offset: 0x000E3DEC
	private void GeneratePunchPositionTargets()
	{
		this.vector3s = new Vector3[5];
		this.vector3s[4] = base.transform.eulerAngles;
		this.vector3s[0] = base.transform.position;
		this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06002CAF RID: 11439 RVA: 0x000E5D58 File Offset: 0x000E3F58
	private void GeneratePunchRotationTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[0] = base.transform.eulerAngles;
		this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06002CB0 RID: 11440 RVA: 0x000E5EA8 File Offset: 0x000E40A8
	private void GeneratePunchScaleTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.localScale;
		this.vector3s[1] = Vector3.zero;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x06002CB1 RID: 11441 RVA: 0x000E5FE4 File Offset: 0x000E41E4
	private void ApplyRectTargets()
	{
		this.rects[2].x = this.ease(this.rects[0].x, this.rects[1].x, this.percentage);
		this.rects[2].y = this.ease(this.rects[0].y, this.rects[1].y, this.percentage);
		this.rects[2].width = this.ease(this.rects[0].width, this.rects[1].width, this.percentage);
		this.rects[2].height = this.ease(this.rects[0].height, this.rects[1].height, this.percentage);
		this.tweenArguments["onupdateparams"] = this.rects[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.rects[1];
		}
	}

	// Token: 0x06002CB2 RID: 11442 RVA: 0x000E6160 File Offset: 0x000E4360
	private void ApplyColorTargets()
	{
		this.colors[0, 2].r = this.ease(this.colors[0, 0].r, this.colors[0, 1].r, this.percentage);
		this.colors[0, 2].g = this.ease(this.colors[0, 0].g, this.colors[0, 1].g, this.percentage);
		this.colors[0, 2].b = this.ease(this.colors[0, 0].b, this.colors[0, 1].b, this.percentage);
		this.colors[0, 2].a = this.ease(this.colors[0, 0].a, this.colors[0, 1].a, this.percentage);
		this.tweenArguments["onupdateparams"] = this.colors[0, 2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.colors[0, 1];
		}
	}

	// Token: 0x06002CB3 RID: 11443 RVA: 0x000E62E0 File Offset: 0x000E44E0
	private void ApplyVector3Targets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		this.tweenArguments["onupdateparams"] = this.vector3s[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.vector3s[1];
		}
	}

	// Token: 0x06002CB4 RID: 11444 RVA: 0x000E6418 File Offset: 0x000E4618
	private void ApplyVector2Targets()
	{
		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
		this.tweenArguments["onupdateparams"] = this.vector2s[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.vector2s[1];
		}
	}

	// Token: 0x06002CB5 RID: 11445 RVA: 0x000E650C File Offset: 0x000E470C
	private void ApplyFloatTargets()
	{
		this.floats[2] = this.ease(this.floats[0], this.floats[1], this.percentage);
		this.tweenArguments["onupdateparams"] = this.floats[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.floats[1];
		}
	}

	// Token: 0x06002CB6 RID: 11446 RVA: 0x000E658C File Offset: 0x000E478C
	private void ApplyColorToTargets()
	{
		for (int i = 0; i < this.colors.GetLength(0); i++)
		{
			this.colors[i, 2].r = this.ease(this.colors[i, 0].r, this.colors[i, 1].r, this.percentage);
			this.colors[i, 2].g = this.ease(this.colors[i, 0].g, this.colors[i, 1].g, this.percentage);
			this.colors[i, 2].b = this.ease(this.colors[i, 0].b, this.colors[i, 1].b, this.percentage);
			this.colors[i, 2].a = this.ease(this.colors[i, 0].a, this.colors[i, 1].a, this.percentage);
		}
		if (base.GetComponent(typeof(GUITexture)))
		{
			base.GetComponent<GUITexture>().color = this.colors[0, 2];
		}
		else if (base.GetComponent(typeof(GUIText)))
		{
			base.GetComponent<GUIText>().material.color = this.colors[0, 2];
		}
		else if (base.GetComponent<Renderer>())
		{
			for (int j = 0; j < this.colors.GetLength(0); j++)
			{
				base.GetComponent<Renderer>().materials[j].SetColor(this.namedcolorvalue.ToString(), this.colors[j, 2]);
			}
		}
		else if (base.GetComponent<Light>())
		{
			base.GetComponent<Light>().color = this.colors[0, 2];
		}
		if (this.percentage == 1f)
		{
			if (base.GetComponent(typeof(GUITexture)))
			{
				base.GetComponent<GUITexture>().color = this.colors[0, 1];
			}
			else if (base.GetComponent(typeof(GUIText)))
			{
				base.GetComponent<GUIText>().material.color = this.colors[0, 1];
			}
			else if (base.GetComponent<Renderer>())
			{
				for (int k = 0; k < this.colors.GetLength(0); k++)
				{
					base.GetComponent<Renderer>().materials[k].SetColor(this.namedcolorvalue.ToString(), this.colors[k, 1]);
				}
			}
			else if (base.GetComponent<Light>())
			{
				base.GetComponent<Light>().color = this.colors[0, 1];
			}
		}
	}

	// Token: 0x06002CB7 RID: 11447 RVA: 0x000E68DC File Offset: 0x000E4ADC
	private void ApplyAudioToTargets()
	{
		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
		this.audioSource.volume = this.vector2s[2].x;
		this.audioSource.pitch = this.vector2s[2].y;
		if (this.percentage == 1f)
		{
			this.audioSource.volume = this.vector2s[1].x;
			this.audioSource.pitch = this.vector2s[1].y;
		}
	}

	// Token: 0x06002CB8 RID: 11448 RVA: 0x000E69F4 File Offset: 0x000E4BF4
	private void ApplyStabTargets()
	{
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x000E69F8 File Offset: 0x000E4BF8
	private void ApplyMoveToPathTargets()
	{
		this.preUpdate = base.transform.position;
		float num = this.ease(0f, 1f, this.percentage);
		if (this.isLocal)
		{
			base.transform.localPosition = this.path.Interp(Mathf.Clamp(num, 0f, 1f));
		}
		else
		{
			base.transform.position = this.path.Interp(Mathf.Clamp(num, 0f, 1f));
		}
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			float num2;
			if (this.tweenArguments.Contains("lookahead"))
			{
				num2 = (float)this.tweenArguments["lookahead"];
			}
			else
			{
				num2 = iTween.Defaults.lookAhead;
			}
			float num3 = this.ease(0f, 1f, Mathf.Min(1f, this.percentage + num2));
			this.tweenArguments["looktarget"] = this.path.Interp(Mathf.Clamp(num3, 0f, 1f));
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x000E6B8C File Offset: 0x000E4D8C
	private void ApplyMoveToTargets()
	{
		this.preUpdate = base.transform.position;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localPosition = this.vector3s[2];
		}
		else
		{
			base.transform.position = this.vector3s[2];
		}
		if (this.percentage == 1f)
		{
			if (this.isLocal)
			{
				base.transform.localPosition = this.vector3s[1];
			}
			else
			{
				base.transform.position = this.vector3s[1];
			}
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06002CBB RID: 11451 RVA: 0x000E6D54 File Offset: 0x000E4F54
	private void ApplyMoveByTargets()
	{
		this.preUpdate = base.transform.position;
		Vector3 vector = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			vector = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[4];
		}
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = vector;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06002CBC RID: 11452 RVA: 0x000E6F3C File Offset: 0x000E513C
	private void ApplyScaleToTargets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.localScale = this.vector3s[2];
		if (this.percentage == 1f)
		{
			base.transform.localScale = this.vector3s[1];
		}
	}

	// Token: 0x06002CBD RID: 11453 RVA: 0x000E7060 File Offset: 0x000E5260
	private void ApplyLookToTargets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(this.vector3s[2]);
		}
	}

	// Token: 0x06002CBE RID: 11454 RVA: 0x000E718C File Offset: 0x000E538C
	private void ApplyRotateToTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(this.vector3s[2]);
		}
		if (this.percentage == 1f)
		{
			if (this.isLocal)
			{
				base.transform.localRotation = Quaternion.Euler(this.vector3s[1]);
			}
			else
			{
				base.transform.rotation = Quaternion.Euler(this.vector3s[1]);
			}
		}
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x000E7370 File Offset: 0x000E5570
	private void ApplyRotateAddTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06002CC0 RID: 11456 RVA: 0x000E74F8 File Offset: 0x000E56F8
	private void ApplyShakePositionTargets()
	{
		this.preUpdate = base.transform.position;
		Vector3 vector = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			vector = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[3];
		}
		if (this.percentage == 0f)
		{
			base.transform.Translate(this.vector3s[1], this.space);
		}
		base.transform.position = this.vector3s[0];
		float num = 1f - this.percentage;
		this.vector3s[2].x = UnityEngine.Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = UnityEngine.Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = UnityEngine.Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		base.transform.Translate(this.vector3s[2], this.space);
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = vector;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06002CC1 RID: 11457 RVA: 0x000E76F0 File Offset: 0x000E58F0
	private void ApplyShakeScaleTargets()
	{
		if (this.percentage == 0f)
		{
			base.transform.localScale = this.vector3s[1];
		}
		base.transform.localScale = this.vector3s[0];
		float num = 1f - this.percentage;
		this.vector3s[2].x = UnityEngine.Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = UnityEngine.Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = UnityEngine.Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		base.transform.localScale += this.vector3s[2];
	}

	// Token: 0x06002CC2 RID: 11458 RVA: 0x000E7830 File Offset: 0x000E5A30
	private void ApplyShakeRotationTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		if (this.percentage == 0f)
		{
			base.transform.Rotate(this.vector3s[1], this.space);
		}
		base.transform.eulerAngles = this.vector3s[0];
		float num = 1f - this.percentage;
		this.vector3s[2].x = UnityEngine.Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = UnityEngine.Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = UnityEngine.Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		base.transform.Rotate(this.vector3s[2], this.space);
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06002CC3 RID: 11459 RVA: 0x000E79C8 File Offset: 0x000E5BC8
	private void ApplyPunchPositionTargets()
	{
		this.preUpdate = base.transform.position;
		Vector3 vector = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			vector = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[4];
		}
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = vector;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06002CC4 RID: 11460 RVA: 0x000E7CBC File Offset: 0x000E5EBC
	private void ApplyPunchRotationTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06002CC5 RID: 11461 RVA: 0x000E7F50 File Offset: 0x000E6150
	private void ApplyPunchScaleTargets()
	{
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.localScale = this.vector3s[0] + this.vector3s[2];
	}

	// Token: 0x06002CC6 RID: 11462 RVA: 0x000E8168 File Offset: 0x000E6368
	private IEnumerator TweenDelay()
	{
		this.delayStarted = Time.time;
		yield return new WaitForSeconds(this.delay);
		if (this.wasPaused)
		{
			this.wasPaused = false;
			this.TweenStart();
		}
		yield break;
	}

	// Token: 0x06002CC7 RID: 11463 RVA: 0x000E8184 File Offset: 0x000E6384
	private void TweenStart()
	{
		this.CallBack("onstart");
		if (!this.loop)
		{
			this.ConflictCheck();
			this.GenerateTargets();
		}
		if (this.type == "stab")
		{
			this.audioSource.PlayOneShot(this.audioSource.clip);
		}
		if (this.type == "move" || this.type == "scale" || this.type == "rotate" || this.type == "punch" || this.type == "shake" || this.type == "curve" || this.type == "look")
		{
			this.EnableKinematic();
		}
		this.isRunning = true;
	}

	// Token: 0x06002CC8 RID: 11464 RVA: 0x000E8280 File Offset: 0x000E6480
	private IEnumerator TweenRestart()
	{
		if (this.delay > 0f)
		{
			this.delayStarted = Time.time;
			yield return new WaitForSeconds(this.delay);
		}
		this.loop = true;
		this.TweenStart();
		yield break;
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x000E829C File Offset: 0x000E649C
	private void TweenUpdate()
	{
		this.apply();
		this.CallBack("onupdate");
		this.UpdatePercentage();
	}

	// Token: 0x06002CCA RID: 11466 RVA: 0x000E82BC File Offset: 0x000E64BC
	private void TweenComplete()
	{
		this.isRunning = false;
		if (this.percentage > 0.5f)
		{
			this.percentage = 1f;
		}
		else
		{
			this.percentage = 0f;
		}
		this.apply();
		if (this.type == "value")
		{
			this.CallBack("onupdate");
		}
		if (this.loopType == iTween.LoopType.none)
		{
			this.Dispose();
		}
		else
		{
			this.TweenLoop();
		}
		this.CallBack("oncomplete");
	}

	// Token: 0x06002CCB RID: 11467 RVA: 0x000E8350 File Offset: 0x000E6550
	private void TweenLoop()
	{
		this.DisableKinematic();
		iTween.LoopType loopType = this.loopType;
		if (loopType != iTween.LoopType.loop)
		{
			if (loopType == iTween.LoopType.pingPong)
			{
				this.reverse = !this.reverse;
				this.runningTime = 0f;
				base.StartCoroutine("TweenRestart");
			}
		}
		else
		{
			this.percentage = 0f;
			this.runningTime = 0f;
			this.apply();
			base.StartCoroutine("TweenRestart");
		}
	}

	// Token: 0x06002CCC RID: 11468 RVA: 0x000E83DC File Offset: 0x000E65DC
	public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
	{
		Rect rect = new Rect(iTween.FloatUpdate(currentValue.x, targetValue.x, speed), iTween.FloatUpdate(currentValue.y, targetValue.y, speed), iTween.FloatUpdate(currentValue.width, targetValue.width, speed), iTween.FloatUpdate(currentValue.height, targetValue.height, speed));
		return rect;
	}

	// Token: 0x06002CCD RID: 11469 RVA: 0x000E8444 File Offset: 0x000E6644
	public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
	{
		Vector3 vector = targetValue - currentValue;
		currentValue += vector * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x06002CCE RID: 11470 RVA: 0x000E8474 File Offset: 0x000E6674
	public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
	{
		Vector2 vector = targetValue - currentValue;
		currentValue += vector * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x06002CCF RID: 11471 RVA: 0x000E84A4 File Offset: 0x000E66A4
	public static float FloatUpdate(float currentValue, float targetValue, float speed)
	{
		float num = targetValue - currentValue;
		currentValue += num * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x06002CD0 RID: 11472 RVA: 0x000E84C4 File Offset: 0x000E66C4
	public static void FadeUpdate(GameObject target, Hashtable args)
	{
		args["a"] = args["alpha"];
		iTween.ColorUpdate(target, args);
	}

	// Token: 0x06002CD1 RID: 11473 RVA: 0x000E84E4 File Offset: 0x000E66E4
	public static void FadeUpdate(GameObject target, float alpha, float time)
	{
		iTween.FadeUpdate(target, iTween.Hash(new object[] { "alpha", alpha, "time", time }));
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x000E851C File Offset: 0x000E671C
	public static void ColorUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Color[] array = new Color[4];
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					iTween.ColorUpdate(transform.gameObject, args);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			array[0] = (array[1] = target.GetComponent<GUITexture>().color);
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			array[0] = (array[1] = target.GetComponent<GUIText>().material.color);
		}
		else if (target.GetComponent<Renderer>())
		{
			array[0] = (array[1] = target.GetComponent<Renderer>().material.color);
		}
		else if (target.GetComponent<Light>())
		{
			array[0] = (array[1] = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			array[1] = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				array[1].r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				array[1].g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				array[1].b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				array[1].a = (float)args["a"];
			}
		}
		array[3].r = Mathf.SmoothDamp(array[0].r, array[1].r, ref array[2].r, num);
		array[3].g = Mathf.SmoothDamp(array[0].g, array[1].g, ref array[2].g, num);
		array[3].b = Mathf.SmoothDamp(array[0].b, array[1].b, ref array[2].b, num);
		array[3].a = Mathf.SmoothDamp(array[0].a, array[1].a, ref array[2].a, num);
		if (target.GetComponent(typeof(GUITexture)))
		{
			target.GetComponent<GUITexture>().color = array[3];
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			target.GetComponent<GUIText>().material.color = array[3];
		}
		else if (target.GetComponent<Renderer>())
		{
			target.GetComponent<Renderer>().material.color = array[3];
		}
		else if (target.GetComponent<Light>())
		{
			target.GetComponent<Light>().color = array[3];
		}
	}

	// Token: 0x06002CD3 RID: 11475 RVA: 0x000E8980 File Offset: 0x000E6B80
	public static void ColorUpdate(GameObject target, Color color, float time)
	{
		iTween.ColorUpdate(target, iTween.Hash(new object[] { "color", color, "time", time }));
	}

	// Token: 0x06002CD4 RID: 11476 RVA: 0x000E89B8 File Offset: 0x000E6BB8
	public static void AudioUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector2[] array = new Vector2[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent(typeof(AudioSource)))
			{
				Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.");
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		array[0] = (array[1] = new Vector2(audioSource.volume, audioSource.pitch));
		if (args.Contains("volume"))
		{
			array[1].x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			array[1].y = (float)args["pitch"];
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		audioSource.volume = array[3].x;
		audioSource.pitch = array[3].y;
	}

	// Token: 0x06002CD5 RID: 11477 RVA: 0x000E8B74 File Offset: 0x000E6D74
	public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
	{
		iTween.AudioUpdate(target, iTween.Hash(new object[] { "volume", volume, "pitch", pitch, "time", time }));
	}

	// Token: 0x06002CD6 RID: 11478 RVA: 0x000E8BC8 File Offset: 0x000E6DC8
	public static void RotateUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 eulerAngles = target.transform.eulerAngles;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		if (flag)
		{
			array[0] = target.transform.localEulerAngles;
		}
		else
		{
			array[0] = target.transform.eulerAngles;
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				array[1] = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["rotation"];
			}
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
		if (flag)
		{
			target.transform.localEulerAngles = array[3];
		}
		else
		{
			target.transform.eulerAngles = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			target.transform.eulerAngles = eulerAngles;
			target.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(eulerAngles2));
		}
	}

	// Token: 0x06002CD7 RID: 11479 RVA: 0x000E8E34 File Offset: 0x000E7034
	public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
	{
		iTween.RotateUpdate(target, iTween.Hash(new object[] { "rotation", rotation, "time", time }));
	}

	// Token: 0x06002CD8 RID: 11480 RVA: 0x000E8E6C File Offset: 0x000E706C
	public static void ScaleUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		array[0] = (array[1] = target.transform.localScale);
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				array[1] = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		target.transform.localScale = array[3];
	}

	// Token: 0x06002CD9 RID: 11481 RVA: 0x000E90B8 File Offset: 0x000E72B8
	public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
	{
		iTween.ScaleUpdate(target, iTween.Hash(new object[] { "scale", scale, "time", time }));
	}

	// Token: 0x06002CDA RID: 11482 RVA: 0x000E90F0 File Offset: 0x000E72F0
	public static void MoveUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 position = target.transform.position;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = iTween.Defaults.isLocal;
		}
		if (flag)
		{
			array[0] = (array[1] = target.transform.localPosition);
		}
		else
		{
			array[0] = (array[1] = target.transform.position);
		}
		if (args.Contains("position"))
		{
			if (args["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["position"];
				array[1] = transform.position;
			}
			else if (args["position"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["position"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		if (args.Contains("orienttopath") && (bool)args["orienttopath"])
		{
			args["looktarget"] = array[3];
		}
		if (args.Contains("looktarget"))
		{
			iTween.LookUpdate(target, args);
		}
		if (flag)
		{
			target.transform.localPosition = array[3];
		}
		else
		{
			target.transform.position = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 position2 = target.transform.position;
			target.transform.position = position;
			target.GetComponent<Rigidbody>().MovePosition(position2);
		}
	}

	// Token: 0x06002CDB RID: 11483 RVA: 0x000E945C File Offset: 0x000E765C
	public static void MoveUpdate(GameObject target, Vector3 position, float time)
	{
		iTween.MoveUpdate(target, iTween.Hash(new object[] { "position", position, "time", time }));
	}

	// Token: 0x06002CDC RID: 11484 RVA: 0x000E9494 File Offset: 0x000E7694
	public static void LookUpdate(GameObject target, Hashtable args)
	{
		iTween.CleanArgs(args);
		Vector3[] array = new Vector3[5];
		float num;
		if (args.Contains("looktime"))
		{
			num = (float)args["looktime"];
			num *= iTween.Defaults.updateTimePercentage;
		}
		else if (args.Contains("time"))
		{
			num = (float)args["time"] * 0.15f;
			num *= iTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = iTween.Defaults.updateTime;
		}
		array[0] = target.transform.eulerAngles;
		if (args.Contains("looktarget"))
		{
			if (args["looktarget"].GetType() == typeof(Transform))
			{
				Transform transform = target.transform;
				Transform transform2 = (Transform)args["looktarget"];
				Vector3? vector = (Vector3?)args["up"];
				transform.LookAt(transform2, (vector == null) ? iTween.Defaults.up : vector.Value);
			}
			else if (args["looktarget"].GetType() == typeof(Vector3))
			{
				Transform transform3 = target.transform;
				Vector3 vector2 = (Vector3)args["looktarget"];
				Vector3? vector3 = (Vector3?)args["up"];
				transform3.LookAt(vector2, (vector3 == null) ? iTween.Defaults.up : vector3.Value);
			}
			array[1] = target.transform.eulerAngles;
			target.transform.eulerAngles = array[0];
			array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
			array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
			array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
			target.transform.eulerAngles = array[3];
			if (args.Contains("axis"))
			{
				array[4] = target.transform.eulerAngles;
				string text = (string)args["axis"];
				if (text != null)
				{
					if (!(text == "x"))
					{
						if (!(text == "y"))
						{
							if (text == "z")
							{
								array[4].x = array[0].x;
								array[4].y = array[0].y;
							}
						}
						else
						{
							array[4].x = array[0].x;
							array[4].z = array[0].z;
						}
					}
					else
					{
						array[4].y = array[0].y;
						array[4].z = array[0].z;
					}
				}
				target.transform.eulerAngles = array[4];
			}
			return;
		}
		Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!");
	}

	// Token: 0x06002CDD RID: 11485 RVA: 0x000E9838 File Offset: 0x000E7A38
	public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
	{
		iTween.LookUpdate(target, iTween.Hash(new object[] { "looktarget", looktarget, "time", time }));
	}

	// Token: 0x06002CDE RID: 11486 RVA: 0x000E9870 File Offset: 0x000E7A70
	public static float PathLength(Transform[] path)
	{
		Vector3[] array = new Vector3[path.Length];
		float num = 0f;
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		Vector3[] array2 = iTween.PathControlPointGenerator(array);
		Vector3 vector = iTween.Interp(array2, 0f);
		int num2 = path.Length * 20;
		for (int j = 1; j <= num2; j++)
		{
			float num3 = (float)j / (float)num2;
			Vector3 vector2 = iTween.Interp(array2, num3);
			num += Vector3.Distance(vector, vector2);
			vector = vector2;
		}
		return num;
	}

	// Token: 0x06002CDF RID: 11487 RVA: 0x000E990C File Offset: 0x000E7B0C
	public static float PathLength(Vector3[] path)
	{
		float num = 0f;
		Vector3[] array = iTween.PathControlPointGenerator(path);
		Vector3 vector = iTween.Interp(array, 0f);
		int num2 = path.Length * 20;
		for (int i = 1; i <= num2; i++)
		{
			float num3 = (float)i / (float)num2;
			Vector3 vector2 = iTween.Interp(array, num3);
			num += Vector3.Distance(vector, vector2);
			vector = vector2;
		}
		return num;
	}

	// Token: 0x06002CE0 RID: 11488 RVA: 0x000E9970 File Offset: 0x000E7B70
	public static Texture2D CameraTexture(Color color)
	{
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
		Color[] array = new Color[Screen.width * Screen.height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06002CE1 RID: 11489 RVA: 0x000E99D0 File Offset: 0x000E7BD0
	public static void PutOnPath(GameObject target, Vector3[] path, float percent)
	{
		target.transform.position = iTween.Interp(iTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06002CE2 RID: 11490 RVA: 0x000E99EC File Offset: 0x000E7BEC
	public static void PutOnPath(Transform target, Vector3[] path, float percent)
	{
		target.position = iTween.Interp(iTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06002CE3 RID: 11491 RVA: 0x000E9A00 File Offset: 0x000E7C00
	public static void PutOnPath(GameObject target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.transform.position = iTween.Interp(iTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06002CE4 RID: 11492 RVA: 0x000E9A58 File Offset: 0x000E7C58
	public static void PutOnPath(Transform target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.position = iTween.Interp(iTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06002CE5 RID: 11493 RVA: 0x000E9AA8 File Offset: 0x000E7CA8
	public static Vector3 PointOnPath(Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		return iTween.Interp(iTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06002CE6 RID: 11494 RVA: 0x000E9AF4 File Offset: 0x000E7CF4
	public static void DrawLine(Vector3[] line)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06002CE7 RID: 11495 RVA: 0x000E9B10 File Offset: 0x000E7D10
	public static void DrawLine(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, color, "gizmos");
		}
	}

	// Token: 0x06002CE8 RID: 11496 RVA: 0x000E9B28 File Offset: 0x000E7D28
	public static void DrawLine(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06002CE9 RID: 11497 RVA: 0x000E9B80 File Offset: 0x000E7D80
	public static void DrawLine(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06002CEA RID: 11498 RVA: 0x000E9BD4 File Offset: 0x000E7DD4
	public static void DrawLineGizmos(Vector3[] line)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06002CEB RID: 11499 RVA: 0x000E9BF0 File Offset: 0x000E7DF0
	public static void DrawLineGizmos(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, color, "gizmos");
		}
	}

	// Token: 0x06002CEC RID: 11500 RVA: 0x000E9C08 File Offset: 0x000E7E08
	public static void DrawLineGizmos(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06002CED RID: 11501 RVA: 0x000E9C60 File Offset: 0x000E7E60
	public static void DrawLineGizmos(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06002CEE RID: 11502 RVA: 0x000E9CB4 File Offset: 0x000E7EB4
	public static void DrawLineHandles(Vector3[] line)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06002CEF RID: 11503 RVA: 0x000E9CD0 File Offset: 0x000E7ED0
	public static void DrawLineHandles(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			iTween.DrawLineHelper(line, color, "handles");
		}
	}

	// Token: 0x06002CF0 RID: 11504 RVA: 0x000E9CE8 File Offset: 0x000E7EE8
	public static void DrawLineHandles(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06002CF1 RID: 11505 RVA: 0x000E9D40 File Offset: 0x000E7F40
	public static void DrawLineHandles(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			iTween.DrawLineHelper(array, color, "handles");
		}
	}

	// Token: 0x06002CF2 RID: 11506 RVA: 0x000E9D94 File Offset: 0x000E7F94
	public static Vector3 PointOnPath(Vector3[] path, float percent)
	{
		return iTween.Interp(iTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06002CF3 RID: 11507 RVA: 0x000E9DA4 File Offset: 0x000E7FA4
	public static void DrawPath(Vector3[] path)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06002CF4 RID: 11508 RVA: 0x000E9DC0 File Offset: 0x000E7FC0
	public static void DrawPath(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, color, "gizmos");
		}
	}

	// Token: 0x06002CF5 RID: 11509 RVA: 0x000E9DD8 File Offset: 0x000E7FD8
	public static void DrawPath(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06002CF6 RID: 11510 RVA: 0x000E9E30 File Offset: 0x000E8030
	public static void DrawPath(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06002CF7 RID: 11511 RVA: 0x000E9E84 File Offset: 0x000E8084
	public static void DrawPathGizmos(Vector3[] path)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06002CF8 RID: 11512 RVA: 0x000E9EA0 File Offset: 0x000E80A0
	public static void DrawPathGizmos(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, color, "gizmos");
		}
	}

	// Token: 0x06002CF9 RID: 11513 RVA: 0x000E9EB8 File Offset: 0x000E80B8
	public static void DrawPathGizmos(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, iTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06002CFA RID: 11514 RVA: 0x000E9F10 File Offset: 0x000E8110
	public static void DrawPathGizmos(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06002CFB RID: 11515 RVA: 0x000E9F64 File Offset: 0x000E8164
	public static void DrawPathHandles(Vector3[] path)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06002CFC RID: 11516 RVA: 0x000E9F80 File Offset: 0x000E8180
	public static void DrawPathHandles(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			iTween.DrawPathHelper(path, color, "handles");
		}
	}

	// Token: 0x06002CFD RID: 11517 RVA: 0x000E9F98 File Offset: 0x000E8198
	public static void DrawPathHandles(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, iTween.Defaults.color, "handles");
		}
	}

	// Token: 0x06002CFE RID: 11518 RVA: 0x000E9FF0 File Offset: 0x000E81F0
	public static void DrawPathHandles(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			iTween.DrawPathHelper(array, color, "handles");
		}
	}

	// Token: 0x06002CFF RID: 11519 RVA: 0x000EA044 File Offset: 0x000E8244
	public static void CameraFadeDepth(int depth)
	{
		if (iTween.cameraFade)
		{
			iTween.cameraFade.transform.position = new Vector3(iTween.cameraFade.transform.position.x, iTween.cameraFade.transform.position.y, (float)depth);
		}
	}

	// Token: 0x06002D00 RID: 11520 RVA: 0x000EA0A4 File Offset: 0x000E82A4
	public static void CameraFadeDestroy()
	{
		if (iTween.cameraFade)
		{
			UnityEngine.Object.Destroy(iTween.cameraFade);
		}
	}

	// Token: 0x06002D01 RID: 11521 RVA: 0x000EA0C0 File Offset: 0x000E82C0
	public static void CameraFadeSwap(Texture2D texture)
	{
		if (iTween.cameraFade)
		{
			iTween.cameraFade.GetComponent<GUITexture>().texture = texture;
		}
	}

	// Token: 0x06002D02 RID: 11522 RVA: 0x000EA0E4 File Offset: 0x000E82E4
	public static GameObject CameraFadeAdd(Texture2D texture, int depth)
	{
		if (iTween.cameraFade)
		{
			return null;
		}
		iTween.cameraFade = new GameObject("iTween Camera Fade");
		iTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)depth);
		iTween.cameraFade.AddComponent<GUITexture>();
		iTween.cameraFade.GetComponent<GUITexture>().texture = texture;
		iTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return iTween.cameraFade;
	}

	// Token: 0x06002D03 RID: 11523 RVA: 0x000EA17C File Offset: 0x000E837C
	public static GameObject CameraFadeAdd(Texture2D texture)
	{
		if (iTween.cameraFade)
		{
			return null;
		}
		iTween.cameraFade = new GameObject("iTween Camera Fade");
		iTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)iTween.Defaults.cameraFadeDepth);
		iTween.cameraFade.AddComponent<GUITexture>();
		iTween.cameraFade.GetComponent<GUITexture>().texture = texture;
		iTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return iTween.cameraFade;
	}

	// Token: 0x06002D04 RID: 11524 RVA: 0x000EA218 File Offset: 0x000E8418
	public static GameObject CameraFadeAdd()
	{
		if (iTween.cameraFade)
		{
			return null;
		}
		iTween.cameraFade = new GameObject("iTween Camera Fade");
		iTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)iTween.Defaults.cameraFadeDepth);
		iTween.cameraFade.AddComponent<GUITexture>();
		iTween.cameraFade.GetComponent<GUITexture>().texture = iTween.CameraTexture(Color.black);
		iTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return iTween.cameraFade;
	}

	// Token: 0x06002D05 RID: 11525 RVA: 0x000EA2BC File Offset: 0x000E84BC
	public static void Resume(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			iTween.enabled = true;
		}
	}

	// Token: 0x06002D06 RID: 11526 RVA: 0x000EA300 File Offset: 0x000E8500
	public static void Resume(GameObject target, bool includechildren)
	{
		iTween.Resume(target);
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					iTween.Resume(transform.gameObject, true);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	// Token: 0x06002D07 RID: 11527 RVA: 0x000EA378 File Offset: 0x000E8578
	public static void Resume(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				iTween.enabled = true;
			}
		}
	}

	// Token: 0x06002D08 RID: 11528 RVA: 0x000EA3F8 File Offset: 0x000E85F8
	public static void Resume(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				iTween.enabled = true;
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					iTween.Resume(transform.gameObject, type, true);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	// Token: 0x06002D09 RID: 11529 RVA: 0x000EA4E4 File Offset: 0x000E86E4
	public static void Resume()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject gameObject = (GameObject)hashtable["target"];
			iTween.Resume(gameObject);
		}
	}

	// Token: 0x06002D0A RID: 11530 RVA: 0x000EA534 File Offset: 0x000E8734
	public static void Resume(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject gameObject = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, gameObject);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.Resume((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x06002D0B RID: 11531 RVA: 0x000EA5C0 File Offset: 0x000E87C0
	public static void Pause(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			if (iTween.delay > 0f)
			{
				iTween.delay -= Time.time - iTween.delayStarted;
				iTween.StopCoroutine("TweenDelay");
			}
			iTween.isPaused = true;
			iTween.enabled = false;
		}
	}

	// Token: 0x06002D0C RID: 11532 RVA: 0x000EA640 File Offset: 0x000E8840
	public static void Pause(GameObject target, bool includechildren)
	{
		iTween.Pause(target);
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					iTween.Pause(transform.gameObject, true);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	// Token: 0x06002D0D RID: 11533 RVA: 0x000EA6B8 File Offset: 0x000E88B8
	public static void Pause(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				if (iTween.delay > 0f)
				{
					iTween.delay -= Time.time - iTween.delayStarted;
					iTween.StopCoroutine("TweenDelay");
				}
				iTween.isPaused = true;
				iTween.enabled = false;
			}
		}
	}

	// Token: 0x06002D0E RID: 11534 RVA: 0x000EA774 File Offset: 0x000E8974
	public static void Pause(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				if (iTween.delay > 0f)
				{
					iTween.delay -= Time.time - iTween.delayStarted;
					iTween.StopCoroutine("TweenDelay");
				}
				iTween.isPaused = true;
				iTween.enabled = false;
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					iTween.Pause(transform.gameObject, type, true);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	// Token: 0x06002D0F RID: 11535 RVA: 0x000EA89C File Offset: 0x000E8A9C
	public static void Pause()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject gameObject = (GameObject)hashtable["target"];
			iTween.Pause(gameObject);
		}
	}

	// Token: 0x06002D10 RID: 11536 RVA: 0x000EA8EC File Offset: 0x000E8AEC
	public static void Pause(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject gameObject = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, gameObject);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.Pause((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x06002D11 RID: 11537 RVA: 0x000EA978 File Offset: 0x000E8B78
	public static int Count()
	{
		return iTween.tweens.Count;
	}

	// Token: 0x06002D12 RID: 11538 RVA: 0x000EA984 File Offset: 0x000E8B84
	public static int Count(string type)
	{
		int num = 0;
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			string text = (string)hashtable["type"] + (string)hashtable["method"];
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002D13 RID: 11539 RVA: 0x000EAA10 File Offset: 0x000E8C10
	public static int Count(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		return components.Length;
	}

	// Token: 0x06002D14 RID: 11540 RVA: 0x000EAA34 File Offset: 0x000E8C34
	public static int Count(GameObject target, string type)
	{
		int num = 0;
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002D15 RID: 11541 RVA: 0x000EAAB8 File Offset: 0x000E8CB8
	public static void Stop()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject gameObject = (GameObject)hashtable["target"];
			iTween.Stop(gameObject);
		}
		iTween.tweens.Clear();
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x000EAB14 File Offset: 0x000E8D14
	public static void Stop(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject gameObject = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, gameObject);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.Stop((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x06002D17 RID: 11543 RVA: 0x000EABA0 File Offset: 0x000E8DA0
	public static void StopByName(string name)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			GameObject gameObject = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, gameObject);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			iTween.StopByName((GameObject)arrayList[j], name);
		}
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x000EAC2C File Offset: 0x000E8E2C
	public static void Stop(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			iTween.Dispose();
		}
	}

	// Token: 0x06002D19 RID: 11545 RVA: 0x000EAC70 File Offset: 0x000E8E70
	public static void Stop(GameObject target, bool includechildren)
	{
		iTween.Stop(target);
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					iTween.Stop(transform.gameObject, true);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x000EACE8 File Offset: 0x000E8EE8
	public static void Stop(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				iTween.Dispose();
			}
		}
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x000EAD68 File Offset: 0x000E8F68
	public static void StopByName(GameObject target, string name)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			if (iTween._name == name)
			{
				iTween.Dispose();
			}
		}
	}

	// Token: 0x06002D1C RID: 11548 RVA: 0x000EADBC File Offset: 0x000E8FBC
	public static void Stop(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			string text = iTween.type + iTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				iTween.Dispose();
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					iTween.Stop(transform.gameObject, type, true);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	// Token: 0x06002D1D RID: 11549 RVA: 0x000EAEA8 File Offset: 0x000E90A8
	public static void StopByName(GameObject target, string name, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			if (iTween._name == name)
			{
				iTween.Dispose();
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					iTween.StopByName(transform.gameObject, name, true);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	// Token: 0x06002D1E RID: 11550 RVA: 0x000EAF68 File Offset: 0x000E9168
	public static Hashtable Hash(params object[] args)
	{
		Hashtable hashtable = new Hashtable(args.Length / 2);
		if (args.Length % 2 != 0)
		{
			Debug.LogError("Tween Error: Hash requires an even number of arguments!");
			return null;
		}
		for (int i = 0; i < args.Length - 1; i += 2)
		{
			hashtable.Add(args[i], args[i + 1]);
		}
		return hashtable;
	}

	// Token: 0x06002D1F RID: 11551 RVA: 0x000EAFBC File Offset: 0x000E91BC
	private void Awake()
	{
		this.RetrieveArgs();
		this.lastRealTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06002D20 RID: 11552 RVA: 0x000EAFD0 File Offset: 0x000E91D0
	private IEnumerator Start()
	{
		if (this.delay > 0f)
		{
			yield return base.StartCoroutine("TweenDelay");
		}
		this.TweenStart();
		yield break;
	}

	// Token: 0x06002D21 RID: 11553 RVA: 0x000EAFEC File Offset: 0x000E91EC
	private void Update()
	{
		if (this.isRunning && !this.physics)
		{
			if (!this.reverse)
			{
				if (this.percentage < 1f)
				{
					this.TweenUpdate();
				}
				else
				{
					this.TweenComplete();
				}
			}
			else if (this.percentage > 0f)
			{
				this.TweenUpdate();
			}
			else
			{
				this.TweenComplete();
			}
		}
	}

	// Token: 0x06002D22 RID: 11554 RVA: 0x000EB064 File Offset: 0x000E9264
	private void FixedUpdate()
	{
		if (this.isRunning && this.physics)
		{
			if (!this.reverse)
			{
				if (this.percentage < 1f)
				{
					this.TweenUpdate();
				}
				else
				{
					this.TweenComplete();
				}
			}
			else if (this.percentage > 0f)
			{
				this.TweenUpdate();
			}
			else
			{
				this.TweenComplete();
			}
		}
	}

	// Token: 0x06002D23 RID: 11555 RVA: 0x000EB0DC File Offset: 0x000E92DC
	private void LateUpdate()
	{
		if (this.tweenArguments.Contains("looktarget") && this.isRunning && (this.type == "move" || this.type == "shake" || this.type == "punch"))
		{
			iTween.LookUpdate(base.gameObject, this.tweenArguments);
		}
	}

	// Token: 0x06002D24 RID: 11556 RVA: 0x000EB15C File Offset: 0x000E935C
	private void OnEnable()
	{
		if (this.isRunning)
		{
			this.EnableKinematic();
		}
		if (this.isPaused)
		{
			this.isPaused = false;
			if (this.delay > 0f)
			{
				this.wasPaused = true;
				this.ResumeDelay();
			}
		}
	}

	// Token: 0x06002D25 RID: 11557 RVA: 0x000EB1AC File Offset: 0x000E93AC
	private void OnDisable()
	{
		this.DisableKinematic();
	}

	// Token: 0x06002D26 RID: 11558 RVA: 0x000EB1B4 File Offset: 0x000E93B4
	private static void DrawLineHelper(Vector3[] line, Color color, string method)
	{
		Gizmos.color = color;
		for (int i = 0; i < line.Length - 1; i++)
		{
			if (method == "gizmos")
			{
				Gizmos.DrawLine(line[i], line[i + 1]);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
		}
	}

	// Token: 0x06002D27 RID: 11559 RVA: 0x000EB22C File Offset: 0x000E942C
	private static void DrawPathHelper(Vector3[] path, Color color, string method)
	{
		Vector3[] array = iTween.PathControlPointGenerator(path);
		Vector3 vector = iTween.Interp(array, 0f);
		Gizmos.color = color;
		int num = path.Length * 20;
		for (int i = 1; i <= num; i++)
		{
			float num2 = (float)i / (float)num;
			Vector3 vector2 = iTween.Interp(array, num2);
			if (method == "gizmos")
			{
				Gizmos.DrawLine(vector2, vector);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
			vector = vector2;
		}
	}

	// Token: 0x06002D28 RID: 11560 RVA: 0x000EB2B8 File Offset: 0x000E94B8
	private static Vector3[] PathControlPointGenerator(Vector3[] path)
	{
		int num = 2;
		Vector3[] array = new Vector3[path.Length + num];
		Array.Copy(path, 0, array, 1, path.Length);
		array[0] = array[1] + (array[1] - array[2]);
		array[array.Length - 1] = array[array.Length - 2] + (array[array.Length - 2] - array[array.Length - 3]);
		if (array[1] == array[array.Length - 2])
		{
			Vector3[] array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
			array2[0] = array2[array2.Length - 3];
			array2[array2.Length - 1] = array2[2];
			array = new Vector3[array2.Length];
			Array.Copy(array2, array, array2.Length);
		}
		return array;
	}

	// Token: 0x06002D29 RID: 11561 RVA: 0x000EB3EC File Offset: 0x000E95EC
	private static Vector3 Interp(Vector3[] pts, float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 vector = pts[num2];
		Vector3 vector2 = pts[num2 + 1];
		Vector3 vector3 = pts[num2 + 2];
		Vector3 vector4 = pts[num2 + 3];
		return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
	}

	// Token: 0x06002D2A RID: 11562 RVA: 0x000EB504 File Offset: 0x000E9704
	private static void Launch(GameObject target, Hashtable args)
	{
		if (!args.Contains("id"))
		{
			args["id"] = iTween.GenerateID();
		}
		if (!args.Contains("target"))
		{
			args["target"] = target;
		}
		iTween.tweens.Insert(0, args);
		target.AddComponent<iTween>();
	}

	// Token: 0x06002D2B RID: 11563 RVA: 0x000EB560 File Offset: 0x000E9760
	private static Hashtable CleanArgs(Hashtable args)
	{
		Hashtable hashtable = new Hashtable(args.Count);
		Hashtable hashtable2 = new Hashtable(args.Count);
		IDictionaryEnumerator enumerator = args.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		IDictionaryEnumerator enumerator2 = hashtable.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
				if (dictionaryEntry2.Value.GetType() == typeof(int))
				{
					int num = (int)dictionaryEntry2.Value;
					float num2 = (float)num;
					args[dictionaryEntry2.Key] = num2;
				}
				if (dictionaryEntry2.Value.GetType() == typeof(double))
				{
					double num3 = (double)dictionaryEntry2.Value;
					float num4 = (float)num3;
					args[dictionaryEntry2.Key] = num4;
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = enumerator2 as IDisposable) != null)
			{
				disposable2.Dispose();
			}
		}
		IDictionaryEnumerator enumerator3 = args.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				object obj3 = enumerator3.Current;
				DictionaryEntry dictionaryEntry3 = (DictionaryEntry)obj3;
				hashtable2.Add(dictionaryEntry3.Key.ToString().ToLower(), dictionaryEntry3.Value);
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = enumerator3 as IDisposable) != null)
			{
				disposable3.Dispose();
			}
		}
		args = hashtable2;
		return args;
	}

	// Token: 0x06002D2C RID: 11564 RVA: 0x000EB72C File Offset: 0x000E992C
	private static string GenerateID()
	{
		int num = 15;
		char[] array = new char[]
		{
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
			'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
			'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D',
			'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
			'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
			'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7',
			'8'
		};
		int num2 = array.Length - 1;
		string text = string.Empty;
		for (int i = 0; i < num; i++)
		{
			text += array[(int)Mathf.Floor((float)UnityEngine.Random.Range(0, num2))];
		}
		return text;
	}

	// Token: 0x06002D2D RID: 11565 RVA: 0x000EB790 File Offset: 0x000E9990
	private void RetrieveArgs()
	{
		IEnumerator enumerator = iTween.tweens.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Hashtable hashtable = (Hashtable)obj;
				if ((GameObject)hashtable["target"] == base.gameObject)
				{
					this.tweenArguments = hashtable;
					break;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		this.id = (string)this.tweenArguments["id"];
		this.type = (string)this.tweenArguments["type"];
		this._name = (string)this.tweenArguments["name"];
		this.method = (string)this.tweenArguments["method"];
		if (this.tweenArguments.Contains("time"))
		{
			this.time = (float)this.tweenArguments["time"];
		}
		else
		{
			this.time = iTween.Defaults.time;
		}
		if (base.GetComponent<Rigidbody>() != null)
		{
			this.physics = true;
		}
		if (this.tweenArguments.Contains("delay"))
		{
			this.delay = (float)this.tweenArguments["delay"];
		}
		else
		{
			this.delay = iTween.Defaults.delay;
		}
		if (this.tweenArguments.Contains("namedcolorvalue"))
		{
			if (this.tweenArguments["namedcolorvalue"].GetType() == typeof(iTween.NamedValueColor))
			{
				this.namedcolorvalue = (iTween.NamedValueColor)this.tweenArguments["namedcolorvalue"];
			}
			else
			{
				try
				{
					this.namedcolorvalue = (iTween.NamedValueColor)Enum.Parse(typeof(iTween.NamedValueColor), (string)this.tweenArguments["namedcolorvalue"], true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported namedcolorvalue supplied! Default will be used.");
					this.namedcolorvalue = iTween.NamedValueColor._Color;
				}
			}
		}
		else
		{
			this.namedcolorvalue = iTween.Defaults.namedColorValue;
		}
		if (this.tweenArguments.Contains("looptype"))
		{
			if (this.tweenArguments["looptype"].GetType() == typeof(iTween.LoopType))
			{
				this.loopType = (iTween.LoopType)this.tweenArguments["looptype"];
			}
			else
			{
				try
				{
					this.loopType = (iTween.LoopType)Enum.Parse(typeof(iTween.LoopType), (string)this.tweenArguments["looptype"], true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported loopType supplied! Default will be used.");
					this.loopType = iTween.LoopType.none;
				}
			}
		}
		else
		{
			this.loopType = iTween.LoopType.none;
		}
		if (this.tweenArguments.Contains("easetype"))
		{
			if (this.tweenArguments["easetype"].GetType() == typeof(iTween.EaseType))
			{
				this.easeType = (iTween.EaseType)this.tweenArguments["easetype"];
			}
			else
			{
				try
				{
					this.easeType = (iTween.EaseType)Enum.Parse(typeof(iTween.EaseType), (string)this.tweenArguments["easetype"], true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported easeType supplied! Default will be used.");
					this.easeType = iTween.Defaults.easeType;
				}
			}
		}
		else
		{
			this.easeType = iTween.Defaults.easeType;
		}
		if (this.tweenArguments.Contains("space"))
		{
			if (this.tweenArguments["space"].GetType() == typeof(Space))
			{
				this.space = (Space)this.tweenArguments["space"];
			}
			else
			{
				try
				{
					this.space = (Space)Enum.Parse(typeof(Space), (string)this.tweenArguments["space"], true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported space supplied! Default will be used.");
					this.space = iTween.Defaults.space;
				}
			}
		}
		else
		{
			this.space = iTween.Defaults.space;
		}
		if (this.tweenArguments.Contains("islocal"))
		{
			this.isLocal = (bool)this.tweenArguments["islocal"];
		}
		else
		{
			this.isLocal = iTween.Defaults.isLocal;
		}
		if (this.tweenArguments.Contains("ignoretimescale"))
		{
			this.useRealTime = (bool)this.tweenArguments["ignoretimescale"];
		}
		else
		{
			this.useRealTime = iTween.Defaults.useRealTime;
		}
		this.GetEasingFunction();
	}

	// Token: 0x06002D2E RID: 11566 RVA: 0x000EBCAC File Offset: 0x000E9EAC
	private void GetEasingFunction()
	{
		switch (this.easeType)
		{
		case iTween.EaseType.easeInQuad:
			this.ease = new iTween.EasingFunction(this.easeInQuad);
			break;
		case iTween.EaseType.easeOutQuad:
			this.ease = new iTween.EasingFunction(this.easeOutQuad);
			break;
		case iTween.EaseType.easeInOutQuad:
			this.ease = new iTween.EasingFunction(this.easeInOutQuad);
			break;
		case iTween.EaseType.easeInCubic:
			this.ease = new iTween.EasingFunction(this.easeInCubic);
			break;
		case iTween.EaseType.easeOutCubic:
			this.ease = new iTween.EasingFunction(this.easeOutCubic);
			break;
		case iTween.EaseType.easeInOutCubic:
			this.ease = new iTween.EasingFunction(this.easeInOutCubic);
			break;
		case iTween.EaseType.easeInQuart:
			this.ease = new iTween.EasingFunction(this.easeInQuart);
			break;
		case iTween.EaseType.easeOutQuart:
			this.ease = new iTween.EasingFunction(this.easeOutQuart);
			break;
		case iTween.EaseType.easeInOutQuart:
			this.ease = new iTween.EasingFunction(this.easeInOutQuart);
			break;
		case iTween.EaseType.easeInQuint:
			this.ease = new iTween.EasingFunction(this.easeInQuint);
			break;
		case iTween.EaseType.easeOutQuint:
			this.ease = new iTween.EasingFunction(this.easeOutQuint);
			break;
		case iTween.EaseType.easeInOutQuint:
			this.ease = new iTween.EasingFunction(this.easeInOutQuint);
			break;
		case iTween.EaseType.easeInSine:
			this.ease = new iTween.EasingFunction(this.easeInSine);
			break;
		case iTween.EaseType.easeOutSine:
			this.ease = new iTween.EasingFunction(this.easeOutSine);
			break;
		case iTween.EaseType.easeInOutSine:
			this.ease = new iTween.EasingFunction(this.easeInOutSine);
			break;
		case iTween.EaseType.easeInExpo:
			this.ease = new iTween.EasingFunction(this.easeInExpo);
			break;
		case iTween.EaseType.easeOutExpo:
			this.ease = new iTween.EasingFunction(this.easeOutExpo);
			break;
		case iTween.EaseType.easeInOutExpo:
			this.ease = new iTween.EasingFunction(this.easeInOutExpo);
			break;
		case iTween.EaseType.easeInCirc:
			this.ease = new iTween.EasingFunction(this.easeInCirc);
			break;
		case iTween.EaseType.easeOutCirc:
			this.ease = new iTween.EasingFunction(this.easeOutCirc);
			break;
		case iTween.EaseType.easeInOutCirc:
			this.ease = new iTween.EasingFunction(this.easeInOutCirc);
			break;
		case iTween.EaseType.linear:
			this.ease = new iTween.EasingFunction(this.linear);
			break;
		case iTween.EaseType.spring:
			this.ease = new iTween.EasingFunction(this.spring);
			break;
		case iTween.EaseType.easeInBounce:
			this.ease = new iTween.EasingFunction(this.easeInBounce);
			break;
		case iTween.EaseType.easeOutBounce:
			this.ease = new iTween.EasingFunction(this.easeOutBounce);
			break;
		case iTween.EaseType.easeInOutBounce:
			this.ease = new iTween.EasingFunction(this.easeInOutBounce);
			break;
		case iTween.EaseType.easeInBack:
			this.ease = new iTween.EasingFunction(this.easeInBack);
			break;
		case iTween.EaseType.easeOutBack:
			this.ease = new iTween.EasingFunction(this.easeOutBack);
			break;
		case iTween.EaseType.easeInOutBack:
			this.ease = new iTween.EasingFunction(this.easeInOutBack);
			break;
		case iTween.EaseType.easeInElastic:
			this.ease = new iTween.EasingFunction(this.easeInElastic);
			break;
		case iTween.EaseType.easeOutElastic:
			this.ease = new iTween.EasingFunction(this.easeOutElastic);
			break;
		case iTween.EaseType.easeInOutElastic:
			this.ease = new iTween.EasingFunction(this.easeInOutElastic);
			break;
		}
	}

	// Token: 0x06002D2F RID: 11567 RVA: 0x000EC02C File Offset: 0x000EA22C
	private void UpdatePercentage()
	{
		if (this.useRealTime)
		{
			this.runningTime += Time.realtimeSinceStartup - this.lastRealTime;
		}
		else
		{
			this.runningTime += Time.deltaTime;
		}
		if (this.reverse)
		{
			this.percentage = 1f - this.runningTime / this.time;
		}
		else
		{
			this.percentage = this.runningTime / this.time;
		}
		this.lastRealTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06002D30 RID: 11568 RVA: 0x000EC0BC File Offset: 0x000EA2BC
	private void CallBack(string callbackType)
	{
		if (this.tweenArguments.Contains(callbackType) && !this.tweenArguments.Contains("ischild"))
		{
			GameObject gameObject;
			if (this.tweenArguments.Contains(callbackType + "target"))
			{
				gameObject = (GameObject)this.tweenArguments[callbackType + "target"];
			}
			else
			{
				gameObject = base.gameObject;
			}
			if (this.tweenArguments[callbackType].GetType() == typeof(string))
			{
				gameObject.SendMessage((string)this.tweenArguments[callbackType], this.tweenArguments[callbackType + "params"], SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				Debug.LogError("iTween Error: Callback method references must be passed as a String!");
				UnityEngine.Object.Destroy(this);
			}
		}
	}

	// Token: 0x06002D31 RID: 11569 RVA: 0x000EC198 File Offset: 0x000EA398
	private void Dispose()
	{
		for (int i = 0; i < iTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)iTween.tweens[i];
			if ((string)hashtable["id"] == this.id)
			{
				iTween.tweens.RemoveAt(i);
				break;
			}
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06002D32 RID: 11570 RVA: 0x000EC208 File Offset: 0x000EA408
	private void ConflictCheck()
	{
		Component[] components = base.GetComponents(typeof(iTween));
		foreach (iTween iTween in components)
		{
			if (iTween.type == "value")
			{
				return;
			}
			if (iTween.isRunning && iTween.type == this.type)
			{
				if (iTween.method != this.method)
				{
					return;
				}
				if (iTween.tweenArguments.Count != this.tweenArguments.Count)
				{
					iTween.Dispose();
					return;
				}
				IDictionaryEnumerator enumerator = this.tweenArguments.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						if (!iTween.tweenArguments.Contains(dictionaryEntry.Key))
						{
							iTween.Dispose();
							return;
						}
						if (!iTween.tweenArguments[dictionaryEntry.Key].Equals(this.tweenArguments[dictionaryEntry.Key]) && (string)dictionaryEntry.Key != "id")
						{
							iTween.Dispose();
							return;
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = enumerator as IDisposable) != null)
					{
						disposable.Dispose();
					}
				}
				this.Dispose();
			}
		}
	}

	// Token: 0x06002D33 RID: 11571 RVA: 0x000EC38C File Offset: 0x000EA58C
	private void EnableKinematic()
	{
	}

	// Token: 0x06002D34 RID: 11572 RVA: 0x000EC390 File Offset: 0x000EA590
	private void DisableKinematic()
	{
	}

	// Token: 0x06002D35 RID: 11573 RVA: 0x000EC394 File Offset: 0x000EA594
	private void ResumeDelay()
	{
		base.StartCoroutine("TweenDelay");
	}

	// Token: 0x06002D36 RID: 11574 RVA: 0x000EC3A4 File Offset: 0x000EA5A4
	private float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x06002D37 RID: 11575 RVA: 0x000EC3B0 File Offset: 0x000EA5B0
	private float clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float num5;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			num5 = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			num5 = start + num4;
		}
		else
		{
			num5 = start + (end - start) * value;
		}
		return num5;
	}

	// Token: 0x06002D38 RID: 11576 RVA: 0x000EC428 File Offset: 0x000EA628
	private float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x06002D39 RID: 11577 RVA: 0x000EC48C File Offset: 0x000EA68C
	private float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	// Token: 0x06002D3A RID: 11578 RVA: 0x000EC49C File Offset: 0x000EA69C
	private float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2f) + start;
	}

	// Token: 0x06002D3B RID: 11579 RVA: 0x000EC4B4 File Offset: 0x000EA6B4
	private float easeInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value + start;
		}
		value -= 1f;
		return -end / 2f * (value * (value - 2f) - 1f) + start;
	}

	// Token: 0x06002D3C RID: 11580 RVA: 0x000EC50C File Offset: 0x000EA70C
	private float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	// Token: 0x06002D3D RID: 11581 RVA: 0x000EC51C File Offset: 0x000EA71C
	private float easeOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	// Token: 0x06002D3E RID: 11582 RVA: 0x000EC53C File Offset: 0x000EA73C
	private float easeInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value + 2f) + start;
	}

	// Token: 0x06002D3F RID: 11583 RVA: 0x000EC590 File Offset: 0x000EA790
	private float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	// Token: 0x06002D40 RID: 11584 RVA: 0x000EC5A4 File Offset: 0x000EA7A4
	private float easeOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * (value * value * value * value - 1f) + start;
	}

	// Token: 0x06002D41 RID: 11585 RVA: 0x000EC5C8 File Offset: 0x000EA7C8
	private float easeInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value + start;
		}
		value -= 2f;
		return -end / 2f * (value * value * value * value - 2f) + start;
	}

	// Token: 0x06002D42 RID: 11586 RVA: 0x000EC624 File Offset: 0x000EA824
	private float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	// Token: 0x06002D43 RID: 11587 RVA: 0x000EC638 File Offset: 0x000EA838
	private float easeOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	// Token: 0x06002D44 RID: 11588 RVA: 0x000EC65C File Offset: 0x000EA85C
	private float easeInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value * value * value + 2f) + start;
	}

	// Token: 0x06002D45 RID: 11589 RVA: 0x000EC6B8 File Offset: 0x000EA8B8
	private float easeInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value / 1f * 1.5707964f) + end + start;
	}

	// Token: 0x06002D46 RID: 11590 RVA: 0x000EC6D8 File Offset: 0x000EA8D8
	private float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value / 1f * 1.5707964f) + start;
	}

	// Token: 0x06002D47 RID: 11591 RVA: 0x000EC6F8 File Offset: 0x000EA8F8
	private float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.1415927f * value / 1f) - 1f) + start;
	}

	// Token: 0x06002D48 RID: 11592 RVA: 0x000EC724 File Offset: 0x000EA924
	private float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
	}

	// Token: 0x06002D49 RID: 11593 RVA: 0x000EC74C File Offset: 0x000EA94C
	private float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
	}

	// Token: 0x06002D4A RID: 11594 RVA: 0x000EC778 File Offset: 0x000EA978
	private float easeInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	// Token: 0x06002D4B RID: 11595 RVA: 0x000EC7EC File Offset: 0x000EA9EC
	private float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	// Token: 0x06002D4C RID: 11596 RVA: 0x000EC80C File Offset: 0x000EAA0C
	private float easeOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	// Token: 0x06002D4D RID: 11597 RVA: 0x000EC830 File Offset: 0x000EAA30
	private float easeInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	// Token: 0x06002D4E RID: 11598 RVA: 0x000EC8A0 File Offset: 0x000EAAA0
	private float easeInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - this.easeOutBounce(0f, end, num - value) + start;
	}

	// Token: 0x06002D4F RID: 11599 RVA: 0x000EC8CC File Offset: 0x000EAACC
	private float easeOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.95454544f;
		return end * (7.5625f * value * value + 0.984375f) + start;
	}

	// Token: 0x06002D50 RID: 11600 RVA: 0x000EC974 File Offset: 0x000EAB74
	private float easeInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num / 2f)
		{
			return this.easeInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return this.easeOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x06002D51 RID: 11601 RVA: 0x000EC9DC File Offset: 0x000EABDC
	private float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x000ECA10 File Offset: 0x000EAC10
	private float easeOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value = value / 1f - 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x000ECA50 File Offset: 0x000EAC50
	private float easeInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x06002D54 RID: 11604 RVA: 0x000ECAD0 File Offset: 0x000EACD0
	private float punch(float amplitude, float value)
	{
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num = 0.3f;
		float num2 = num / 6.2831855f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.2831855f / num);
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x000ECB48 File Offset: 0x000EAD48
	private float easeInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return -(num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x000ECC00 File Offset: 0x000EAE00
	private float easeOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) + end + start;
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x000ECCB0 File Offset: 0x000EAEB0
	private float easeInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num / 2f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) * 0.5f + end + start;
	}

	// Token: 0x04001E4A RID: 7754
	public static ArrayList tweens = new ArrayList();

	// Token: 0x04001E4B RID: 7755
	private static GameObject cameraFade;

	// Token: 0x04001E4C RID: 7756
	public string id;

	// Token: 0x04001E4D RID: 7757
	public string type;

	// Token: 0x04001E4E RID: 7758
	public string method;

	// Token: 0x04001E4F RID: 7759
	public iTween.EaseType easeType;

	// Token: 0x04001E50 RID: 7760
	public float time;

	// Token: 0x04001E51 RID: 7761
	public float delay;

	// Token: 0x04001E52 RID: 7762
	public iTween.LoopType loopType;

	// Token: 0x04001E53 RID: 7763
	public bool isRunning;

	// Token: 0x04001E54 RID: 7764
	public bool isPaused;

	// Token: 0x04001E55 RID: 7765
	public string _name;

	// Token: 0x04001E56 RID: 7766
	private float runningTime;

	// Token: 0x04001E57 RID: 7767
	private float percentage;

	// Token: 0x04001E58 RID: 7768
	private float delayStarted;

	// Token: 0x04001E59 RID: 7769
	private bool kinematic;

	// Token: 0x04001E5A RID: 7770
	private bool isLocal;

	// Token: 0x04001E5B RID: 7771
	private bool loop;

	// Token: 0x04001E5C RID: 7772
	private bool reverse;

	// Token: 0x04001E5D RID: 7773
	private bool wasPaused;

	// Token: 0x04001E5E RID: 7774
	private bool physics;

	// Token: 0x04001E5F RID: 7775
	private Hashtable tweenArguments;

	// Token: 0x04001E60 RID: 7776
	private Space space;

	// Token: 0x04001E61 RID: 7777
	private iTween.EasingFunction ease;

	// Token: 0x04001E62 RID: 7778
	private iTween.ApplyTween apply;

	// Token: 0x04001E63 RID: 7779
	private AudioSource audioSource;

	// Token: 0x04001E64 RID: 7780
	private Vector3[] vector3s;

	// Token: 0x04001E65 RID: 7781
	private Vector2[] vector2s;

	// Token: 0x04001E66 RID: 7782
	private Color[,] colors;

	// Token: 0x04001E67 RID: 7783
	private float[] floats;

	// Token: 0x04001E68 RID: 7784
	private Rect[] rects;

	// Token: 0x04001E69 RID: 7785
	private iTween.CRSpline path;

	// Token: 0x04001E6A RID: 7786
	private Vector3 preUpdate;

	// Token: 0x04001E6B RID: 7787
	private Vector3 postUpdate;

	// Token: 0x04001E6C RID: 7788
	private iTween.NamedValueColor namedcolorvalue;

	// Token: 0x04001E6D RID: 7789
	private float lastRealTime;

	// Token: 0x04001E6E RID: 7790
	private bool useRealTime;

	// Token: 0x02000826 RID: 2086
	// (Invoke) Token: 0x06002D5A RID: 11610
	private delegate float EasingFunction(float start, float end, float value);

	// Token: 0x02000827 RID: 2087
	// (Invoke) Token: 0x06002D5E RID: 11614
	private delegate void ApplyTween();

	// Token: 0x02000828 RID: 2088
	public enum EaseType
	{
		// Token: 0x04001E71 RID: 7793
		easeInQuad,
		// Token: 0x04001E72 RID: 7794
		easeOutQuad,
		// Token: 0x04001E73 RID: 7795
		easeInOutQuad,
		// Token: 0x04001E74 RID: 7796
		easeInCubic,
		// Token: 0x04001E75 RID: 7797
		easeOutCubic,
		// Token: 0x04001E76 RID: 7798
		easeInOutCubic,
		// Token: 0x04001E77 RID: 7799
		easeInQuart,
		// Token: 0x04001E78 RID: 7800
		easeOutQuart,
		// Token: 0x04001E79 RID: 7801
		easeInOutQuart,
		// Token: 0x04001E7A RID: 7802
		easeInQuint,
		// Token: 0x04001E7B RID: 7803
		easeOutQuint,
		// Token: 0x04001E7C RID: 7804
		easeInOutQuint,
		// Token: 0x04001E7D RID: 7805
		easeInSine,
		// Token: 0x04001E7E RID: 7806
		easeOutSine,
		// Token: 0x04001E7F RID: 7807
		easeInOutSine,
		// Token: 0x04001E80 RID: 7808
		easeInExpo,
		// Token: 0x04001E81 RID: 7809
		easeOutExpo,
		// Token: 0x04001E82 RID: 7810
		easeInOutExpo,
		// Token: 0x04001E83 RID: 7811
		easeInCirc,
		// Token: 0x04001E84 RID: 7812
		easeOutCirc,
		// Token: 0x04001E85 RID: 7813
		easeInOutCirc,
		// Token: 0x04001E86 RID: 7814
		linear,
		// Token: 0x04001E87 RID: 7815
		spring,
		// Token: 0x04001E88 RID: 7816
		easeInBounce,
		// Token: 0x04001E89 RID: 7817
		easeOutBounce,
		// Token: 0x04001E8A RID: 7818
		easeInOutBounce,
		// Token: 0x04001E8B RID: 7819
		easeInBack,
		// Token: 0x04001E8C RID: 7820
		easeOutBack,
		// Token: 0x04001E8D RID: 7821
		easeInOutBack,
		// Token: 0x04001E8E RID: 7822
		easeInElastic,
		// Token: 0x04001E8F RID: 7823
		easeOutElastic,
		// Token: 0x04001E90 RID: 7824
		easeInOutElastic,
		// Token: 0x04001E91 RID: 7825
		punch
	}

	// Token: 0x02000829 RID: 2089
	public enum LoopType
	{
		// Token: 0x04001E93 RID: 7827
		none,
		// Token: 0x04001E94 RID: 7828
		loop,
		// Token: 0x04001E95 RID: 7829
		pingPong
	}

	// Token: 0x0200082A RID: 2090
	public enum NamedValueColor
	{
		// Token: 0x04001E97 RID: 7831
		_Color,
		// Token: 0x04001E98 RID: 7832
		_SpecColor,
		// Token: 0x04001E99 RID: 7833
		_Emission,
		// Token: 0x04001E9A RID: 7834
		_ReflectColor
	}

	// Token: 0x0200082B RID: 2091
	public static class Defaults
	{
		// Token: 0x04001E9B RID: 7835
		public static float time = 1f;

		// Token: 0x04001E9C RID: 7836
		public static float delay = 0f;

		// Token: 0x04001E9D RID: 7837
		public static iTween.NamedValueColor namedColorValue = iTween.NamedValueColor._Color;

		// Token: 0x04001E9E RID: 7838
		public static iTween.LoopType loopType = iTween.LoopType.none;

		// Token: 0x04001E9F RID: 7839
		public static iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

		// Token: 0x04001EA0 RID: 7840
		public static float lookSpeed = 3f;

		// Token: 0x04001EA1 RID: 7841
		public static bool isLocal = false;

		// Token: 0x04001EA2 RID: 7842
		public static Space space = Space.Self;

		// Token: 0x04001EA3 RID: 7843
		public static bool orientToPath = false;

		// Token: 0x04001EA4 RID: 7844
		public static Color color = Color.white;

		// Token: 0x04001EA5 RID: 7845
		public static float updateTimePercentage = 0.05f;

		// Token: 0x04001EA6 RID: 7846
		public static float updateTime = 1f * iTween.Defaults.updateTimePercentage;

		// Token: 0x04001EA7 RID: 7847
		public static int cameraFadeDepth = 999999;

		// Token: 0x04001EA8 RID: 7848
		public static float lookAhead = 0.05f;

		// Token: 0x04001EA9 RID: 7849
		public static bool useRealTime = false;

		// Token: 0x04001EAA RID: 7850
		public static Vector3 up = Vector3.up;
	}

	// Token: 0x0200082C RID: 2092
	private class CRSpline
	{
		// Token: 0x06002D62 RID: 11618 RVA: 0x000ECE5C File Offset: 0x000EB05C
		public CRSpline(params Vector3[] pts)
		{
			this.pts = new Vector3[pts.Length];
			Array.Copy(pts, this.pts, pts.Length);
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x000ECE84 File Offset: 0x000EB084
		public Vector3 Interp(float t)
		{
			int num = this.pts.Length - 3;
			int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
			float num3 = t * (float)num - (float)num2;
			Vector3 vector = this.pts[num2];
			Vector3 vector2 = this.pts[num2 + 1];
			Vector3 vector3 = this.pts[num2 + 2];
			Vector3 vector4 = this.pts[num2 + 3];
			return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
		}

		// Token: 0x04001EAB RID: 7851
		public Vector3[] pts;
	}
}
