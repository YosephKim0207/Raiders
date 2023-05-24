using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000208 RID: 520
[InspectorDropdownName("Bosses/Infinilich/NegativeSpacePie1")]
public class InfinilichNegativeSpacePie1 : Script
{
	// Token: 0x060007BF RID: 1983 RVA: 0x00025AD0 File Offset: 0x00023CD0
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		float aim = base.AimDirection;
		int i = 0;
		InfinilichNegativeSpacePie1.SafeEndAngle = aim + 180f + 27.5f * InfinilichNegativeSpacePie1.DeltaRay;
		int halfRays = 28;
		for (int j = 0; j < halfRays; j++)
		{
			float angle = aim + 180f - ((float)j + 0.5f) * InfinilichNegativeSpacePie1.DeltaRay;
			InfinilichNegativeSpacePie1.RayBullet leadBullet = this.SpawnRay(angle, i);
			this.m_leadBullets.AddFirst(leadBullet);
			angle = aim + 180f + ((float)j + 0.5f) * InfinilichNegativeSpacePie1.DeltaRay;
			leadBullet = this.SpawnRay(angle, i);
			this.m_leadBullets.AddLast(leadBullet);
			i++;
			int remainingRays = halfRays - j;
			yield return base.Wait((remainingRays < 5) ? (10 - remainingRays * 2) : 1);
		}
		yield return base.Wait(52);
		base.StartTask(this.HandleGaps());
		i = 0;
		while (!this.m_done)
		{
			float currentSpeed = 0.42f;
			if (BraveMathCollege.ClampAngle180(base.AimDirection - InfinilichNegativeSpacePie1.SafeEndAngle) < -3f)
			{
				currentSpeed *= 0.5f;
			}
			GameActor target = base.BulletBank.aiActor.PlayerTarget;
			if (target && target.IsFalling)
			{
				currentSpeed = 0f;
			}
			float deltaAngle = ((i >= 90) ? currentSpeed : Mathf.Lerp(0f, currentSpeed, (float)i / 90f));
			InfinilichNegativeSpacePie1.SafeEndAngle += deltaAngle;
			int destroyedCount = 0;
			for (LinkedListNode<InfinilichNegativeSpacePie1.RayBullet> node = this.m_leadBullets.First; node != null; node = node.Next)
			{
				node.Value.Angle += deltaAngle;
				if (node.Value.Destroyed)
				{
					destroyedCount++;
				}
			}
			i++;
			yield return base.Wait(1);
		}
		for (LinkedListNode<InfinilichNegativeSpacePie1.RayBullet> node = this.m_leadBullets.First; node != null; node = node.Next)
		{
			node.Value.ShouldDestroy = true;
		}
		this.m_leadBullets.Clear();
		base.ForceEnd();
		yield break;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00025AEC File Offset: 0x00023CEC
	private InfinilichNegativeSpacePie1.RayBullet SpawnRay(float angle, int spawnDelay)
	{
		InfinilichNegativeSpacePie1.RayBullet rayBullet = null;
		for (int i = 0; i < 14; i++)
		{
			InfinilichNegativeSpacePie1.RayBullet rayBullet2 = new InfinilichNegativeSpacePie1.RayBullet(rayBullet, angle, spawnDelay, base.Position);
			if (rayBullet == null)
			{
				rayBullet = rayBullet2;
			}
			base.Fire(new Offset(Mathf.Lerp(1.5f, 22f, (float)i / 13f), 0f, angle, string.Empty, DirectionType.Absolute), new Speed(0f, SpeedType.Absolute), rayBullet2);
		}
		return rayBullet;
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x00025B60 File Offset: 0x00023D60
	private IEnumerator HandleGaps()
	{
		int lastRotationDirection = -1;
		for (int i = 0; i < 6; i++)
		{
			yield return base.Wait(5);
			int rotateDirection = ((lastRotationDirection <= 0) ? Mathf.RoundToInt(BraveUtility.RandomSign()) : (-1));
			lastRotationDirection = rotateDirection;
			if (lastRotationDirection > 0)
			{
				LinkedListNode<InfinilichNegativeSpacePie1.RayBullet> linkedListNode = this.m_leadBullets.Last;
				for (int l = 0; l < 4; l++)
				{
					linkedListNode.Value.DoTell = true;
					linkedListNode = linkedListNode.Previous;
				}
			}
			else
			{
				LinkedListNode<InfinilichNegativeSpacePie1.RayBullet> node = this.m_leadBullets.First;
				for (int m = 0; m < 1; m++)
				{
					node.Value.DoTell = true;
					node = node.Next;
				}
				yield return base.Wait(1);
			}
			yield return base.Wait(30);
			if (rotateDirection > 0)
			{
				InfinilichNegativeSpacePie1.SafeEndAngle -= InfinilichNegativeSpacePie1.DeltaRay * 4f;
				for (int j = 0; j < 150; j++)
				{
					LinkedListNode<InfinilichNegativeSpacePie1.RayBullet> node2 = this.m_leadBullets.Last;
					for (int n = 0; n < 4; n++)
					{
						node2.Value.Angle += (45f - InfinilichNegativeSpacePie1.DeltaRay) / 150f;
						node2 = node2.Previous;
					}
					yield return base.Wait(1);
				}
				for (int num = 0; num < 4; num++)
				{
					LinkedListNode<InfinilichNegativeSpacePie1.RayBullet> last = this.m_leadBullets.Last;
					this.m_leadBullets.Remove(last, false);
					this.m_leadBullets.AddFirst(last);
					last.Value.DoTell = false;
				}
			}
			else
			{
				for (int k = 0; k < 90; k++)
				{
					LinkedListNode<InfinilichNegativeSpacePie1.RayBullet> node3 = this.m_leadBullets.First;
					for (int num2 = 0; num2 < 1; num2++)
					{
						node3.Value.Angle -= (45f - InfinilichNegativeSpacePie1.DeltaRay) / 90f;
						node3 = node3.Next;
					}
					yield return base.Wait(1);
				}
				for (int num3 = 0; num3 < 1; num3++)
				{
					LinkedListNode<InfinilichNegativeSpacePie1.RayBullet> first = this.m_leadBullets.First;
					this.m_leadBullets.Remove(first, false);
					this.m_leadBullets.AddLast(first);
					first.Value.DoTell = false;
				}
				InfinilichNegativeSpacePie1.SafeEndAngle += InfinilichNegativeSpacePie1.DeltaRay * 1f;
			}
		}
		this.m_done = true;
		yield break;
	}

	// Token: 0x040007AC RID: 1964
	private const int NumRays = 56;

	// Token: 0x040007AD RID: 1965
	private const float SafeDegrees = 45f;

	// Token: 0x040007AE RID: 1966
	private const int NumBullets = 14;

	// Token: 0x040007AF RID: 1967
	private const float RayLength = 22f;

	// Token: 0x040007B0 RID: 1968
	private const int SetupTime = 80;

	// Token: 0x040007B1 RID: 1969
	private const float SpinSpeed = 0.42f;

	// Token: 0x040007B2 RID: 1970
	private const int NumTransitions = 6;

	// Token: 0x040007B3 RID: 1971
	private const int MidTransitionTime = 35;

	// Token: 0x040007B4 RID: 1972
	private const int TransitionTellTime = 30;

	// Token: 0x040007B5 RID: 1973
	private const int ForwardCount = 4;

	// Token: 0x040007B6 RID: 1974
	private const int ForwardTransitionTime = 150;

	// Token: 0x040007B7 RID: 1975
	private const int BackwardCount = 1;

	// Token: 0x040007B8 RID: 1976
	private const int BackwardTransitionTime = 90;

	// Token: 0x040007B9 RID: 1977
	private static float DeltaRay = 5.7272725f;

	// Token: 0x040007BA RID: 1978
	private static float SafeEndAngle;

	// Token: 0x040007BB RID: 1979
	private PooledLinkedList<InfinilichNegativeSpacePie1.RayBullet> m_leadBullets = new PooledLinkedList<InfinilichNegativeSpacePie1.RayBullet>();

	// Token: 0x040007BC RID: 1980
	private bool m_done;

	// Token: 0x02000209 RID: 521
	public class RayBullet : Bullet
	{
		// Token: 0x060007C3 RID: 1987 RVA: 0x00025B88 File Offset: 0x00023D88
		public RayBullet(InfinilichNegativeSpacePie1.RayBullet leadBullet, float angle, int spawnDelay, Vector2 origin)
			: base("pieBullet", false, false, false)
		{
			this.m_leadBullet = leadBullet;
			this.Angle = angle;
			this.m_spawnDelay = spawnDelay;
			this.m_origin = origin;
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00025BB8 File Offset: 0x00023DB8
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x00025BC0 File Offset: 0x00023DC0
		public bool DoTell
		{
			get
			{
				return this.m_doTell;
			}
			set
			{
				if (this.m_doTell != value)
				{
					if (value)
					{
						this.Projectile.spriteAnimator.Play();
					}
					else
					{
						this.Projectile.spriteAnimator.StopAndResetFrameToDefault();
					}
					this.m_doTell = value;
				}
			}
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00025C00 File Offset: 0x00023E00
		protected override IEnumerator Top()
		{
			float radius = (base.Position - this.m_origin).magnitude;
			float Magnitude = UnityEngine.Random.Range(1.1f, 1.4f);
			float Period = UnityEngine.Random.Range(1.35f, 1.65f);
			if (this.m_spawnDelay < 80)
			{
				yield return base.Wait(80 - this.m_spawnDelay);
			}
			float startingOffset = UnityEngine.Random.value;
			base.ManualControl = true;
			int i = 0;
			for (;;)
			{
				if (this.ShouldDestroy || (this.m_leadBullet != null && this.m_leadBullet.ShouldDestroy))
				{
					radius += 0.25f;
				}
				if (this.m_leadBullet != null)
				{
					this.DoTell = this.m_leadBullet.DoTell;
					this.Angle = this.m_leadBullet.Angle;
				}
				float offsetMagnitude = Mathf.SmoothStep(-Magnitude, Magnitude, Mathf.PingPong(startingOffset + (float)i / 60f * Period, 1f));
				if (i < 60)
				{
					offsetMagnitude *= (float)i / 60f;
				}
				base.Position = this.m_origin + BraveMathCollege.DegreesToVector(this.Angle, radius + offsetMagnitude);
				i++;
				yield return base.Wait(1);
			}
			yield break;
		}

		// Token: 0x040007BD RID: 1981
		public bool ShouldDestroy;

		// Token: 0x040007BE RID: 1982
		public int DestroyDirection;

		// Token: 0x040007BF RID: 1983
		public float Angle;

		// Token: 0x040007C0 RID: 1984
		private InfinilichNegativeSpacePie1.RayBullet m_leadBullet;

		// Token: 0x040007C1 RID: 1985
		private int m_spawnDelay;

		// Token: 0x040007C2 RID: 1986
		private Vector2 m_origin;

		// Token: 0x040007C3 RID: 1987
		private bool m_doTell;
	}
}
