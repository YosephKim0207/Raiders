using System;

namespace FullInspector
{
	// Token: 0x020005D1 RID: 1489
	public interface IInspectedMemberFilter
	{
		// Token: 0x0600234E RID: 9038
		bool IsInterested(InspectedProperty property);

		// Token: 0x0600234F RID: 9039
		bool IsInterested(InspectedMethod method);
	}
}
