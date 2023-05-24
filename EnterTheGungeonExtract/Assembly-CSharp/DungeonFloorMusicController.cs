using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000F6F RID: 3951
public class DungeonFloorMusicController : MonoBehaviour
{
	// Token: 0x17000BE6 RID: 3046
	// (get) Token: 0x06005528 RID: 21800 RVA: 0x00205608 File Offset: 0x00203808
	public DungeonFloorMusicController.DungeonMusicState CurrentState
	{
		get
		{
			return this.m_currentState;
		}
	}

	// Token: 0x17000BE7 RID: 3047
	// (get) Token: 0x06005529 RID: 21801 RVA: 0x00205610 File Offset: 0x00203810
	public bool MusicOverridden
	{
		get
		{
			return this.m_overrideMusic;
		}
	}

	// Token: 0x17000BE8 RID: 3048
	// (get) Token: 0x0600552A RID: 21802 RVA: 0x00205618 File Offset: 0x00203818
	public bool ShouldPulseLightFX
	{
		get
		{
			return this.m_overrideMusic && !this.m_isVictoryState;
		}
	}

	// Token: 0x0600552B RID: 21803 RVA: 0x00205634 File Offset: 0x00203834
	private void LateUpdate()
	{
		if (this.m_cooldownTimerRemaining > 0f)
		{
			this.m_cooldownTimerRemaining -= BraveTime.DeltaTime;
			if (this.m_cooldownTimerRemaining <= 0f)
			{
				this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.CALM);
				this.m_cooldownTimerRemaining = -1f;
			}
		}
		if (this.m_changedToArcadeTimer > 0f)
		{
			this.m_changedToArcadeTimer -= BraveTime.DeltaTime;
		}
	}

	// Token: 0x0600552C RID: 21804 RVA: 0x002056A8 File Offset: 0x002038A8
	public void ClearCoreMusicEventID()
	{
		Debug.Log("Clearing Core Music ID!");
		this.m_lastMusicChangeTime = -1000f;
		this.m_overrideMusic = false;
		this.m_isVictoryState = false;
		this.m_coreMusicEventID = 0U;
	}

	// Token: 0x0600552D RID: 21805 RVA: 0x002056D4 File Offset: 0x002038D4
	public void OnAkMusicEvent(object cookie, AkCallbackType eventType, object info)
	{
		if (eventType == AkCallbackType.AK_Starvation)
		{
			Debug.Log(string.Concat(new object[] { "Core music event: ", this.m_cachedMusicEventCore, " STARVING with playing ID: ", this.m_coreMusicEventID }));
		}
		else if (eventType == AkCallbackType.AK_EndOfEvent)
		{
			Debug.Log(string.Concat(new object[] { "Core music event: ", this.m_cachedMusicEventCore, " ENDING with playing ID: ", this.m_coreMusicEventID }));
		}
	}

	// Token: 0x0600552E RID: 21806 RVA: 0x00205764 File Offset: 0x00203964
	public void ResetForNewFloor(Dungeon d)
	{
		this.m_overrideMusic = false;
		this.m_isVictoryState = false;
		this.m_lastMusicChangeTime = -1000f;
		GameManager.Instance.FlushMusicAudio();
		if (!string.IsNullOrEmpty(d.musicEventName))
		{
			this.m_cachedMusicEventCore = d.musicEventName;
		}
		else
		{
			this.m_cachedMusicEventCore = "Play_MUS_Dungeon_Theme_01";
		}
		this.m_coreMusicEventID = AkSoundEngine.PostEvent(this.m_cachedMusicEventCore, GameManager.Instance.gameObject, 33U, new AkCallbackManager.EventCallback(this.OnAkMusicEvent), null);
		Debug.LogWarning(string.Concat(new object[] { "Posting core music event: ", this.m_cachedMusicEventCore, " with playing ID: ", this.m_coreMusicEventID }));
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST && GameManager.Instance.PrimaryPlayer.characterIdentity != PlayableCharacters.Bullet)
		{
			this.m_overrideMusic = true;
			AkSoundEngine.PostEvent("Play_MUS_Ending_State_01", GameManager.Instance.gameObject);
		}
		else
		{
			this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.FLOOR_INTRO);
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER && GameStatsManager.Instance.AnyPastBeaten())
		{
			AkSoundEngine.PostEvent("Play_MUS_State_Winner", GameManager.Instance.gameObject);
		}
	}

	// Token: 0x0600552F RID: 21807 RVA: 0x002058A4 File Offset: 0x00203AA4
	public void UpdateCoreMusicEvent()
	{
		if (this.m_coreMusicEventID == 0U)
		{
			this.ResetForNewFloor(GameManager.Instance.Dungeon);
		}
	}

	// Token: 0x06005530 RID: 21808 RVA: 0x002058C4 File Offset: 0x00203AC4
	public void SwitchToArcadeMusic()
	{
		this.m_changedToArcadeTimer = 0.1f;
		this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.ARCADE);
	}

	// Token: 0x06005531 RID: 21809 RVA: 0x002058DC File Offset: 0x00203ADC
	public void StartArcadeGame()
	{
		AkSoundEngine.PostEvent("Play_MUS_Winchester_state_Game", base.gameObject);
	}

	// Token: 0x06005532 RID: 21810 RVA: 0x002058F0 File Offset: 0x00203AF0
	public void SwitchToCustomMusic(string customMusicEvent, GameObject source, bool useSwitch, string switchEvent)
	{
		this.m_cooldownTimerRemaining = -1f;
		AkSoundEngine.PostEvent(customMusicEvent, source);
		if (useSwitch)
		{
			AkSoundEngine.PostEvent(switchEvent, source);
		}
		this.m_currentState = (DungeonFloorMusicController.DungeonMusicState)(-1);
	}

	// Token: 0x06005533 RID: 21811 RVA: 0x0020591C File Offset: 0x00203B1C
	public void SwitchToVictoryMusic()
	{
		this.m_cooldownTimerRemaining = -1f;
		AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
		AkSoundEngine.PostEvent("Play_MUS_Victory_Theme_01", base.gameObject);
		this.m_overrideMusic = true;
		this.m_isVictoryState = true;
	}

	// Token: 0x06005534 RID: 21812 RVA: 0x0020595C File Offset: 0x00203B5C
	public void SwitchToEndTimesMusic()
	{
		this.m_cooldownTimerRemaining = -1f;
		AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
		AkSoundEngine.PostEvent("Play_MUS_Space_Intro_01", base.gameObject);
		this.m_overrideMusic = true;
		this.m_isVictoryState = false;
	}

	// Token: 0x06005535 RID: 21813 RVA: 0x0020599C File Offset: 0x00203B9C
	public void SwitchToDragunTwo()
	{
		this.m_cooldownTimerRemaining = -1f;
		AkSoundEngine.PostEvent("Stop_MUS_All", GameManager.Instance.gameObject);
		AkSoundEngine.PostEvent("Play_MUS_Boss_Theme_Dragun_02", GameManager.Instance.gameObject);
		this.m_overrideMusic = true;
	}

	// Token: 0x06005536 RID: 21814 RVA: 0x002059DC File Offset: 0x00203BDC
	public void SwitchToBossMusic(string bossMusicString, GameObject source)
	{
		bool flag = GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH || GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH;
		flag |= GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON;
		flag |= GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST && GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Bullet;
		flag = true;
		if (flag && this.m_isVictoryState)
		{
			this.EndVictoryMusic();
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST && GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Convict)
		{
			return;
		}
		this.m_cooldownTimerRemaining = -1f;
		AkSoundEngine.PostEvent("Stop_MUS_All", source);
		AkSoundEngine.PostEvent(bossMusicString, source);
		this.m_overrideMusic = true;
	}

	// Token: 0x06005537 RID: 21815 RVA: 0x00205AB4 File Offset: 0x00203CB4
	public void EndBossMusic()
	{
		AkSoundEngine.PostEvent("Stop_MUS_Boss_Theme", base.gameObject);
		this.m_overrideMusic = false;
		AkSoundEngine.PostEvent("Play_MUS_Victory_Theme_01", base.gameObject);
		this.m_isVictoryState = true;
	}

	// Token: 0x06005538 RID: 21816 RVA: 0x00205AE8 File Offset: 0x00203CE8
	public void EndBossMusicNoVictory()
	{
		AkSoundEngine.PostEvent("Stop_MUS_Boss_Theme", base.gameObject);
		AkSoundEngine.PostEvent(this.m_cachedMusicEventCore, base.gameObject);
		this.m_overrideMusic = false;
		this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.CALM);
	}

	// Token: 0x06005539 RID: 21817 RVA: 0x00205B1C File Offset: 0x00203D1C
	public void EndVictoryMusic()
	{
		this.m_overrideMusic = false;
		this.m_isVictoryState = false;
		AkSoundEngine.PostEvent("Stop_MUS_All", GameManager.Instance.gameObject);
		AkSoundEngine.PostEvent(this.m_cachedMusicEventCore, GameManager.Instance.gameObject);
	}

	// Token: 0x0600553A RID: 21818 RVA: 0x00205B58 File Offset: 0x00203D58
	private void SwitchToState(DungeonFloorMusicController.DungeonMusicState targetState)
	{
		if (this.m_changedToArcadeTimer > 0f && targetState == DungeonFloorMusicController.DungeonMusicState.CALM && this.m_currentState == DungeonFloorMusicController.DungeonMusicState.ARCADE)
		{
			return;
		}
		Debug.Log(string.Concat(new object[]
		{
			"Attemping to switch to state: ",
			targetState.ToString(),
			" with core ID: ",
			this.m_coreMusicEventID
		}));
		if (this.m_overrideMusic)
		{
			return;
		}
		switch (targetState)
		{
		case DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_B:
			AkSoundEngine.PostEvent("Play_MUS_Dungeon_State_LoopB", GameManager.Instance.gameObject);
			break;
		default:
			if (targetState != DungeonFloorMusicController.DungeonMusicState.FLOOR_INTRO)
			{
				if (targetState != DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_A)
				{
					if (targetState != DungeonFloorMusicController.DungeonMusicState.CALM)
					{
						if (targetState != DungeonFloorMusicController.DungeonMusicState.SHOP)
						{
							if (targetState != DungeonFloorMusicController.DungeonMusicState.SECRET)
							{
								if (targetState != DungeonFloorMusicController.DungeonMusicState.ARCADE)
								{
									if (targetState != DungeonFloorMusicController.DungeonMusicState.FOYER_ELEVATOR)
									{
										if (targetState == DungeonFloorMusicController.DungeonMusicState.FOYER_SORCERESS)
										{
											this.m_cooldownTimerRemaining = -1f;
											AkSoundEngine.PostEvent("Play_MUS_State_Sorceress", GameManager.Instance.gameObject);
										}
									}
									else
									{
										this.m_cooldownTimerRemaining = -1f;
										AkSoundEngine.PostEvent("Play_MUS_State_Elevator", GameManager.Instance.gameObject);
									}
								}
								else
								{
									this.m_cooldownTimerRemaining = -1f;
									AkSoundEngine.PostEvent("Play_MUS_Dungeon_State_Winchester", GameManager.Instance.gameObject);
									AkSoundEngine.PostEvent("Play_MUS_Winchester_State_Drone", base.gameObject);
								}
							}
							else
							{
								this.m_cooldownTimerRemaining = -1f;
								AkSoundEngine.PostEvent("Play_MUS_Dungeon_State_Secret", GameManager.Instance.gameObject);
							}
						}
						else
						{
							this.m_cooldownTimerRemaining = -1f;
							AkSoundEngine.PostEvent("Play_MUS_Dungeon_State_Shop", GameManager.Instance.gameObject);
						}
					}
					else
					{
						this.m_cooldownTimerRemaining = -1f;
						if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER && GameStatsManager.Instance.AnyPastBeaten())
						{
							AkSoundEngine.PostEvent("Play_MUS_Dungeon_State_Winner", GameManager.Instance.gameObject);
						}
						else
						{
							AkSoundEngine.PostEvent("Play_MUS_Dungeon_State_Drone", GameManager.Instance.gameObject);
						}
					}
				}
				else
				{
					AkSoundEngine.PostEvent("Play_MUS_Dungeon_State_LoopA", GameManager.Instance.gameObject);
				}
			}
			else
			{
				this.m_cooldownTimerRemaining = -1f;
				AkSoundEngine.PostEvent("Play_MUS_Dungeon_State_Intro", GameManager.Instance.gameObject);
			}
			break;
		case DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_C:
			AkSoundEngine.PostEvent("Play_MUS_Dungeon_State_LoopC", GameManager.Instance.gameObject);
			break;
		case DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_D:
			AkSoundEngine.PostEvent("Play_MUS_Dungeon_State_LoopD", GameManager.Instance.gameObject);
			break;
		}
		Debug.Log("Successfully switched to state: " + targetState.ToString());
		this.m_currentState = targetState;
	}

	// Token: 0x0600553B RID: 21819 RVA: 0x00205E14 File Offset: 0x00204014
	public void NotifyRoomSuddenlyHasEnemies(RoomHandler newRoom)
	{
		if (newRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
		{
			this.m_cooldownTimerRemaining = -1f;
			if (this.m_currentState == DungeonFloorMusicController.DungeonMusicState.FLOOR_INTRO || this.m_currentState == DungeonFloorMusicController.DungeonMusicState.CALM || this.m_currentState == DungeonFloorMusicController.DungeonMusicState.SHOP)
			{
				this.SwitchToActiveMusic(null);
			}
		}
	}

	// Token: 0x0600553C RID: 21820 RVA: 0x00205E6C File Offset: 0x0020406C
	public void SwitchToActiveMusic(DungeonFloorMusicController.DungeonMusicState? excludedState)
	{
		if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.TUTORIAL)
		{
			bool flag = GameManager.Instance.RandomIntForCurrentRun % 4 == 1;
			if (flag)
			{
				if (excludedState != null)
				{
					if (excludedState.Value == DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_C)
					{
						this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_D);
					}
					else if (excludedState.Value == DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_D)
					{
						this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_C);
					}
					else
					{
						this.SwitchToState((UnityEngine.Random.value >= 0.5f) ? DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_D : DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_C);
					}
				}
				else
				{
					this.SwitchToState((UnityEngine.Random.value >= 0.5f) ? DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_D : DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_C);
				}
			}
			else if (excludedState != null)
			{
				if (excludedState.Value == DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_A)
				{
					this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_B);
				}
				else if (excludedState.Value == DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_B)
				{
					this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_A);
				}
				else
				{
					this.SwitchToState((UnityEngine.Random.value >= 0.5f) ? DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_B : DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_A);
				}
			}
			else
			{
				this.SwitchToState((UnityEngine.Random.value >= 0.5f) ? DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_B : DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_A);
			}
		}
		else
		{
			this.m_lastMusicChangeTime = Time.realtimeSinceStartup;
			if (excludedState != null && excludedState.Value == DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_A)
			{
				this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_B);
			}
			else if (excludedState != null && excludedState.Value == DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_B)
			{
				this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_A);
			}
			else
			{
				this.SwitchToState((UnityEngine.Random.value <= 0.5f) ? DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_B : DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_A);
			}
		}
	}

	// Token: 0x0600553D RID: 21821 RVA: 0x00206040 File Offset: 0x00204240
	public void NotifyEnteredNewRoom(RoomHandler newRoom)
	{
		this.UpdateCoreMusicEvent();
		if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON)
		{
			if (newRoom != null && (newRoom.RoomVisualSubtype == 7 || newRoom.RoomVisualSubtype == 8))
			{
				if (this.m_cachedMusicEventCore != "Play_MUS_Space_Theme_01")
				{
					AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
					this.m_currentState = DungeonFloorMusicController.DungeonMusicState.FLOOR_INTRO;
					this.m_cachedMusicEventCore = "Play_MUS_Space_Theme_01";
					AkSoundEngine.PostEvent("Play_MUS_Space_Theme_01", GameManager.Instance.gameObject);
				}
			}
			else if (this.m_cachedMusicEventCore != "Play_MUS_Office_Theme_01")
			{
				AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
				this.m_currentState = DungeonFloorMusicController.DungeonMusicState.FLOOR_INTRO;
				this.m_cachedMusicEventCore = "Play_MUS_Office_Theme_01";
				AkSoundEngine.PostEvent("Play_MUS_Office_Theme_01", GameManager.Instance.gameObject);
			}
		}
		if (newRoom.area != null && newRoom.area.runtimePrototypeData != null && newRoom.area.runtimePrototypeData.UsesCustomMusic && !string.IsNullOrEmpty(newRoom.area.runtimePrototypeData.CustomMusicEvent))
		{
			this.SwitchToCustomMusic(newRoom.area.runtimePrototypeData.CustomMusicEvent, base.gameObject, newRoom.area.runtimePrototypeData.UsesCustomSwitch, newRoom.area.runtimePrototypeData.CustomMusicSwitch);
		}
		else if (newRoom.area != null && newRoom.area.runtimePrototypeData != null && newRoom.area.runtimePrototypeData.UsesCustomMusicState)
		{
			this.m_cooldownTimerRemaining = -1f;
			this.SwitchToState(newRoom.area.runtimePrototypeData.CustomMusicState);
		}
		else if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			this.m_cooldownTimerRemaining = -1f;
			this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.CALM);
		}
		else if (newRoom.IsShop && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
		{
			this.m_lastMusicChangeTime = Time.realtimeSinceStartup;
			this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.SHOP);
		}
		else if (newRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
		{
			this.m_cooldownTimerRemaining = -1f;
			if (this.m_isVictoryState || this.m_currentState == DungeonFloorMusicController.DungeonMusicState.FLOOR_INTRO || this.m_currentState == DungeonFloorMusicController.DungeonMusicState.CALM || this.m_currentState == DungeonFloorMusicController.DungeonMusicState.SHOP || this.m_currentState == DungeonFloorMusicController.DungeonMusicState.SECRET || this.m_currentState == DungeonFloorMusicController.DungeonMusicState.ARCADE || this.m_currentState == (DungeonFloorMusicController.DungeonMusicState)(-1))
			{
				if (this.m_isVictoryState)
				{
					this.EndVictoryMusic();
				}
				this.SwitchToActiveMusic(null);
			}
			else if (this.m_currentState == DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_A)
			{
				if (UnityEngine.Random.value > 0.5f && Time.realtimeSinceStartup - this.m_lastMusicChangeTime > this.MUSIC_CHANGE_TIMER)
				{
					this.m_lastMusicChangeTime = Time.realtimeSinceStartup;
					this.SwitchToActiveMusic(new DungeonFloorMusicController.DungeonMusicState?(DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_A));
				}
			}
			else if (this.m_currentState == DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_B && UnityEngine.Random.value > 0.5f && Time.realtimeSinceStartup - this.m_lastMusicChangeTime > this.MUSIC_CHANGE_TIMER)
			{
				this.m_lastMusicChangeTime = Time.realtimeSinceStartup;
				this.SwitchToActiveMusic(new DungeonFloorMusicController.DungeonMusicState?(DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_B));
			}
		}
		else if (newRoom.WasEverSecretRoom)
		{
			this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.SECRET);
		}
		else if (this.m_currentState == DungeonFloorMusicController.DungeonMusicState.SHOP || this.m_currentState == DungeonFloorMusicController.DungeonMusicState.ARCADE || this.m_currentState == DungeonFloorMusicController.DungeonMusicState.SECRET || this.m_currentState == DungeonFloorMusicController.DungeonMusicState.FOYER_ELEVATOR || this.m_currentState == DungeonFloorMusicController.DungeonMusicState.FOYER_SORCERESS)
		{
			this.SwitchToState(DungeonFloorMusicController.DungeonMusicState.CALM);
		}
	}

	// Token: 0x0600553E RID: 21822 RVA: 0x00206408 File Offset: 0x00204608
	public void NotifyCurrentRoomEnemiesCleared()
	{
		this.m_cooldownTimerRemaining = this.COOLDOWN_TIMER;
	}

	// Token: 0x04004E0A RID: 19978
	private DungeonFloorMusicController.DungeonMusicState m_currentState;

	// Token: 0x04004E0B RID: 19979
	private float m_cooldownTimerRemaining = -1f;

	// Token: 0x04004E0C RID: 19980
	private float COOLDOWN_TIMER = 22.5f;

	// Token: 0x04004E0D RID: 19981
	private float MUSIC_CHANGE_TIMER = 40f;

	// Token: 0x04004E0E RID: 19982
	private float m_lastMusicChangeTime;

	// Token: 0x04004E0F RID: 19983
	private string m_cachedMusicEventCore = string.Empty;

	// Token: 0x04004E10 RID: 19984
	private bool m_overrideMusic;

	// Token: 0x04004E11 RID: 19985
	private bool m_isVictoryState;

	// Token: 0x04004E12 RID: 19986
	private float m_changedToArcadeTimer = -1f;

	// Token: 0x04004E13 RID: 19987
	private uint m_coreMusicEventID;

	// Token: 0x02000F70 RID: 3952
	public enum DungeonMusicState
	{
		// Token: 0x04004E15 RID: 19989
		FLOOR_INTRO,
		// Token: 0x04004E16 RID: 19990
		ACTIVE_SIDE_A = 10,
		// Token: 0x04004E17 RID: 19991
		ACTIVE_SIDE_B = 20,
		// Token: 0x04004E18 RID: 19992
		ACTIVE_SIDE_C = 23,
		// Token: 0x04004E19 RID: 19993
		ACTIVE_SIDE_D = 25,
		// Token: 0x04004E1A RID: 19994
		CALM = 30,
		// Token: 0x04004E1B RID: 19995
		SHOP = 40,
		// Token: 0x04004E1C RID: 19996
		SECRET = 50,
		// Token: 0x04004E1D RID: 19997
		ARCADE = 60,
		// Token: 0x04004E1E RID: 19998
		FOYER_ELEVATOR = 70,
		// Token: 0x04004E1F RID: 19999
		FOYER_SORCERESS = 75
	}
}
