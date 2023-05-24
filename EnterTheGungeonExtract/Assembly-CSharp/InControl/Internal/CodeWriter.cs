using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InControl.Internal
{
	// Token: 0x0200080A RID: 2058
	internal class CodeWriter
	{
		// Token: 0x06002BA3 RID: 11171 RVA: 0x000DD5CC File Offset: 0x000DB7CC
		public CodeWriter()
		{
			this.indent = 0;
			this.stringBuilder = new StringBuilder(4096);
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x000DD5EC File Offset: 0x000DB7EC
		public void IncreaseIndent()
		{
			this.indent++;
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x000DD5FC File Offset: 0x000DB7FC
		public void DecreaseIndent()
		{
			this.indent--;
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x000DD60C File Offset: 0x000DB80C
		public void Append(string code)
		{
			this.Append(false, code);
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x000DD618 File Offset: 0x000DB818
		public void Append(bool trim, string code)
		{
			if (trim)
			{
				code = code.Trim();
			}
			string[] array = Regex.Split(code, "\\r?\\n|\\n");
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				string text = array[i];
				if (!text.All(new Func<char, bool>(char.IsWhiteSpace)))
				{
					this.stringBuilder.Append('\t', this.indent);
					this.stringBuilder.Append(text);
				}
				if (i < num - 1)
				{
					this.stringBuilder.Append('\n');
				}
			}
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x000DD6BC File Offset: 0x000DB8BC
		public void AppendLine(string code)
		{
			this.Append(code);
			this.stringBuilder.Append('\n');
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000DD6D4 File Offset: 0x000DB8D4
		public void AppendLine(int count)
		{
			this.stringBuilder.Append('\n', count);
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x000DD6E8 File Offset: 0x000DB8E8
		public void AppendFormat(string format, params object[] args)
		{
			this.Append(string.Format(format, args));
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x000DD6F8 File Offset: 0x000DB8F8
		public void AppendLineFormat(string format, params object[] args)
		{
			this.AppendLine(string.Format(format, args));
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x000DD708 File Offset: 0x000DB908
		public override string ToString()
		{
			return this.stringBuilder.ToString();
		}

		// Token: 0x04001DDC RID: 7644
		private const char NewLine = '\n';

		// Token: 0x04001DDD RID: 7645
		private int indent;

		// Token: 0x04001DDE RID: 7646
		private StringBuilder stringBuilder;
	}
}
