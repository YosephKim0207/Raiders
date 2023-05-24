using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020010CD RID: 4301
public class TrapSpawnerController : BraveBehaviour
{
	// Token: 0x06005EB5 RID: 24245 RVA: 0x00245F9C File Offset: 0x0024419C
	public void Start()
	{
		this.m_room = base.GetComponent<AIActor>().ParentRoom;
		if (this.SpawnOnIntroFinished)
		{
			GenericIntroDoer component = base.GetComponent<GenericIntroDoer>();
			component.OnIntroFinished = (Action)Delegate.Combine(component.OnIntroFinished, new Action(this.OnIntroFinished));
		}
		if (this.DestroyOnDeath)
		{
			base.healthHaver.OnDeath += this.OnDeath;
		}
	}

	// Token: 0x06005EB6 RID: 24246 RVA: 0x00246010 File Offset: 0x00244210
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005EB7 RID: 24247 RVA: 0x00246018 File Offset: 0x00244218
	private void OnIntroFinished()
	{
		if (this.SpawnOnIntroFinished)
		{
			base.StartCoroutine(this.SpawnTraps());
		}
	}

	// Token: 0x06005EB8 RID: 24248 RVA: 0x00246034 File Offset: 0x00244234
	private void OnDeath(Vector2 vector2)
	{
		if (this.DestroyOnDeath)
		{
			this.DestroyTraps();
		}
	}

	// Token: 0x06005EB9 RID: 24249 RVA: 0x00246048 File Offset: 0x00244248
	private IEnumerator SpawnTraps()
	{
		for (int i = 0; i < this.RoomPositionOffsets.Count; i++)
		{
			if (i < this.SpawnDelays.Count && this.SpawnDelays[i] > 0f)
			{
				yield return new WaitForSeconds(this.SpawnDelays[i]);
			}
			Vector2 pos = this.m_room.area.UnitBottomLeft + this.RoomPositionOffsets[i];
			base.StartCoroutine(this.SpawnTrap(pos));
		}
		yield break;
	}

	// Token: 0x06005EBA RID: 24250 RVA: 0x00246064 File Offset: 0x00244264
	private IEnumerator SpawnTrap(Vector2 pos)
	{
		if (this.PoofVfx)
		{
			SpawnManager.SpawnVFX(this.PoofVfx, pos + this.VfxOffset, Quaternion.identity);
		}
		if (this.VfxLeadTime > 0f)
		{
			yield return new WaitForSeconds(this.VfxLeadTime);
		}
		GameObject trap = UnityEngine.Object.Instantiate<GameObject>(this.Trap, pos, Quaternion.identity);
		if (this.AdditionalTriggerDelayTime > 0f)
		{
			BasicTrapController component = trap.GetComponent<BasicTrapController>();
			if (component)
			{
				component.TemporarilyDisableTrap(this.AdditionalTriggerDelayTime);
			}
		}
		this.m_traps.Add(trap);
		yield break;
	}

	// Token: 0x06005EBB RID: 24251 RVA: 0x00246088 File Offset: 0x00244288
	private void DestroyTraps()
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
		{
			return;
		}
		for (int i = 0; i < this.m_traps.Count; i++)
		{
			GameManager.Instance.StartCoroutine(this.DestroyTrap(this.m_traps[i]));
		}
	}

	// Token: 0x06005EBC RID: 24252 RVA: 0x002460F4 File Offset: 0x002442F4
	private IEnumerator DestroyTrap(GameObject trap)
	{
		if (this.PoofVfx)
		{
			SpawnManager.SpawnVFX(this.PoofVfx, trap.transform.position.XY() + this.VfxOffset, Quaternion.identity);
		}
		if (this.VfxLeadTime > 0f)
		{
			yield return new WaitForSeconds(this.VfxLeadTime);
		}
		UnityEngine.Object.Destroy(trap);
		yield break;
	}

	// Token: 0x040058E9 RID: 22761
	[Header("Spawn Info")]
	public GameObject Trap;

	// Token: 0x040058EA RID: 22762
	public GameObject PoofVfx;

	// Token: 0x040058EB RID: 22763
	public List<Vector2> RoomPositionOffsets;

	// Token: 0x040058EC RID: 22764
	public List<float> SpawnDelays;

	// Token: 0x040058ED RID: 22765
	public Vector2 VfxOffset;

	// Token: 0x040058EE RID: 22766
	public float VfxLeadTime;

	// Token: 0x040058EF RID: 22767
	public float AdditionalTriggerDelayTime;

	// Token: 0x040058F0 RID: 22768
	[Header("Spawn Triggers")]
	public bool SpawnOnIntroFinished;

	// Token: 0x040058F1 RID: 22769
	[Header("Destroy Triggers")]
	public bool DestroyOnDeath;

	// Token: 0x040058F2 RID: 22770
	private RoomHandler m_room;

	// Token: 0x040058F3 RID: 22771
	private List<GameObject> m_traps = new List<GameObject>();
}
