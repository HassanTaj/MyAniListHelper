using Microsoft.EntityFrameworkCore.Storage;

namespace AniListHelper.Infrastructure {
    internal class Constants {
        public class App {
            public const int CLIENT_ID = 10498;
            public const string SECRET = "LA30Uar6JqxCwjGTdSwz5SYWKOHzCAGPN0WbAdM0";
            public const string REDIRECT_URI = "myapp://";

            //public const string REDIRECT_URI = "localhost://callback";
            public static string AUTH_URI => $"https://anilist.co/api/v2/oauth/authorize?client_id={CLIENT_ID}&response_type=token";

        }
        public const string USER = "user";
        public class Auth {
            public const string TOKEN = "auth_token";
            public const string TOKEN_EXPIRY = "auth_token_expiry";
        }
        public class Database {
            public const string DATABASE_NAME = "anilisthelper.db";
            public const SQLite.SQLiteOpenFlags Flags =
      // open the database in read/write mode
      SQLite.SQLiteOpenFlags.ReadWrite |
      // create the database if it doesn't exist
      SQLite.SQLiteOpenFlags.Create |
      // enable multi-threaded database access
      SQLite.SQLiteOpenFlags.SharedCache;

            public static string DatabasePath =>
                Path.Combine(FileSystem.AppDataDirectory, DATABASE_NAME);
        }
    }
}
