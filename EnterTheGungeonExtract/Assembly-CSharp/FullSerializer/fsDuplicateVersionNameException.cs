using System;

namespace FullSerializer
{
	// Token: 0x020005A6 RID: 1446
	public sealed class fsDuplicateVersionNameException : Exception
	{
		// Token: 0x06002241 RID: 8769 RVA: 0x000968B0 File Offset: 0x00094AB0
		public fsDuplicateVersionNameException(Type typeA, Type typeB, string version)
			: base(string.Concat(new object[] { typeA, " and ", typeB, " have the same version string (", version, "); please change one of them." }))
		{
		}
	}
}
