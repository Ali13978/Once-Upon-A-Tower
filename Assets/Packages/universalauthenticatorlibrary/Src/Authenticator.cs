using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UniversalAuthenticatorLibrary
{
    [Serializable]
    public class Authenticator : MonoBehaviour
    {
        /**
        * Default value for shouldInvalidateAfter(), 1 week in seconds
        */
        private uint defaultInvalidateAfter = 604800;

        [SerializeField] private ButtonStyle ButtonStyle;

        private Chain Chain;

        private UALOptions Options;

        //  /**
        //   * @param chains     This represents each of the chains that the dapp provides support for.
        //   *
        //   * @param options    Optional argument that is intended for Authenticator specific options.
        //   */
        //  constructor(public chains: Chain[], public options?: any) {}

        public Authenticator(Chain chain, UALOptions options)
        {
            Chain = chain;
            Options = options;
        }



        //  /**
        //   * Attempts to render the Authenticator and updates the authenticator's state, accordingly
        //   */
        public virtual void Init(Chain chain, UALOptions options)
        {
            Chain = chain;
            Options = options;
        }

        /**
         * Returns the style of the Button that will be rendered.
         */
        public ButtonStyle GetStyle() => ButtonStyle;

        //  /**
        //   * Returns whether or not the button should render based on the operating environment and other factors.
        //   * ie. If your Authenticator App does not support mobile, it returns false when running in a mobile browser.
        //   */
        public virtual bool ShouldRender()
        {
            throw new NotImplementedException();
        }

        /**
         * Returns whether or not the dapp should attempt to auto login with the Authenticator app.
         * Auto login will only occur when there is only one Authenticator that returns shouldRender() true and
         * shouldAutoLogin() true.
         */
        public virtual bool ShouldAutoLogin()
        {
            throw new NotImplementedException();
        }

        /**
         * Returns the amount of seconds after the authentication will be invalid for logging in on new
         * browser sessions.  Setting this value to zero will cause users to re-attempt authentication on
         * every new browser session.  Please note that the invalidate time will be saved client-side and
         * should not be relied on for security.
         */
        public uint ShouldInvalidateAfter() => defaultInvalidateAfter;

        ///**
        // * Login using the Authenticator App. This can return one or more users depending on multiple chain support.
        // *
        // * @param accountName  The account name of the user for Authenticators that do not store accounts (optional)
        // */
        public virtual Task<User> Login(string accountName = null)
        {
            throw new NotImplementedException();
        }

        /**
         * Logs the user out of the dapp. This will be strongly dependent on each Authenticator app's patterns.
         */
        public virtual Task Logout()
        {
            throw new NotImplementedException();
        }
    }
}