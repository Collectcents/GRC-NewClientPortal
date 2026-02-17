using System.Text.Json;
using GRC_NewClientPortal.Models;
using Microsoft.AspNetCore.Mvc;

public class ACHController : Controller
{
    private const string TempDataModelKey = "ACH_Model";
    private const string TempDataErrorsKey = "ACH_Errors";

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ACHAuthorizationViewModel());
    }


    //[HttpGet]
    //public IActionResult Index()
    //{
    //    var model = new ACHAuthorizationViewModel();

    //    // Peek so TempData isn't consumed early
    //    var modelJson = TempData.Peek(TempDataModelKey) as string;
    //    var errorsJson = TempData.Peek(TempDataErrorsKey) as string;

    //    if (!string.IsNullOrWhiteSpace(modelJson))
    //    {
    //        model = JsonSerializer.Deserialize<ACHAuthorizationViewModel>(modelJson)
    //                ?? new ACHAuthorizationViewModel();
    //    }

    //    if (!string.IsNullOrWhiteSpace(errorsJson))
    //    {
    //        var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(errorsJson);
    //        if (dict != null)
    //        {
    //            foreach (var kvp in dict)
    //                foreach (var error in kvp.Value)
    //                    ModelState.AddModelError(kvp.Key, error);
    //        }

    //        // Now that we've used them, clear them
    //        TempData.Remove(TempDataModelKey);
    //        TempData.Remove(TempDataErrorsKey);
    //    }

    //    return View(model);
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ACHAuthorizationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // AJAX: return the form HTML only (partial) with ModelState errors
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_AchForm", model);

            // Non-AJAX fallback (optional)
            return View(model);
        }

        // Save / process here

        // AJAX: tell browser where to go next
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(new { ok = true, redirectUrl = Url.Action(nameof(Success)) });

        return RedirectToAction(nameof(Success));
    }


    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public IActionResult Index(ACHAuthorizationViewModel model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        TempData[TempDataModelKey] = JsonSerializer.Serialize(model);

    //        var errors = ModelState
    //            .Where(x => x.Value?.Errors?.Count > 0)
    //            .ToDictionary(
    //                kvp => kvp.Key,
    //                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
    //            );

    //        TempData[TempDataErrorsKey] = JsonSerializer.Serialize(errors);

    //        return RedirectToAction(nameof(Index));
    //    }

    //    // Save/process here

    //    return RedirectToAction(nameof(Success));
    //}

    [HttpGet]
    public IActionResult Success() => View();
}
