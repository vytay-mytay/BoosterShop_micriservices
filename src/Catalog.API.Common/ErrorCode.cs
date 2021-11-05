namespace Catalog.API.Common
{
    public static class ErrorCode
    {
        public const string BadRequest = "BAD_REQUEST_ERROR";

        public const string Unauthorized = "UNAUTHORIZED_ERROR";

        public const string Forbidden = "FORBIDDEN_ERROR";

        public const string NotFound = "NOT_FOUND_ERROR";

        public const string NotAllowed = "NOT_ALLOWED_ERROR";

        public const string NotAcceptable = "NOT_ACCEPTABLE_ERROR";

        public const string RequestTimeout = "REQUEST_TIMEOUT_ERROR";

        public const string Conflict = "CONFLICT_ERROR";

        public const string Unprocessable = "UNPROCESSABLE_ERROR";

        public const string InternalServerError = "INTERNAL_SERVER_ERROR";

        public const string GatewayTimeout = "GATEWAY_TIMEOUT_ERROR";

        public const string ExpiredLink = "LINK_HAS_EXPIRED";

        public const string BlockedUser = "BLOCKED_USER";

        public const string SlotUnavailable = "SLOT_UNAVAILABLE";

        public const string NoActiveSubscription = "NO_ACTIVE_SUBSCRIPTION";

        public const string ExpiredSubscription = "EXPIRED_SUBSCRIPTION";

        public const string NotReviewedSession = "NOT_REVIEWED_TRAINING";
    }
}
