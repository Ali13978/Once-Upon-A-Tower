using Rewired;
using UnityEngine;

public class GameInput : SingletonMonoBehaviour<GameInput>
{
	[HideInInspector]
	public bool Left;

	[HideInInspector]
	public bool Right;

	[HideInInspector]
	public bool Down;

	[HideInInspector]
	public bool Up;

	private Player rewiredPlayer;

	private SwipeRecognizer swipeRecognizer;

	private bool _enabled;

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			_enabled = (value && !SingletonMonoBehaviour<Gui>.Instance.Active && (Gui.Views.RateUs == null || !Gui.Views.RateUs.Visible));
		}
	}

	private void Start()
	{
		Enabled = true;
		swipeRecognizer = new SwipeRecognizer((!Gui.AppleTV) ? 0.1f : 0.2f);
		swipeRecognizer.gestureRecognizedEvent += OnSwipe;
		TouchKit.addGestureRecognizer(swipeRecognizer);
		rewiredPlayer = ReInput.players.GetSystemPlayer();
		UnityEngine.Debug.Log("ReInput.controllers.Joysticks.Count: " + ReInput.controllers.Joysticks.Count);
		for (int i = 0; i < ReInput.controllers.Joysticks.Count; i++)
		{
			Joystick controller = ReInput.controllers.Joysticks[i];
			rewiredPlayer.controllers.AddController(controller, removeFromOtherPlayers: true);
		}
		UnityEngine.Debug.Log("rewiredPlayer.controllers.joystickCount: " + rewiredPlayer.controllers.joystickCount);
		ReInput.ControllerConnectedEvent += OnControllerConnected;
		ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
		ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
	}

	private void OnControllerConnected(ControllerStatusChangedEventArgs args)
	{
		UnityEngine.Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
		rewiredPlayer.controllers.AddController(args.controllerType, args.controllerId, removeFromOtherPlayers: true);
		if (!Gui.Views.StartView.PlayButton.Visible)
		{
			Gui.Views.StartView.Reload();
		}
	}

	private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
	{
		UnityEngine.Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
		rewiredPlayer.controllers.RemoveController(args.controllerType, args.controllerId);
		if (Gui.Views.StartView.PlayButton.Visible)
		{
			Gui.Views.StartView.Reload();
		}
	}

	private void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
	{
		UnityEngine.Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
	}

	private void OnDestroy()
	{
		ReInput.ControllerConnectedEvent -= OnControllerConnected;
		ReInput.ControllerDisconnectedEvent -= OnControllerDisconnected;
		ReInput.ControllerPreDisconnectEvent -= OnControllerPreDisconnect;
	}

	private void Update()
	{
		if (Enabled)
		{
			if (rewiredPlayer.GetButtonDown(0))
			{
				MoveLeft();
			}
			if (rewiredPlayer.GetButtonDown(1))
			{
				MoveRight();
			}
			if (rewiredPlayer.GetButtonDown(3))
			{
				MoveDown();
			}
			if (rewiredPlayer.GetButtonDown(2))
			{
				MoveUp();
			}
			if (swipeRecognizer.state == TKGestureRecognizerState.RecognizedAndStillRecognizing)
			{
				OnSwipe(swipeRecognizer);
			}
		}
	}

	private void OnSwipe(SwipeRecognizer r)
	{
		if (Enabled && !SingletonMonoBehaviour<Gui>.Instance.FindViewInPosition(r.startPoint))
		{
			switch (r.completedSwipeDirection)
			{
			case SwipeDirection.Left | SwipeDirection.Right:
			case SwipeDirection.Left | SwipeDirection.Up:
			case SwipeDirection.Right | SwipeDirection.Up:
			case SwipeDirection.Left | SwipeDirection.Right | SwipeDirection.Up:
				break;
			case SwipeDirection.Right:
				MoveRight();
				break;
			case SwipeDirection.Left:
				MoveLeft();
				break;
			case SwipeDirection.Down:
				MoveDown();
				break;
			case SwipeDirection.Up:
				MoveUp();
				break;
			}
		}
	}

	private void MoveLeft()
	{
		Left = true;
	}

	private void MoveRight()
	{
		Right = true;
	}

	private void MoveDown()
	{
		Down = true;
	}

	private void MoveUp()
	{
		Up = true;
	}

	private void LateUpdate()
	{
		Left = (Right = (Up = (Down = false)));
	}
}
