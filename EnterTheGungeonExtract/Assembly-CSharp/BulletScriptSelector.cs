using System;
using System.Collections.Generic;
using System.Linq;
using Brave.BulletScript;
using FullInspector;
using FullInspector.Internal;
using UnityEngine;

// Token: 0x0200033B RID: 827
[Serializable]
public class BulletScriptSelector : fiInspectorOnly, tkCustomEditor
{
	// Token: 0x06000CCD RID: 3277 RVA: 0x0003D808 File Offset: 0x0003BA08
	public Bullet CreateInstance()
	{
		Type type = Type.GetType(this.scriptTypeName);
		if (type == null)
		{
			Debug.LogError("Unknown type! " + this.scriptTypeName);
			return null;
		}
		return (Bullet)Activator.CreateInstance(type);
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0003D84C File Offset: 0x0003BA4C
	public bool IsNull
	{
		get
		{
			return string.IsNullOrEmpty(this.scriptTypeName) || this.scriptTypeName == "null";
		}
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x0003D874 File Offset: 0x0003BA74
	public BulletScriptSelector Clone()
	{
		return new BulletScriptSelector
		{
			scriptTypeName = this.scriptTypeName
		};
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x0003D894 File Offset: 0x0003BA94
	tkControlEditor tkCustomEditor.GetEditor()
	{
		return new tkControlEditor(new tk<BulletScriptSelector, tkDefaultContext>.Popup(tk<BulletScriptSelector, tkDefaultContext>.Val<fiGUIContent>((BulletScriptSelector o, tkDefaultContext c) => new fiGUIContent(c.Label.text)), tk<BulletScriptSelector, tkDefaultContext>.Val<GUIContent[]>((BulletScriptSelector o) => o.GetLabels()), tk<BulletScriptSelector, tkDefaultContext>.Val<int>(delegate(BulletScriptSelector o)
		{
			if (string.IsNullOrEmpty(o.scriptTypeName))
			{
				return 0;
			}
			return Math.Max(0, Array.FindIndex<string>(o.GetTypeNames(), (string gc) => gc == o.scriptTypeName));
		}), delegate(BulletScriptSelector o, tkDefaultContext c, int v)
		{
			o.scriptTypeName = o.GetTypeNames()[v];
			if (o.scriptTypeName == "null")
			{
				o.scriptTypeName = null;
			}
			return o;
		}));
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x0003D930 File Offset: 0x0003BB30
	public GUIContent[] GetLabels()
	{
		if (BulletScriptSelector._types == null)
		{
			this.InitEditorCache();
		}
		return BulletScriptSelector._labels;
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x0003D948 File Offset: 0x0003BB48
	public string[] GetTypeNames()
	{
		if (BulletScriptSelector._types == null)
		{
			this.InitEditorCache();
		}
		return BulletScriptSelector._typeNames;
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x0003D960 File Offset: 0x0003BB60
	private void InitEditorCache()
	{
		List<Type> list = new List<Type>();
		list.Add(null);
		list.AddRange(fiRuntimeReflectionUtility.AllSimpleCreatableTypesDerivingFrom(typeof(Script)));
		list.Remove(typeof(Script));
		list.AddRange(fiRuntimeReflectionUtility.AllSimpleCreatableTypesDerivingFrom(typeof(ScriptLite)));
		list.Remove(typeof(ScriptLite));
		BulletScriptSelector._types = list.ToArray();
		BulletScriptSelector._typeNames = BulletScriptSelector._types.Select((Type t) => (t != null) ? t.FullName : "null").ToArray<string>();
		BulletScriptSelector._labels = BulletScriptSelector._types.Select(delegate(Type t)
		{
			if (t == null)
			{
				return new GUIContent("null");
			}
			InspectorDropdownNameAttribute inspectorDropdownNameAttribute = Attribute.GetCustomAttribute(t, typeof(InspectorDropdownNameAttribute)) as InspectorDropdownNameAttribute;
			if (inspectorDropdownNameAttribute != null)
			{
				return new GUIContent(inspectorDropdownNameAttribute.DisplayName);
			}
			return new GUIContent(t.FullName);
		}).ToArray<GUIContent>();
	}

	// Token: 0x04000D7C RID: 3452
	public string scriptTypeName;

	// Token: 0x04000D7D RID: 3453
	private static Type[] _types;

	// Token: 0x04000D7E RID: 3454
	private static string[] _typeNames;

	// Token: 0x04000D7F RID: 3455
	private static GUIContent[] _labels;
}
