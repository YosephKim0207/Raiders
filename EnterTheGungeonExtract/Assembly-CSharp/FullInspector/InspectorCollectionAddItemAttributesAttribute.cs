using System;
using System.Linq;
using System.Reflection;
using FullInspector.Internal;
using FullSerializer.Internal;

namespace FullInspector
{
	// Token: 0x020005F6 RID: 1526
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorCollectionAddItemAttributesAttribute : Attribute
	{
		// Token: 0x060023D9 RID: 9177 RVA: 0x0009CE1C File Offset: 0x0009B01C
		public InspectorCollectionAddItemAttributesAttribute(Type attributes)
		{
			if (!typeof(fiICollectionAttributeProvider).Resolve().IsAssignableFrom(attributes.Resolve()))
			{
				throw new ArgumentException("Must be an instance of FullInspector.fiICollectionAttributeProvider", "attributes");
			}
			fiICollectionAttributeProvider fiICollectionAttributeProvider = (fiICollectionAttributeProvider)Activator.CreateInstance(attributes);
			this.AttributeProvider = fiAttributeProvider.Create(fiICollectionAttributeProvider.GetAttributes().ToArray<object>());
		}

		// Token: 0x040018F0 RID: 6384
		public MemberInfo AttributeProvider;
	}
}
