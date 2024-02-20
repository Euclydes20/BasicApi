namespace Api.Auxiliary
{
    public class UserAgentInfo
    {
        public string Browser { get; private set; } = string.Empty;
        public string BrowserVersion { get; private set; } = string.Empty;
        public string OS { get; private set; } = string.Empty;
        public bool AmbientIdentified { get; private set; } = false;

        public UserAgentInfo(string userAgent)
        {
            DefineValues(userAgent);
        }

        public override string ToString()
        {
            if (!AmbientIdentified)
                return string.Empty;

            return $"{Browser} {BrowserVersion} | {OS}";
        }

        private void DefineValues(string userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
            {
                AmbientIdentified = false;
                return;
            }

            (string, string) browserInfo = GetBrowserInfo(userAgent);
            if (string.IsNullOrWhiteSpace(browserInfo.Item1) || string.IsNullOrWhiteSpace(browserInfo.Item2))
            {
                AmbientIdentified = false;
                return;
            }

            string os = GetOSInfo(userAgent);
            if (string.IsNullOrWhiteSpace(os))
            {
                AmbientIdentified = false;
                return;
            }

            Browser = browserInfo.Item1;
            BrowserVersion = browserInfo.Item2;
            OS = os;
            AmbientIdentified = true;
        }

        private static readonly string _browserEdgeOldIdentifier = "Edge";
        private static readonly string[] _browserEdgeOldIdentifierKeys = ["Edge/"];
        private static readonly string _browserEdgeOldName = "Edge Old";
        private static readonly string _browserEdgeIdentifier = "Edg";
        private static readonly string[] _browserEdgeIdentifierKeys = ["Edg/"];
        private static readonly string _browserEdgeName = "Edge";
        private static readonly string _browserChromeIdentifier = "Chrome";
        private static readonly string[] _browserChromeIdentifierKeys = ["Chrome/"];
        private static readonly string _browserChromeName = "Google Chrome";
        private static readonly string _browserSafariIdentifier = "Safari";
        private static readonly string[] _browserSafariIdentifierKeys = ["Safari/"];
        private static readonly string _browserSafariName = "Safari";
        private static readonly string _browserFirefoxIdentifier = "Firefox";
        private static readonly string[] _browserFirefoxIdentifierKeys = ["Firefox/"];
        private static readonly string _browserFirefoxName = "Mozilla Firefox";
        private static readonly string _browserIE11Identifier = "rv";
        private static readonly string[] _browserIE11IdentifierKeys = ["rv:"];
        private static readonly string _browserIE11Name = "Internet Explorer 11";
        private static readonly string _browserIE6_10Identifier = "MSIE";
        private static readonly string[] _browserIE6_10IdentifierKeys = ["MSIE"];
        private static readonly string _browserIE6_10Name = "Internet Explorer 6";
        private static readonly string _browserOtherIdentifier = "Other";
        private static readonly string[] _browserOtherIdentifierKeys = ["Other"];
        private static readonly string _browserOtherName = "Other";

        private static (string, string) GetBrowserInfo(string userAgent)
        {
            string getVersion(string[] identifierKeys)
                => userAgent.Split(identifierKeys, StringSplitOptions.None).GetValue(1)?.ToString()?.Split(" ").GetValue(0)?.ToString() ?? string.Empty;

            //Edge - Old
            if (userAgent.Contains(_browserEdgeOldIdentifier, StringComparison.OrdinalIgnoreCase))
                return (_browserEdgeOldName, getVersion(_browserEdgeOldIdentifierKeys));

            //Edge
            if (userAgent.Contains(_browserEdgeIdentifier, StringComparison.OrdinalIgnoreCase))
                return (_browserEdgeName, getVersion(_browserEdgeIdentifierKeys));

            //Chrome
            if (userAgent.Contains(_browserChromeIdentifier, StringComparison.OrdinalIgnoreCase))
                return (_browserChromeName, getVersion(_browserChromeIdentifierKeys));

            //Safari
            if (userAgent.Contains(_browserSafariIdentifier, StringComparison.OrdinalIgnoreCase))
                return (_browserSafariName, getVersion(_browserSafariIdentifierKeys));

            //Firefox
            if (userAgent.Contains(_browserFirefoxIdentifier, StringComparison.OrdinalIgnoreCase))
                return (_browserFirefoxName, getVersion(_browserFirefoxIdentifierKeys));

            //IE11
            if (userAgent.Contains(_browserIE11Identifier, StringComparison.OrdinalIgnoreCase))
                return (_browserIE11Name, getVersion(_browserIE11IdentifierKeys));

            //IE6-10
            if (userAgent.Contains(_browserIE6_10Identifier, StringComparison.OrdinalIgnoreCase))
                return (_browserIE6_10Name, getVersion(_browserIE6_10IdentifierKeys));

            //Other
            if (userAgent.Contains(_browserOtherIdentifier, StringComparison.OrdinalIgnoreCase))
                return (_browserOtherName, getVersion(_browserOtherIdentifierKeys));

            return (string.Empty, string.Empty);
        }

        private static readonly string _osWindowsNTIdentifier = "Windows NT";
        private static readonly string _osWindowsPhoneIdentifier = "Windows Phone";
        private static readonly string _osWindows64Identifier = "Win64";
        private static readonly string _osWindows32Identifier = "Win32";
        private static readonly string _osMacIdentifier = "Macintosh";
        private static readonly string _osAndroidIdentifier = "Android";
        private static readonly string _osIPhoneIdentifier = "iPhone";
        private static readonly string _osIPadIdentifier = "iPad";
        private static readonly Dictionary<string, string> _osIdentifierDescription = new()
        {
            [_osWindowsPhoneIdentifier] = "Windows Phone",
            [_osWindows64Identifier] = "Windows 64-bit",
            [_osWindows32Identifier] = "Windows 32-bit",
            [_osMacIdentifier] = "MacOS",
            [_osAndroidIdentifier] = "Android",
            [_osIPhoneIdentifier] = "IOS (iPhone)",
            [_osIPadIdentifier] = "IOS (iPad)",
        };

        private static string GetOSInfo(string userAgent)
        {
            // Windows
            if (userAgent.Contains(_osWindowsNTIdentifier, StringComparison.OrdinalIgnoreCase))
            {
                // Windows Phone
                if (userAgent.Contains(_osWindowsPhoneIdentifier, StringComparison.OrdinalIgnoreCase))
                    return _osIdentifierDescription[_osWindowsPhoneIdentifier];

                // Windows 64
                if (userAgent.Contains(_osWindows64Identifier, StringComparison.OrdinalIgnoreCase))
                    return _osIdentifierDescription[_osWindows64Identifier];

                // Windows 32
                else
                    return _osIdentifierDescription[_osWindows32Identifier];
            }

            // Mac
            if (userAgent.Contains(_osMacIdentifier, StringComparison.OrdinalIgnoreCase))
                return _osIdentifierDescription[_osMacIdentifier];

            // Android
            if (userAgent.Contains(_osAndroidIdentifier, StringComparison.OrdinalIgnoreCase))
                return _osIdentifierDescription[_osAndroidIdentifier];

            // IOS - IPhone
            if (userAgent.Contains(_osIPhoneIdentifier, StringComparison.OrdinalIgnoreCase))
                return _osIdentifierDescription[_osIPhoneIdentifier];

            // IOS - IPad
            if (userAgent.Contains(_osIPadIdentifier, StringComparison.OrdinalIgnoreCase))
                return _osIdentifierDescription[_osIPadIdentifier];

            return string.Empty;
        }
    }
}
