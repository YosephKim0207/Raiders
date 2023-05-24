using System;

namespace FullSerializer
{
	// Token: 0x020005AF RID: 1455
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class fsPropertyAttribute : Attribute
	{
		// Token: 0x06002277 RID: 8823 RVA: 0x00097E2C File Offset: 0x0009602C
		public fsPropertyAttribute()
			: this(string.Empty)
		{
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00097E3C File Offset: 0x0009603C
		public fsPropertyAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x00097E4C File Offset: 0x0009604C
		public fsPropertyAttribute(bool deserializeOnly)
		{
			this.DeserializeOnly = deserializeOnly;
		}

		// Token: 0x04001853 RID: 6227
		public string Name;

		// Token: 0x04001854 RID: 6228
		public bool DeserializeOnly;
	}
}
