using UnityEngine;
using UnityEngine.UIElements;

    public class AtomicAssetsErrorPanel : MonoBehaviour
    {
        /*
         * Child-controls
         */

        public VisualElement Root;

        private Button _okButton;

        private Label _errorTextShort;
        private Label _errorTextLong;

        private VisualElement _childContentContainer;

        void Start()
        {
            Root = GetComponent<UIDocument>().rootVisualElement;

            _okButton = Root.Q<Button>("ok-button");
            _errorTextShort = Root.Q<Label>("error-text-short");
            _errorTextLong = Root.Q<Label>("error-text-long");
            _childContentContainer = Root.Q<VisualElement>("child-content-container");

            Hide(Root);

            _okButton.clickable.clicked += () =>
            { 
                Hide(Root);
            };
        }

        public void ErrorText(string errorShort, string errorLong)
        {
            Root.BringToFront();

            _childContentContainer.Clear();

            _errorTextShort.text = errorShort;
            _errorTextLong.text = errorLong;

            Show(_errorTextShort);
            Show(_errorTextLong);

            Show(Root);
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
    }
