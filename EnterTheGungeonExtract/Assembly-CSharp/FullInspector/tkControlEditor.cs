using System;

namespace FullInspector
{
	// Token: 0x0200066E RID: 1646
	public class tkControlEditor
	{
		// Token: 0x0600259D RID: 9629 RVA: 0x000A1178 File Offset: 0x0009F378
		public tkControlEditor(tkIControl control)
			: this(false, control)
		{
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x000A1184 File Offset: 0x0009F384
		public tkControlEditor(bool debug, tkIControl control)
		{
			this.Debug = debug;
			this.Control = control;
			int num = 0;
			control.InitializeId(ref num);
		}

		// Token: 0x040019A2 RID: 6562
		public bool Debug;

		// Token: 0x040019A3 RID: 6563
		public tkIControl Control;

		// Token: 0x040019A4 RID: 6564
		public object Context;
	}
}
