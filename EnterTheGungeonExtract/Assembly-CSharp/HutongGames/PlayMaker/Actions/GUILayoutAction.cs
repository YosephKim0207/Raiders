using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009C9 RID: 2505
	[Tooltip("GUILayout base action - don't use!")]
	public abstract class GUILayoutAction : FsmStateAction
	{
		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x0600361B RID: 13851 RVA: 0x001152E8 File Offset: 0x001134E8
		public GUILayoutOption[] LayoutOptions
		{
			get
			{
				if (this.options == null)
				{
					this.options = new GUILayoutOption[this.layoutOptions.Length];
					for (int i = 0; i < this.layoutOptions.Length; i++)
					{
						this.options[i] = this.layoutOptions[i].GetGUILayoutOption();
					}
				}
				return this.options;
			}
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x00115348 File Offset: 0x00113548
		public override void Reset()
		{
			this.layoutOptions = new LayoutOption[0];
		}

		// Token: 0x04002769 RID: 10089
		public LayoutOption[] layoutOptions;

		// Token: 0x0400276A RID: 10090
		private GUILayoutOption[] options;
	}
}
