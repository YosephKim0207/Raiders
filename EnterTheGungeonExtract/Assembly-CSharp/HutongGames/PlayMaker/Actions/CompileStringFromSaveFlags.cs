using System;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C96 RID: 3222
	[Tooltip("Sets the value of a String Variable, based upon GungeonFlags.")]
	[ActionCategory(ActionCategory.String)]
	public class CompileStringFromSaveFlags : FsmStateAction
	{
		// Token: 0x060044FA RID: 17658 RVA: 0x00164A6C File Offset: 0x00162C6C
		public override void Reset()
		{
			this.stringVariable = null;
			this.stringComponents = new FsmString[0];
			this.flagComponents = new GungeonFlags[0];
			this.valueComponents = new FsmBool[0];
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x00164A9C File Offset: 0x00162C9C
		public override void OnEnter()
		{
			this.DoSetStringValue();
			base.Finish();
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x00164AAC File Offset: 0x00162CAC
		private void DoSetStringValue()
		{
			if (this.stringVariable == null)
			{
				return;
			}
			List<string> list = new List<string>();
			for (int i = 0; i < this.stringComponents.Length; i++)
			{
				if (this.flagComponents[i] == GungeonFlags.NONE || GameStatsManager.Instance.GetFlag(this.flagComponents[i]) == this.valueComponents[i].Value)
				{
					list.Add(StringTableManager.GetString(this.stringComponents[i].Value));
				}
			}
			string text = string.Empty;
			if (list.Count > 0)
			{
				text += list[0];
				char[] array = text.ToCharArray();
				array[0] = char.ToUpper(array[0]);
				text = new string(array);
				for (int j = 1; j < list.Count; j++)
				{
					if (list.Count == 2)
					{
						string text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							" ",
							StringTableManager.GetString("#AND"),
							" ",
							list[j]
						});
					}
					else if (j == list.Count - 1)
					{
						string text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							", ",
							StringTableManager.GetString("#AND"),
							" ",
							list[j]
						});
					}
					else
					{
						text = text + ", " + list[j];
					}
				}
				text += ".";
			}
			this.stringVariable.Value = text;
		}

		// Token: 0x0400370E RID: 14094
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x0400370F RID: 14095
		public FsmString[] stringComponents;

		// Token: 0x04003710 RID: 14096
		public GungeonFlags[] flagComponents;

		// Token: 0x04003711 RID: 14097
		public FsmBool[] valueComponents;
	}
}
