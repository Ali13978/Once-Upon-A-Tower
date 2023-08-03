using HyperionApiClient;
using HyperionApiClient.Clients;
using HyperionApiClient.Exceptions;
using HyperionApiClient.Responses;
using UnityEngine;
using UnityEngine.UIElements;

public class HyperionExamplePanel : MonoBehaviour
{
    /**
     * Child-Controls
     */
    public VisualElement Root;

    private VisualElement _accountBox;
    private VisualElement _infoBox;
    private VisualElement _searchDetails;
    private VisualElement _blockBox;
    private VisualElement _loadingMask;
    private VisualElement _searchBox;

    private Label _accountLabel;
    private Label _creatorLabel;
    private Label _timeSpanLabel;
    private Label _blockNumberLabel;
    private Label _trxIdLabel;
    private Label _queryLabel;
    private Label _infoLabel;

    private Label _confirmLabel;
    private Label _previousBlockLabel;
    //private Label _blockCPULimitLabel;
    private Label _timestampBlockLabel;
    private Label _blockNumberBlockLabel;
    private Label _blockActionMrootLabel;
    private Label _blockTransactionMrootLabel;
    private Label _blockProducerSignatureLabel;
    //private Label _headBlockIdLabel;
    private Label _blockCPULimitLabel;
    private Label _blockNETLimitLabel;
    private Label _chainIdLabel;
    private Label _forkDbHeadBlockIdLabel;
    private Label _forkDbHeadBlockNumLabel;
    private Label _headBlockIdLabel;
    private Label _headBlockNumLabel;
    private Label _headBlockProducerLabel;
    private Label _headBlockTimeLabel;

    private Button _searchButton;
    private Button _accountButton;
    private Button _infoButton;
    private Button _blockButton;
    private Button _closeViewButton;

    private static TextField _textFieldValue;

    /**
     * Fields/Properties
     */

    private AccountsClient _accountsClient;
    private ChainClient _chainClient;

    public HyperionErrorPanel HyperionErrorPanel;


    private bool _activeButton;
    void Start()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;

        _accountsClient = new AccountsClient(new HttpHandler())
        {
            BaseUrl = "https://wax.eosphere.io/"
        };

        _chainClient = new ChainClient(new HttpHandler())
        {
            BaseUrl = "https://wax.eosphere.io/"
        };

        _textFieldValue = Root.Q<TextField>("collection-name-or-id-textfield");

        _accountLabel = Root.Q<Label>("account-name-label");
        _creatorLabel = Root.Q<Label>("creator-label");
        _timeSpanLabel = Root.Q<Label>("timespan-label");
        _blockNumberLabel = Root.Q<Label>("block-number-label");
        _trxIdLabel = Root.Q<Label>("transaction-id-label");
        _queryLabel = Root.Q<Label>("query-label");
        _infoLabel = Root.Q<Label>("info-label");

        _confirmLabel = Root.Q<Label>("confirm-label");
        _previousBlockLabel = Root.Q<Label>("previous-block-label");
        _timestampBlockLabel = Root.Q<Label>("timestamp-block-label");
        _blockNumberBlockLabel = Root.Q<Label>("block-number-block-label");
        _blockActionMrootLabel = Root.Q<Label>("block-action-mroot-label");
        _blockProducerSignatureLabel = Root.Q<Label>("block-producer-signature-label");
        _blockTransactionMrootLabel = Root.Q<Label>("block-transaction-mroot-label");

        _headBlockIdLabel = Root.Q<Label>("head-block-id-label");
        _blockCPULimitLabel = Root.Q<Label>("block-cpu-limit-label");
        _blockNETLimitLabel = Root.Q<Label>("block-net-limit-label");
        _chainIdLabel = Root.Q<Label>("chain-id-label");
        _forkDbHeadBlockIdLabel = Root.Q<Label>("fork-block-id-label");
        _forkDbHeadBlockNumLabel = Root.Q<Label>("fork-block-number-label");
        _headBlockIdLabel = Root.Q<Label>("head-block-id-label");
        _headBlockNumLabel = Root.Q<Label>("head-block-number-label");
        _headBlockProducerLabel = Root.Q<Label>("head-block-producer-label");
        _headBlockTimeLabel = Root.Q<Label>("head-block-time-label");

        _accountBox = Root.Q<VisualElement>("account-box");
        _blockBox = Root.Q<VisualElement>("block-box");
        _infoBox = Root.Q<VisualElement>("info-box");
        _searchDetails = Root.Q<VisualElement>("search-details");
        _loadingMask = Root.Q<VisualElement>("loading-mask");
        _searchBox = Root.Q<VisualElement>("search-box");

        _searchButton = Root.Q<Button>("search-button");
        _accountButton = Root.Q<Button>("account-button");
        _infoButton = Root.Q<Button>("info-button");
        _blockButton = Root.Q<Button>("block-button");
        _closeViewButton = Root.Q<Button>("close-view-button");

        _textFieldValue.label = "Account";
        _textFieldValue.RegisterCallback<ClickEvent>(evt =>
        {
            _textFieldValue.value= "";
            if (_textFieldValue.label == "Info") Hide(_searchDetails);
            else Show(_searchDetails);
        });

        Hide(_loadingMask);
        Hide(_searchDetails);

