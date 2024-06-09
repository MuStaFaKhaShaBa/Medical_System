﻿using Medical.Models;
using Medical.Repositories;
using Medical.Specifications;
using Medical.Specifications.ApplicationUser_;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DoctorsController : Controller
    {
        private readonly ApplicationUserRepo _userRepo;

        public DoctorsController(ApplicationUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ActionResult> Index(BaseGlobalSpecs<ApplicationUserNavigations, ApplicationUserSearch> specs)
        {
            specs.Search ??= new() { UserRole = UserRole.Doctor };

            var specification = new ApplicationUserSpecifications(specs);

            var entities = await _userRepo.GetAllAsync(specification, specs.Pagination);
            return View(entities);
        }
    }
}
