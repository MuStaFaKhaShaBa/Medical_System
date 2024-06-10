using Medical.Models;
using Medical.Repositories;
using Medical.Specifications;
using Medical.Specifications.ApplicationUser_;
using Medical.Specifications.PatientMedical_;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PatientsController : Controller
    {
        private readonly ApplicationUserRepo _userRepo;
        private readonly PatientMedicalRepo _patientMedicalRepo;

        public PatientsController(ApplicationUserRepo userRepo, PatientMedicalRepo patientMedicalRepo)
        {
            _userRepo = userRepo;
            _patientMedicalRepo = patientMedicalRepo;
        }

        public async Task<ActionResult> Index(BaseGlobalSpecs<ApplicationUserNavigations, ApplicationUserSearch> specs)
        {
            specs.Search ??= new() { UserRole = UserRole.Patient };

            var specification = new ApplicationUserSpecifications(specs);

            var entities = await _userRepo.GetAllAsync(specification, specs.Pagination);
            return View(entities);
        }

        public async Task<ActionResult> Profile(int id)
        {
            var patient = await _userRepo.GetByIdAsync(id);

            if (patient == null) { NotFound(); }

            var patientModel = new ApplicationUserVM(patient);

            return View(patientModel);
        }
    }
}
