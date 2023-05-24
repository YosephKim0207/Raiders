using System;
using System.Collections;
using UnityEngine;

// Token: 0x020013E1 RID: 5089
public class RatBootsItem : PassiveItem
{
	// Token: 0x06007379 RID: 29561 RVA: 0x002DF078 File Offset: 0x002DD278
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		player.OnAboutToFall = (Func<bool, bool>)Delegate.Combine(player.OnAboutToFall, new Func<bool, bool>(this.HandleAboutToFall));
	}

	// Token: 0x0600737A RID: 29562 RVA: 0x002DF0A4 File Offset: 0x002DD2A4
	protected void EnableShader(PlayerController user)
	{
		if (!user)
		{
			return;
		}
		Material[] array = user.SetOverrideShader(ShaderCache.Acquire("Brave/Internal/RainbowChestShader"));
		for (int i = 0; i < array.Length; i++)
		{
			if (!(array[i] == null))
			{
				array[i].SetFloat("_AllColorsToggle", 1f);
			}
		}
	}

	// Token: 0x0600737B RID: 29563 RVA: 0x002DF108 File Offset: 0x002DD308
	protected override void Update()
	{
		base.Update();
		if (base.Owner && this.m_extantFloor)
		{
			Vector2 centerPosition = base.Owner.CenterPosition;
			this.m_extantFloor.renderer.sharedMaterial.SetVector("_PlayerPos", new Vector4(centerPosition.x, centerPosition.y, 0f, 0f));
		}
		if (Time.timeScale <= 0f)
		{
			this.m_lastFrameAboutToFall = Time.frameCount;
		}
		else
		{
			if (!this.m_wasAboutToFallLastFrame && this.m_extantFloor)
			{
				SpawnManager.Despawn(this.m_extantFloor.gameObject);
				this.m_extantFloor = null;
			}
			this.m_wasAboutToFallLastFrame = false;
		}
		this.ProcessRatStatus(base.Owner, false);
	}

	// Token: 0x0600737C RID: 29564 RVA: 0x002DF1E4 File Offset: 0x002DD3E4
	private void ProcessRatStatus(PlayerController player, bool forceDisable = false)
	{
		bool flag = player && player.HasActiveBonusSynergy(CustomSynergyType.RESOURCEFUL_RAT, false) && !forceDisable;
		if (flag && !this.m_transformed)
		{
			this.m_lastPlayer = player;
			if (player)
			{
				this.m_transformed = true;
				player.OverrideAnimationLibrary = this.RatAnimationLibrary;
				player.SetOverrideShader(ShaderCache.Acquire(player.LocalShaderName));
				if (player.characterIdentity == PlayableCharacters.Eevee)
				{
					player.GetComponent<CharacterAnimationRandomizer>().AddOverrideAnimLibrary(this.RatAnimationLibrary);
				}
				player.PlayerIsRatTransformed = true;
				player.stats.RecalculateStats(player, false, false);
			}
		}
		else if (this.m_transformed && !flag)
		{
			if (this.m_lastPlayer)
			{
				this.m_lastPlayer.OverrideAnimationLibrary = null;
				this.m_lastPlayer.ClearOverrideShader();
				if (this.m_lastPlayer.characterIdentity == PlayableCharacters.Eevee)
				{
					this.m_lastPlayer.GetComponent<CharacterAnimationRandomizer>().RemoveOverrideAnimLibrary(this.RatAnimationLibrary);
				}
				this.m_lastPlayer.PlayerIsRatTransformed = false;
				this.m_lastPlayer.stats.RecalculateStats(this.m_lastPlayer, false, false);
				this.m_lastPlayer = null;
			}
			this.m_transformed = false;
		}
	}

	// Token: 0x0600737D RID: 29565 RVA: 0x002DF328 File Offset: 0x002DD528
	private void LateUpdate()
	{
		if (this.m_extantFloor)
		{
			this.m_extantFloor.UpdateZDepth();
		}
	}

	// Token: 0x0600737E RID: 29566 RVA: 0x002DF348 File Offset: 0x002DD548
	public override DebrisObject Drop(PlayerController player)
	{
		player.OnAboutToFall = (Func<bool, bool>)Delegate.Remove(player.OnAboutToFall, new Func<bool, bool>(this.HandleAboutToFall));
		if (this.m_invulnerable)
		{
			player.healthHaver.IsVulnerable = true;
		}
		return base.Drop(player);
	}

	// Token: 0x0600737F RID: 29567 RVA: 0x002DF398 File Offset: 0x002DD598
	protected override void OnDestroy()
	{
		if (base.Owner)
		{
			PlayerController owner = base.Owner;
			owner.OnAboutToFall = (Func<bool, bool>)Delegate.Remove(owner.OnAboutToFall, new Func<bool, bool>(this.HandleAboutToFall));
			if (this.m_invulnerable)
			{
				base.Owner.healthHaver.IsVulnerable = true;
			}
		}
		if (this.m_transformed)
		{
			this.ProcessRatStatus(null, true);
		}
		base.OnDestroy();
	}

	// Token: 0x06007380 RID: 29568 RVA: 0x002DF414 File Offset: 0x002DD614
	private IEnumerator HandleInvulnerability()
	{
		this.m_invulnerable = true;
		this.EnableShader(base.Owner);
		while (this.m_extantFloor && !this.m_frameWasPartialPit)
		{
			if (base.Owner)
			{
				base.Owner.healthHaver.IsVulnerable = false;
			}
			yield return null;
		}
		if (base.Owner)
		{
			base.Owner.ClearOverrideShader();
		}
		if (base.Owner)
		{
			base.Owner.healthHaver.IsVulnerable = true;
		}
		this.m_invulnerable = false;
		yield break;
	}

	// Token: 0x06007381 RID: 29569 RVA: 0x002DF430 File Offset: 0x002DD630
	private bool HandleAboutToFall(bool partialPit)
	{
		if (base.Owner && base.Owner.IsFlying)
		{
			return false;
		}
		if (!partialPit && !this.m_invulnerable)
		{
			base.StartCoroutine(this.HandleInvulnerability());
		}
		this.m_frameWasPartialPit = partialPit;
		this.m_wasAboutToFallLastFrame = true;
		if (Time.frameCount <= this.m_lastFrameAboutToFall)
		{
			this.m_lastFrameAboutToFall = Time.frameCount - 1;
		}
		if (Time.frameCount != this.m_lastFrameAboutToFall + 1)
		{
			this.m_elapsedAboutToFall = 0f;
		}
		if (partialPit)
		{
			this.m_elapsedAboutToFall = 0f;
		}
		this.m_lastFrameAboutToFall = Time.frameCount;
		this.m_elapsedAboutToFall += BraveTime.DeltaTime;
		if (this.m_elapsedAboutToFall < this.HoverTime)
		{
			if (!this.m_extantFloor)
			{
				GameObject gameObject = SpawnManager.SpawnVFX(this.FloorVFX, false);
				gameObject.transform.parent = base.Owner.transform;
				tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
				component.PlaceAtPositionByAnchor(base.Owner.SpriteBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				component.IsPerpendicular = false;
				component.HeightOffGround = -2.25f;
				component.UpdateZDepth();
				this.m_extantFloor = component;
			}
			if (this.m_elapsedAboutToFall > this.HoverTime - this.FlickerPortion)
			{
				bool flag = Mathf.PingPong(this.m_elapsedAboutToFall - (this.HoverTime - this.FlickerPortion), this.FlickerFrequency * 2f) < this.FlickerFrequency;
				this.m_extantFloor.renderer.enabled = flag;
			}
			else
			{
				this.m_extantFloor.renderer.enabled = true;
			}
			return false;
		}
		if (this.m_extantFloor)
		{
			SpawnManager.Despawn(this.m_extantFloor.gameObject);
			this.m_extantFloor = null;
		}
		return true;
	}

	// Token: 0x04007518 RID: 29976
	public float HoverTime = 2f;

	// Token: 0x04007519 RID: 29977
	public float FlickerPortion = 0.5f;

	// Token: 0x0400751A RID: 29978
	public float FlickerFrequency = 0.1f;

	// Token: 0x0400751B RID: 29979
	public GameObject FloorVFX;

	// Token: 0x0400751C RID: 29980
	public tk2dSpriteAnimation RatAnimationLibrary;

	// Token: 0x0400751D RID: 29981
	private tk2dSprite m_extantFloor;

	// Token: 0x0400751E RID: 29982
	private bool m_transformed;

	// Token: 0x0400751F RID: 29983
	private PlayerController m_lastPlayer;

	// Token: 0x04007520 RID: 29984
	private bool m_frameWasPartialPit;

	// Token: 0x04007521 RID: 29985
	private bool m_invulnerable;

	// Token: 0x04007522 RID: 29986
	private bool m_wasAboutToFallLastFrame;

	// Token: 0x04007523 RID: 29987
	private float m_elapsedAboutToFall;

	// Token: 0x04007524 RID: 29988
	private int m_lastFrameAboutToFall;
}
