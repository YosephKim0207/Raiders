using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000941 RID: 2369
	[Tooltip("Enables/Disables the PlayMakerGUI component in the scene. Note, you need a PlayMakerGUI component in the scene to see OnGUI actions. However, OnGUI can be very expensive on mobile devices. This action lets you turn OnGUI on/off (e.g., turn it on for a menu, and off during gameplay).")]
	[ActionCategory(ActionCategory.GUI)]
	public class EnableGUI : FsmStateAction
	{
		// Token: 0x060033D6 RID: 13270 RVA: 0x0010E5E0 File Offset: 0x0010C7E0
		public override void Reset()
		{
			this.enableGUI = true;
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x0010E5F0 File Offset: 0x0010C7F0
		public override void OnEnter()
		{
			PlayMakerGUI.Instance.enabled = this.enableGUI.Value;
			base.Finish();
		}

		// Token: 0x040024F8 RID: 9464
		[Tooltip("Set to True to enable, False to disable.")]
		public FsmBool enableGUI;
	}
}
