using System;
using System.Collections.Generic;

namespace DaikonForge.Editor
{
	// Token: 0x02000524 RID: 1316
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public class InspectorGroupOrderAttribute : Attribute
	{
		// Token: 0x06001FA2 RID: 8098 RVA: 0x0008DFA4 File Offset: 0x0008C1A4
		public InspectorGroupOrderAttribute(params string[] groups)
		{
			this.Groups.AddRange(groups);
		}

		// Token: 0x04001753 RID: 5971
		public List<string> Groups = new List<string>();
	}
}
