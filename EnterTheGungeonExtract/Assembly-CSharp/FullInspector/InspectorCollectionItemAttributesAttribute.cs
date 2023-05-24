using System;
using System.Linq;
using System.Reflection;
using FullInspector.Internal;
using FullSerializer.Internal;

namespace FullInspector
{
	// Token: 0x020005F7 RID: 1527
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class InspectorCollectionItemAttributesAttribute : Attribute
	{
		// Token: 0x060023DA RID: 9178 RVA: 0x0009CE80 File Offset: 0x0009B080
		public InspectorCollectionItemAttributesAttribute(Type attributes)
		{
			if (!typeof(fiICollectionAttributeProvider).Resolve().IsAssignableFrom(attributes.Resolve()))
			{
				throw new ArgumentException("Must be an instance of FullInspector.fiICollectionAttributeProvider", "attributes");
			}
			fiICollectionAttributeProvider fiICollectionAttributeProvider = (fiICollectionAttributeProvider)Activator.CreateInstance(attributes);
			this.AttributeProvider = fiAttributeProvider.Create(fiICollectionAttributeProvider.GetAttributes().ToArray<object>());
		}

		// Token: 0x040018F1 RID: 6385
		public MemberInfo AttributeProvider;
	}
}
