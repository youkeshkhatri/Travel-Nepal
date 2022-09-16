﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GhumGham_Nepal.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GhumGham_Nepal.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly ProjectContext _context;

        public RegistrationsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Registrations
        public async Task<IActionResult> Index()
        {
            var LoginStatus = _context.Logins.Any(x => x.LoginStatus == true);

            if(LoginStatus == true)
            {
                return _context.Registrations != null ?
                        View(await _context.Registrations.ToListAsync()) :
                        Problem("Entity set 'ProjectContext.Registrations'  is null.");
            }
            return RedirectToAction("Login", "Login");
            
        }

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var LoginStatus = _context.Logins.Any(x => x.LoginStatus == true);

            if (LoginStatus == true)
            {
                if (id == null || _context.Registrations == null)
                {
                    return NotFound();
                }

                var registration = await _context.Registrations
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (registration == null)
                {
                    return NotFound();
                }

                return View(registration);
            }
            return RedirectToAction("Login", "Login");
        }

        // GET: Registrations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Registrations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Phone,Username,Password,ConfirmPassword")] Registration registration)
        {
            var EmailExist = _context.Registrations.Any(e => e.Email == registration.Email);
            var UsernameExist = _context.Registrations.Any(e => e.Email == registration.Username);

            if (ModelState.IsValid)
            {
                if (EmailExist)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                }
                else if (UsernameExist)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                }
                else
                {
                    _context.Add(registration);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(registration);
        }

        // GET: Registrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var LoginStatus = _context.Logins.Any(x => x.LoginStatus == true);

            if (LoginStatus == true)
            {
                if (id == null || _context.Registrations == null)
                {
                    return NotFound();
                }

                var registration = await _context.Registrations.FindAsync(id);
                if (registration == null)
                {
                    return NotFound();
                }
                return View(registration);
            }
            return RedirectToAction("Login", "Login");
        }

        // POST: Registrations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Phone,Username,Password,ConfirmPassword")] Registration registration)
        {
            if (id != registration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.Id.Value))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(registration);
        }


        // POST: Registrations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Registrations == null)
            {
                return Problem("Entity set 'ProjectContext.Registrations'  is null.");
            }
            var registration = await _context.Registrations.FindAsync(id);
            if (registration != null)
            {
                _context.Registrations.Remove(registration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(int id)
        {
            return (_context.Registrations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
