using System;
using System.Collections.Generic;
using System.IO;

namespace FullInspector.Internal
{
	// Token: 0x0200055F RID: 1375
	public static class fiDirectory
	{
		// Token: 0x060020B6 RID: 8374 RVA: 0x00090CF0 File Offset: 0x0008EEF0
		public static bool Exists(string path)
		{
			return Directory.Exists(path);
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00090CF8 File Offset: 0x0008EEF8
		public static void CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x00090D04 File Offset: 0x0008EF04
		public static IEnumerable<string> GetDirectories(string path)
		{
			return Directory.GetDirectories(path);
		}
	}
}
