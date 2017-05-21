namespace Common
{
    public static class ErrorCodes
    {
        public const int UNEXPECTED = 1;
        public const int NOT_AUTH = 2;
        public const int AUTH_EMAIL_IN_USE = 3;
        public const int AUTH_EMAIL_NOT_FOUND = 4;
        public const int AUTH_EMAIL_OR_PASSWORD_INCORRECT = 5;
        public const int AUTH_NOT_ASSIGNED_TO_USER = 6;
        public const int AUTH_EMAIL_INVALID = 7;
        public const int AUTH_PASSWORD_CONFIRMATION_NOT_EQ = 8;
        public const int AUTH_PASSWORD_LENGTH = 9;
        public const int AUTH_INVALID_TOKEN = 12;
        public const int AUTH_INVALID_GRANT_TYPE = 13;
        public const int NOT_FOUND = 10;
        public const int ALREADY_EXISTS = 11;
        public const int OPERATION_ERROR = 14;
    }
}
