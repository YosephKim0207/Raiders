using System;
using UnityEngine;

// Token: 0x020010D3 RID: 4307
[RequireComponent(typeof(AIActor))]
public class WaftModifier : BraveBehaviour
{
	// Token: 0x06005ED7 RID: 24279 RVA: 0x002466C8 File Offset: 0x002448C8
	private void Start()
	{
		base.gameActor.MovementModifiers += this.ModifyVelocity;
	}

	// Token: 0x06005ED8 RID: 24280 RVA: 0x002466E4 File Offset: 0x002448E4
	protected override void OnDestroy()
	{
		base.gameActor.MovementModifiers -= this.ModifyVelocity;
	}

	// Token: 0x06005ED9 RID: 24281 RVA: 0x00246700 File Offset: 0x00244900
	public void ModifyVelocity(ref Vector2 volundaryVel, ref Vector2 involuntaryVel)
	{
		if (!this.modifierEnabled)
		{
			return;
		}
		Vector2 vector = new Vector2(-volundaryVel.y, volundaryVel.x);
		Vector2 vector2 = vector.normalized;
		if (volundaryVel == Vector2.zero)
		{
			vector2 = Vector2.right;
		}
		float num = Mathf.Sin(Time.timeSinceLevelLoad * this.waftFrequency) * this.waftMagnitude;
		Vector2 vector3 = vector2 * num;
		Vector2 vector4 = Vector2.zero;
		if (this.fleeWalls && GameManager.Instance.Dungeon.data[base.specRigidbody.UnitBottomCenter.ToIntVector2(VectorConversions.Floor)].isOccludedByTopWall)
		{
			vector4 = Vector2.up * this.waftMagnitude;
		}
		volundaryVel += vector3 + vector4;
	}

	// Token: 0x04005907 RID: 22791
	public bool modifierEnabled = true;

	// Token: 0x04005908 RID: 22792
	public float waftMagnitude = 1f;

	// Token: 0x04005909 RID: 22793
	public float waftFrequency = 3f;

	// Token: 0x0400590A RID: 22794
	public bool fleeWalls = true;
}
