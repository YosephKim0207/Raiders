using System;

namespace InControl
{
	// Token: 0x0200080E RID: 2062
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class SingletonPrefabAttribute : Attribute
	{
		// Token: 0x06002BB3 RID: 11187 RVA: 0x000DD8B8 File Offset: 0x000DBAB8
		public SingletonPrefabAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x04001DE6 RID: 7654
		public readonly string Name;
	}
}
