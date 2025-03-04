using System;
using System.Collections.Generic;

#nullable disable

namespace cloudmenu_api.ResponseModels
{
    public enum RepMdStatus
    {
        S_200_OK = 200,
        S_201_Created = 201,
        S_202_Accepted = 202,
        S_203_NonAuthoritativeInformation = 203,
        S_204_NoContent = 204,
        S_205_ResetContent = 205,
        S_206_PartialContent = 206,
        S_207_MultiStatus = 207,
        S_400_BadRequest = 400,
        S_401_Unauthorized = 401,
        S_402_PaymentRequired = 402,
        S_403_Forbidden = 403,
        S_404_NotFound = 404,
        S_405_MethodNotAllowed = 405,
        S_406_NotAcceptable = 406,
        S_407_ProxyAuthenticationRequired = 407,
        S_408_RequestTimeout = 408,
        S_500_InternalServerError = 500,
        S_601_SysErrorMsg = 601,
        S_602_SysInfoMsg = 602
    }
}
