using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001083 RID: 4227
public class CorpseSpawnController : BraveBehaviour
{
	// Token: 0x06005D11 RID: 23825 RVA: 0x0023A624 File Offset: 0x00238824
	public void Start()
	{
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
	}

	// Token: 0x06005D12 RID: 23826 RVA: 0x0023A650 File Offset: 0x00238850
	public void Update()
	{
		if (this.m_state == CorpseSpawnController.State.Prespawn)
		{
			if (!base.aiAnimator.IsPlaying(this.PrespawnAnim))
			{
				base.aiAnimator.PlayUntilFinished(this.SpawnAnim, false, null, -1f, false);
				this.m_state = CorpseSpawnController.State.Spawning;
			}
		}
		else if (this.m_state == CorpseSpawnController.State.Spawning && !base.aiAnimator.IsPlaying(this.SpawnAnim))
		{
			Vector2 vector = base.transform.position;
			Vector2 vector2 = ((!this.m_isRight) ? this.LeftSpawnOffset : this.RightSpawnOffset);
			AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid(this.EnemyGuid), vector + vector2, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(vector.ToIntVector2(VectorConversions.Round)), true, AIActor.AwakenAnimationType.Default, true);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (this.m_state != CorpseSpawnController.State.None && this.CancelOnRoomClear && this.m_room.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear) == 0)
		{
			this.m_state = CorpseSpawnController.State.None;
			base.aiAnimator.PlayUntilCancelled(this.PrespawnAnim, false, null, -1f, false);
			base.aiAnimator.enabled = false;
			base.spriteAnimator.enabled = false;
			base.sprite.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_COMPLEX;
			base.GetComponent<DebrisObject>().FadeToOverrideColor(new Color(0f, 0f, 0f, 0.6f), 0.25f, 0f);
			base.GetComponent<Renderer>().material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTiltedCutoutFastPixelShadow");
		}
	}

	// Token: 0x06005D13 RID: 23827 RVA: 0x0023A7EC File Offset: 0x002389EC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005D14 RID: 23828 RVA: 0x0023A7F4 File Offset: 0x002389F4
	public void Init(AIActor aiActor)
	{
		this.m_room = aiActor.ParentRoom;
		float num = this.PrespawnTime;
		CorpseSpawnController[] array = UnityEngine.Object.FindObjectsOfType<CorpseSpawnController>();
		if (array != null && array.Length > 1)
		{
			num += (float)(array.Length - 1) * this.AdditionalPrespawnTime;
		}
		this.m_isRight = !aiActor.sprite.CurrentSprite.name.Contains("left");
		base.aiAnimator.FacingDirection = (float)((!this.m_isRight) ? 180 : 0);
		base.aiAnimator.PlayForDuration(this.PrespawnAnim, num, false, null, -1f, false);
		this.m_state = CorpseSpawnController.State.Prespawn;
	}

	// Token: 0x06005D15 RID: 23829 RVA: 0x0023A8A0 File Offset: 0x00238AA0
	private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.m_state != CorpseSpawnController.State.None && clip.GetFrame(frame).eventInfo == "perp")
		{
			base.sprite.IsPerpendicular = true;
		}
	}

	// Token: 0x040056D1 RID: 22225
	[CheckDirectionalAnimation(null)]
	public string PrespawnAnim = "corpse_prespawn";

	// Token: 0x040056D2 RID: 22226
	[CheckDirectionalAnimation(null)]
	public string SpawnAnim = "corpse_spawn";

	// Token: 0x040056D3 RID: 22227
	[EnemyIdentifier]
	public string EnemyGuid;

	// Token: 0x040056D4 RID: 22228
	public Vector2 LeftSpawnOffset;

	// Token: 0x040056D5 RID: 22229
	public Vector2 RightSpawnOffset;

	// Token: 0x040056D6 RID: 22230
	public float PrespawnTime = 5f;

	// Token: 0x040056D7 RID: 22231
	public float AdditionalPrespawnTime = 5f;

	// Token: 0x040056D8 RID: 22232
	public bool CancelOnRoomClear = true;

	// Token: 0x040056D9 RID: 22233
	private CorpseSpawnController.State m_state;

	// Token: 0x040056DA RID: 22234
	private bool m_isRight;

	// Token: 0x040056DB RID: 22235
	private RoomHandler m_room;

	// Token: 0x02001084 RID: 4228
	private enum State
	{
		// Token: 0x040056DD RID: 22237
		None,
		// Token: 0x040056DE RID: 22238
		Prespawn,
		// Token: 0x040056DF RID: 22239
		Spawning
	}
}
