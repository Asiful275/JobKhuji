
using JobKhuji.DataAccess;
using JobKhuji.Entities.Models;
using JobKhuji.Framework.Constants;
using JobKhuji.Web.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobKhuji.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // =====================================
        // GET: /Account/Register
        // =====================================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterViewModel());
        }

        // =====================================
        // POST: /Account/Register
        // =====================================

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            // Validate form fields
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Only JobSeeker and Employer can register.
            // Admin registration is not allowed.
            if (model.Role != RoleNames.JobSeeker &&
                model.Role != RoleNames.Employer)
            {
                ModelState.AddModelError(
                    nameof(model.Role),
                    "Invalid account type.");

                return View(model);
            }

            // Employer must provide company name
            if (model.Role == RoleNames.Employer)
            {
                if (string.IsNullOrWhiteSpace(model.CompanyName))
                {
                    ModelState.AddModelError(
                        nameof(model.CompanyName),
                        "Company name is required for employers.");

                    return View(model);
                }
            }

            // Validate email
            string email = model.Email.Trim();

            var existingUser =
                await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "This email is already registered.");

                return View(model);
            }

            // Begin database transaction
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            // Create Identity User
            var user = new ApplicationUser
            {
                FullName = model.FullName.Trim(),
                UserName = email,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(
                user,
                model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                await transaction.RollbackAsync();
                return View(model);
            }

            // Assign role to new user
            var roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    model.Role);

            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                await transaction.RollbackAsync();
                return View(model);
            }

            // =====================================
            // CREATE JOB SEEKER PROFILE
            // =====================================

            if (model.Role == RoleNames.JobSeeker)
            {
                var profile = new JobSeekerProfile
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow
                };

                _context.JobSeekerProfiles.Add(profile);
            }

            // =====================================
            // CREATE EMPLOYER PROFILE
            // =====================================

            else if (model.Role == RoleNames.Employer)
            {
                var profile = new EmployerProfile
                {
                    UserId = user.Id,
                    CompanyName = model.CompanyName!.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                _context.EmployerProfiles.Add(profile);
            }

            // Save profile to database
            await _context.SaveChangesAsync();

            // Commit transaction
            await transaction.CommitAsync();

            TempData["SuccessMessage"] =
                "Registration successful. Please log in.";

            return RedirectToAction(nameof(Login));
        }

        // =====================================
        // GET: /Account/Login
        // =====================================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View(new LoginViewModel());
        }

        // =====================================
        // POST: /Account/Login
        // =====================================

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(
                model.Email.Trim());

            if (user == null || !user.IsActive)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid login attempt.");

                return View(model);
            }

            var result =
                await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // Prevent unsafe external redirects
                if (!string.IsNullOrEmpty(returnUrl) &&
                    Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your account is temporarily locked.");
            }
            else if (result.RequiresTwoFactor)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Two-factor authentication is required.");
            }
            else
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid login attempt.");
            }

            return View(model);
        }

        // =====================================
        // POST: /Account/Logout
        // =====================================

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        // =====================================
        // GET: /Account/AccessDenied
        // =====================================

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
