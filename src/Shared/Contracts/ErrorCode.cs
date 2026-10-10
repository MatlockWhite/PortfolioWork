using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    /// <summary>
    /// The ErrorCode class contains constants for common error codes used in API responses.
    /// 
    /// NOTE:   I am going to use an exhaustive approach and account for all the HTTP status codes, even though I will not use them all. 
    ///         This is to ensure that I have a complete list of error codes and one could more easily add new ones in the future if needed.
    ///         These error codes are pulled from the standard HTTP status codes found on Wikipedia at: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes.
    /// </summary>
    public static class ErrorCode
    {
        // 100 - 199: Informational
        public const string Continue = "CONTINUE";                                              // 100
        public const string SwitchingProtocols = "SWITCHING_PROTOCOLS";                         // 101
        public const string Processing = "PROCESSING";                                          // 102
        public const string EarlyHints = "EARLY_HINTS";                                         // 103

        // 200 - 299: Success
        public const string OK = "OK";                                                          // 200
        public const string Created = "CREATED";                                                // 201
        public const string Accepted = "ACCEPTED";                                              // 202
        public const string NonAuthoritativeInformation = "NON_AUTHORITATIVE_INFORMATION";      // 203
        public const string NoContent = "NO_CONTENT";                                           // 204
        public const string ResetContent = "RESET_CONTENT";                                     // 205
        public const string PartialContent = "PARTIAL_CONTENT";                                 // 206
        public const string MultiStatus = "MULTI_STATUS";                                       // 207
        public const string AlreadyReported = "ALREADY_REPORTED";                               // 208
        public const string IMUsed = "IM_USED";                                                 // 226

        // 300 - 399: Redirection
        public const string MultipleChoices = "MULTIPLE_CHOICES";                               // 300
        public const string MovedPermanently = "MOVED_PERMANENTLY";                             // 301
        public const string Found = "FOUND";                                                    // 302
        public const string SeeOther = "SEE_OTHER";                                             // 303
        public const string NotModified = "NOT_MODIFIED";                                       // 304
        public const string UseProxy = "USE_PROXY";                                             // 305
        public const string TemporaryRedirect = "TEMPORARY_REDIRECT";                           // 307
        public const string PermanentRedirect = "PERMANENT_REDIRECT";                           // 308

        // 400 - 499: Client Error
        public const string BadRequest = "BAD_REQUEST";                                         // 400
        public const string Unauthorized = "UNAUTHORIZED";                                      // 401
        public const string PaymentRequired = "PAYMENT_REQUIRED";                               // 402
        public const string Forbidden = "FORBIDDEN";                                            // 403
        public const string NotFound = "NOT_FOUND";                                             // 404
        public const string MethodNotAllowed = "METHOD_NOT_ALLOWED";                            // 405
        public const string NotAcceptable = "NOT_ACCEPTABLE";                                   // 406
        public const string ProxyAuthenticationRequired = "PROXY_AUTHENTICATION_REQUIRED";      // 407
        public const string RequestTimeout = "REQUEST_TIMEOUT";                                 // 408
        public const string Conflict = "CONFLICT";                                              // 409
        public const string Gone = "GONE";                                                      // 410
        public const string LengthRequired = "LENGTH_REQUIRED";                                 // 411
        public const string PreconditionFailed = "PRECONDITION_FAILED";                         // 412
        public const string PayloadTooLarge = "PAYLOAD_TOO_LARGE";                              // 413
        public const string URITooLong = "URI_TOO_LONG";                                        // 414
        public const string UnsupportedMediaType = "UNSUPPORTED_MEDIA_TYPE";                    // 415
        public const string RangeNotSatisfiable = "RANGE_NOT_SATISFIABLE";                      // 416
        public const string ExpectationFailed = "EXPECTATION_FAILED";                           // 417
        public const string ImATeapot = "IM_A_TEAPOT";                                          // 418  // My Personal Favorite.
        public const string MisdirectedRequest = "MISDIRECTED_REQUEST";                         // 421
        public const string UnprocessableEntity = "UNPROCESSABLE_ENTITY";                       // 422
        public const string Locked = "LOCKED";                                                  // 423
        public const string FailedDependency = "FAILED_DEPENDENCY";                             // 424
        public const string TooEarly = "TOO_EARLY";                                             // 425
        public const string UpgradeRequired = "UPGRADE_REQUIRED";                               // 426
        public const string PreconditionRequired = "PRECONDITION_REQUIRED";                     // 428
        public const string TooManyRequests = "TOO_MANY_REQUESTS";                              // 429
        public const string RequestHeaderFieldsTooLarge = "REQUEST_HEADER_FIELDS_TOO_LARGE";    // 431
        public const string UnavailableForLegalReasons = "UNAVAILABLE_FOR_LEGAL_REASONS";       // 451

        // 500 - 599: Server Error
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";                      // 500
        public const string NotImplemented = "NOT_IMPLEMENTED";                                 // 501
        public const string BadGateway = "BAD_GATEWAY";                                         // 502
        public const string ServiceUnavailable = "SERVICE_UNAVAILABLE";                         // 503
        public const string GatewayTimeout = "GATEWAY_TIMEOUT";                                 // 504
        public const string HTTPVersionNotSupported = "HTTP_VERSION_NOT_SUPPORTED";             // 505
        public const string VariantAlsoNegotiates = "VARIANT_ALSO_NEGOTIATES";                  // 506
        public const string InsufficientStorage = "INSUFFICIENT_STORAGE";                       // 507
        public const string LoopDetected = "LOOP_DETECTED";                                     // 508
        public const string NotExtended = "NOT_EXTENDED";                                       // 510
        public const string NetworkAuthenticationRequired = "NETWORK_AUTHENTICATION_REQUIRED";  // 511

        // Additional Custom Error Codes can be added here as needed.
    }

    /// <summary>
    /// The HTTPStatusCode class contains constants for common HTTP status codes used in API responses.
    /// </summary>
    public static class HTTPStatusCode
    {
        // 100 - 199: Informational
        public const int Continue = 100;                                                // CONTINUE
        public const int SwitchingProtocols = 101;                                      // SWITCHING_PROTOCOLS
        public const int Processing = 102;                                              // PROCESSING
        public const int EarlyHints = 103;                                              // EARLY_HINTS

        // 200 - 299: Success
        public const int OK = 200;                                                      // OK
        public const int Created = 201;                                                 // CREATED
        public const int Accepted = 202;                                                // ACCEPTED
        public const int NonAuthoritativeInformation = 203;                             // NON_AUTHORITATIVE_INFORMATION
        public const int NoContent = 204;                                               // NO_CONTENT
        public const int ResetContent = 205;                                            // RESET_CONTENT
        public const int PartialContent = 206;                                          // PARTIAL_CONTENT
        public const int MultiStatus = 207;                                             // MULTI_STATUS
        public const int AlreadyReported = 208;                                         // ALREADY_REPORTED
        public const int IMUsed = 226;                                                  // IM_USED

        // 300 - 399: Redirection
        public const int MultipleChoices = 300;                                         // MULTIPLE_CHOICES
        public const int MovedPermanently = 301;                                        // MOVED_PERMANENTLY
        public const int Found = 302;                                                   // FOUND
        public const int SeeOther = 303;                                                // SEE_OTHER
        public const int NotModified = 304;                                             // NOT_MODIFIED
        public const int UseProxy = 305;                                                // USE_PROXY
        public const int TemporaryRedirect = 307;                                       // TEMPORARY_REDIRECT
        public const int PermanentRedirect = 308;                                       // PERMANENT_REDIRECT

        // 400 - 499: Client Error
        public const int BadRequest = 400;                                              // BAD_REQUEST
        public const int Unauthorized = 401;                                            // UNAUTHORIZED
        public const int PaymentRequired = 402;                                         // PAYMENT_REQUIRED
        public const int Forbidden = 403;                                               // FORBIDDEN
        public const int NotFound = 404;                                                // NOT_FOUND
        public const int MethodNotAllowed = 405;                                        // METHOD_NOT_ALLOWED
        public const int NotAcceptable = 406;                                           // NOT_ACCEPTABLE
        public const int ProxyAuthenticationRequired = 407;                             // PROXY_AUTHENTICATION_REQUIRED
        public const int RequestTimeout = 408;                                          // REQUEST_TIMEOUT
        public const int Conflict = 409;                                                // CONFLICT
        public const int Gone = 410;                                                    // GONE
        public const int LengthRequired = 411;                                          // LENGTH_REQUIRED
        public const int PreconditionFailed = 412;                                      // PRECONDITION_FAILED
        public const int PayloadTooLarge = 413;                                         // PAYLOAD_TOO_LARGE
        public const int URITooLong = 414;                                              // URI_TOO_LONG
        public const int UnsupportedMediaType = 415;                                    // UNSUPPORTED_MEDIA_TYPE
        public const int RangeNotSatisfiable = 416;                                     // RANGE_NOT_SATISFIABLE
        public const int ExpectationFailed = 417;                                       // EXPECTATION_FAILED
        public const int ImATeapot = 418;                                               // IM_A_TEAPOT
        public const int MisdirectedRequest = 421;                                      // MIS_DIRECTED_REQUEST
        public const int UnprocessableEntity = 422;                                     // UNPROCESSABLE_ENTITY
        public const int Locked = 423;                                                  // LOCKED
        public const int FailedDependency = 424;                                        // FAILED_DEPENDENCY
        public const int TooEarly = 425;                                                // TOO_EARLY
        public const int UpgradeRequired = 426;                                         // UPGRADE_REQUIRED
        public const int PreconditionRequired = 428;                                    // PRECONDITION_REQUIRED
        public const int TooManyRequests = 429;                                         // TOO_MANY_REQUESTS
        public const int RequestHeaderFieldsTooLarge = 431;                             // REQUEST_HEADER_FIELDS_TOO_LARGE
        public const int UnavailableForLegalReasons = 451;                              // UNAVAILABLE_FOR_LEGAL_REASONS

        // 500 - 599: Server Error
        public const int InternalServerError = 500;                                     // INTERNAL_SERVER_ERROR
        public const int NotImplemented = 501;                                          // NOT_IMPLEMENTED
        public const int BadGateway = 502;                                              // BAD_GATEWAY
        public const int ServiceUnavailable = 503;                                      // SERVICE_UNAVAILABLE
        public const int GatewayTimeout = 504;                                          // GATEWAY_TIMEOUT
        public const int HTTPVersionNotSupported = 505;                                 // HTTP_VERSION_NOT_SUPPORTED
        public const int VariantAlsoNegotiates = 506;                                   // VARIANT_ALSO_NEGOTIATES
        public const int InsufficientStorage = 507;                                     // INSUFFICIENT_STORAGE
        public const int LoopDetected = 508;                                            // LOOP_DETECTED
        public const int NotExtended = 510;                                             // NOT_EXTENDED
        public const int NetworkAuthenticationRequired = 511;                           // NETWORK_AUTHENTICATION_REQUIRED

        // Additional Custom Status Codes can be added here as needed.
    }

    /// <summary>
    /// The following Hashmap ensures that the ErrorCode and HTTPStatusCode constants are consistent with each other.
    /// </summary>
    public static class ErrorCodeStatusCodeMap
    {
        /// <summary>
        /// Creates and sets the Hashmap for the ErrorCode and HTTPStatusCode constants to ensure they are consistent with each other.
        /// 
        /// NOTE:   For the Dictionary/hashmap, we are using the ErrorCode as the key and the HTTPStatusCode as the value.
        ///         This is because the HTTPStatusCode is finite and has multiple references to the same value depending on a variety of Values such as:
        ///         OS, Servers, Programming preferences, and Services the HTTP Response is requresting information from.
        ///         
        ///         For Example:
        ///             HTTP Status Code - 451 
        ///                 Normal conditions: the content is Unavailable for Legal Reasons
        ///                 Microsoft: uses as a Redirect to another webpage.
        ///         
        ///         Although I will not be iterating any duplicate entries in this testbed, I would be remiss to not include a solution that could account for it as I do in professional Dev.
        ///         If one was to account for multiple entries they should be allocated to a specialized function where that is being properly handled.
        /// </summary>
        private static readonly Dictionary<string, int> CodeMap = new()
        {
            // 100 - 199: Informational
            { ErrorCode.Continue, HTTPStatusCode.Continue },
            { ErrorCode.SwitchingProtocols, HTTPStatusCode.SwitchingProtocols },
            { ErrorCode.Processing, HTTPStatusCode.Processing },
            { ErrorCode.EarlyHints, HTTPStatusCode.EarlyHints },

            // 200 - 299: Success
            { ErrorCode.OK, HTTPStatusCode.OK },
            { ErrorCode.Created, HTTPStatusCode.Created },
            { ErrorCode.Accepted, HTTPStatusCode.Accepted },
            { ErrorCode.NonAuthoritativeInformation, HTTPStatusCode.NonAuthoritativeInformation },
            { ErrorCode.NoContent, HTTPStatusCode.NoContent },
            { ErrorCode.ResetContent, HTTPStatusCode.ResetContent },
            { ErrorCode.PartialContent, HTTPStatusCode.PartialContent },
            { ErrorCode.MultiStatus, HTTPStatusCode.MultiStatus },
            { ErrorCode.AlreadyReported, HTTPStatusCode.AlreadyReported },
            { ErrorCode.IMUsed, HTTPStatusCode.IMUsed },

            // 300 - 399: Redirection
            { ErrorCode.MultipleChoices, HTTPStatusCode.MultipleChoices },
            { ErrorCode.MovedPermanently, HTTPStatusCode.MovedPermanently },
            { ErrorCode.Found, HTTPStatusCode.Found },
            { ErrorCode.SeeOther, HTTPStatusCode.SeeOther },
            { ErrorCode.NotModified, HTTPStatusCode.NotModified },
            { ErrorCode.UseProxy, HTTPStatusCode.UseProxy },
            { ErrorCode.TemporaryRedirect, HTTPStatusCode.TemporaryRedirect },
            { ErrorCode.PermanentRedirect, HTTPStatusCode.PermanentRedirect },

            // 400 - 499: Client Error
            { ErrorCode.BadRequest, HTTPStatusCode.BadRequest },
            { ErrorCode.Unauthorized, HTTPStatusCode.Unauthorized },
            { ErrorCode.PaymentRequired, HTTPStatusCode.PaymentRequired },
            { ErrorCode.Forbidden, HTTPStatusCode.Forbidden },
            { ErrorCode.NotFound, HTTPStatusCode.NotFound },
            { ErrorCode.MethodNotAllowed, HTTPStatusCode.MethodNotAllowed },
            { ErrorCode.NotAcceptable, HTTPStatusCode.NotAcceptable },
            { ErrorCode.ProxyAuthenticationRequired, HTTPStatusCode.ProxyAuthenticationRequired },
            { ErrorCode.RequestTimeout, HTTPStatusCode.RequestTimeout },
            { ErrorCode.Conflict, HTTPStatusCode.Conflict },
            { ErrorCode.Gone, HTTPStatusCode.Gone },
            { ErrorCode.LengthRequired, HTTPStatusCode.LengthRequired },
            { ErrorCode.PreconditionFailed, HTTPStatusCode.PreconditionFailed },
            { ErrorCode.PayloadTooLarge, HTTPStatusCode.PayloadTooLarge },
            { ErrorCode.URITooLong, HTTPStatusCode.URITooLong },
            { ErrorCode.UnsupportedMediaType, HTTPStatusCode.UnsupportedMediaType },
            { ErrorCode.RangeNotSatisfiable, HTTPStatusCode.RangeNotSatisfiable },
            { ErrorCode.ExpectationFailed, HTTPStatusCode.ExpectationFailed },
            { ErrorCode.ImATeapot, HTTPStatusCode.ImATeapot },
            { ErrorCode.MisdirectedRequest, HTTPStatusCode.MisdirectedRequest },
            { ErrorCode.UnprocessableEntity, HTTPStatusCode.UnprocessableEntity },
            { ErrorCode.Locked, HTTPStatusCode.Locked },
            { ErrorCode.FailedDependency, HTTPStatusCode.FailedDependency },
            { ErrorCode.TooEarly, HTTPStatusCode.TooEarly },
            { ErrorCode.UpgradeRequired, HTTPStatusCode.UpgradeRequired },
            { ErrorCode.PreconditionRequired, HTTPStatusCode.PreconditionRequired },
            { ErrorCode.TooManyRequests, HTTPStatusCode.TooManyRequests },
            { ErrorCode.RequestHeaderFieldsTooLarge, HTTPStatusCode.RequestHeaderFieldsTooLarge },
            { ErrorCode.UnavailableForLegalReasons, HTTPStatusCode.UnavailableForLegalReasons },

            // 500 - 599: Server Error
            { ErrorCode.InternalServerError, HTTPStatusCode.InternalServerError },
            { ErrorCode.NotImplemented, HTTPStatusCode.NotImplemented },
            { ErrorCode.BadGateway, HTTPStatusCode.BadGateway },
            { ErrorCode.ServiceUnavailable, HTTPStatusCode.ServiceUnavailable },
            { ErrorCode.GatewayTimeout, HTTPStatusCode.GatewayTimeout },
            { ErrorCode.HTTPVersionNotSupported, HTTPStatusCode.HTTPVersionNotSupported },
            { ErrorCode.VariantAlsoNegotiates, HTTPStatusCode.VariantAlsoNegotiates },
            { ErrorCode.InsufficientStorage, HTTPStatusCode.InsufficientStorage },
            { ErrorCode.LoopDetected, HTTPStatusCode.LoopDetected },
            { ErrorCode.NotExtended, HTTPStatusCode.NotExtended },
            { ErrorCode.NetworkAuthenticationRequired, HTTPStatusCode.NetworkAuthenticationRequired }

            // Additional Custom Mappings can be added here as needed.
        };

        /// <summary>
        /// Gets the HTTP status code associated with the given ErrorCode.
        /// NOTE:   I typically return nulls and account for this in the exception handlers at the function level.
        /// </summary>
        /// <param name="errorCode">The error code for which to get the associated HTTP status code.</param>
        /// <returns>The HTTP status code associated with the given error code.</returns>
        public static int? GetStatusCode(string errorCode)
        {
            if (CodeMap.TryGetValue(errorCode, out var statusCode))
            {
                return statusCode;
            }

            return null;
        }

        /// <summary>
        /// Gets the ErrorCode associated with the given HTTP status code.
        /// NOTE:   As stated previously we will not be utilizing duplicate HTTP status Codes and as such we can use the function like this as it will either 
        ///         return the single correct value or a null to be properly handled at the function level.
        /// </summary>
        /// <param name="statusCode">The HTTP status code for which to get the associated error code.</param>
        /// <returns>The error code associated with the given HTTP status code.</returns>
        public static string? GetErrorCode(int statusCode)
        {
            return CodeMap.FirstOrDefault(kvp => kvp.Value == statusCode).Key;
        }
    }
}
