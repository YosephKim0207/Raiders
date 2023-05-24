using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EDE RID: 3806
	public class FlowBuilderDebugger
	{
		// Token: 0x06005114 RID: 20756 RVA: 0x001CAF94 File Offset: 0x001C9194
		public FlowBuilderDebugger()
		{
			this.builder = new StringBuilder();
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x001CAFA8 File Offset: 0x001C91A8
		public void Log(string s)
		{
			this.builder.AppendLine(s);
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x001CAFB8 File Offset: 0x001C91B8
		public void Log(RoomHandler parent, RoomHandler child)
		{
			this.builder.AppendLine(parent.area.prototypeRoom.name + " built " + child.area.prototypeRoom.name);
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x001CAFF0 File Offset: 0x001C91F0
		public void LogMonoHeapStatus()
		{
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x001CAFF4 File Offset: 0x001C91F4
		public void FinalizeLog()
		{
			string text = Application.dataPath + "\\dungeonDebug.txt";
			if (File.Exists(text))
			{
				FileInfo fileInfo = new FileInfo(text);
				fileInfo.IsReadOnly = false;
				File.Delete(text);
			}
			StreamWriter streamWriter = new StreamWriter(text);
			streamWriter.WriteLine(this.builder.ToString());
			streamWriter.Close();
		}

		// Token: 0x0400491E RID: 18718
		protected StringBuilder builder;
	}
}
