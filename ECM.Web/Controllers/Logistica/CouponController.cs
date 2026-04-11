// ==========================================
// ECM.Web.Controllers.Logistica.CouponController.cs
// ==========================================
using ECM.Application.Services.Logistica;
using ECM.Domain.Common.Enums;
using ECM.Web.Models.Logistica;
using Microsoft.AspNetCore.Mvc;

namespace ECM.Web.Controllers.Logistica;

public class CouponController : Controller
{
    private readonly CouponService _couponService;

    public CouponController(CouponService couponService)
    {
        _couponService = couponService;
    }

    // GET: Coupon
    public async Task<IActionResult> Index()
    {
        var result = await _couponService.GetValidCouponsAsync();
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(new List<CouponViewModel>());
        }

        var viewModels = new List<CouponViewModel>();
        if (result.Data != null)
        {
            viewModels = result.Data.Select(c => new CouponViewModel
            {
                Id = c.Id,
                Code = c.Code,
                Type = c.Type,
                Value = c.Value,
                MinimumOrderAmount = c.MinimumOrderAmount,
                ExpirationDate = c.ExpirationDate,
                MaxUsage = c.MaxUsage,
                TimesUsed = c.TimesUsed,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt
            }).ToList();
        }

        return View(viewModels);
    }

    // GET: Coupon/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _couponService.GetCouponByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            return NotFound();
        }

        var coupon = result.Data;
        var viewModel = new CouponViewModel
        {
            Id = coupon.Id,
            Code = coupon.Code,
            Type = coupon.Type,
            Value = coupon.Value,
            MinimumOrderAmount = coupon.MinimumOrderAmount,
            ExpirationDate = coupon.ExpirationDate,
            MaxUsage = coupon.MaxUsage,
            TimesUsed = coupon.TimesUsed,
            IsActive = coupon.IsActive,
            CreatedAt = coupon.CreatedAt
        };

        return View(viewModel);
    }

    // GET: Coupon/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Coupon/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCouponViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _couponService.CreateCouponAsync(
                model.Code,
                model.Type,
                model.Value,
                model.ExpirationDate,
                model.MaxUsage,
                model.MinimumOrderAmount
            );

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            if (result.Message != null) ModelState.AddModelError("", result.Message);
        }

        return View(model);
    }

    // GET: Coupon/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _couponService.GetCouponByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            return NotFound();
        }

        var coupon = result.Data;
        var viewModel = new EditCouponViewModel
        {
            Id = coupon.Id,
            Code = coupon.Code,
            Type = coupon.Type,
            Value = coupon.Value,
            MinimumOrderAmount = coupon.MinimumOrderAmount,
            ExpirationDate = coupon.ExpirationDate,
            MaxUsage = coupon.MaxUsage
        };

        return View(viewModel);
    }

    // POST: Coupon/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditCouponViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _couponService.UpdateCouponAsync(
                id,
                model.Value,
                model.MinimumOrderAmount,
                model.ExpirationDate,
                model.MaxUsage
            );
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Message);
        }

        return View(model);
    }

    // POST: Coupon/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _couponService.DisableCouponAsync(id);
        if (result.Success)
        {
            TempData["SuccessMessage"] = result.Message;
        }
        else
        {
            TempData["ErrorMessage"] = result.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: Coupon/Validate
    public IActionResult Validate()
    {
        return View();
    }

    // POST: Coupon/Validate
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Validate(string code, decimal orderAmount)
    {
        var result = await _couponService.ValidateCouponAsync(code, orderAmount);
        if (result.Success && result.Data != null)
        {
            var discount = result.Data.Type switch
            {
                DiscountType.Percentage => orderAmount * (result.Data.Value / 100),
                DiscountType.FixedAmount => result.Data.Value,
                _ => 0
            };

            TempData["SuccessMessage"] = $"¡Cupón válido! Descuento: {discount:C}";
        }
        else
        {
            TempData["ErrorMessage"] = result.Message;
        }

        return RedirectToAction(nameof(Validate));
    }
}