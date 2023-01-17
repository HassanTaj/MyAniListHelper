namespace AniListHelper.Infrastructure {
    public class SecureStorageProcessor {
        public async void SetAuthToken(string tokenValue, DateTimeOffset? expiresIn) {
            await SecureStorage.Default.SetAsync(Constants.Auth.TOKEN, tokenValue);
            await SecureStorage.Default.SetAsync(Constants.Auth.TOKEN_EXPIRY, expiresIn.ToString());
        }
        public async Task<string> GetAuthToken() {
            string oauthToken = await SecureStorage.Default.GetAsync(Constants.Auth.TOKEN);
            //TODO: Add Expiry logic
            if (oauthToken == null) {
                // No value is associated with the key "oauth_token"
            }
            return oauthToken;
        }
        public async void RemoveToken(string token) {
            string oauthToken = await SecureStorage.Default.GetAsync(token);

            if (oauthToken == null) {
                // No value is associated with the key "oauth_token"
            }
        }
    }
}
