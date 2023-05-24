using System;
using System.Collections.Generic;
using FullInspector.Internal;
using FullSerializer.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000607 RID: 1543
	public abstract class fiValue<T> : fiValueProxyEditor, fiIValueProxyAPI, ISerializationCallbackReceiver
	{
		// Token: 0x06002418 RID: 9240 RVA: 0x0009D51C File Offset: 0x0009B71C
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.Serialize();
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x0009D524 File Offset: 0x0009B724
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.Deserialize();
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x0009D52C File Offset: 0x0009B72C
		// (set) Token: 0x0600241B RID: 9243 RVA: 0x0009D53C File Offset: 0x0009B73C
		object fiIValueProxyAPI.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.Value = (T)((object)value);
			}
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x0009D54C File Offset: 0x0009B74C
		void fiIValueProxyAPI.SaveState()
		{
			this.Serialize();
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x0009D554 File Offset: 0x0009B754
		void fiIValueProxyAPI.LoadState()
		{
			this.Deserialize();
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x0009D55C File Offset: 0x0009B75C
		private void Serialize()
		{
			FullSerializerSerializer fullSerializerSerializer = fiSingletons.Get<FullSerializerSerializer>();
			ListSerializationOperator listSerializationOperator = fiSingletons.Get<ListSerializationOperator>();
			listSerializationOperator.SerializedObjects = new List<UnityEngine.Object>();
			try
			{
				this.SerializedState = fullSerializerSerializer.Serialize(typeof(T).Resolve(), this.Value, listSerializationOperator);
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Exception caught when serializing ",
					this,
					" (with type ",
					base.GetType(),
					")\n",
					ex
				}));
			}
			this.SerializedObjectReferences = listSerializationOperator.SerializedObjects;
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x0009D608 File Offset: 0x0009B808
		private void Deserialize()
		{
			if (this.SerializedObjectReferences == null)
			{
				this.SerializedObjectReferences = new List<UnityEngine.Object>();
			}
			FullSerializerSerializer fullSerializerSerializer = fiSingletons.Get<FullSerializerSerializer>();
			ListSerializationOperator listSerializationOperator = fiSingletons.Get<ListSerializationOperator>();
			listSerializationOperator.SerializedObjects = this.SerializedObjectReferences;
			if (!string.IsNullOrEmpty(this.SerializedState))
			{
				try
				{
					this.Value = (T)((object)fullSerializerSerializer.Deserialize(typeof(T).Resolve(), this.SerializedState, listSerializationOperator));
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Exception caught when deserializing ",
						this,
						" (with type ",
						base.GetType(),
						");\n",
						ex
					}));
				}
			}
		}

		// Token: 0x04001908 RID: 6408
		public T Value;

		// Token: 0x04001909 RID: 6409
		[HideInInspector]
		[SerializeField]
		private string SerializedState;

		// Token: 0x0400190A RID: 6410
		[HideInInspector]
		[SerializeField]
		private List<UnityEngine.Object> SerializedObjectReferences;
	}
}
