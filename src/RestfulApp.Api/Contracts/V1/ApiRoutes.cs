namespace RestfulApp.Api.Contracts.V1;

public static class ApiRoutes
{
    public const string Version = "v1";
    public const string Root = "api";
    public const string Base = $"{Root}/{Version}";
    public static class Items
    {
        public const string ItemId = "{itemId}";

        public const string GetAll = $"{Base}/items";
        public const string Get = $"{Base}/items/{ItemId}";
        public const string Create = $"{Base}/items";
        public const string Update = $"{Base}/items/{ItemId}";
        public const string Delete = $"{Base}/items/{ItemId}";
    }

    public static class Identity
    {
        public const string Login = $"{Base}/identity/login";
        public const string Register = $"{Base}/identity/register";
        public const string Refresh = $"{Base}/identity/refresh";
    }
}