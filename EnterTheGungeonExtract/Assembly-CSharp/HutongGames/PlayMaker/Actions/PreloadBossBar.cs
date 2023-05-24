using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CAA RID: 3242
	[ActionCategory(".NPCs")]
	public class PreloadBossBar : FsmStateAction
	{
		// Token: 0x06004541 RID: 17729 RVA: 0x00166CFC File Offset: 0x00164EFC
		public override void OnEnter()
		{
			GameUIRoot.Instance.bossController.ForceUpdateBossHealth(100f, 100f, StringTableManager.GetEnemiesString("#MANFRED_ENCNAME", -1));
			GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_Boss_Theme_Gull", base.Owner.gameObject);
			base.Finish();
		}
	}
}
