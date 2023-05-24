using System;

namespace FullSerializer
{
	// Token: 0x020005AD RID: 1453
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public sealed class fsObjectAttribute : Attribute
	{
		// Token: 0x0600226E RID: 8814 RVA: 0x00097DD8 File Offset: 0x00095FD8
		public fsObjectAttribute()
		{
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x00097DE8 File Offset: 0x00095FE8
		public fsObjectAttribute(string versionString, params Type[] previousModels)
		{
			this.VersionString = versionString;
			this.PreviousModels = previousModels;
		}

		// Token: 0x0400184E RID: 6222
		public Type[] PreviousModels;

		// Token: 0x0400184F RID: 6223
		public string VersionString;

		// Token: 0x04001850 RID: 6224
		public fsMemberSerialization MemberSerialization = fsMemberSerialization.Default;

		// Token: 0x04001851 RID: 6225
		public Type Converter;

		// Token: 0x04001852 RID: 6226
		public Type Processor;
	}
}
