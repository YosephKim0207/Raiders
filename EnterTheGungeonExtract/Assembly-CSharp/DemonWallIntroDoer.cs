using System;
using UnityEngine;

// Token: 0x0200100F RID: 4111
[RequireComponent(typeof(GenericIntroDoer))]
public class DemonWallIntroDoer : SpecificIntroDoer
{
	// Token: 0x060059FB RID: 23035 RVA: 0x00226354 File Offset: 0x00224554
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x17000D04 RID: 3332
	// (get) Token: 0x060059FC RID: 23036 RVA: 0x0022635C File Offset: 0x0022455C
	public override Vector2? OverrideOutroPosition
	{
		get
		{
			return new Vector2?(base.GetComponent<DemonWallController>().CameraPos);
		}
	}

	// Token: 0x060059FD RID: 23037 RVA: 0x00226370 File Offset: 0x00224570
	public override void OnCameraIntro()
	{
		base.aiAnimator.PlayUntilCancelled(this.preIntro, false, null, -1f, false);
	}

	// Token: 0x060059FE RID: 23038 RVA: 0x0022638C File Offset: 0x0022458C
	public override void OnCleanup()
	{
		base.aiAnimator.EndAnimation();
	}

	// Token: 0x060059FF RID: 23039 RVA: 0x0022639C File Offset: 0x0022459C
	public override void EndIntro()
	{
		base.GetComponent<DemonWallController>().ModifyCamera(true);
	}

	// Token: 0x04005363 RID: 21347
	public string preIntro;
}
