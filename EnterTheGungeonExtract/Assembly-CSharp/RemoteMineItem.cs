using System;
using UnityEngine;

// Token: 0x02001487 RID: 5255
public class RemoteMineItem : PlayerItem
{
	// Token: 0x0600777C RID: 30588 RVA: 0x002FA04C File Offset: 0x002F824C
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_mine_set_01", base.gameObject);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.objectToSpawn, user.specRigidbody.UnitCenter, Quaternion.identity);
		this.m_originalSprite = base.sprite.spriteId;
		base.sprite.SetSprite(this.detonatorSprite);
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		this.m_extantEffect = gameObject.GetComponent<RemoteMineController>();
		if (component != null)
		{
			component.PlaceAtPositionByAnchor(user.specRigidbody.UnitCenter.ToVector3ZUp(component.transform.position.z), tk2dBaseSprite.Anchor.MiddleCenter);
		}
		this.m_isCurrentlyActive = true;
	}

	// Token: 0x0600777D RID: 30589 RVA: 0x002FA100 File Offset: 0x002F8300
	public override void Update()
	{
		if (this.m_extantEffect == null)
		{
			base.Update();
		}
		else if (TimeTubeCreditsController.IsTimeTubing)
		{
			UnityEngine.Object.Destroy(this.m_extantEffect.gameObject);
			this.m_extantEffect = null;
		}
	}

	// Token: 0x0600777E RID: 30590 RVA: 0x002FA140 File Offset: 0x002F8340
	protected override void OnPreDrop(PlayerController user)
	{
		if (this.m_isCurrentlyActive)
		{
			this.DoActiveEffect(user);
		}
		base.OnPreDrop(user);
	}

	// Token: 0x0600777F RID: 30591 RVA: 0x002FA15C File Offset: 0x002F835C
	protected override void DoActiveEffect(PlayerController user)
	{
		if (this.m_extantEffect != null)
		{
			this.m_extantEffect.Detonate();
			this.m_extantEffect = null;
		}
		base.sprite.SetSprite(this.m_originalSprite);
		this.m_isCurrentlyActive = false;
	}

	// Token: 0x06007780 RID: 30592 RVA: 0x002FA19C File Offset: 0x002F839C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400796F RID: 31087
	public GameObject objectToSpawn;

	// Token: 0x04007970 RID: 31088
	public string detonatorSprite = "c4_transmitter_001";

	// Token: 0x04007971 RID: 31089
	protected RemoteMineController m_extantEffect;

	// Token: 0x04007972 RID: 31090
	protected int m_originalSprite;
}
