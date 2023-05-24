using System;
using System.Collections.Generic;
using FullInspector.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000603 RID: 1539
	public abstract class BaseObject : fiValueProxyEditor, fiIValueProxyAPI, ISerializedObject, ISerializationCallbackReceiver
	{
		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x0009D44C File Offset: 0x0009B64C
		// (set) Token: 0x060023FE RID: 9214 RVA: 0x0009D454 File Offset: 0x0009B654
		List<UnityEngine.Object> ISerializedObject.SerializedObjectReferences
		{
			get
			{
				return this._objectReferences;
			}
			set
			{
				this._objectReferences = value;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x060023FF RID: 9215 RVA: 0x0009D460 File Offset: 0x0009B660
		// (set) Token: 0x06002400 RID: 9216 RVA: 0x0009D468 File Offset: 0x0009B668
		List<string> ISerializedObject.SerializedStateKeys
		{
			get
			{
				return this._serializedStateKeys;
			}
			set
			{
				this._serializedStateKeys = value;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002401 RID: 9217 RVA: 0x0009D474 File Offset: 0x0009B674
		// (set) Token: 0x06002402 RID: 9218 RVA: 0x0009D47C File Offset: 0x0009B67C
		List<string> ISerializedObject.SerializedStateValues
		{
			get
			{
				return this._serializedStateValues;
			}
			set
			{
				this._serializedStateValues = value;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06002403 RID: 9219 RVA: 0x0009D488 File Offset: 0x0009B688
		// (set) Token: 0x06002404 RID: 9220 RVA: 0x0009D490 File Offset: 0x0009B690
		bool ISerializedObject.IsRestored { get; set; }

		// Token: 0x06002405 RID: 9221 RVA: 0x0009D49C File Offset: 0x0009B69C
		void ISerializedObject.RestoreState()
		{
			fiISerializedObjectUtility.RestoreState<FullSerializerSerializer>(this);
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x0009D4A8 File Offset: 0x0009B6A8
		void ISerializedObject.SaveState()
		{
			fiISerializedObjectUtility.SaveState<FullSerializerSerializer>(this);
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x0009D4B4 File Offset: 0x0009B6B4
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			fiISerializedObjectUtility.RestoreState<FullSerializerSerializer>(this);
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x0009D4C0 File Offset: 0x0009B6C0
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			fiISerializedObjectUtility.SaveState<FullSerializerSerializer>(this);
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06002409 RID: 9225 RVA: 0x0009D4CC File Offset: 0x0009B6CC
		// (set) Token: 0x0600240A RID: 9226 RVA: 0x0009D4D0 File Offset: 0x0009B6D0
		object fiIValueProxyAPI.Value
		{
			get
			{
				return this;
			}
			set
			{
			}
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x0009D4D4 File Offset: 0x0009B6D4
		void fiIValueProxyAPI.SaveState()
		{
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x0009D4D8 File Offset: 0x0009B6D8
		void fiIValueProxyAPI.LoadState()
		{
		}

		// Token: 0x04001904 RID: 6404
		[SerializeField]
		[NotSerialized]
		[HideInInspector]
		private List<UnityEngine.Object> _objectReferences;

		// Token: 0x04001905 RID: 6405
		[SerializeField]
		[HideInInspector]
		[NotSerialized]
		private List<string> _serializedStateKeys;

		// Token: 0x04001906 RID: 6406
		[SerializeField]
		[HideInInspector]
		[NotSerialized]
		private List<string> _serializedStateValues;
	}
}