        BindButtons();
    }

    #region Button Binding
    private void BindButtons()
    {
        _searchButton.clicked += SearchAsset;

        _accountButton.clickable.clicked += () =>
        {
            _activeButton = true;
            _textFieldValue.label = "Account";
            _textFieldValue.value = "Type an account to search";
            _searchButton.text = "Search Account";
            _queryLabel.text = "Query various details about a specific account on the blockchain.";
            _infoLabel.text = "Type an account and search";

            Hide(_searchDetails);
            Show(_searchBox);
            Show(_accountBox);
            Hide(_blockBox); 
            Hide(_infoBox);
        };

        _blockButton.clickable.clicked += () =>
        {
            _textFieldValue.label = "Block";
            _searchButton.text = "Search Block";
            _queryLabel.text = "Query various details about a specific block on the blockchain.";
            _infoLabel.text = "Type a block and search";
            _textFieldValue.value = "Type a block to search";
            Hide(_searchDetails);
            Show(_searchBox);
            Show(_blockBox);
            Hide(_accountBox);
            Hide(_infoBox);
        };

        _infoButton.clickable.clicked += async () =>
        {
            _textFieldValue.label = "Info";
            _textFieldValue.value = "";

            Hide(_searchBox);
            Hide(_accountBox);
            Hide(_blockBox);
            Hide(_searchDetails);
            Show(_loadingMask);

            while (_textFieldValue.label == "Info")
            { var info = await _chainClient.GetInfoAsync(); Show(_infoBox); Rebind(info); Hide(_loadingMask);}

            Hide(_infoBox);
        };
    }

    #endregion

    #region Rebind

    /// <summary>
    /// Rebind Method for binding GetCreatorResponse api
    /// </summary>
    /// <param name="creatorResponse"></param>
    private void Rebind(GetCreatorResponse creatorResponse)
    {
        _accountLabel.text = creatorResponse.Account;
        _timeSpanLabel.text = creatorResponse.Timestamp;
        _trxIdLabel.text = creatorResponse.TrxId;
        _blockNumberLabel.text = creatorResponse.BlockNum.ToString();
        _creatorLabel.text = creatorResponse.Creator;
    }

    /// <summary>
    /// Rebind Method for binding GetBlockResponse2 api
    /// </summary>
    /// <param name="blockResponse"></param>

    private void Rebind(GetBlockResponse2 blockResponse)
    {
        _blockNumberBlockLabel.text = blockResponse.BlockNum.ToString();
        _blockActionMrootLabel.text = blockResponse.ActionMroot;
        _confirmLabel.text = blockResponse.Confirmed.ToString();
        _previousBlockLabel.text = blockResponse.Previous;
        _blockProducerSignatureLabel.text = blockResponse.Producer;
        _timestampBlockLabel.text = blockResponse.Timestamp;
        _blockTransactionMrootLabel.text = blockResponse.TransactionMroot;
    }

    /// <summary>
    /// Rebind Method for binding GetInfoResponse api
    /// </summary>
    /// <param name="info"></param>
    private void Rebind(GetInfoResponse info)
    {
        _headBlockIdLabel.text = info.HeadBlockId;
        _blockCPULimitLabel.text = info.BlockCpuLimit.ToString();
        _blockNETLimitLabel.text = info.BlockNetLimit.ToString();
        _chainIdLabel.text = info.ChainId;
        _forkDbHeadBlockIdLabel.text = info.ForkDbHeadBlockId;
        _forkDbHeadBlockNumLabel.text = info.ForkDbHeadBlockNum.ToString();
        _headBlockIdLabel.text = info.HeadBlockId;
        _headBlockNumLabel.text = info.HeadBlockNum.ToString();
        _headBlockProducerLabel.text = info.HeadBlockProducer;
        _headBlockTimeLabel.text = info.HeadBlockTime;
    }

    #endregion

    #region Others

    /// <summary>
    /// SearchAsset Method to evaluate input search for hyperion api client
    /// </summary>
    private async void SearchAsset()
    {
        try
        {
            switch (_textFieldValue.label)
            {
                case "Account":
                    Show(_loadingMask);

                    var account = await _accountsClient.GetCreatorAsync(_textFieldValue.value);
                    if (account != null)
                    {
                        Show(_accountBox);
                        Hide(_searchDetails);
                        Rebind(account);
                        Hide(_loadingMask);
                    }
                    else Debug.Log("account not found");
                    break;

                case "Block":
                    
                    Show(_blockBox);
                    Show(_loadingMask);
                    var block = await _chainClient.GetBlockAsync(_textFieldValue.value);
                    if (block != null)
                    {
                        Rebind(block);
                        Hide(_searchDetails);
                        Hide(_loadingMask);
                    }
                    else Debug.Log("block not found");
                    break;

                case "":
                    break;
            }
        }
        catch (ApiException ex)
        {
            HyperionErrorPanel.ErrorText("Content Error", ex.Content);
            Hide(_loadingMask);
            Show(HyperionErrorPanel.Root);
        }
    }

    /// <summary>
    /// Extension-method to show an UI Element (set it to visible)
    /// </summary>
    /// <param name="element"></param>
    public void Show(VisualElement element)
    {
        if (element == null)
            return;

        element.style.visibility = Visibility.Visible;
        element.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Extension-method to hide an UI Element (set it to invisible)
    /// </summary>
    /// <param name="element"></param>
    public void Hide(VisualElement element)
    {
        if (element == null)
            return;

        element.style.visibility = Visibility.Hidden;
        element.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Called when ctrl + v is pressed in browser (webgl)
    /// </summary>
    /// <param name="pastedText">The pasted text.</param>
    public void OnBrowserClipboardPaste(string pastedText)
    {
        if (string.IsNullOrEmpty(pastedText))
            return;

        if (_textFieldValue != null && _textFieldValue.focusController.focusedElement == _textFieldValue)
            _textFieldValue.SetValueWithoutNotify(pastedText);
    }

    #endregion
}
