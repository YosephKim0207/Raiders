using System;
using InControl;

namespace MultiplayerWithBindingsExample
{
	// Token: 0x02000681 RID: 1665
	public class PlayerActions : PlayerActionSet
	{
		// Token: 0x060025E0 RID: 9696 RVA: 0x000A22F0 File Offset: 0x000A04F0
		public PlayerActions()
		{
			this.Green = base.CreatePlayerAction("Green");
			this.Red = base.CreatePlayerAction("Red");
			this.Blue = base.CreatePlayerAction("Blue");
			this.Yellow = base.CreatePlayerAction("Yellow");
			this.Left = base.CreatePlayerAction("Left");
			this.Right = base.CreatePlayerAction("Right");
			this.Up = base.CreatePlayerAction("Up");
			this.Down = base.CreatePlayerAction("Down");
			this.Rotate = base.CreateTwoAxisPlayerAction(this.Left, this.Right, this.Down, this.Up);
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000A23B0 File Offset: 0x000A05B0
		public static PlayerActions CreateWithKeyboardBindings()
		{
			PlayerActions playerActions = new PlayerActions();
			playerActions.Green.AddDefaultBinding(new Key[] { Key.A });
			playerActions.Red.AddDefaultBinding(new Key[] { Key.S });
			playerActions.Blue.AddDefaultBinding(new Key[] { Key.D });
			playerActions.Yellow.AddDefaultBinding(new Key[] { Key.F });
			playerActions.Up.AddDefaultBinding(new Key[] { Key.UpArrow });
			playerActions.Down.AddDefaultBinding(new Key[] { Key.DownArrow });
			playerActions.Left.AddDefaultBinding(new Key[] { Key.LeftArrow });
			playerActions.Right.AddDefaultBinding(new Key[] { Key.RightArrow });
			return playerActions;
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000A2474 File Offset: 0x000A0674
		public static PlayerActions CreateWithJoystickBindings()
		{
			PlayerActions playerActions = new PlayerActions();
			playerActions.Green.AddDefaultBinding(InputControlType.Action1);
			playerActions.Red.AddDefaultBinding(InputControlType.Action2);
			playerActions.Blue.AddDefaultBinding(InputControlType.Action3);
			playerActions.Yellow.AddDefaultBinding(InputControlType.Action4);
			playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
			playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
			playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
			playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
			playerActions.Up.AddDefaultBinding(InputControlType.DPadUp);
			playerActions.Down.AddDefaultBinding(InputControlType.DPadDown);
			playerActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
			playerActions.Right.AddDefaultBinding(InputControlType.DPadRight);
			return playerActions;
		}

		// Token: 0x040019C2 RID: 6594
		public PlayerAction Green;

		// Token: 0x040019C3 RID: 6595
		public PlayerAction Red;

		// Token: 0x040019C4 RID: 6596
		public PlayerAction Blue;

		// Token: 0x040019C5 RID: 6597
		public PlayerAction Yellow;

		// Token: 0x040019C6 RID: 6598
		public PlayerAction Left;

		// Token: 0x040019C7 RID: 6599
		public PlayerAction Right;

		// Token: 0x040019C8 RID: 6600
		public PlayerAction Up;

		// Token: 0x040019C9 RID: 6601
		public PlayerAction Down;

		// Token: 0x040019CA RID: 6602
		public PlayerTwoAxisAction Rotate;
	}
}
