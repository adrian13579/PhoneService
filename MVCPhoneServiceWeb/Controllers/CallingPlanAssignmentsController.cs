﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repo;
using MVCPhoneServiceWeb.Utils;

namespace MVCPhoneServiceWeb.Controllers
{
    public class CallingPlanAssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CallingPlanAssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MobilePhoneCallingPlanAssignments
        public async Task<IActionResult> Index(int cpage, string phoneNumber, string month, string year, string callingPlan,
            string pNCheck, string monthCheck, string yearCheck, string cPCheck,
            string page, string next, string previous)
        {
            //
            var _month = (month != null && SD.months.ContainsKey(month)) ? SD.months[month] : -1;
            if (_month == -1)
                _month = Parse.IntTryParse(month);
            var _year = Parse.IntTryParse(year);
            //
            Tuple<bool, string>[] show = SD.Show(new List<string>() { pNCheck, monthCheck, yearCheck, cPCheck }, new List<string>() { phoneNumber, month, year, callingPlan });
            ViewData["columns"] = show;
            //
            var cpa = await _context.CallingPlanAssignments.Include(m => m.CallingPlan).Include(m => m.PhoneLine).ToListAsync();
            List<CallingPlanAssignment> _cpa = cpa.ToList();
            List<CallingPlanAssignment> final_result = new List<CallingPlanAssignment>();

            var phoneNumberList = (phoneNumber != null) ? phoneNumber.Split(", ").ToList() : new List<string>();
            _cpa = DataFilter<CallingPlanAssignment>.Filter(phoneNumberList, (m) => m.PhoneNumber, _cpa).ToList();
            var callingPlanList = (callingPlan != null) ? callingPlan.Split(", ").ToList() : new List<string>();
            _cpa = DataFilter<CallingPlanAssignment>.Filter(callingPlanList, (m) => m.CallingPlanId, _cpa).ToList();
            _cpa = DataFilter<CallingPlanAssignment>.Filter(_month, (m) => m.Month, _cpa).ToList();
            _cpa = DataFilter<CallingPlanAssignment>.Filter(_year, (m) => m.Year, _cpa).ToList();

            //separar en paginas
            _cpa = _cpa.OrderBy(m => m.Year).ThenBy(m=>m.Month).ToList();
            var result = Paging<CallingPlanAssignment>.Pages(_cpa, page, cpage, (next != null), (previous != null));

            ViewData["top"] = result.Item2;
            ViewData["mult"] = result.Item3;
            ViewData["page"] = result.Item4;

            return View(result.Item1);
        }

        // GET: MobilePhoneCallingPlanAssignments/Details/5
        public async Task<IActionResult> Details(string phoneNumber,string month, string year,string callingPlanId)
        {
            if (phoneNumber == null || month==null || year == null || callingPlanId ==null)
            {
                return NotFound();
            }
            //
            var _month = (month != null && SD.months.ContainsKey(month)) ? SD.months[month] : -1;
            if (_month == -1)
                _month = Parse.IntTryParse(month);
            var _year = Parse.IntTryParse(year);
            //
            var callingPlanAssignment = await _context.CallingPlanAssignments
                .Include(m => m.CallingPlan)
                .Include(m => m.PhoneLine)
                .FirstOrDefaultAsync(m => m.PhoneNumber == phoneNumber && m.Month == _month && m.Year == _year && m.CallingPlanId==callingPlanId);
            if (callingPlanAssignment == null)
            {
                return NotFound();
            }

            return View(callingPlanAssignment);
        }

        // GET: MobilePhoneCallingPlanAssignments/Create
        public IActionResult Create()
        {
            ViewData["CallingPlanId"] = new SelectList(_context.CallingPlans, "CallingPlanId", "CallingPlanId");
            ViewData["PhoneNumber"] = new SelectList(_context.PhoneLines, "PhoneNumber", "PhoneNumber");
            //{ "enero", 1 }, { "febrero", 2 }, { "marzo", 3 }, { "abril", 4 }, { "mayo", 5 }, { "junio", 6 }, { "julio", 7 }, 
            //{ "agosto", 8 }, { "septiembre", 9 }, { "octubre", 10 }, { "noviembre", 11 }, { "diciembre", 12 }
            List<MonthSelect> months = new List<MonthSelect>() { new MonthSelect("enero", 1), new MonthSelect("febrero", 2), new MonthSelect("marzo", 3), 
                new MonthSelect("abril", 4), new MonthSelect("mayo", 5), new MonthSelect("junio", 6), new MonthSelect("julio", 7), new MonthSelect("agosto", 8), 
                new MonthSelect("septiembre", 9), new MonthSelect("octubre", 10), new MonthSelect("noviembre", 11), new MonthSelect("diciembre", 12) };
            ViewData["Month"] = new SelectList(months, "MonthNumber", "Name"); 
            return View();
        }

        // POST: MobilePhoneCallingPlanAssignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PhoneNumber,Month,Year,CallingPlanId")] CallingPlanAssignment callingPlanAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(callingPlanAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { pNCheck = "On", cPCheck = "On", monthCheck = "On", yearCheck = "On" });
            }
            ViewData["CallingPlanId"] = new SelectList(_context.CallingPlans, "CallingPlanId", "CallingPlanId", callingPlanAssignment.CallingPlanId);
            ViewData["PhoneNumber"] = new SelectList(_context.PhoneLines, "PhoneNumber", "PhoneNumber", callingPlanAssignment.PhoneNumber);
            List<MonthSelect> months = new List<MonthSelect>() { new MonthSelect("enero", 1), new MonthSelect("febrero", 2), new MonthSelect("marzo", 3),
                new MonthSelect("abril", 4), new MonthSelect("mayo", 5), new MonthSelect("junio", 6), new MonthSelect("julio", 7), new MonthSelect("agosto", 8),
                new MonthSelect("septiembre", 9), new MonthSelect("octubre", 10), new MonthSelect("noviembre", 11), new MonthSelect("diciembre", 12) };
            ViewData["Month"] = new SelectList(months, "MonthNumber", "Name");
            return View(callingPlanAssignment);
        }

        // GET: MobilePhoneCallingPlanAssignments/Edit/5
        public async Task<IActionResult> Edit(string phoneNumber, string month, string year, string callingPlanId)
        {
            if (phoneNumber == null || month == null || year == null || callingPlanId == null)
            {
                return NotFound();
            }
            //
            var _month = (month != null && SD.months.ContainsKey(month)) ? SD.months[month] : -1;
            if (_month == -1)
                _month = Parse.IntTryParse(month);
            var _year = Parse.IntTryParse(year);
            //
            var callingPlanAssignment = await _context.CallingPlanAssignments
                .Include(m => m.CallingPlan)
                .Include(m => m.PhoneLine)
                .FirstOrDefaultAsync(m => m.PhoneNumber == phoneNumber && m.Month == _month && m.Year == _year && m.CallingPlanId == callingPlanId);
            if (callingPlanAssignment == null)
            {
                return NotFound();
            }
            ViewData["CallingPlanId"] = new SelectList(_context.CallingPlans, "CallingPlanId", "CallingPlanId", callingPlanAssignment.CallingPlanId);
            ViewData["PhoneNumber"] = new SelectList(_context.PhoneLines, "PhoneNumber", "PhoneNumber", callingPlanAssignment.PhoneNumber);
            List<MonthSelect> months = new List<MonthSelect>() { new MonthSelect("enero", 1), new MonthSelect("febrero", 2), new MonthSelect("marzo", 3),
                new MonthSelect("abril", 4), new MonthSelect("mayo", 5), new MonthSelect("junio", 6), new MonthSelect("julio", 7), new MonthSelect("agosto", 8),
                new MonthSelect("septiembre", 9), new MonthSelect("octubre", 10), new MonthSelect("noviembre", 11), new MonthSelect("diciembre", 12) };
            ViewData["Month"] = new SelectList(months, "MonthNumber", "Name");
            return View(callingPlanAssignment);
        }

        // POST: MobilePhoneCallingPlanAssignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("PhoneNumber,Month,Year,CallingPlanId")] CallingPlanAssignment callingPlanAssignment)
        {

            //
            Tuple<bool, string>[] show = new Tuple<bool, string>[4];
            var check1 = true;
            var check2 = true;
            var check3 = true;
            var check4 = true;


            show[0] = new Tuple<bool, string>(check1, null);
            show[1] = new Tuple<bool, string>(check2, null);
            show[2] = new Tuple<bool, string>(check3, null);
            show[3] = new Tuple<bool, string>(check4, null);
            //
            if (ModelState.IsValid)
            {
                _context.Add(callingPlanAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { pNCheck = "On", cPCheck = "On", monthCheck = "On", yearCheck = "On" });
            }
            ViewData["top"] = 20;
            ViewData["mult"] = 0;
            ViewData["columns"] = show;
            ViewData["page"] = 0;
            if (CallingPlanAssignmentExists( callingPlanAssignment))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(callingPlanAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CallingPlanAssignmentExists(callingPlanAssignment))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { pNCheck = "On", cPCheck = "On", monthCheck = "On", yearCheck = "On" });
            }
            ViewData["CallingPlanId"] = new SelectList(_context.CallingPlans, "CallingPlanId", "CallingPlanId", callingPlanAssignment.CallingPlanId);
            ViewData["PhoneNumber"] = new SelectList(_context.PhoneLines, "PhoneNumber", "PhoneNumber", callingPlanAssignment.PhoneNumber);
            List<MonthSelect> months = new List<MonthSelect>() { new MonthSelect("enero", 1), new MonthSelect("febrero", 2), new MonthSelect("marzo", 3),
                new MonthSelect("abril", 4), new MonthSelect("mayo", 5), new MonthSelect("junio", 6), new MonthSelect("julio", 7), new MonthSelect("agosto", 8),
                new MonthSelect("septiembre", 9), new MonthSelect("octubre", 10), new MonthSelect("noviembre", 11), new MonthSelect("diciembre", 12) };
            ViewData["Month"] = new SelectList(months, "MonthNumber", "Name");
            return View(callingPlanAssignment);
        }

        // GET: MobilePhoneCallingPlanAssignments/Delete/5
        public async Task<IActionResult> Delete(string phoneNumber, string month, string year, string callingPlanId)
        {
            if (phoneNumber == null || month == null || year == null || callingPlanId == null)
            {
                return NotFound();
            }
            //
            var _month = (month != null && SD.months.ContainsKey(month)) ? SD.months[month] : -1;
            if (_month == -1)
                _month = Parse.IntTryParse(month);
            var _year = Parse.IntTryParse(year);
            //
            var callingPlanAssignment = await _context.CallingPlanAssignments
                .Include(m => m.CallingPlan)
                .Include(m => m.PhoneLine)
                .FirstOrDefaultAsync(m => m.PhoneNumber == phoneNumber && m.Month == _month && m.Year == _year && m.CallingPlanId==callingPlanId);
            if (callingPlanAssignment == null)
            {
                return NotFound();
            }

            return View(callingPlanAssignment);
        }

        // POST: MobilePhoneCallingPlanAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("PhoneNumber,Month,Year,CallingPlanId")] CallingPlanAssignment CallingPlanAssignment)
        {
            var _CallingPlanAssignment = await _context.CallingPlanAssignments
                .Include(m => m.CallingPlan)
                .Include(m => m.PhoneLine)
                .FirstOrDefaultAsync(m => m.PhoneNumber == CallingPlanAssignment.PhoneNumber && m.Month == CallingPlanAssignment.Month && m.Year == CallingPlanAssignment.Year && m.CallingPlanId == CallingPlanAssignment.CallingPlanId);
            //var _CallingPlanAssignment = await _context.CallingPlanAssignments.FindAsync(CallingPlanAssignment.PhoneNumber,CallingPlanAssignment.Month, CallingPlanAssignment.Year, CallingPlanAssignment.CallingPlanId);
            _context.CallingPlanAssignments.Remove(_CallingPlanAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { pNCheck = "On", cPCheck = "On", monthCheck = "On", yearCheck = "On" });
        }

        private bool CallingPlanAssignmentExists(CallingPlanAssignment m)
        {
            return _context.CallingPlanAssignments.Any(e => e.PhoneNumber == m.PhoneNumber &&
                                                                       e.CallingPlanId==e.CallingPlanId && e.Month == m.Month && m.Year == m.Year);
        }
    }
}