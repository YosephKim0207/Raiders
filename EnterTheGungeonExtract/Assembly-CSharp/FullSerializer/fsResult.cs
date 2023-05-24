using System;
using System.Collections.Generic;
using System.Linq;

namespace FullSerializer
{
	// Token: 0x020005B0 RID: 1456
	public struct fsResult
	{
		// Token: 0x0600227A RID: 8826 RVA: 0x00097E5C File Offset: 0x0009605C
		public void AddMessage(string message)
		{
			if (this._messages == null)
			{
				this._messages = new List<string>();
			}
			this._messages.Add(message);
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x00097E80 File Offset: 0x00096080
		public void AddMessages(fsResult result)
		{
			if (result._messages == null)
			{
				return;
			}
			if (this._messages == null)
			{
				this._messages = new List<string>();
			}
			this._messages.AddRange(result._messages);
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x00097EB8 File Offset: 0x000960B8
		public fsResult Merge(fsResult other)
		{
			this._success = this._success && other._success;
			if (other._messages != null)
			{
				if (this._messages == null)
				{
					this._messages = new List<string>(other._messages);
				}
				else
				{
					this._messages.AddRange(other._messages);
				}
			}
			return this;
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x00097F28 File Offset: 0x00096128
		public static fsResult Warn(string warning)
		{
			return new fsResult
			{
				_success = true,
				_messages = new List<string> { warning }
			};
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x00097F5C File Offset: 0x0009615C
		public static fsResult Fail(string warning)
		{
			return new fsResult
			{
				_success = false,
				_messages = new List<string> { warning }
			};
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x00097F90 File Offset: 0x00096190
		public static fsResult operator +(fsResult a, fsResult b)
		{
			return a.Merge(b);
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x00097F9C File Offset: 0x0009619C
		public bool Failed
		{
			get
			{
				return !this._success;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002281 RID: 8833 RVA: 0x00097FA8 File Offset: 0x000961A8
		public bool Succeeded
		{
			get
			{
				return this._success;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x00097FB0 File Offset: 0x000961B0
		public bool HasWarnings
		{
			get
			{
				return this._messages != null && this._messages.Any<string>();
			}
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x00097FCC File Offset: 0x000961CC
		public fsResult AssertSuccess()
		{
			if (this.Failed)
			{
				throw this.AsException;
			}
			return this;
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x00097FE8 File Offset: 0x000961E8
		public fsResult AssertSuccessWithoutWarnings()
		{
			if (this.Failed || this.RawMessages.Any<string>())
			{
				throw this.AsException;
			}
			return this;
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002285 RID: 8837 RVA: 0x00098014 File Offset: 0x00096214
		public Exception AsException
		{
			get
			{
				if (!this.Failed && !this.RawMessages.Any<string>())
				{
					throw new Exception("Only a failed result can be converted to an exception");
				}
				return new Exception(this.FormattedMessages);
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x00098048 File Offset: 0x00096248
		public IEnumerable<string> RawMessages
		{
			get
			{
				if (this._messages != null)
				{
					return this._messages;
				}
				return fsResult.EmptyStringArray;
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002287 RID: 8839 RVA: 0x00098064 File Offset: 0x00096264
		public string FormattedMessages
		{
			get
			{
				return string.Join(",\n", this.RawMessages.ToArray<string>());
			}
		}

		// Token: 0x04001855 RID: 6229
		private static readonly string[] EmptyStringArray = new string[0];

		// Token: 0x04001856 RID: 6230
		private bool _success;

		// Token: 0x04001857 RID: 6231
		private List<string> _messages;

		// Token: 0x04001858 RID: 6232
		public static fsResult Success = new fsResult
		{
			_success = true
		};
	}
}
