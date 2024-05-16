namespace Fiona.IDE.Compiler.Tokens;

internal enum TokenType
{
    UsingBegin,
    UsingEnd,
    Using,
    Route,
    Endpoint,
    BodyBegin,
    BodyEnd,
    Comment,
    Class,
    Method,
    ReturnType,
}


internal static  class TokenTypeExtension {

    public static string GetTokenKeyword(this TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.UsingBegin => "usingBegin",
            TokenType.UsingEnd => "usingEnd",
            TokenType.Using => "using",
            TokenType.Route => "route:",
            TokenType.Endpoint => "endpoint:",
            TokenType.BodyBegin => "bodyBegin",
            TokenType.BodyEnd => "bodyEnd",
            TokenType.Comment => "//",
            TokenType.Class => "class",
            TokenType.Method => "method:",
            TokenType.ReturnType => "return:",
            _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
        };
    }
}