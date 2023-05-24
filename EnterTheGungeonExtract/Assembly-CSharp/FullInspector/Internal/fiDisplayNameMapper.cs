using System;
using System.Collections.Generic;
using System.Text;

namespace FullInspector.Internal
{
	// Token: 0x02000550 RID: 1360
	public static class fiDisplayNameMapper
	{
		// Token: 0x06002051 RID: 8273 RVA: 0x0008F920 File Offset: 0x0008DB20
		public static string Map(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				return string.Empty;
			}
			string text;
			if (!fiDisplayNameMapper._mappedNames.TryGetValue(propertyName, out text))
			{
				text = fiDisplayNameMapper.MapInternal(propertyName);
				fiDisplayNameMapper._mappedNames[propertyName] = text;
			}
			return text;
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x0008F964 File Offset: 0x0008DB64
		private static string MapInternal(string propertyName)
		{
			if (propertyName.StartsWith("m_") && propertyName != "m_")
			{
				propertyName = propertyName.Substring(2);
			}
			int num = 0;
			while (num < propertyName.Length && propertyName[num] == '_')
			{
				num++;
			}
			if (num >= propertyName.Length)
			{
				return propertyName;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			for (int i = num; i < propertyName.Length; i++)
			{
				char c = propertyName[i];
				if (c == '_')
				{
					flag = true;
				}
				else
				{
					if (flag)
					{
						flag = false;
						c = char.ToUpper(c);
					}
					if (i != num && fiDisplayNameMapper.ShouldInsertSpace(i, propertyName))
					{
						stringBuilder.Append(' ');
					}
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x0008FA44 File Offset: 0x0008DC44
		private static bool ShouldInsertSpace(int currentIndex, string str)
		{
			return char.IsUpper(str[currentIndex]) && currentIndex + 1 < str.Length && !char.IsUpper(str[currentIndex + 1]);
		}

		// Token: 0x04001795 RID: 6037
		private static readonly Dictionary<string, string> _mappedNames = new Dictionary<string, string>();
	}
}
