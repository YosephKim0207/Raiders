using System;

namespace FullSerializer
{
	// Token: 0x020005AE RID: 1454
	public abstract class fsObjectProcessor
	{
		// Token: 0x06002271 RID: 8817 RVA: 0x00097E10 File Offset: 0x00096010
		public virtual bool CanProcess(Type type)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x00097E18 File Offset: 0x00096018
		public virtual void OnBeforeSerialize(Type storageType, object instance)
		{
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x00097E1C File Offset: 0x0009601C
		public virtual void OnAfterSerialize(Type storageType, object instance, ref fsData data)
		{
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x00097E20 File Offset: 0x00096020
		public virtual void OnBeforeDeserialize(Type storageType, ref fsData data)
		{
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x00097E24 File Offset: 0x00096024
		public virtual void OnBeforeDeserializeAfterInstanceCreation(Type storageType, object instance, ref fsData data)
		{
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x00097E28 File Offset: 0x00096028
		public virtual void OnAfterDeserialize(Type storageType, object instance)
		{
		}
	}
}
