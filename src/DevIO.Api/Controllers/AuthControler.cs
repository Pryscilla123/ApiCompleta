using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
    [Route("api/auth")]
    public class AuthControler : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthControler(INotificador notificador,
                             SignInManager<IdentityUser> signInManager,
                             UserManager<IdentityUser> userManager) : base(notificador)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserViewModel registerUserViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUserViewModel.Email,
                Email = registerUserViewModel.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                return CustomResponse(ModelState);
            }

            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }

            return CustomResponse(registerUserViewModel);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserViewModel loginUserViewModel)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUserViewModel.Email, loginUserViewModel.Password, 
                                                                  isPersistent: false, lockoutOnFailure: true);

            if(result.Succeeded)
            {
                return CustomResponse(loginUserViewModel);
            }

            if(result.IsLockedOut)
            {
                NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas.");
                return CustomResponse(loginUserViewModel);
            }

            NotificarErro("Usuário ou senha inválidos.");
            return CustomResponse(loginUserViewModel);
        }
    }
}
