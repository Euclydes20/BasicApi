namespace Api.Auxiliary
{
    public enum ResponseCode
    {
        Success = 0,
        UncknownError,
        InvalidCredencials,
        Need2FAAuthentication,
        Invalid2FACode,
    }
}
