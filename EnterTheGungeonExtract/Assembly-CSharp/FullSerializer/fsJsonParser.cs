using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FullSerializer
{
	// Token: 0x020005AA RID: 1450
	public class fsJsonParser
	{
		// Token: 0x0600224D RID: 8781 RVA: 0x00096990 File Offset: 0x00094B90
		private fsJsonParser(string input)
		{
			this._input = input;
			this._start = 0;
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x000969B8 File Offset: 0x00094BB8
		private fsResult MakeFailure(string message)
		{
			int num = Math.Max(0, this._start - 20);
			int num2 = Math.Min(50, this._input.Length - num);
			string text = string.Concat(new string[]
			{
				"Error while parsing: ",
				message,
				"; context = <",
				this._input.Substring(num, num2),
				">"
			});
			return fsResult.Fail(text);
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x00096A28 File Offset: 0x00094C28
		private bool TryMoveNext()
		{
			if (this._start < this._input.Length)
			{
				this._start++;
				return true;
			}
			return false;
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x00096A54 File Offset: 0x00094C54
		private bool HasValue()
		{
			return this.HasValue(0);
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x00096A60 File Offset: 0x00094C60
		private bool HasValue(int offset)
		{
			return this._start + offset >= 0 && this._start + offset < this._input.Length;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x00096A88 File Offset: 0x00094C88
		private char Character()
		{
			return this.Character(0);
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x00096A94 File Offset: 0x00094C94
		private char Character(int offset)
		{
			return this._input[this._start + offset];
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x00096AAC File Offset: 0x00094CAC
		private void SkipSpace()
		{
			while (this.HasValue())
			{
				char c = this.Character();
				if (char.IsWhiteSpace(c))
				{
					this.TryMoveNext();
				}
				else
				{
					if (!this.HasValue(1) || this.Character(0) != '/')
					{
						break;
					}
					if (this.Character(1) == '/')
					{
						while (this.HasValue() && !Environment.NewLine.Contains(string.Empty + this.Character()))
						{
							this.TryMoveNext();
						}
					}
					else if (this.Character(1) == '*')
					{
						this.TryMoveNext();
						this.TryMoveNext();
						while (this.HasValue(1))
						{
							if (this.Character(0) == '*' && this.Character(1) == '/')
							{
								this.TryMoveNext();
								this.TryMoveNext();
								this.TryMoveNext();
								break;
							}
							this.TryMoveNext();
						}
					}
				}
			}
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x00096BC4 File Offset: 0x00094DC4
		private bool IsHex(char c)
		{
			return (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x00096BFC File Offset: 0x00094DFC
		private uint ParseSingleChar(char c1, uint multipliyer)
		{
			uint num = 0U;
			if (c1 >= '0' && c1 <= '9')
			{
				num = (uint)(c1 - '0') * multipliyer;
			}
			else if (c1 >= 'A' && c1 <= 'F')
			{
				num = (uint)(c1 - 'A' + '\n') * multipliyer;
			}
			else if (c1 >= 'a' && c1 <= 'f')
			{
				num = (uint)(c1 - 'a' + '\n') * multipliyer;
			}
			return num;
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x00096C64 File Offset: 0x00094E64
		private uint ParseUnicode(char c1, char c2, char c3, char c4)
		{
			uint num = this.ParseSingleChar(c1, 4096U);
			uint num2 = this.ParseSingleChar(c2, 256U);
			uint num3 = this.ParseSingleChar(c3, 16U);
			uint num4 = this.ParseSingleChar(c4, 1U);
			return num + num2 + num3 + num4;
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x00096CA8 File Offset: 0x00094EA8
		private fsResult TryUnescapeChar(out char escaped)
		{
			this.TryMoveNext();
			if (!this.HasValue())
			{
				escaped = ' ';
				return this.MakeFailure("Unexpected end of input after \\");
			}
			char c = this.Character();
			switch (c)
			{
			case 'a':
				this.TryMoveNext();
				escaped = '\a';
				return fsResult.Success;
			case 'b':
				this.TryMoveNext();
				escaped = '\b';
				return fsResult.Success;
			default:
				switch (c)
				{
				case 'r':
					this.TryMoveNext();
					escaped = '\r';
					return fsResult.Success;
				default:
					if (c == '/')
					{
						this.TryMoveNext();
						escaped = '/';
						return fsResult.Success;
					}
					if (c == '0')
					{
						this.TryMoveNext();
						escaped = '\0';
						return fsResult.Success;
					}
					if (c == '"')
					{
						this.TryMoveNext();
						escaped = '"';
						return fsResult.Success;
					}
					if (c == '\\')
					{
						this.TryMoveNext();
						escaped = '\\';
						return fsResult.Success;
					}
					if (c != 'n')
					{
						escaped = '\0';
						return this.MakeFailure(string.Format("Invalid escape sequence \\{0}", this.Character()));
					}
					this.TryMoveNext();
					escaped = '\n';
					return fsResult.Success;
				case 't':
					this.TryMoveNext();
					escaped = '\t';
					return fsResult.Success;
				case 'u':
					this.TryMoveNext();
					if (this.IsHex(this.Character(0)) && this.IsHex(this.Character(1)) && this.IsHex(this.Character(2)) && this.IsHex(this.Character(3)))
					{
						uint num = this.ParseUnicode(this.Character(0), this.Character(1), this.Character(2), this.Character(3));
						this.TryMoveNext();
						this.TryMoveNext();
						this.TryMoveNext();
						this.TryMoveNext();
						escaped = (char)num;
						return fsResult.Success;
					}
					escaped = '\0';
					return this.MakeFailure(string.Format("invalid escape sequence '\\u{0}{1}{2}{3}'\n", new object[]
					{
						this.Character(0),
						this.Character(1),
						this.Character(2),
						this.Character(3)
					}));
				}
				break;
			case 'f':
				this.TryMoveNext();
				escaped = '\f';
				return fsResult.Success;
			}
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x00096EF8 File Offset: 0x000950F8
		private fsResult TryParseExact(string content)
		{
			for (int i = 0; i < content.Length; i++)
			{
				if (this.Character() != content[i])
				{
					return this.MakeFailure("Expected " + content[i]);
				}
				if (!this.TryMoveNext())
				{
					return this.MakeFailure("Unexpected end of content when parsing " + content);
				}
			}
			return fsResult.Success;
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x00096F70 File Offset: 0x00095170
		private fsResult TryParseTrue(out fsData data)
		{
			fsResult fsResult = this.TryParseExact("true");
			if (fsResult.Succeeded)
			{
				data = new fsData(true);
				return fsResult.Success;
			}
			data = null;
			return fsResult;
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x00096FA8 File Offset: 0x000951A8
		private fsResult TryParseFalse(out fsData data)
		{
			fsResult fsResult = this.TryParseExact("false");
			if (fsResult.Succeeded)
			{
				data = new fsData(false);
				return fsResult.Success;
			}
			data = null;
			return fsResult;
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x00096FE0 File Offset: 0x000951E0
		private fsResult TryParseNull(out fsData data)
		{
			fsResult fsResult = this.TryParseExact("null");
			if (fsResult.Succeeded)
			{
				data = new fsData();
				return fsResult.Success;
			}
			data = null;
			return fsResult;
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00097018 File Offset: 0x00095218
		private bool IsSeparator(char c)
		{
			return char.IsWhiteSpace(c) || c == ',' || c == '}' || c == ']';
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x00097040 File Offset: 0x00095240
		private fsResult TryParseNumber(out fsData data)
		{
			int start = this._start;
			while (this.TryMoveNext() && this.HasValue() && !this.IsSeparator(this.Character()))
			{
			}
			string text = this._input.Substring(start, this._start - start);
			if (text.Contains(".") || text == "Infinity" || text == "-Infinity" || text == "NaN")
			{
				double num;
				if (!double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out num))
				{
					data = null;
					return this.MakeFailure("Bad double format with " + text);
				}
				data = new fsData(num);
				return fsResult.Success;
			}
			else
			{
				long num2;
				if (!long.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out num2))
				{
					data = null;
					return this.MakeFailure("Bad Int64 format with " + text);
				}
				data = new fsData(num2);
				return fsResult.Success;
			}
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x00097148 File Offset: 0x00095348
		private fsResult TryParseString(out string str)
		{
			this._cachedStringBuilder.Length = 0;
			if (this.Character() != '"' || !this.TryMoveNext())
			{
				str = string.Empty;
				return this.MakeFailure("Expected initial \" when parsing a string");
			}
			while (this.HasValue() && this.Character() != '"')
			{
				char c = this.Character();
				if (c == '\\')
				{
					char c2;
					fsResult fsResult = this.TryUnescapeChar(out c2);
					if (fsResult.Failed)
					{
						str = string.Empty;
						return fsResult;
					}
					this._cachedStringBuilder.Append(c2);
				}
				else
				{
					this._cachedStringBuilder.Append(c);
					if (!this.TryMoveNext())
					{
						str = string.Empty;
						return this.MakeFailure("Unexpected end of input when reading a string");
					}
				}
			}
			if (!this.HasValue() || this.Character() != '"' || !this.TryMoveNext())
			{
				str = string.Empty;
				return this.MakeFailure("No closing \" when parsing a string");
			}
			str = this._cachedStringBuilder.ToString();
			return fsResult.Success;
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x0009725C File Offset: 0x0009545C
		private fsResult TryParseArray(out fsData arr)
		{
			if (this.Character() != '[')
			{
				arr = null;
				return this.MakeFailure("Expected initial [ when parsing an array");
			}
			if (!this.TryMoveNext())
			{
				arr = null;
				return this.MakeFailure("Unexpected end of input when parsing an array");
			}
			this.SkipSpace();
			List<fsData> list = new List<fsData>();
			while (this.HasValue() && this.Character() != ']')
			{
				fsData fsData;
				fsResult fsResult = this.RunParse(out fsData);
				if (fsResult.Failed)
				{
					arr = null;
					return fsResult;
				}
				list.Add(fsData);
				this.SkipSpace();
				if (this.HasValue() && this.Character() == ',')
				{
					if (!this.TryMoveNext())
					{
						break;
					}
					this.SkipSpace();
				}
			}
			if (!this.HasValue() || this.Character() != ']' || !this.TryMoveNext())
			{
				arr = null;
				return this.MakeFailure("No closing ] for array");
			}
			arr = new fsData(list);
			return fsResult.Success;
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x0009735C File Offset: 0x0009555C
		private fsResult TryParseObject(out fsData obj)
		{
			if (this.Character() != '{')
			{
				obj = null;
				return this.MakeFailure("Expected initial { when parsing an object");
			}
			if (!this.TryMoveNext())
			{
				obj = null;
				return this.MakeFailure("Unexpected end of input when parsing an object");
			}
			this.SkipSpace();
			Dictionary<string, fsData> dictionary = new Dictionary<string, fsData>((!fsConfig.IsCaseSensitive) ? StringComparer.CurrentCultureIgnoreCase : StringComparer.CurrentCulture);
			while (this.HasValue() && this.Character() != '}')
			{
				this.SkipSpace();
				string text;
				fsResult fsResult = this.TryParseString(out text);
				if (fsResult.Failed)
				{
					obj = null;
					return fsResult;
				}
				this.SkipSpace();
				if (!this.HasValue() || this.Character() != ':' || !this.TryMoveNext())
				{
					obj = null;
					return this.MakeFailure("Expected : after key \"" + text + "\"");
				}
				this.SkipSpace();
				fsData fsData;
				fsResult = this.RunParse(out fsData);
				if (fsResult.Failed)
				{
					obj = null;
					return fsResult;
				}
				dictionary.Add(text, fsData);
				this.SkipSpace();
				if (this.HasValue() && this.Character() == ',')
				{
					if (!this.TryMoveNext())
					{
						break;
					}
					this.SkipSpace();
				}
			}
			if (!this.HasValue() || this.Character() != '}' || !this.TryMoveNext())
			{
				obj = null;
				return this.MakeFailure("No closing } for object");
			}
			obj = new fsData(dictionary);
			return fsResult.Success;
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000974E0 File Offset: 0x000956E0
		private fsResult RunParse(out fsData data)
		{
			this.SkipSpace();
			if (!this.HasValue())
			{
				data = null;
				return this.MakeFailure("Unexpected end of input");
			}
			char c = this.Character();
			switch (c)
			{
			case '+':
			case '-':
			case '.':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				break;
			default:
				if (c != '"')
				{
					if (c != 'I' && c != 'N')
					{
						if (c == '[')
						{
							return this.TryParseArray(out data);
						}
						if (c == 'f')
						{
							return this.TryParseFalse(out data);
						}
						if (c == 'n')
						{
							return this.TryParseNull(out data);
						}
						if (c == 't')
						{
							return this.TryParseTrue(out data);
						}
						if (c != '{')
						{
							data = null;
							return this.MakeFailure("unable to parse; invalid token \"" + this.Character() + "\"");
						}
						return this.TryParseObject(out data);
					}
				}
				else
				{
					string text;
					fsResult fsResult = this.TryParseString(out text);
					if (fsResult.Failed)
					{
						data = null;
						return fsResult;
					}
					data = new fsData(text);
					return fsResult.Success;
				}
				break;
			}
			return this.TryParseNumber(out data);
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x0009761C File Offset: 0x0009581C
		public static fsResult Parse(string input, out fsData data)
		{
			if (string.IsNullOrEmpty(input))
			{
				data = null;
				return fsResult.Fail("No input");
			}
			fsJsonParser fsJsonParser = new fsJsonParser(input);
			return fsJsonParser.RunParse(out data);
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x00097650 File Offset: 0x00095850
		public static fsData Parse(string input)
		{
			fsData fsData;
			fsJsonParser.Parse(input, out fsData).AssertSuccess();
			return fsData;
		}

		// Token: 0x04001847 RID: 6215
		private int _start;

		// Token: 0x04001848 RID: 6216
		private string _input;

		// Token: 0x04001849 RID: 6217
		private readonly StringBuilder _cachedStringBuilder = new StringBuilder(256);
	}
}
