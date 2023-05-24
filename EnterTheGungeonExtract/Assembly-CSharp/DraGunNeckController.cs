using System;

// Token: 0x02001022 RID: 4130
public class DraGunNeckController : BraveBehaviour
{
	// Token: 0x06005A93 RID: 23187 RVA: 0x00229A9C File Offset: 0x00227C9C
	public void Start()
	{
		base.aiActor = base.transform.parent.GetComponent<AIActor>();
	}

	// Token: 0x06005A94 RID: 23188 RVA: 0x00229AB4 File Offset: 0x00227CB4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005A95 RID: 23189 RVA: 0x00229ABC File Offset: 0x00227CBC
	public void TriggerAnimationEvent(string eventInfo)
	{
		base.aiActor.behaviorSpeculator.TriggerAnimationEvent(eventInfo);
	}
}
