using System;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x020005F4 RID: 1524
	[Serializable]
	public class fiUnityObjectReference
	{
		// Token: 0x060023D0 RID: 9168 RVA: 0x0009CD44 File Offset: 0x0009AF44
		public fiUnityObjectReference()
		{
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x0009CD4C File Offset: 0x0009AF4C
		public fiUnityObjectReference(UnityEngine.Object target)
		{
			this.Target = target;
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x0009CD5C File Offset: 0x0009AF5C
		public bool IsValid
		{
			get
			{
				return this.Target != null;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x060023D3 RID: 9171 RVA: 0x0009CD6C File Offset: 0x0009AF6C
		// (set) Token: 0x060023D4 RID: 9172 RVA: 0x0009CD8C File Offset: 0x0009AF8C
		public UnityEngine.Object Target
		{
			get
			{
				if (this._target == null)
				{
					this.TryRestoreFromInstanceId();
				}
				return this._target;
			}
			set
			{
				this._target = value;
			}
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x0009CD98 File Offset: 0x0009AF98
		private void TryRestoreFromInstanceId()
		{
			if (!object.ReferenceEquals(this._target, null))
			{
				int instanceID = this._target.GetInstanceID();
				this._target = fiLateBindings.EditorUtility.InstanceIDToObject(instanceID);
			}
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x0009CDD0 File Offset: 0x0009AFD0
		public override int GetHashCode()
		{
			if (!this.IsValid)
			{
				return 0;
			}
			return this.Target.GetHashCode();
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x0009CDEC File Offset: 0x0009AFEC
		public override bool Equals(object obj)
		{
			fiUnityObjectReference fiUnityObjectReference = obj as fiUnityObjectReference;
			return fiUnityObjectReference != null && fiUnityObjectReference.Target == this.Target;
		}

		// Token: 0x040018EF RID: 6383
		[SerializeField]
		private UnityEngine.Object _target;
	}
}
