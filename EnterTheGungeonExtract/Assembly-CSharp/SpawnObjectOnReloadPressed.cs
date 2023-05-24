using System;
using System.Collections;
using UnityEngine;

// Token: 0x020014B1 RID: 5297
public class SpawnObjectOnReloadPressed : MonoBehaviour
{
	// Token: 0x0600786D RID: 30829 RVA: 0x003024A4 File Offset: 0x003006A4
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnInitializedWithOwner = (Action<GameActor>)Delegate.Combine(gun.OnInitializedWithOwner, new Action<GameActor>(this.OnGunInitialized));
		Gun gun2 = this.m_gun;
		gun2.OnDropped = (Action)Delegate.Combine(gun2.OnDropped, new Action(this.OnGunDroppedOrDestroyed));
		if (this.RequiresActualReload)
		{
			Gun gun3 = this.m_gun;
			gun3.OnAutoReload = (Action<PlayerController, Gun>)Delegate.Combine(gun3.OnAutoReload, new Action<PlayerController, Gun>(this.HandleAutoReload));
		}
		Gun gun4 = this.m_gun;
		gun4.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun4.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReloadPressed));
		if (this.m_gun.CurrentOwner != null)
		{
			this.OnGunInitialized(this.m_gun.CurrentOwner);
		}
	}

	// Token: 0x0600786E RID: 30830 RVA: 0x0030258C File Offset: 0x0030078C
	private void HandleAutoReload(PlayerController arg1, Gun arg2)
	{
		this.HandleReloadPressed(arg1, arg2, false);
	}

	// Token: 0x0600786F RID: 30831 RVA: 0x00302598 File Offset: 0x00300798
	private void HandleReloadPressed(PlayerController user, Gun sourceGun, bool actual)
	{
		if (this.RequiresSynergy && (!user || !user.HasActiveBonusSynergy(this.RequiredSynergy, false)))
		{
			return;
		}
		if (this.m_semaphore)
		{
			return;
		}
		if (this.RequiresActualReload && sourceGun.ClipShotsRemaining == sourceGun.ClipCapacity)
		{
			return;
		}
		this.m_semaphore = this.RequiresActualReload;
		if (!this.m_gun.IsFiring || this.RequiresActualReload)
		{
			if (!string.IsNullOrEmpty(this.AnimToPlay))
			{
				if (!sourceGun.spriteAnimator.IsPlaying(this.AnimToPlay))
				{
					user.StartCoroutine(this.DoSpawn(user, 0f));
				}
			}
			else
			{
				user.StartCoroutine(this.DoSpawn(user, 0f));
			}
		}
		if (this.m_semaphore)
		{
			user.StartCoroutine(this.HandleReloadDelay(sourceGun));
		}
	}

	// Token: 0x06007870 RID: 30832 RVA: 0x00302688 File Offset: 0x00300888
	private IEnumerator HandleReloadDelay(Gun sourceGun)
	{
		yield return new WaitForSeconds(sourceGun.reloadTime);
		this.m_semaphore = false;
		yield break;
	}

	// Token: 0x06007871 RID: 30833 RVA: 0x003026AC File Offset: 0x003008AC
	protected IEnumerator DoSpawn(PlayerController user, float angleFromAim)
	{
		if (!string.IsNullOrEmpty(this.AnimToPlay))
		{
			this.m_gun.spriteAnimator.Play(this.AnimToPlay);
		}
		if (this.DelayTime > 0f)
		{
			float ela = 0f;
			while (ela < this.DelayTime)
			{
				ela += BraveTime.DeltaTime;
				yield return null;
			}
		}
		if (!this)
		{
			yield break;
		}
		Projectile spawnProj = this.SpawnObject.GetComponent<Projectile>();
		if (spawnProj != null)
		{
			Vector2 vector = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
			UnityEngine.Object.Instantiate<GameObject>(this.SpawnObject, this.m_gun.barrelOffset.position, Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(vector)));
		}
		else if (this.tossForce == 0f)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.SpawnObject, this.m_gun.barrelOffset.position, Quaternion.identity);
			tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
			if (component != null)
			{
				component.PlaceAtPositionByAnchor(this.m_gun.barrelOffset.position, tk2dBaseSprite.Anchor.MiddleCenter);
				if (component.specRigidbody != null)
				{
					component.specRigidbody.RegisterGhostCollisionException(user.specRigidbody);
				}
			}
			gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
			if (!this.orphaned)
			{
				gameObject.transform.parent = this.m_gun.barrelOffset;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localPosition = Vector3.zero;
			}
			else
			{
				gameObject.transform.rotation = ((!this.preventRotation) ? this.m_gun.barrelOffset.rotation : Quaternion.identity);
				gameObject.transform.localScale = this.m_gun.barrelOffset.lossyScale;
				gameObject.transform.position = this.m_gun.barrelOffset.position;
			}
		}
		else
		{
			Vector3 vector2 = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
			Vector3 vector3 = this.m_gun.barrelOffset.position;
			if (vector2.x < 0f)
			{
				vector3 += Vector3.left;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.SpawnObject, vector3, Quaternion.identity);
			Vector2 vector4 = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
			vector4 = Quaternion.Euler(0f, 0f, angleFromAim) * vector4;
			DebrisObject debrisObject = LootEngine.DropItemWithoutInstantiating(gameObject2, gameObject2.transform.position, vector4, this.tossForce, false, false, true, false);
			debrisObject.Priority = EphemeralObject.EphemeralPriority.Critical;
			debrisObject.bounceCount = ((!this.canBounce) ? 0 : 1);
		}
		DeadlyDeadlyGoopManager.IgniteGoopsLine(this.m_gun.barrelOffset.position.XY(), this.m_gun.barrelOffset.position.XY() + (this.m_gun.barrelOffset.rotation * Vector3.right * 2.5f).XY(), 2f);
		yield break;
	}

	// Token: 0x06007872 RID: 30834 RVA: 0x003026D8 File Offset: 0x003008D8
	private void OnGunInitialized(GameActor obj)
	{
		if (this.m_playerOwner != null)
		{
			this.OnGunDroppedOrDestroyed();
		}
		if (obj == null)
		{
			return;
		}
		if (obj is PlayerController)
		{
			this.m_playerOwner = obj as PlayerController;
		}
	}

	// Token: 0x06007873 RID: 30835 RVA: 0x00302718 File Offset: 0x00300918
	private void OnDestroy()
	{
		this.OnGunDroppedOrDestroyed();
	}

	// Token: 0x06007874 RID: 30836 RVA: 0x00302720 File Offset: 0x00300920
	private void OnGunDroppedOrDestroyed()
	{
		if (this.m_playerOwner != null)
		{
			this.m_playerOwner = null;
		}
	}

	// Token: 0x04007A94 RID: 31380
	public GameObject SpawnObject;

	// Token: 0x04007A95 RID: 31381
	public float tossForce;

	// Token: 0x04007A96 RID: 31382
	public float DelayTime;

	// Token: 0x04007A97 RID: 31383
	public bool canBounce = true;

	// Token: 0x04007A98 RID: 31384
	[ShowInInspectorIf("tossForce", 0, false)]
	public bool orphaned = true;

	// Token: 0x04007A99 RID: 31385
	[ShowInInspectorIf("tossForce", 0, false)]
	public bool preventRotation;

	// Token: 0x04007A9A RID: 31386
	[CheckAnimation(null)]
	public string AnimToPlay;

	// Token: 0x04007A9B RID: 31387
	public bool RequiresSynergy;

	// Token: 0x04007A9C RID: 31388
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04007A9D RID: 31389
	public bool RequiresActualReload;

	// Token: 0x04007A9E RID: 31390
	private Gun m_gun;

	// Token: 0x04007A9F RID: 31391
	private PlayerController m_playerOwner;

	// Token: 0x04007AA0 RID: 31392
	private bool m_semaphore;
}
