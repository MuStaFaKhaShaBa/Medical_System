using Medical.Repositories;
using Medical.Specifications.PatientMedical_;
using Medical.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Medical.Models;
using Medical.Data.Entities;
using Medical.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Medical.Controllers
{
    public class PatientMedicalsController : Controller
    {
        private readonly PatientMedicalRepo _patientMedicalRepo;
        private readonly ApplicationUserRepo _applicationUserRepo;
        private readonly IConfiguration _configuration;

        public PatientMedicalsController(PatientMedicalRepo patientMedicalRepo, ApplicationUserRepo applicationUserRepo, IConfiguration configuration)
        {
            _patientMedicalRepo = patientMedicalRepo;
            _applicationUserRepo = applicationUserRepo;
            _configuration = configuration;
        }

        // GET: PatientMedicalsController
        public async Task<ActionResult> Index(int patientId, BaseGlobalSpecs<PatientMedicalNavigations, PatientMedicalSearch>? specs)
        {
            specs ??= new();
            specs.Search ??= new() { PatientId = patientId };
            var specification = new PatientMedicalSpecifications(specs);
            ViewBag.Patient = await _applicationUserRepo.GetByIdAsync(patientId);

            var entities = await _patientMedicalRepo.GetAllAsync(specification, specs.Pagination);
            return View(entities);
        }

        [Authorize(Roles = "Admin,Doctor")]
        // GET: PatientMedicalsController/Create
        public async Task<ActionResult> Create(int patientId)
        {
            var user = await _applicationUserRepo.GetByIdAsync(patientId);
            return View(new PatientMedicalCreateVM() { PatientId = patientId, Patient = user });
        }

        // POST: PatientMedicalsController/Create
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PatientMedicalCreateVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var patientMedical = new PatientMedical
                    {
                        PatientId = model.PatientId,
                        Type = model.Type,
                        Text = model.Text,
                    };

                    if (model.File != null)
                    {
                        var fileName = Path.GetFileName(model.File.FileName);
                        var folderPath = _configuration["reports:upload"];

                        patientMedical.FileName = DocumentSettings.SaveFile(fileName, model.File, folderPath);
                    }

                    await _patientMedicalRepo.AddAsync(patientMedical);

                    if (await _patientMedicalRepo.CommitAsync() > 0)
                        return RedirectToAction(nameof(Index), new {patientId = model.PatientId });

                    ModelState.AddModelError("", "Something Went Wrong");
                }

                return View(model);
            }
            catch
            {
                // Log the error here
                return View(model);
            }
        }


        // GET: PatientMedicalsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PatientMedicalsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatientMedicalsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatientMedicalsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
