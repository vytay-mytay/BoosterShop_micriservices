using authentication.Common.Constants;
using authentication.Models.ResponseModels;

namespace authentication.Helpers
{
    public class ErrorHelper
    {
        public static ErrorResponseModel GetError(int code)
        {
            var errorResponse = new ErrorResponseModel();

            switch (code)
            {
                case 400:
                    errorResponse.Code = ErrorCode.BadRequest;
                    break;
                case 401:
                    errorResponse.Code = ErrorCode.Unauthorized;
                    errorResponse.AddError("token", "Token invalid");
                    break;
                case 403:
                    errorResponse.Code = ErrorCode.Forbidden;
                    break;
                case 404:
                    errorResponse.Code = ErrorCode.NotFound;
                    break;
                case 405:
                    errorResponse.Code = ErrorCode.NotAllowed;
                    break;
                case 500:
                    errorResponse.Code = ErrorCode.InternalServerError;
                    break;
            }

            return errorResponse;
        }
    }
}
