using System;
using System.Collections;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x0200023A RID: 570
[InspectorDropdownName("ManfredsRival/ShieldThrow1")]
public class ManfredsRivalShieldThrow1 : Script
{
	// Token: 0x06000894 RID: 2196 RVA: 0x000297C8 File Offset: 0x000279C8
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		RoomHandler room = base.BulletBank.aiActor.ParentRoom;
		Vector2 leftPos = BraveUtility.RandomVector2(new Vector2(room.area.UnitLeft + 2f, room.area.UnitBottom + 2f), new Vector2(room.area.UnitCenter.x - 2f, room.area.UnitTop - 2f));
		Vector2 rightPos = BraveUtility.RandomVector2(new Vector2(room.area.UnitCenter.x + 2f, room.area.UnitBottom + 2f), new Vector2(room.area.UnitRight - 2f, room.area.UnitTop - 2f));
		this.FireShield(leftPos - base.Position);
		this.FireShield(new Vector2(0f, 0f));
		this.FireShield(rightPos - base.Position);
		this.FireShield(this.BulletManager.PlayerPosition() - base.Position);
		yield return base.Wait(160);
		yield break;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x000297E4 File Offset: 0x000279E4
	private void FireShield(Vector2 endOffset)
	{
		this.FireExpandingLine(new Vector2(-0.5f, -1f), new Vector2(0.5f, -1f), 4, endOffset);
		this.FireExpandingLine(new Vector2(-0.8f, -0.7f), new Vector2(-0.8f, 0.2f), 4, endOffset);
		this.FireExpandingLine(new Vector2(0.8f, -0.7f), new Vector2(0.8f, 0.2f), 4, endOffset);
		this.FireExpandingLine(new Vector2(-0.8f, 0.2f), new Vector2(-0.15f, 1f), 4, endOffset);
		this.FireExpandingLine(new Vector2(0.8f, 0.2f), new Vector2(0.15f, 1f), 4, endOffset);
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x000298B0 File Offset: 0x00027AB0
	private void FireExpandingLine(Vector2 start, Vector2 end, int numBullets, Vector2 endOffset)
	{
		start *= 0.5f;
		end *= 0.5f;
		for (int i = 0; i < numBullets; i++)
		{
			float num = ((numBullets > 1) ? ((float)i / ((float)numBullets - 1f)) : 0.5f);
			Vector2 vector = Vector2.Lerp(start, end, num);
			vector.y *= -1f;
			base.Fire(new Offset(vector * 4f, 0f, string.Empty, DirectionType.Absolute), new Direction(vector.ToAngle(), DirectionType.Absolute, -1f), new ManfredsRivalShieldThrow1.ShieldBullet(endOffset));
		}
	}

	// Token: 0x0400088B RID: 2187
	private const int WaitTime = 70;

	// Token: 0x0400088C RID: 2188
	private const int TravelTime = 90;

	// Token: 0x0400088D RID: 2189
	private const int HoldTime = 480;

	// Token: 0x0200023B RID: 571
	public class ShieldBullet : Bullet
	{
		// Token: 0x06000897 RID: 2199 RVA: 0x0002995C File Offset: 0x00027B5C
		public ShieldBullet(Vector2 endOffset)
			: base("shield", false, false, false)
		{
			this.m_endOffset = endOffset;
			base.SuppressVfx = true;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0002997C File Offset: 0x00027B7C
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			yield return base.Wait(70);
			Vector2 start = base.Position;
			Vector2 end = base.Position + this.m_endOffset;
			base.ManualControl = true;
			for (int i = 0; i < 90; i++)
			{
				float t = (float)(i + 1) / 90f;
				base.Position = new Vector2(Mathf.SmoothStep(start.x, end.x, t), Mathf.SmoothStep(start.y, end.y, t));
				yield return base.Wait(1);
			}
			yield return base.Wait(480);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0400088E RID: 2190
		private Vector2 m_endOffset;
	}
}
