using System;
using System.Collections.Generic;
using FullInspector.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000542 RID: 1346
	public abstract class BaseScriptableObject<TSerializer> : CommonBaseScriptableObject, ISerializedObject, ISerializationCallbackReceiver where TSerializer : BaseSerializer
	{
		// Token: 0x06001FFE RID: 8190 RVA: 0x0008EE50 File Offset: 0x0008D050
		static BaseScriptableObject()
		{
			BehaviorTypeToSerializerTypeMap.Register(typeof(BaseBehavior<TSerializer>), typeof(TSerializer));
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x0008EE74 File Offset: 0x0008D074
		protected virtual void OnEnable()
		{
			fiSerializationManager.OnUnityObjectAwake<TSerializer>(this);
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x0008EE7C File Offset: 0x0008D07C
		protected virtual void OnValidate()
		{
			if (!Application.isPlaying && !((ISerializedObject)this).IsRestored)
			{
				this.RestoreState();
			}
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x0008EE9C File Offset: 0x0008D09C
		[ContextMenu("Save Current State")]
		public void SaveState()
		{
			fiISerializedObjectUtility.SaveState<TSerializer>(this);
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x0008EEA8 File Offset: 0x0008D0A8
		[ContextMenu("Restore Saved State")]
		public void RestoreState()
		{
			fiISerializedObjectUtility.RestoreState<TSerializer>(this);
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06002004 RID: 8196 RVA: 0x0008EEB4 File Offset: 0x0008D0B4
		// (set) Token: 0x06002005 RID: 8197 RVA: 0x0008EEBC File Offset: 0x0008D0BC
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

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002006 RID: 8198 RVA: 0x0008EEC8 File Offset: 0x0008D0C8
		// (set) Token: 0x06002007 RID: 8199 RVA: 0x0008EED0 File Offset: 0x0008D0D0
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

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002008 RID: 8200 RVA: 0x0008EEDC File Offset: 0x0008D0DC
		// (set) Token: 0x06002009 RID: 8201 RVA: 0x0008EEE4 File Offset: 0x0008D0E4
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

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x0600200A RID: 8202 RVA: 0x0008EEF0 File Offset: 0x0008D0F0
		// (set) Token: 0x0600200B RID: 8203 RVA: 0x0008EEF8 File Offset: 0x0008D0F8
		bool ISerializedObject.IsRestored { get; set; }

		// Token: 0x0600200C RID: 8204 RVA: 0x0008EF04 File Offset: 0x0008D104
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			((ISerializedObject)this).IsRestored = false;
			fiSerializationManager.OnUnityObjectDeserialize<TSerializer>(this);
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x0008EF14 File Offset: 0x0008D114
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			fiSerializationManager.OnUnityObjectSerialize<TSerializer>(this);
		}

		// Token: 0x04001779 RID: 6009
		[HideInInspector]
		[NotSerialized]
		[SerializeField]
		private List<UnityEngine.Object> _objectReferences;

		// Token: 0x0400177A RID: 6010
		[SerializeField]
		[NotSerialized]
		[HideInInspector]
		private List<string> _serializedStateKeys;

		// Token: 0x0400177B RID: 6011
		[HideInInspector]
		[SerializeField]
		[NotSerialized]
		private List<string> _serializedStateValues;
	}
}
