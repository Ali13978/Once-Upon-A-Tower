using System.Collections.Generic;
using UnityEngine;

public class GuiViews : MonoBehaviour
{
	public ItemsView ItemsView;

	public InGameStatsView InGameStatsView;

	public LoseView LoseView;

	public GuiView FPSView;

	public WheelView WheelView;

	public SaveMeView SaveMeView;

	public StartView StartView;

	public CharacterSelectView CharacterSelect;

	public CharacterBuyView CharacterBuy;

	public CharacterGetView CharacterGet;

	public CharacterDealView CharacterDeal;

	public CharacterDealMessageView CharacterDealMessage;

	public MissionsView MissionsView;

	public LoseMissionsView LoseMissionsView;

	public StoreProcessingView StoreProcessing;

	public InGameStoreItemView InGameStoreItemView;

	public GuiView TutorialView;

	public ComboView ComboView;

	public ItemGetView ItemGet;

	public BuySaveMePackView BuySaveMePackView;

	public PauseButtonView PauseButtonView;

	public PauseView PauseView;

	public SettingsView SettingsView;

	public InGameMissionCompleteView InGameMissionCompleteView;

	public MissionText MissionTextTemplate;

	public CharacterFreeView CharacterFree;

	public GameCenterDisabledView GameCenterDisabled;

	public ConfirmResetView ConfirmReset;

	public CutsceneFrame CutsceneFrame;

	public RateUsView RateUs;

	public ThankYouView ThankYou;

	public GuiView LoadingView;

	public ConfirmExitView ConfirmExit;

	public List<GuiView> All = new List<GuiView>();
}
