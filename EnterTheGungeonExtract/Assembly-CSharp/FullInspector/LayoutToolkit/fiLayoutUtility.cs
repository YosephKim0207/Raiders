using System;

namespace FullInspector.LayoutToolkit
{
	// Token: 0x0200060E RID: 1550
	public static class fiLayoutUtility
	{
		// Token: 0x0600243B RID: 9275 RVA: 0x0009DAA8 File Offset: 0x0009BCA8
		public static fiLayout Margin(float margin, fiLayout layout)
		{
			return new fiHorizontalLayout
			{
				margin,
				new fiVerticalLayout { margin, layout, margin },
				margin
			};
		}
	}
}
