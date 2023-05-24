using System;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using FullInspector.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000540 RID: 1344
	[Skip]
	public abstract class BaseBehavior<TSerializer> : CommonBaseBehavior, ISerializedObject, ISerializationCallbackReceiver where TSerializer : BaseSerializer
	{
		// Token: 0x06001FEC RID: 8172 RVA: 0x0008ED74 File Offset: 0x0008CF74
		static BaseBehavior()
		{
			BehaviorTypeToSerializerTypeMap.Register(typeof(BaseBehavior<TSerializer>), typeof(TSerializer));
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x0008ED98 File Offset: 0x0008CF98
		protected virtual void Awake()
		{
			fiSerializationManager.OnUnityObjectAwake<TSerializer>(this);
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x0008EDA0 File Offset: 0x0008CFA0
		protected virtual void OnValidate()
		{
			if (!Application.isPlaying && !((ISerializedObject)this).IsRestored)
			{
				this.RestoreState();
			}
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x0008EDC0 File Offset: 0x0008CFC0
		[ContextMenu("Save Current State")]
		public void SaveState()
		{
			fiISerializedObjectUtility.SaveState<TSerializer>(this);
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0008EDCC File Offset: 0x0008CFCC
		[ContextMenu("Restore Saved State")]
		public void RestoreState()
		{
			fiISerializedObjectUtility.RestoreState<TSerializer>(this);
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001FF2 RID: 8178 RVA: 0x0008EDD8 File Offset: 0x0008CFD8
		// (set) Token: 0x06001FF3 RID: 8179 RVA: 0x0008EDE0 File Offset: 0x0008CFE0
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

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001FF4 RID: 8180 RVA: 0x0008EDEC File Offset: 0x0008CFEC
		// (set) Token: 0x06001FF5 RID: 8181 RVA: 0x0008EDF4 File Offset: 0x0008CFF4
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

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001FF6 RID: 8182 RVA: 0x0008EE00 File Offset: 0x0008D000
		// (set) Token: 0x06001FF7 RID: 8183 RVA: 0x0008EE08 File Offset: 0x0008D008
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

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06001FF8 RID: 8184 RVA: 0x0008EE14 File Offset: 0x0008D014
		// (set) Token: 0x06001FF9 RID: 8185 RVA: 0x0008EE1C File Offset: 0x0008D01C
		bool ISerializedObject.IsRestored { get; set; }

		// Token: 0x06001FFA RID: 8186 RVA: 0x0008EE28 File Offset: 0x0008D028
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			((ISerializedObject)this).IsRestored = false;
			fiSerializationManager.OnUnityObjectDeserialize<TSerializer>(this);
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x0008EE38 File Offset: 0x0008D038
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			fiSerializationManager.OnUnityObjectSerialize<TSerializer>(this);
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x0008EE40 File Offset: 0x0008D040
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x04001775 RID: 6005
		[NotSerialized]
		[HideInInspector]
		[SerializeField]
		private List<UnityEngine.Object> _objectReferences;

		// Token: 0x04001776 RID: 6006
		[HideInInspector]
		[NotSerialized]
		[SerializeField]
		private List<string> _serializedStateKeys;

		// Token: 0x04001777 RID: 6007
		[HideInInspector]
		[SerializeField]
		[NotSerialized]
		private List<string> _serializedStateValues;
	}
}
