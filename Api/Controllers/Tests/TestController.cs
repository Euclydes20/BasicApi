using Api.Auxiliary;
using Api.Domain.Tests;
using Api.Models;
using Api.Models.Exceptions;
using Api.Security;
using DeviceDetectorNET;
using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCSharp.HttpUserAgentParser;
using MyCSharp.HttpUserAgentParser.Providers;
using Shyjus.BrowserDetection;
using System.Net;
using UAParser;

namespace Api.Controllers.Tests
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]")]
    [Tags("Tests")]
    public class TestController(ITestService testService, TwoFactorAuthenticator twoFactorAuthenticator, IBrowserDetector browserDetector, IHttpUserAgentParserProvider httpUserAgentParserProvider) : ControllerBase
    {
        private readonly ITestService _testService = testService;
        private readonly TwoFactorAuthenticator _twoFactorAuthenticator = twoFactorAuthenticator;
        private readonly IBrowserDetector _browserDetector = browserDetector;
        private readonly IHttpUserAgentParserProvider _httpUserAgentParserProvider = httpUserAgentParserProvider;

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("DeleteAllWithEF")]
        public async Task<IActionResult> DeleteAllWithEFAsync()
        {
            var response = new ResponseInfo<int>();
            try
            {
                response.Data = await _testService.DeleteAllWithEFAsync();
                response.Message = $"Deleted {response.Data} registers.";
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("DeleteAllWithLQ")]
        public async Task<IActionResult> DeleteAllWithLQAsync()
        {
            var response = new ResponseInfo<int>();
            try
            {
                response.Data = await _testService.DeleteAllWithLQAsync();
                response.Message = $"Deleted {response.Data} registers.";

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("AddWithEF")]
        public async Task<IActionResult> AddWithEFAsync(Test test)
        {
            var response = new ResponseInfo<Test>();
            try
            {
                response.Data = await _testService.AddWithEFAsync(test);

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("AddWithLQ")]
        public async Task<IActionResult> AddWithLQAsync(Test test)
        {
            var response = new ResponseInfo<Test>();
            try
            {
                response.Data = await _testService.AddWithLQAsync(test);

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("DeleteWithEF/{testId}")]
        public async Task<IActionResult> DeleteWithEFAsync(int testId)
        {
            var response = new ResponseInfo<object>();
            try
            {
                await _testService.DeleteWithEFAsync(testId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("DeleteWithLQ/{testId}")]
        public async Task<IActionResult> DeleteWithLQAsync(int testId)
        {
            var response = new ResponseInfo<object>();
            try
            {
                await _testService.DeleteWithLQAsync(testId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("AddWithEF/{quantity}/{multipleAdd?}")]
        public async Task<IActionResult> AddRandomWithEFAsync(int quantity, bool multipleAdd = false)
        {
            var response = new ResponseInfo<IList<Test>>();
            try
            {
                response.Message = "50 primeiros e últimos registros adicionados.";
                response.Data = await _testService.AddRandomWithEFAsync(quantity, multipleAdd);
                response.Data = response.Data.Take(50).Concat(response.Data.TakeLast(quantity - 50)).ToList();

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("AddWithLQ/{quantity}/{multipleAdd?}")]
        public async Task<IActionResult> AddRandomWithLQAsync(int quantity, bool multipleAdd = false)
        {
            var response = new ResponseInfo<IList<Test>>();
            try
            {
                response.Message = "50 primeiros e últimos registros adicionados.";
                response.Data = await _testService.AddRandomWithLQAsync(quantity, multipleAdd);
                response.Data = response.Data.Take(50).Concat(response.Data.TakeLast(quantity - 50)).ToList();

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("GenerateQRCodeGoogleTFA/{email}")]
        public async Task<IActionResult> GenerateQRCodeGoogleTFA(string email)
        {
            var response = new ResponseInfo<string>();
            try
            {
                string key = Guid.NewGuid().ToString().Replace("-", "")[..10];
                SetupCode setupInfo = _twoFactorAuthenticator.GenerateSetupCode("Garagem do Código (2FA)", email, key, false, 3);

                response.Message = $"Secret Key: {key}.\nCopy the Base64 QRCode and past in browser to view.";
                response.Data = setupInfo.QrCodeSetupImageUrl;

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpPost]
        [Route("ValidateCodeGoogleTFA/{code}/{key}")]
        public async Task<IActionResult> ValidateCodeGoogleTFA(string code, string key)
        {
            var response = new ResponseInfo<bool>();
            try
            {
                response.Data = _twoFactorAuthenticator.ValidateTwoFactorPIN(key, code);
                response.Message = response.Data ? "Valid." : "Invalid.";

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile()
        {
            var response = new ResponseInfo<byte[]>();
            try
            {
                //File(new FileStream(@"C:\Users\Euclydes\Documents\GerenciadorDailys\2024\02\09_02_2024.txt", FileMode.Open, FileAccess.Read), "application/octet-stream");
                //response.Data = File(await new StreamContent(new FileStream(@"C:\Users\Euclydes\Documents\GerenciadorDailys\2024\02\09_02_2024.txt", FileMode.Open, FileAccess.Read)).ReadAsByteArrayAsync(), "application/octet-stream");

                //response.Data = new FileStreamResult(new FileStream(@"C:\Users\Euclydes\Documents\GerenciadorDailys\2024\02\09_02_2024.txt", FileMode.Open, FileAccess.Read), "application/octet-stream");

                //using (var stream = new FileStream(@"C:\Users\Euclydes\Documents\GerenciadorDailys\2024\02\09_02_2024.txt", FileMode.Open, FileAccess.Read))
                //response.Data = new FormFile(stream, 0, stream.Length, string.Empty, $"{Path.GetFileNameWithoutExtension(stream.Name)}");

                //response.Data = new ByteArrayContent(System.IO.File.ReadAllBytes(@"C:\Users\Euclydes\Documents\GerenciadorDailys\2024\02\09_02_2024.txt"));
                //return Ok(response);

                return File(System.IO.File.ReadAllBytes(@"C:\Users\Euclydes\Documents\Projetos Programacao\Visual Studio\GerenciadorDeDailys\GerenciadorDeDailys\bin\Debug\GerenciadorDeDailys.exe"), "application/octet-stream");//Ok(response);

                // @"C:\Users\Euclydes\Documents\Projetos Programacao\Visual Studio\GerenciadorDeDailys\GerenciadorDeDailys\bin\Debug\GerenciadorDeDailys.exe"
                // @"C:\Users\Euclydes\Documents\GerenciadorDailys\2024\02\09_02_2024.txt"

                /*using (var sr = new StreamReader(@"C:\Users\Euclydes\Documents\Projetos Programacao\Visual Studio\GerenciadorDeDailys\GerenciadorDeDailys\bin\Debug\GerenciadorDeDailys.exe"))
                {
                    string text = sr.ReadToEnd();
                    response.Data = Encoding.Latin1.GetBytes(text);
                }*/
                //response.Data = System.IO.File.ReadAllBytes(@"C:\Users\Euclydes\Documents\Projetos Programacao\Visual Studio\GerenciadorDeDailys\GerenciadorDeDailys\bin\Debug\GerenciadorDeDailys.exe");
                //response.Data = System.IO.File.ReadAllText(@"C:\Users\Euclydes\Documents\GerenciadorDailys\2024\02\09_02_2024.txt", Encoding.Latin1);

                //return Ok(response);

                //return File(new FileStream(@"C:\Users\Euclydes\Documents\GerenciadorDailys\2024\02\09_02_2024.txt", FileMode.Open, FileAccess.Read), "application/octet-stream", "arquivo.txt");
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }
        
        [AllowAnonymous]
        [Authorization(true)]
        [HttpGet]
        [Route("SomeTest")]
        public async Task<IActionResult> SomeTest()
        {
            var response = new ResponseInfo();
            try
            {
                string? removeIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                    ?? throw new ResponseException("User connection address not identified.");

                string? userAgent = HttpContext.Request.Headers.UserAgent.ToString() 
                    ?? throw new ResponseException("User agent not identified.");

                UserAgentInfo userAgentInfo = new(userAgent);
                if (!userAgentInfo.AmbientIdentified)
                    throw new ResponseException("Request ambient not identified.");

                string ambientInfo = $"{removeIpAddress} -> {userAgentInfo}";

                response.Success = true;
                response.Message = ambientInfo;

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpGet]
        [Route("DeviceDetectorNET")]
        public async Task<IActionResult> DeviceDetectorNET()
        {
            var response = new ResponseInfo<object>();
            try
            {
                string? userAgent = HttpContext.Request.Headers.UserAgent.ToString()
                    ?? throw new ResponseException("User agent not identified.");
                
                DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

                //Dictionary<string, string?> headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
                //ClientHints clientHints = ClientHints.Factory(headers);
                // ESTÁ CAUSANDO IDENTIFICAÇÃO INCORRETA DO BROWSER, INFORMANDO 'Iridium'
                DeviceDetector deviceDetector = new(userAgent/*, clientHints*/);

                deviceDetector.SetCache(new DictionaryCache());

                deviceDetector.DiscardBotInformation();

                //deviceDetector.SkipBotDetection();

                deviceDetector.Parse();

                if (deviceDetector.IsBot())
                {
                    // handle bots,spiders,crawlers,...
                    var botInfo = deviceDetector.GetBot().Match;
                    
                    response.Success = true;
                    response.Message = "Bot detected.";

                    response.Data = new
                    {
                        botInfo.Name,
                        botInfo.Url,
                        botInfo.Producer,
                        botInfo.Category,
                    };
                }
                else
                {
                    ClientMatchResult clientInfo = deviceDetector.GetClient().Match ?? new(); // holds information about browser, feed reader, media player, ...
                    OsMatchResult osInfo = deviceDetector.GetOs().Match ?? new();
                    string device = deviceDetector.GetDeviceName() ?? string.Empty;
                    string brand = deviceDetector.GetBrandName() ?? string.Empty;
                    string model = deviceDetector.GetModel() ?? string.Empty;

                    response.Success = true;

                    var mountedObject = new
                    {
                        Client = new
                        {
                            clientInfo.Name,
                            clientInfo.Version,
                            clientInfo.Type,
                        },
                        OS = new
                        {
                            osInfo.Name,
                            osInfo.Version,
                            osInfo.Platform,
                            osInfo.Family,
                            osInfo.ShortName,
                        },
                        Device = device,
                        Brand = brand,
                        Model = model,
                    };

                    response.Data = mountedObject;
                    /*int hash1 = response.Data.GetHashCode();
                    response.Data = mountedObject with
                    {
                        Brand = "POCO",
                    };
                    int hash2 = response.Data.GetHashCode();*/

                    response.Message = $"Result hash: {response.Data.GetHashCode()}";
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }
        
        [AllowAnonymous]
        [Authorization(true)]
        [HttpGet]
        [Route("UAParser")]
        public async Task<IActionResult> UAParser()
        {
            var response = new ResponseInfo<object>();
            try
            {
                string? userAgent = HttpContext.Request.Headers.UserAgent.ToString()
                    ?? throw new ResponseException("User agent not identified.");

                // get a parser with the embedded regex patterns
                Parser parser = Parser.GetDefault();

                // get a parser using externally supplied yaml definitions
                // var uaParser = Parser.FromYaml(yamlString);

                ClientInfo clientInfo = parser.Parse(userAgent);
                
                response.Success = true;

                var mountedObject = new
                {
                    Client = new
                    {
                        clientInfo.UA.Family,
                        clientInfo.UA.Major,
                        clientInfo.UA.Minor,
                        clientInfo.UA.Patch,
                    },
                    OS = new
                    {
                        clientInfo.OS.Family,
                        clientInfo.OS.Major,
                        clientInfo.OS.Minor,
                        clientInfo.OS.Patch,
                        clientInfo.OS.PatchMinor,
                    },
                    Device = new
                    {
                        clientInfo.Device.Family,
                        clientInfo.Device.Model,
                        clientInfo.Device.Brand,
                        clientInfo.Device.IsSpider,
                    }
                };

                response.Data = mountedObject;

                response.Message = $"Result hash: {response.Data.GetHashCode()}";

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }

        [AllowAnonymous]
        [Authorization(true)]
        [HttpGet]
        [Route("BrowserDetector")]
        public async Task<IActionResult> BrowserDetector()
        {
            var response = new ResponseInfo<object>();
            try
            {
                string? userAgent = HttpContext.Request.Headers.UserAgent.ToString()
                    ?? throw new ResponseException("User agent not identified.");

                IBrowser browser = _browserDetector.Browser
                    ?? throw new ResponseException("Request ambient not identified.");

                response.Success = true;

                var mountedObject = new
                {
                    browser.Name,
                    browser.Version,
                    browser.OS,
                    browser.DeviceType,
                };

                response.Data = mountedObject;

                response.Message = $"Result hash: {response.Data.GetHashCode()}";

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }
        
        [AllowAnonymous]
        [Authorization(true)]
        [HttpGet]
        [Route("HttpUserAgentParser")]
        public async Task<IActionResult> HttpUserAgentParser()
        {
            var response = new ResponseInfo<object>();
            try
            {
                string? userAgent = HttpContext.Request.Headers.UserAgent.ToString()
                    ?? throw new ResponseException("User agent not identified.");

                HttpUserAgentInformation information = _httpUserAgentParserProvider.Parse(userAgent);

                response.Success = true;

                var mountedObject = new
                {
                    information.Name,
                    information.Version,
                    information.Type,
                    Platform = new
                    {
                        information.Platform?.Name,
                        information.Platform?.PlatformType,
                    },
                    information.MobileDeviceType,
                };

                response.Data = mountedObject;

                response.Message = $"Result hash: {response.Data.GetHashCode()}";

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }
    }
}
