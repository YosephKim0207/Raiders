using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x0200057C RID: 1404
	public static class fiSerializationManager
	{
		// Token: 0x0600213F RID: 8511 RVA: 0x00092968 File Offset: 0x00090B68
		static fiSerializationManager()
		{
			if (fiUtility.IsEditor)
			{
				fiLateBindings.EditorApplication.AddUpdateFunc(new Action(fiSerializationManager.OnEditorUpdate));
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x000929DC File Offset: 0x00090BDC
		private static bool SupportsMultithreading<TSerializer>() where TSerializer : BaseSerializer
		{
			bool flag;
			if (!fiSettings.ForceDisableMultithreadedSerialization && !fiUtility.IsUnity4)
			{
				TSerializer tserializer = fiSingletons.Get<TSerializer>();
				flag = tserializer.SupportsMultithreading;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x00092A14 File Offset: 0x00090C14
		public static void OnUnityObjectAwake<TSerializer>(ISerializedObject obj) where TSerializer : BaseSerializer
		{
			if (obj.IsRestored)
			{
				return;
			}
			fiSerializationManager.DoDeserialize(obj);
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x00092A28 File Offset: 0x00090C28
		public static void OnUnityObjectDeserialize<TSerializer>(ISerializedObject obj) where TSerializer : BaseSerializer
		{
			if (fiSerializationManager.SupportsMultithreading<TSerializer>())
			{
				fiSerializationManager.DoDeserialize(obj);
				return;
			}
			if (!fiUtility.IsEditor)
			{
				return;
			}
			object obj2 = fiSerializationManager.s_pendingDeserializations;
			lock (obj2)
			{
				fiSerializationManager.s_pendingDeserializations.Add(obj);
			}
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x00092A84 File Offset: 0x00090C84
		public static void OnUnityObjectSerialize<TSerializer>(ISerializedObject obj) where TSerializer : BaseSerializer
		{
			if (fiSerializationManager.SupportsMultithreading<TSerializer>())
			{
				fiSerializationManager.DoSerialize(obj);
				return;
			}
			if (!fiUtility.IsEditor)
			{
				return;
			}
			object obj2 = fiSerializationManager.s_pendingSerializations;
			lock (obj2)
			{
				fiSerializationManager.s_pendingSerializations.Add(obj);
			}
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x00092AE0 File Offset: 0x00090CE0
		private static void OnEditorUpdate()
		{
			if (Application.isPlaying)
			{
				if (fiSerializationManager.s_pendingDeserializations.Count > 0 || fiSerializationManager.s_pendingSerializations.Count > 0 || fiSerializationManager.s_snapshots.Count > 0)
				{
					fiSerializationManager.s_pendingDeserializations.Clear();
					fiSerializationManager.s_pendingSerializations.Clear();
					fiSerializationManager.s_snapshots.Clear();
				}
				return;
			}
			if (fiLateBindings.EditorApplication.isPlaying && BraveUtility.isLoadingLevel)
			{
				return;
			}
			while (fiSerializationManager.s_pendingDeserializations.Count > 0)
			{
				object obj = fiSerializationManager.s_pendingDeserializations;
				ISerializedObject serializedObject;
				lock (obj)
				{
					serializedObject = fiSerializationManager.s_pendingDeserializations[fiSerializationManager.s_pendingDeserializations.Count - 1];
					fiSerializationManager.s_pendingDeserializations.RemoveAt(fiSerializationManager.s_pendingDeserializations.Count - 1);
				}
				if (!(serializedObject is UnityEngine.Object) || !((UnityEngine.Object)serializedObject == null))
				{
					fiSerializationManager.DoDeserialize(serializedObject);
				}
			}
			while (fiSerializationManager.s_pendingSerializations.Count > 0)
			{
				object obj2 = fiSerializationManager.s_pendingSerializations;
				ISerializedObject serializedObject2;
				lock (obj2)
				{
					serializedObject2 = fiSerializationManager.s_pendingSerializations[fiSerializationManager.s_pendingSerializations.Count - 1];
					fiSerializationManager.s_pendingSerializations.RemoveAt(fiSerializationManager.s_pendingSerializations.Count - 1);
				}
				if (!(serializedObject2 is UnityEngine.Object) || !((UnityEngine.Object)serializedObject2 == null))
				{
					fiSerializationManager.DoSerialize(serializedObject2);
				}
			}
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x00092C7C File Offset: 0x00090E7C
		private static void DoDeserialize(ISerializedObject obj)
		{
			obj.RestoreState();
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x00092C84 File Offset: 0x00090E84
		private static void DoSerialize(ISerializedObject obj)
		{
			if (fiSerializationManager.DisableAutomaticSerialization)
			{
				return;
			}
			bool flag = obj is UnityEngine.Object && fiSerializationManager.DirtyForceSerialize.Contains((UnityEngine.Object)obj);
			if (flag)
			{
				fiSerializationManager.DirtyForceSerialize.Remove((UnityEngine.Object)obj);
			}
			if (!flag && obj is UnityEngine.Object && !fiLateBindings.EditorApplication.isCompilingOrChangingToPlayMode)
			{
				UnityEngine.Object @object = (UnityEngine.Object)obj;
				if (@object is Component)
				{
					@object = ((Component)@object).gameObject;
				}
				UnityEngine.Object object2 = fiLateBindings.Selection.activeObject;
				if (object2 is Component)
				{
					object2 = ((Component)object2).gameObject;
				}
				if (object.ReferenceEquals(@object, object2))
				{
					return;
				}
			}
			fiSerializationManager.CheckForReset(obj);
			obj.SaveState();
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x00092D44 File Offset: 0x00090F44
		private static void CheckForReset(ISerializedObject obj)
		{
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x00092D48 File Offset: 0x00090F48
		private static bool IsNullOrEmpty<T>(IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x040017F7 RID: 6135
		[NonSerialized]
		public static bool DisableAutomaticSerialization = false;

		// Token: 0x040017F8 RID: 6136
		private static readonly List<ISerializedObject> s_pendingDeserializations = new List<ISerializedObject>();

		// Token: 0x040017F9 RID: 6137
		private static readonly List<ISerializedObject> s_pendingSerializations = new List<ISerializedObject>();

		// Token: 0x040017FA RID: 6138
		private static readonly Dictionary<ISerializedObject, fiSerializedObjectSnapshot> s_snapshots = new Dictionary<ISerializedObject, fiSerializedObjectSnapshot>();

		// Token: 0x040017FB RID: 6139
		public static HashSet<UnityEngine.Object> DirtyForceSerialize = new HashSet<UnityEngine.Object>();

		// Token: 0x040017FC RID: 6140
		private static HashSet<ISerializedObject> s_seen = new HashSet<ISerializedObject>();
	}
}
