using System;

// Token: 0x020015A8 RID: 5544
public class NoBlankChecker : BraveBehaviour
{
	// Token: 0x06007F3D RID: 32573 RVA: 0x00336398 File Offset: 0x00334598
	public void Update()
	{
		if (GameManager.Instance.BestActivePlayer != null && GameManager.Instance.BestActivePlayer.Blanks == 0)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("hasNoBlanks");
		}
	}
}
