using System;
using UnityEngine;

// Token: 0x020016E0 RID: 5856
public class DodgeRollSynergyProcessor : MonoBehaviour
{
	// Token: 0x0600883A RID: 34874 RVA: 0x00387570 File Offset: 0x00385770
	private void Awake()
	{
		this.m_item = base.GetComponent<PassiveItem>();
		PassiveItem item = this.m_item;
		item.OnPickedUp = (Action<PlayerController>)Delegate.Combine(item.OnPickedUp, new Action<PlayerController>(this.HandlePickedUp));
	}

	// Token: 0x0600883B RID: 34875 RVA: 0x003875A8 File Offset: 0x003857A8
	private void HandlePickedUp(PlayerController obj)
	{
		this.m_player = obj;
		this.m_player.OnIsRolling += this.HandleRollFrame;
	}

	// Token: 0x0600883C RID: 34876 RVA: 0x003875C8 File Offset: 0x003857C8
	private void HandleRollFrame(PlayerController sourcePlayer)
	{
		if (!this.LeavesGoopTrail)
		{
			return;
		}
		if (!this.m_player)
		{
			return;
		}
		if (!this.m_player.HasActiveBonusSynergy(this.GoopTrailRequiredSynergy, false))
		{
			return;
		}
		if (!this.m_item || this.m_item.Owner != this.m_player)
		{
			this.m_player.OnIsRolling -= this.HandleRollFrame;
			return;
		}
		DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.GoopTrailGoop).AddGoopCircle(this.m_player.specRigidbody.UnitCenter, this.GoopTrailRadius, -1, false, -1);
	}

	// Token: 0x04008D87 RID: 36231
	public bool LeavesGoopTrail;

	// Token: 0x04008D88 RID: 36232
	public CustomSynergyType GoopTrailRequiredSynergy;

	// Token: 0x04008D89 RID: 36233
	public GoopDefinition GoopTrailGoop;

	// Token: 0x04008D8A RID: 36234
	public float GoopTrailRadius;

	// Token: 0x04008D8B RID: 36235
	private PassiveItem m_item;

	// Token: 0x04008D8C RID: 36236
	private PlayerController m_player;
}
