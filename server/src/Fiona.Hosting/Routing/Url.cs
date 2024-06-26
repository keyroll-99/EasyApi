using System.Text.RegularExpressions;
using Fiona.Hosting.Routing.Exceptions;

namespace Fiona.Hosting.Routing;

internal sealed partial class Url : IEquatable<Uri>
{
    private const string OpenParameter = "{";
    private const string CloseParameter = "}";
    private const string ParamMark = "{param}";

    private Url(string url)
    {
        NormalizeUrl = NormalizeUrlRegex().Replace(url, ParamMark);
        if (NormalizeUrl.StartsWith("/"))
        {
            NormalizeUrl = NormalizeUrl[1..];
        }
        OriginalUrl = url;
        SplitUrl = url.Split('/');
        IndexesOfParameters = GetIndexesOfParameters();
    }

    public string NormalizeUrl { get; }
    public string OriginalUrl { get; }
    public string[] SplitUrl { get; }
    public IEnumerable<int> IndexesOfParameters { get; }

    public bool Equals(Uri? other)
    {
        string? uri = other?.AbsolutePath;
        if (uri is null) return false;

        string[] splitUrl = other!.AbsolutePath[1..].Split('/');
        foreach (int parameter in IndexesOfParameters) splitUrl[parameter] = ParamMark;

        return NormalizeUrl == string.Join('/', splitUrl);
    }

    public Url GetPartOfUrl(int howManyParts)
    {
        string[] parts = OriginalUrl.Split('/');
        return new Url(string.Join('/', parts[..howManyParts]));
    }

    public static implicit operator Url(string url)
    {
        return new Url(url);
    }

    [GeneratedRegex(@"\{[^{}]*\}")]
    private static partial Regex NormalizeUrlRegex();

    public bool IsSubUrl(Uri uri)
    {
        string[] splitUrl = uri.AbsolutePath[1..].Split('/');
        foreach (int parameter in IndexesOfParameters) splitUrl[parameter] = ParamMark;

        return string.Join('/', splitUrl).StartsWith(NormalizeUrl);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NormalizeUrl, OriginalUrl, SplitUrl, IndexesOfParameters);
    }

    public HashSet<string> GetNameOfUrlParameters()
    {
        HashSet<string> result = [];

        if (!OriginalUrl.Contains(OpenParameter)) return result;

        int offset = 0;
        while (true)
        {
            int indexOfOpen = OriginalUrl.IndexOf(OpenParameter, offset, StringComparison.Ordinal);
            if (indexOfOpen == -1) break;

            int indexOfClose = OriginalUrl.IndexOf(CloseParameter, offset, StringComparison.Ordinal);
            string variableName = OriginalUrl.Substring(indexOfOpen + 1, indexOfClose - indexOfOpen - 1);
            if (!result.Add(variableName)) throw new ConflictNameOfRouteParametersException(OriginalUrl);

            offset = indexOfClose + 1;
        }

        return result;
    }

    private IEnumerable<int> GetIndexesOfParameters()
    {
        List<int> result = [];
        var splitNormalizedUrl = NormalizeUrl.Split("/").ToList();
        for (int index = 0; index < splitNormalizedUrl.Count; index++)
        {
            string url = splitNormalizedUrl[index];
            if (url == ParamMark) result.Add(index);
        }

        return result;
    }
}