using System;

namespace FullSerializer
{
	// Token: 0x020005A5 RID: 1445
	public sealed class fsMissingVersionConstructorException : Exception
	{
		// Token: 0x06002240 RID: 8768 RVA: 0x0009689C File Offset: 0x00094A9C
		public fsMissingVersionConstructorException(Type versionedType, Type constructorType)
			: base(versionedType + " is missing a constructor for previous model type " + constructorType)
		{
		}
	}
}
