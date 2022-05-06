using System.Security.Cryptography;
using System.Text;
using CryptoP2P.Backend.Data;
using CryptoP2P.Backend.Models;
using CryptoP2P.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoP2P.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  private readonly AppDbContext _ctx;
  private readonly ICryptoVault _cryptoVault;
  public UserController(AppDbContext ctx, ICryptoVault cryptoVault)
  {
    _ctx = ctx;
    _cryptoVault = cryptoVault;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody]RegisterForm registerForm)
  {
    var userExists = _ctx.Users.Any(x => x.Id == registerForm.UserName);
    if(userExists)
      return BadRequest("User already exists");
    if(registerForm.Password != registerForm.PasswordConfirm)
      return BadRequest("Passwords do not match");
    
    User user = new User
    {
      Id = registerForm.UserName
    };

    //  Calculate SHA256 for Private Key Encryption
    using var sha256 = SHA256.Create();
    var passwordHashSha256 = sha256.ComputeHash(Encoding.UTF8.GetBytes(registerForm.Password));

    //  Generate RSA keypair for new user
    using var rsa = RSA.Create();
    //  Generate AES and save encrypted Private Key to user
    using var aes = Aes.Create();

    aes.Key = passwordHashSha256;
    aes.Padding = PaddingMode.PKCS7;
    user.IV = aes.IV;

    var encryptedPrivateKey = aes.EncryptCfb(rsa.ExportRSAPrivateKey(), aes.IV);
    user.EncryptedPrivateKey = encryptedPrivateKey;
    user.PublicKey = rsa.ExportRSAPublicKey();

    await _ctx.Users.AddAsync(user);
    await _ctx.SaveChangesAsync();
    
    return Ok();
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginForm loginForm)
  {
    using var sha512 = SHA512.Create();
    var passwordHash = sha512.ComputeHash(Encoding.UTF8.GetBytes(loginForm.Password));
    
    var userExists = _ctx.Users.Any(x => x.Id == loginForm.Username);
    if(!userExists)
      return BadRequest("Bad credentials given");

    //  User exists, get his RSA pair, decrypt private key and store in ICryptoVault

    using var sha256 = SHA256.Create();
    var privateKeyPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(loginForm.Password));

    var user = _ctx.Users.First(x =>  x.Id == loginForm.Username);
    using var aes = Aes.Create();
    aes.Padding = PaddingMode.PKCS7;
    aes.IV = user.IV;
    aes.Key = privateKeyPassword;

    _cryptoVault.SavePublicKey(user.PublicKey);

    var privateKey = aes.DecryptCfb(user.EncryptedPrivateKey, aes.IV); 
    _cryptoVault.SavePrivateKey(privateKey);
    return Ok();
  }
}