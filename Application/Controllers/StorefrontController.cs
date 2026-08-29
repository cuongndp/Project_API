using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

public class StorefrontController : Microsoft.AspNetCore.Mvc.Controller
{
    [HttpGet]
    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult Monitors() => View();

    [HttpGet]
    public IActionResult Headphones() => View();

    [HttpGet]
    public IActionResult Mice() => View();

    [HttpGet]
    public IActionResult Keyboards() => View();

    [HttpGet]
    public IActionResult BudgetPcs() => View();

    [HttpGet]
    public IActionResult OfficePcs() => View();

    [HttpGet]
    public IActionResult UsedComputers() => View();

    [HttpGet]
    public IActionResult ProductDetails(int? id) => View();

    [HttpGet]
    public IActionResult Register() => View();

    [HttpGet]
    public IActionResult Login() => View();

    [HttpGet]
    public IActionResult ForgotPassword() => View();
}