using Api.Auxiliary;
using Api.Domain.Tests;
using Api.Models;
using Api.Models.Exceptions;
using Api.Security;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Tests
{
    [ApiController]
    [Authorize]
    [Route("v1/[controller]")]
    [Tags("Tests")]
    public class TestController(ITestService testService, TwoFactorAuthenticator twoFactorAuthenticator) : ControllerBase
    {
        private readonly ITestService _testService = testService;
        private readonly TwoFactorAuthenticator _twoFactorAuthenticator = twoFactorAuthenticator;

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
        [Route("SomeTest2")]
        public async Task<IActionResult> SomeTest2()
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

                //deviceDetector.SetCache(new DictionaryCache());

                deviceDetector.DiscardBotInformation();

                deviceDetector.SkipBotDetection();

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
        [Route("SomeTest3")]
        public async Task<IActionResult> SomeTest3()
        {
            var response = new ResponseInfo<string>();
            try
            {
                string? userAgent = HttpContext.Request.Headers.UserAgent.ToString()
                    ?? throw new ResponseException("User agent not identified.");

                BotParser botParser = new();
                botParser.SetUserAgent(userAgent);

                // OPTIONAL: discard bot information. Parse() will then return true instead of information
                botParser.DiscardDetails = true;

                BotMatchResult? result = botParser.Parse().Match;

                if (result is not null)
                {
                    response.Success = true;
                    response.Message = "";

                    response.Data = result.ToString();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Information not identified.";
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Extensions.ResolveResponseException(ex, response));
            }
        }
    }
}
