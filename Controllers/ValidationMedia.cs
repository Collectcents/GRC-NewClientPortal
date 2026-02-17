using GRC_NewClientPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace GRC_NewClientPortal.Controllers
{
    public class ValidationMediaController : Controller
    {
        private static readonly HashSet<string> _permittedExtensions =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ".xls",".xlt",".xlm",".xlsx",".xlsm",
                ".xltx",".xltm",".xlsb",".xla",".xlam",".xll",".xlw",
                ".txt",".zip",".pdf",".csv",".doc",".docx"
            };

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ValidationMediaViewModel());
        }
        private void ValidateUpload(IFormFile? file, string fieldName, bool required)
        {
            // If nothing selected
            if (file == null || file.Length == 0)
            {
                if (required)
                {
                    ModelState.AddModelError(fieldName,
                        "Please provide the file containing validation media.");
                }
                return; 
            }

            var fileName = file.FileName ?? "";
            var ext = Path.GetExtension(fileName);

            if (string.IsNullOrWhiteSpace(ext) || !_permittedExtensions.Contains(ext))
            {
                ModelState.AddModelError(fieldName,
                    $"Invalid file format '{ext}'. Allowed formats: Excel, PDF, CSV, Word, ZIP, TXT.");
                return;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ValidationMediaViewModel model)
        {
            Console.WriteLine("Inside post method");
            model.ContactEmail = (model.ContactEmail ?? "").Trim();
            model.ContactPhone = model.ContactPhone?.Trim();

            if (!string.IsNullOrWhiteSpace(model.ContactPhone))
            {
                var phoneDigits = new string(model.ContactPhone.Where(char.IsDigit).ToArray());

                if (phoneDigits.Length != 10)
                {
                    ModelState.AddModelError(nameof(model.ContactPhone),
                        "Phone number must be exactly 10 digits (e.g., 555-444-1234).");
                }
            }

            if (string.IsNullOrWhiteSpace(model.ContactEmail))
            {
                ModelState.AddModelError(nameof(model.ContactEmail),
                    "A valid Contact E-mail is required");
            }

          
            ValidateUpload(model.ValidationMedia1, nameof(model.ValidationMedia1), required: true);
            ValidateUpload(model.ValidationMedia2, nameof(model.ValidationMedia2), required: false);
            ValidateUpload(model.ValidationMedia3, nameof(model.ValidationMedia3), required: false);

            Console.WriteLine("ModelState.IsValid = " + ModelState.IsValid);

            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Where(m => m.Value.Errors.Count > 0))
                    Console.WriteLine($"{e.Key}: {string.Join(" | ", e.Value.Errors.Select(x => x.ErrorMessage))}");

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    Response.StatusCode = 400;
                    return PartialView("_ValidationMediaForm", model);
                }

                return View(model);
            }

            // Save/process files here...

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { ok = true, redirectUrl = Url.Action(nameof(Success)) });

            Console.WriteLine("Just before redirect to Success");
            return RedirectToAction(nameof(Success));
        }
       

        [HttpGet]
        public IActionResult Success()
        {
            Console.WriteLine("Successful submission");
            return View();
        }
    }
}

//[HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Index(ValidationMediaViewModel model)
//        {
//            if (model.ValidationMedia1 == null || model.ValidationMedia1.Length == 0)
//            {
//                ModelState.AddModelError(nameof(model.ValidationMedia1),
//                    "Please provide the file containing validation media.");
//            }
//            else
//            {
//                var fileName = model.ValidationMedia1.FileName ?? "";
//                var ext = Path.GetExtension(fileName);

//                if (string.IsNullOrWhiteSpace(ext) || !_permittedExtensions.Contains(ext))
//                {
//                    ModelState.AddModelError(nameof(model.ValidationMedia1),
//                        $"Invalid file format '{ext}'. Allowed formats: Excel, PDF, CSV, Word, ZIP, TXT.");
//                }

              
//            }

//            if (!ModelState.IsValid)
//            {
//                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
//                    return PartialView("_ValidationMediaForm", model);

//                return View(model);
//            }

//            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
//                return Json(new { ok = true, redirectUrl = Url.Action(nameof(Success)) });

//            return RedirectToAction(nameof(Success));
//        }

//        [HttpGet]
//        public IActionResult Success()
//        {
//            return View();
//        }
//    }

