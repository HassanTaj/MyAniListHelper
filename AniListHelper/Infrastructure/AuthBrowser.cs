using IdentityModel.OidcClient.Browser;
using System.Diagnostics;
namespace AniListHelper.Infrastructure {

    public class AuthBrowser : IdentityModel.OidcClient.Browser.IBrowser {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default) {
            try {
                Microsoft.Maui.Authentication.WebAuthenticatorResult authResult =
                    await WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri("myapp://")); // Add your callback URL 

                var authorizeResponse = ToRawIdentityUrl(options.EndUrl, authResult);

                return new BrowserResult {
                    Response = authorizeResponse
                };
            }
            catch (TaskCanceledException ex) {
                Debug.WriteLine(ex);
                return new BrowserResult {
                    ResultType = BrowserResultType.UserCancel
                };
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                return new BrowserResult() {
                    ResultType = BrowserResultType.UnknownError,
                    Error = ex.ToString()
                };
            }
        }

        public string ToRawIdentityUrl(string redirectUrl, WebAuthenticatorResult result) {
            try {
                IEnumerable<string> parameters = result.Properties.Select(pair => $"{pair.Key}={pair.Value}");
                var modifiedParameters = parameters.ToList();

                var stateParameter = modifiedParameters
                    .FirstOrDefault(p => p.StartsWith("state", StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(stateParameter)) {
                    // Remove the state key added by WebAuthenticator that includes appInstanceId
                    modifiedParameters = modifiedParameters.Where(p => !p.StartsWith("state", StringComparison.OrdinalIgnoreCase)).ToList();

                    stateParameter = System.Web.HttpUtility.UrlDecode(stateParameter).Split('&').Last();
                    modifiedParameters.Add(stateParameter);
                }
                var values = string.Join("&", modifiedParameters);
                return $"{redirectUrl}#{values}";
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }

    public class Browser : IdentityModel.OidcClient.Browser.IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default) {
            var authenticatorOptions = new WebAuthenticatorOptions {
                CallbackUrl = new Uri("myapp://"),
                Url = new Uri(options.StartUrl)
            };

            // WebAuthenticator.PlatformAuthenticateAsync is in a local file that's altered to locate the 
            // AppxManifest.xml file, as mentioned in my initial post.
            WebAuthenticatorResult authResult =
                await WebAuthenticator.AuthenticateAsync(authenticatorOptions);
            return new BrowserResult() {
                Response = ParseAuthenticatorResult(authResult)
            };
        }

        string ParseAuthenticatorResult(WebAuthenticatorResult result) {
            string code = result?.Properties["code"];
            string scope = result?.Properties["scope"];
            string state = result?.Properties["state"];
            string sessionState = result?.Properties["session_state"];
            return $"{"io.identitymodel.native://callback"}#code={code}&scope={scope}&state={state}&session_state={sessionState}";
        }
    }
}
