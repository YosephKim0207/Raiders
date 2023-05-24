using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011B4 RID: 4532
public class MinorBreakableGroupManager : BraveBehaviour
{
	// Token: 0x0600651B RID: 25883 RVA: 0x00275174 File Offset: 0x00273374
	public void Initialize()
	{
		MinorBreakable[] componentsInChildren = base.GetComponentsInChildren<MinorBreakable>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.RegisterMinorBreakable(componentsInChildren[i]);
		}
		DebrisObject[] componentsInChildren2 = base.GetComponentsInChildren<DebrisObject>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			MinorBreakable component = componentsInChildren2[j].GetComponent<MinorBreakable>();
			if (component == null)
			{
				this.RegisterDebris(componentsInChildren2[j]);
			}
		}
	}

	// Token: 0x0600651C RID: 25884 RVA: 0x002751E4 File Offset: 0x002733E4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600651D RID: 25885 RVA: 0x002751EC File Offset: 0x002733EC
	public Vector2 GetDimensions()
	{
		if (!this.autodetectDimensions)
		{
			return PhysicsEngine.PixelToUnit(this.overridePixelDimensions);
		}
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		float num3 = base.transform.position.x;
		float num4 = base.transform.position.y;
		foreach (tk2dSprite tk2dSprite in base.GetComponentsInChildren<tk2dSprite>(true))
		{
			Transform transform = tk2dSprite.transform;
			Bounds bounds = tk2dSprite.GetBounds();
			float x = bounds.size.x;
			float y = bounds.size.y;
			num = Mathf.Min(num, transform.position.x);
			num2 = Mathf.Min(num2, transform.position.y);
			num3 = Mathf.Max(num3, transform.position.x + x);
			num4 = Mathf.Max(num4, transform.position.y + y);
		}
		return new Vector2(num3 - base.transform.position.x, num4 - base.transform.position.y);
	}

	// Token: 0x0600651E RID: 25886 RVA: 0x0027533C File Offset: 0x0027353C
	public void Destabilize(Vector3 force, float height)
	{
		for (int i = 0; i < this.registeredBreakables.Count; i++)
		{
			MinorBreakable minorBreakable = this.registeredBreakables[i];
			if (minorBreakable != null && minorBreakable)
			{
				if (minorBreakable.sprite && minorBreakable.sprite.attachParent != null)
				{
					minorBreakable.sprite.attachParent.DetachRenderer(minorBreakable.sprite);
					minorBreakable.sprite.attachParent = null;
				}
				DebrisObject component = minorBreakable.GetComponent<DebrisObject>();
				if (component != null)
				{
					component.Trigger(Quaternion.Euler(0f, 0f, Mathf.Lerp(-30f, 30f, UnityEngine.Random.value)) * force, height, 1f);
				}
				else
				{
					minorBreakable.Break(force.XY());
				}
			}
		}
		for (int j = 0; j < this.registeredDebris.Count; j++)
		{
			DebrisObject debrisObject = this.registeredDebris[j];
			if (debrisObject && debrisObject.sprite)
			{
				if (debrisObject.sprite.attachParent != null)
				{
					debrisObject.sprite.attachParent.DetachRenderer(debrisObject.sprite);
					debrisObject.sprite.attachParent = null;
				}
				debrisObject.Trigger(Quaternion.Euler(0f, 0f, Mathf.Lerp(-30f, 30f, UnityEngine.Random.value)) * force, height, 1f);
				j--;
			}
		}
		this.registeredBreakables.Clear();
		this.registeredDebris.Clear();
	}

	// Token: 0x0600651F RID: 25887 RVA: 0x00275504 File Offset: 0x00273704
	public void InformBroken(MinorBreakable mb, Vector2 breakForce, float breakHeight)
	{
		this.DeregisterMinorBreakable(mb);
		for (int i = 0; i < this.registeredBreakables.Count; i++)
		{
			MinorBreakable minorBreakable = this.registeredBreakables[i];
			if (minorBreakable)
			{
				switch (this.behavior)
				{
				case MinorBreakableGroupManager.MinorBreakableGroupBehavior.TRIGGERS_DEBRIS:
				{
					DebrisObject component = minorBreakable.GetComponent<DebrisObject>();
					Vector3 vector = Vector3.zero;
					if (breakForce == Vector2.zero)
					{
						vector = UnityEngine.Random.insideUnitCircle.normalized.ToVector3ZUp(0.5f);
					}
					else
					{
						vector = Quaternion.Euler(0f, 0f, Mathf.Lerp(-30f, 30f, UnityEngine.Random.value)) * breakForce.ToVector3ZUp(0.5f);
					}
					component.Trigger(vector, breakHeight, 1f);
					break;
				}
				case MinorBreakableGroupManager.MinorBreakableGroupBehavior.TRIGGERS_BREAK:
					if (breakForce == Vector2.zero)
					{
						minorBreakable.Break();
					}
					else
					{
						minorBreakable.Break(breakForce);
					}
					break;
				}
			}
		}
		for (int j = 0; j < this.registeredDebris.Count; j++)
		{
			DebrisObject debrisObject = this.registeredDebris[j];
			switch (this.behavior)
			{
			case MinorBreakableGroupManager.MinorBreakableGroupBehavior.TRIGGERS_DEBRIS:
				if (breakForce == Vector2.zero)
				{
					breakForce = UnityEngine.Random.insideUnitCircle.normalized;
				}
				debrisObject.Trigger(breakForce.ToVector3ZUp(0.5f), breakHeight, 1f);
				j--;
				break;
			}
		}
		if (this.behavior != MinorBreakableGroupManager.MinorBreakableGroupBehavior.NONE)
		{
			this.registeredBreakables.Clear();
		}
	}

	// Token: 0x06006520 RID: 25888 RVA: 0x002756E0 File Offset: 0x002738E0
	public void RegisterDebris(DebrisObject d)
	{
		if (!this.registeredDebris.Contains(d))
		{
			this.registeredDebris.Add(d);
		}
		d.groupManager = this;
	}

	// Token: 0x06006521 RID: 25889 RVA: 0x00275708 File Offset: 0x00273908
	public void DeregisterDebris(DebrisObject d)
	{
		d.groupManager = null;
		this.registeredDebris.Remove(d);
	}

	// Token: 0x06006522 RID: 25890 RVA: 0x00275720 File Offset: 0x00273920
	public void RegisterMinorBreakable(MinorBreakable mb)
	{
		if (!this.registeredBreakables.Contains(mb))
		{
			this.registeredBreakables.Add(mb);
		}
		mb.GroupManager = this;
	}

	// Token: 0x06006523 RID: 25891 RVA: 0x00275748 File Offset: 0x00273948
	public void DeregisterMinorBreakable(MinorBreakable mb)
	{
		mb.GroupManager = null;
		this.registeredBreakables.Remove(mb);
	}

	// Token: 0x040060DD RID: 24797
	public MinorBreakableGroupManager.MinorBreakableGroupBehavior behavior;

	// Token: 0x040060DE RID: 24798
	public bool autodetectDimensions = true;

	// Token: 0x040060DF RID: 24799
	public IntVector2 overridePixelDimensions;

	// Token: 0x040060E0 RID: 24800
	private List<MinorBreakable> registeredBreakables = new List<MinorBreakable>();

	// Token: 0x040060E1 RID: 24801
	private List<DebrisObject> registeredDebris = new List<DebrisObject>();

	// Token: 0x020011B5 RID: 4533
	public enum MinorBreakableGroupBehavior
	{
		// Token: 0x040060E3 RID: 24803
		TRIGGERS_DEBRIS,
		// Token: 0x040060E4 RID: 24804
		TRIGGERS_BREAK,
		// Token: 0x040060E5 RID: 24805
		NONE
	}
}
