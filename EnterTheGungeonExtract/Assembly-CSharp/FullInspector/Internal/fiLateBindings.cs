using System;
using System.Reflection;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x02000561 RID: 1377
	public static class fiLateBindings
	{
		// Token: 0x060020BF RID: 8383 RVA: 0x0009138C File Offset: 0x0008F58C
		private static bool VerifyBinding(string name, object obj)
		{
			if (obj == null)
			{
				if (fiUtility.IsEditor)
				{
					Debug.Log("There is no binding for " + name + " even though we are in an editor");
				}
				return false;
			}
			return true;
		}

		// Token: 0x02000562 RID: 1378
		public static class _Bindings
		{
			// Token: 0x040017B7 RID: 6071
			public static Func<string, Type, UnityEngine.Object> _AssetDatabase_LoadAssetAtPath;

			// Token: 0x040017B8 RID: 6072
			public static Func<bool> _EditorApplication_isPlaying;

			// Token: 0x040017B9 RID: 6073
			public static Func<bool> _EditorApplication_isCompilingOrChangingToPlayMode;

			// Token: 0x040017BA RID: 6074
			public static Action<Action> _EditorApplication_AddUpdateAction;

			// Token: 0x040017BB RID: 6075
			public static Action<Action> _EditorApplication_RemUpdateAction;

			// Token: 0x040017BC RID: 6076
			public static Func<double> _EditorApplication_timeSinceStartup;

			// Token: 0x040017BD RID: 6077
			public static Func<string, string, string> _EditorPrefs_GetString;

			// Token: 0x040017BE RID: 6078
			public static Action<string, string> _EditorPrefs_SetString;

			// Token: 0x040017BF RID: 6079
			public static Action<UnityEngine.Object> _EditorUtility_SetDirty;

			// Token: 0x040017C0 RID: 6080
			public static Func<int, UnityEngine.Object> _EditorUtility_InstanceIdToObject;

			// Token: 0x040017C1 RID: 6081
			public static Func<UnityEngine.Object, bool> _EditorUtility_IsPersistent;

			// Token: 0x040017C2 RID: 6082
			public static Func<string, HideFlags, GameObject> _EditorUtility_CreateGameObjectWithHideFlags;

			// Token: 0x040017C3 RID: 6083
			public static Action _EditorGUI_BeginChangeCheck;

			// Token: 0x040017C4 RID: 6084
			public static Func<bool> _EditorGUI_EndChangeCheck;

			// Token: 0x040017C5 RID: 6085
			public static Action<bool> _EditorGUI_BeginDisabledGroup;

			// Token: 0x040017C6 RID: 6086
			public static Action _EditorGUI_EndDisabledGroup;

			// Token: 0x040017C7 RID: 6087
			public static fiLateBindings._Bindings._EditorGUI_Foldout_Type _EditorGUI_Foldout;

			// Token: 0x040017C8 RID: 6088
			public static Action<Rect, string, CommentType> _EditorGUI_HelpBox;

			// Token: 0x040017C9 RID: 6089
			public static fiLateBindings._Bindings._EditorGUI_Slider_Type<int> _EditorGUI_IntSlider;

			// Token: 0x040017CA RID: 6090
			public static fiLateBindings._Bindings._EditorGUI_PopupType _EditorGUI_Popup;

			// Token: 0x040017CB RID: 6091
			public static fiLateBindings._Bindings._EditorGUI_Slider_Type<float> _EditorGUI_Slider;

			// Token: 0x040017CC RID: 6092
			public static Func<GUIStyle> _EditorStyles_label;

			// Token: 0x040017CD RID: 6093
			public static Func<GUIStyle> _EditorStyles_foldout;

			// Token: 0x040017CE RID: 6094
			public static Action<bool> _fiEditorGUI_PushHierarchyMode;

			// Token: 0x040017CF RID: 6095
			public static Action _fiEditorGUI_PopHierarchyMode;

			// Token: 0x040017D0 RID: 6096
			public static Func<string, GameObject, GameObject> _PrefabUtility_CreatePrefab;

			// Token: 0x040017D1 RID: 6097
			public static Func<UnityEngine.Object, bool> _PrefabUtility_IsPrefab;

			// Token: 0x040017D2 RID: 6098
			public static fiLateBindings._Bindings._PropertyEditor_Edit_Type _PropertyEditor_Edit;

			// Token: 0x040017D3 RID: 6099
			public static fiLateBindings._Bindings._PropertyEditor_GetElementHeight_Type _PropertyEditor_GetElementHeight;

			// Token: 0x040017D4 RID: 6100
			public static fiLateBindings._Bindings._PropertyEditor_EditSkipUntilNot_Type _PropertyEditor_EditSkipUntilNot;

			// Token: 0x040017D5 RID: 6101
			public static fiLateBindings._Bindings._PropertyEditor_GetElementHeightSkipUntilNot_Type _PropertyEditor_GetElementHeightSkipUntilNot;

			// Token: 0x040017D6 RID: 6102
			public static Func<UnityEngine.Object> _Selection_activeObject;

			// Token: 0x02000563 RID: 1379
			// (Invoke) Token: 0x060020C1 RID: 8385
			public delegate bool _EditorGUI_Foldout_Type(Rect rect, bool status, GUIContent label, bool toggleOnLabelClick, GUIStyle style);

			// Token: 0x02000564 RID: 1380
			// (Invoke) Token: 0x060020C5 RID: 8389
			public delegate T _EditorGUI_Slider_Type<T>(Rect position, GUIContent label, T value, T leftValue, T rightValue);

			// Token: 0x02000565 RID: 1381
			// (Invoke) Token: 0x060020C9 RID: 8393
			public delegate int _EditorGUI_PopupType(Rect position, GUIContent label, int selectedIndex, GUIContent[] displayedOptions);

			// Token: 0x02000566 RID: 1382
			// (Invoke) Token: 0x060020CD RID: 8397
			public delegate object _PropertyEditor_Edit_Type(Type objType, MemberInfo attrs, Rect rect, GUIContent label, object obj, fiGraphMetadataChild metadata, Type[] skippedEditors);

			// Token: 0x02000567 RID: 1383
			// (Invoke) Token: 0x060020D1 RID: 8401
			public delegate float _PropertyEditor_GetElementHeight_Type(Type objType, MemberInfo attrs, GUIContent label, object obj, fiGraphMetadataChild metadata, Type[] skippedEditors);

			// Token: 0x02000568 RID: 1384
			// (Invoke) Token: 0x060020D5 RID: 8405
			public delegate object _PropertyEditor_EditSkipUntilNot_Type(Type[] skipUntilNot, Type objType, MemberInfo attrs, Rect rect, GUIContent label, object obj, fiGraphMetadataChild metadata);

			// Token: 0x02000569 RID: 1385
			// (Invoke) Token: 0x060020D9 RID: 8409
			public delegate float _PropertyEditor_GetElementHeightSkipUntilNot_Type(Type[] skipUntilNot, Type objType, MemberInfo attrs, GUIContent label, object obj, fiGraphMetadataChild metadata);
		}

		// Token: 0x0200056A RID: 1386
		public static class AssetDatabase
		{
			// Token: 0x060020DC RID: 8412 RVA: 0x000913B8 File Offset: 0x0008F5B8
			public static UnityEngine.Object LoadAssetAtPath(string path, Type type)
			{
				if (fiLateBindings.VerifyBinding("AssetDatabase.LoadAssetAtPath", fiLateBindings._Bindings._AssetDatabase_LoadAssetAtPath))
				{
					return fiLateBindings._Bindings._AssetDatabase_LoadAssetAtPath(path, type);
				}
				return null;
			}
		}

		// Token: 0x0200056B RID: 1387
		public static class EditorApplication
		{
			// Token: 0x1700065E RID: 1630
			// (get) Token: 0x060020DD RID: 8413 RVA: 0x000913DC File Offset: 0x0008F5DC
			public static bool isPlaying
			{
				get
				{
					return !fiLateBindings.VerifyBinding("EditorApplication.isPlaying", fiLateBindings._Bindings._EditorApplication_isPlaying) || fiLateBindings._Bindings._EditorApplication_isPlaying();
				}
			}

			// Token: 0x1700065F RID: 1631
			// (get) Token: 0x060020DE RID: 8414 RVA: 0x00091400 File Offset: 0x0008F600
			public static bool isCompilingOrChangingToPlayMode
			{
				get
				{
					return !fiLateBindings.VerifyBinding("EditorApplication.isCompilingOrChangingToPlayMode", fiLateBindings._Bindings._EditorApplication_isCompilingOrChangingToPlayMode) || fiLateBindings._Bindings._EditorApplication_isCompilingOrChangingToPlayMode();
				}
			}

			// Token: 0x17000660 RID: 1632
			// (get) Token: 0x060020DF RID: 8415 RVA: 0x00091424 File Offset: 0x0008F624
			public static double timeSinceStartup
			{
				get
				{
					if (fiLateBindings.VerifyBinding("EditorApplication.timeSinceStartup", fiLateBindings._Bindings._EditorApplication_timeSinceStartup))
					{
						return fiLateBindings._Bindings._EditorApplication_timeSinceStartup();
					}
					return 0.0;
				}
			}

			// Token: 0x060020E0 RID: 8416 RVA: 0x00091450 File Offset: 0x0008F650
			public static void AddUpdateFunc(Action func)
			{
				if (fiLateBindings.VerifyBinding("EditorApplication.AddUpdateFunc", fiLateBindings._Bindings._EditorApplication_AddUpdateAction))
				{
					fiLateBindings._Bindings._EditorApplication_AddUpdateAction(func);
				}
			}

			// Token: 0x060020E1 RID: 8417 RVA: 0x00091474 File Offset: 0x0008F674
			public static void RemUpdateFunc(Action func)
			{
				if (fiLateBindings.VerifyBinding("EditorApplication.RemUpdateFunc", fiLateBindings._Bindings._EditorApplication_RemUpdateAction))
				{
					fiLateBindings._Bindings._EditorApplication_RemUpdateAction(func);
				}
			}
		}

		// Token: 0x0200056C RID: 1388
		public static class EditorPrefs
		{
			// Token: 0x060020E2 RID: 8418 RVA: 0x00091498 File Offset: 0x0008F698
			public static string GetString(string key, string defaultValue)
			{
				if (fiLateBindings.VerifyBinding("EditorPrefs.GetString", fiLateBindings._Bindings._EditorPrefs_GetString))
				{
					return fiLateBindings._Bindings._EditorPrefs_GetString(key, defaultValue);
				}
				return defaultValue;
			}

			// Token: 0x060020E3 RID: 8419 RVA: 0x000914BC File Offset: 0x0008F6BC
			public static void SetString(string key, string value)
			{
				if (fiLateBindings.VerifyBinding("EditorPrefs.SetString", fiLateBindings._Bindings._EditorPrefs_SetString))
				{
					fiLateBindings._Bindings._EditorPrefs_SetString(key, value);
				}
			}
		}

		// Token: 0x0200056D RID: 1389
		public static class EditorUtility
		{
			// Token: 0x060020E4 RID: 8420 RVA: 0x000914E0 File Offset: 0x0008F6E0
			public static void SetDirty(UnityEngine.Object unityObject)
			{
				if (fiLateBindings.VerifyBinding("EditorUtility.SetDirty", fiLateBindings._Bindings._EditorUtility_SetDirty))
				{
					fiLateBindings._Bindings._EditorUtility_SetDirty(unityObject);
				}
			}

			// Token: 0x060020E5 RID: 8421 RVA: 0x00091504 File Offset: 0x0008F704
			public static UnityEngine.Object InstanceIDToObject(int instanceId)
			{
				if (fiLateBindings.VerifyBinding("EditorUtility.InstanceIdToObject", fiLateBindings._Bindings._EditorUtility_InstanceIdToObject))
				{
					return fiLateBindings._Bindings._EditorUtility_InstanceIdToObject(instanceId);
				}
				return null;
			}

			// Token: 0x060020E6 RID: 8422 RVA: 0x00091528 File Offset: 0x0008F728
			public static bool IsPersistent(UnityEngine.Object unityObject)
			{
				return fiLateBindings.VerifyBinding("EditorUtility.IsPersistent", fiLateBindings._Bindings._EditorUtility_IsPersistent) && fiLateBindings._Bindings._EditorUtility_IsPersistent(unityObject);
			}

			// Token: 0x060020E7 RID: 8423 RVA: 0x0009154C File Offset: 0x0008F74C
			public static GameObject CreateGameObjectWithHideFlags(string name, HideFlags hideFlags)
			{
				if (fiLateBindings.VerifyBinding("EditorUtility.CreateGameObjectWithHideFlags", fiLateBindings._Bindings._EditorUtility_CreateGameObjectWithHideFlags))
				{
					return fiLateBindings._Bindings._EditorUtility_CreateGameObjectWithHideFlags(name, hideFlags);
				}
				return new GameObject(name)
				{
					hideFlags = hideFlags
				};
			}
		}

		// Token: 0x0200056E RID: 1390
		public static class EditorGUI
		{
			// Token: 0x060020E8 RID: 8424 RVA: 0x0009158C File Offset: 0x0008F78C
			public static void BeginChangeCheck()
			{
				if (fiLateBindings.VerifyBinding("EditorGUI.BeginChangeCheck", fiLateBindings._Bindings._EditorGUI_BeginDisabledGroup))
				{
					fiLateBindings._Bindings._EditorGUI_BeginChangeCheck();
				}
			}

			// Token: 0x060020E9 RID: 8425 RVA: 0x000915AC File Offset: 0x0008F7AC
			public static bool EndChangeCheck()
			{
				return fiLateBindings.VerifyBinding("EditorGUI.EndChangeCheck", fiLateBindings._Bindings._EditorGUI_EndDisabledGroup) && fiLateBindings._Bindings._EditorGUI_EndChangeCheck();
			}

			// Token: 0x060020EA RID: 8426 RVA: 0x000915D0 File Offset: 0x0008F7D0
			public static void BeginDisabledGroup(bool disabled)
			{
				if (fiLateBindings.VerifyBinding("EditorGUI.BeginDisabledGroup", fiLateBindings._Bindings._EditorGUI_BeginDisabledGroup))
				{
					fiLateBindings._Bindings._EditorGUI_BeginDisabledGroup(disabled);
				}
			}

			// Token: 0x060020EB RID: 8427 RVA: 0x000915F4 File Offset: 0x0008F7F4
			public static void EndDisabledGroup()
			{
				if (fiLateBindings.VerifyBinding("EditorGUI.EndDisabledGroup", fiLateBindings._Bindings._EditorGUI_EndDisabledGroup))
				{
					fiLateBindings._Bindings._EditorGUI_EndDisabledGroup();
				}
			}

			// Token: 0x060020EC RID: 8428 RVA: 0x00091614 File Offset: 0x0008F814
			public static bool Foldout(Rect rect, bool state, GUIContent label, bool toggleOnLabelClick, GUIStyle style)
			{
				return !fiLateBindings.VerifyBinding("EditorGUI.Foldout", fiLateBindings._Bindings._EditorGUI_Foldout) || fiLateBindings._Bindings._EditorGUI_Foldout(rect, state, label, toggleOnLabelClick, style);
			}

			// Token: 0x060020ED RID: 8429 RVA: 0x0009163C File Offset: 0x0008F83C
			public static void HelpBox(Rect rect, string message, CommentType commentType)
			{
				if (fiLateBindings.VerifyBinding("EditorGUI.HelpBox", fiLateBindings._Bindings._EditorGUI_HelpBox))
				{
					fiLateBindings._Bindings._EditorGUI_HelpBox(rect, message, commentType);
				}
			}

			// Token: 0x060020EE RID: 8430 RVA: 0x00091660 File Offset: 0x0008F860
			public static int IntSlider(Rect position, GUIContent label, int value, int leftValue, int rightValue)
			{
				if (fiLateBindings.VerifyBinding("EditorGUI.IntSlider", fiLateBindings._Bindings._EditorGUI_IntSlider))
				{
					return fiLateBindings._Bindings._EditorGUI_IntSlider(position, label, value, leftValue, rightValue);
				}
				return value;
			}

			// Token: 0x060020EF RID: 8431 RVA: 0x00091688 File Offset: 0x0008F888
			public static int Popup(Rect position, GUIContent label, int selectedIndex, GUIContent[] displayedOptions)
			{
				if (fiLateBindings.VerifyBinding("EditorGUI.Popup", fiLateBindings._Bindings._EditorGUI_Popup))
				{
					return fiLateBindings._Bindings._EditorGUI_Popup(position, label, selectedIndex, displayedOptions);
				}
				return selectedIndex;
			}

			// Token: 0x060020F0 RID: 8432 RVA: 0x000916B0 File Offset: 0x0008F8B0
			public static float Slider(Rect position, GUIContent label, float value, float leftValue, float rightValue)
			{
				if (fiLateBindings.VerifyBinding("EditorGUI.Slider", fiLateBindings._Bindings._EditorGUI_Slider))
				{
					return fiLateBindings._Bindings._EditorGUI_Slider(position, label, value, leftValue, rightValue);
				}
				return value;
			}
		}

		// Token: 0x0200056F RID: 1391
		public static class EditorGUIUtility
		{
			// Token: 0x040017D7 RID: 6103
			public static float standardVerticalSpacing = 2f;

			// Token: 0x040017D8 RID: 6104
			public static float singleLineHeight = 16f;
		}

		// Token: 0x02000570 RID: 1392
		public static class EditorStyles
		{
			// Token: 0x17000661 RID: 1633
			// (get) Token: 0x060020F2 RID: 8434 RVA: 0x000916F0 File Offset: 0x0008F8F0
			public static GUIStyle label
			{
				get
				{
					if (fiLateBindings.VerifyBinding("EditorStyles.label", fiLateBindings._Bindings._EditorStyles_label))
					{
						return fiLateBindings._Bindings._EditorStyles_label();
					}
					return new GUIStyle();
				}
			}

			// Token: 0x17000662 RID: 1634
			// (get) Token: 0x060020F3 RID: 8435 RVA: 0x00091718 File Offset: 0x0008F918
			public static GUIStyle foldout
			{
				get
				{
					if (fiLateBindings.VerifyBinding("EditorStyles.foldout", fiLateBindings._Bindings._EditorStyles_foldout))
					{
						return fiLateBindings._Bindings._EditorStyles_foldout();
					}
					return new GUIStyle();
				}
			}
		}

		// Token: 0x02000571 RID: 1393
		public static class fiEditorGUI
		{
			// Token: 0x060020F4 RID: 8436 RVA: 0x00091740 File Offset: 0x0008F940
			public static void PushHierarchyMode(bool state)
			{
				if (fiLateBindings.VerifyBinding("fiEditorGUI.PushHierarchyMode", fiLateBindings._Bindings._fiEditorGUI_PushHierarchyMode))
				{
					fiLateBindings._Bindings._fiEditorGUI_PushHierarchyMode(state);
				}
			}

			// Token: 0x060020F5 RID: 8437 RVA: 0x00091764 File Offset: 0x0008F964
			public static void PopHierarchyMode()
			{
				if (fiLateBindings.VerifyBinding("fiEditorGUI.PopHierarchyMode", fiLateBindings._Bindings._fiEditorGUI_PopHierarchyMode))
				{
					fiLateBindings._Bindings._fiEditorGUI_PopHierarchyMode();
				}
			}
		}

		// Token: 0x02000572 RID: 1394
		public static class PrefabUtility
		{
			// Token: 0x060020F6 RID: 8438 RVA: 0x00091784 File Offset: 0x0008F984
			public static GameObject CreatePrefab(string path, GameObject template)
			{
				if (fiLateBindings.VerifyBinding("PrefabUtility.CreatePrefab", fiLateBindings._Bindings._PrefabUtility_CreatePrefab))
				{
					return fiLateBindings._Bindings._PrefabUtility_CreatePrefab(path, template);
				}
				return null;
			}

			// Token: 0x060020F7 RID: 8439 RVA: 0x000917A8 File Offset: 0x0008F9A8
			public static bool IsPrefab(UnityEngine.Object unityObject)
			{
				return fiLateBindings.VerifyBinding("PrefabUtility.IsPrefab", fiLateBindings._Bindings._PrefabUtility_IsPrefab) && fiLateBindings._Bindings._PrefabUtility_IsPrefab(unityObject);
			}
		}

		// Token: 0x02000573 RID: 1395
		public static class PropertyEditor
		{
			// Token: 0x060020F8 RID: 8440 RVA: 0x000917CC File Offset: 0x0008F9CC
			public static object Edit(Type objType, MemberInfo attrs, Rect rect, GUIContent label, object obj, fiGraphMetadataChild metadata, params Type[] skippedEditors)
			{
				if (fiLateBindings.VerifyBinding("PropertyEditor.Edit", fiLateBindings._Bindings._PropertyEditor_Edit))
				{
					return fiLateBindings._Bindings._PropertyEditor_Edit(objType, attrs, rect, label, obj, metadata, skippedEditors);
				}
				return obj;
			}

			// Token: 0x060020F9 RID: 8441 RVA: 0x000917FC File Offset: 0x0008F9FC
			public static float GetElementHeight(Type objType, MemberInfo attrs, GUIContent label, object obj, fiGraphMetadataChild metadata, params Type[] skippedEditors)
			{
				if (fiLateBindings.VerifyBinding("PropertyEditor.GetElementHeight", fiLateBindings._Bindings._PropertyEditor_GetElementHeight))
				{
					return fiLateBindings._Bindings._PropertyEditor_GetElementHeight(objType, attrs, label, obj, metadata, skippedEditors);
				}
				return 0f;
			}

			// Token: 0x060020FA RID: 8442 RVA: 0x0009182C File Offset: 0x0008FA2C
			public static object EditSkipUntilNot(Type[] skipUntilNot, Type objType, MemberInfo attrs, Rect rect, GUIContent label, object obj, fiGraphMetadataChild metadata)
			{
				if (fiLateBindings.VerifyBinding("PropertyEditor.EditSkipUntilNot", fiLateBindings._Bindings._PropertyEditor_EditSkipUntilNot))
				{
					return fiLateBindings._Bindings._PropertyEditor_EditSkipUntilNot(skipUntilNot, objType, attrs, rect, label, obj, metadata);
				}
				return obj;
			}

			// Token: 0x060020FB RID: 8443 RVA: 0x0009185C File Offset: 0x0008FA5C
			public static float GetElementHeightSkipUntilNot(Type[] skipUntilNot, Type objType, MemberInfo attrs, GUIContent label, object obj, fiGraphMetadataChild metadata)
			{
				if (fiLateBindings.VerifyBinding("PropertyEditor.GetElementHeightSkipUntilNot", fiLateBindings._Bindings._PropertyEditor_GetElementHeightSkipUntilNot))
				{
					return fiLateBindings._Bindings._PropertyEditor_GetElementHeightSkipUntilNot(skipUntilNot, objType, attrs, label, obj, metadata);
				}
				return 0f;
			}
		}

		// Token: 0x02000574 RID: 1396
		public static class Selection
		{
			// Token: 0x17000663 RID: 1635
			// (get) Token: 0x060020FC RID: 8444 RVA: 0x0009188C File Offset: 0x0008FA8C
			public static UnityEngine.Object activeObject
			{
				get
				{
					if (fiLateBindings.VerifyBinding("Selection.activeObject", fiLateBindings._Bindings._Selection_activeObject))
					{
						return fiLateBindings._Bindings._Selection_activeObject();
					}
					return null;
				}
			}
		}
	}
}
