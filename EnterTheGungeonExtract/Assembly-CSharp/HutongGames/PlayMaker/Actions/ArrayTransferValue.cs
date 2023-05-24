using System;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008E9 RID: 2281
	[Tooltip("Transfer a value from one array to another, basically a copy/cut paste action on steroids.")]
	[NoActionTargets]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayTransferValue : FsmStateAction
	{
		// Token: 0x06003270 RID: 12912 RVA: 0x0010916C File Offset: 0x0010736C
		public override void Reset()
		{
			this.arraySource = null;
			this.arrayTarget = null;
			this.indexToTransfer = null;
			this.copyType = ArrayTransferValue.ArrayTransferType.Copy;
			this.pasteType = ArrayTransferValue.ArrayPasteType.AsLastItem;
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x001091A8 File Offset: 0x001073A8
		public override void OnEnter()
		{
			this.DoTransferValue();
			base.Finish();
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x001091B8 File Offset: 0x001073B8
		private void DoTransferValue()
		{
			if (this.arraySource.IsNone || this.arrayTarget.IsNone)
			{
				return;
			}
			int value = this.indexToTransfer.Value;
			if (value < 0 || value >= this.arraySource.Length)
			{
				base.Fsm.Event(this.indexOutOfRange);
				return;
			}
			object obj = this.arraySource.Values[value];
			if ((ArrayTransferValue.ArrayTransferType)this.copyType.Value == ArrayTransferValue.ArrayTransferType.Cut)
			{
				List<object> list = new List<object>(this.arraySource.Values);
				list.RemoveAt(value);
				this.arraySource.Values = list.ToArray();
			}
			else if ((ArrayTransferValue.ArrayTransferType)this.copyType.Value == ArrayTransferValue.ArrayTransferType.nullify)
			{
				this.arraySource.Values.SetValue(null, value);
			}
			if ((ArrayTransferValue.ArrayPasteType)this.pasteType.Value == ArrayTransferValue.ArrayPasteType.AsFirstItem)
			{
				List<object> list2 = new List<object>(this.arrayTarget.Values);
				list2.Insert(0, obj);
				this.arrayTarget.Values = list2.ToArray();
			}
			else if ((ArrayTransferValue.ArrayPasteType)this.pasteType.Value == ArrayTransferValue.ArrayPasteType.AsLastItem)
			{
				this.arrayTarget.Resize(this.arrayTarget.Length + 1);
				this.arrayTarget.Set(this.arrayTarget.Length - 1, obj);
			}
			else if ((ArrayTransferValue.ArrayPasteType)this.pasteType.Value == ArrayTransferValue.ArrayPasteType.InsertAtSameIndex)
			{
				if (value >= this.arrayTarget.Length)
				{
					base.Fsm.Event(this.indexOutOfRange);
				}
				List<object> list3 = new List<object>(this.arrayTarget.Values);
				list3.Insert(value, obj);
				this.arrayTarget.Values = list3.ToArray();
			}
			else if ((ArrayTransferValue.ArrayPasteType)this.pasteType.Value == ArrayTransferValue.ArrayPasteType.ReplaceAtSameIndex)
			{
				if (value >= this.arrayTarget.Length)
				{
					base.Fsm.Event(this.indexOutOfRange);
				}
				else
				{
					this.arrayTarget.Set(value, obj);
				}
			}
		}

		// Token: 0x04002383 RID: 9091
		[RequiredField]
		[Tooltip("The Array Variable source.")]
		[UIHint(UIHint.Variable)]
		public FsmArray arraySource;

		// Token: 0x04002384 RID: 9092
		[Tooltip("The Array Variable target.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmArray arrayTarget;

		// Token: 0x04002385 RID: 9093
		[Tooltip("The index to transfer.")]
		[MatchFieldType("array")]
		public FsmInt indexToTransfer;

		// Token: 0x04002386 RID: 9094
		[ObjectType(typeof(ArrayTransferValue.ArrayTransferType))]
		[ActionSection("Transfer Options")]
		public FsmEnum copyType;

		// Token: 0x04002387 RID: 9095
		[ObjectType(typeof(ArrayTransferValue.ArrayPasteType))]
		public FsmEnum pasteType;

		// Token: 0x04002388 RID: 9096
		[ActionSection("Result")]
		[Tooltip("Event sent if this array source does not contains that element (described below)")]
		public FsmEvent indexOutOfRange;

		// Token: 0x020008EA RID: 2282
		public enum ArrayTransferType
		{
			// Token: 0x0400238A RID: 9098
			Copy,
			// Token: 0x0400238B RID: 9099
			Cut,
			// Token: 0x0400238C RID: 9100
			nullify
		}

		// Token: 0x020008EB RID: 2283
		public enum ArrayPasteType
		{
			// Token: 0x0400238E RID: 9102
			AsFirstItem,
			// Token: 0x0400238F RID: 9103
			AsLastItem,
			// Token: 0x04002390 RID: 9104
			InsertAtSameIndex,
			// Token: 0x04002391 RID: 9105
			ReplaceAtSameIndex
		}
	}
}
