using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Api.DAL;
using Api.Model;
using Microsoft.Extensions.FileSystemGlobbing;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorseInfoesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public HorseInfoesController(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// https://localhost:5001/api/horseinfoes
        /// Getの引数を設定した時、https://localhost:5001/api/horseinfoes/カレン で結果が返る
        /// FromQueryを使えば複数検索も可能 https://localhost:5001/api/horseinfoes/カ?father=シ&mother=ア で結果が返る。[HttpGet("{horseName}")]の時。
        /// 
        /// https://localhost:5001/api/horseinfoes?horsename=カレン　で、馬名がカレンの結果が帰る
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<HorseInfo> Get(
            [FromQuery] string horseName, [FromQuery]string father, [FromQuery] string mother, [FromQuery]string motherFather,
            [FromQuery] string trainer, [FromQuery] string horseOwner, [FromQuery] string productionRanch, [FromQuery] string origin, [FromQuery] string horseNameMeaning)
        {
            var horse = _context.HorseInfo
                .Where(x => x.HorseName.Contains(horseName ?? "") 
                        && x.Father.Contains(father ?? "")
                        && x.Mother.Contains(mother ?? "")
                        && x.MotherFather.Contains(motherFather ?? "")
                        && x.Trainer.Contains(trainer ?? "")
                        && x.HorseOwner.Contains(horseOwner ?? "")
                        && x.ProductionRanch.Contains(productionRanch ?? "")
                        && x.Origin.Contains(origin ?? "")
                        && x.HorseNameMeaning.Contains(horseNameMeaning ?? ""))
                .OrderBy(x => x.HorseName);
            return horse;
        }

        // GET: HorseInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.HorseInfo.ToListAsync());
        }

        // GET: HorseInfoes/Details/5
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horseInfo = await _context.HorseInfo
                .FirstOrDefaultAsync(m => m.Birthday == id);
            if (horseInfo == null)
            {
                return NotFound();
            }

            return View(horseInfo);
        }

        // GET: HorseInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HorseInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HorseName,Birthday,Father,Mother,MotherFather,MotherMother,Sex,CoatColor,HorseNameMeaning,HorseOwner,Trainer,ProductionRanch,Origin")] HorseInfo horseInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(horseInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(horseInfo);
        }

        // GET: HorseInfoes/Edit/5
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horseInfo = await _context.HorseInfo.FindAsync(id);
            if (horseInfo == null)
            {
                return NotFound();
            }
            return View(horseInfo);
        }

        // POST: HorseInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("HorseName,Birthday,Father,Mother,MotherFather,MotherMother,Sex,CoatColor,HorseNameMeaning,HorseOwner,Trainer,ProductionRanch,Origin")] HorseInfo horseInfo)
        {
            if (id != horseInfo.Birthday)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horseInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorseInfoExists(horseInfo.Birthday))
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
            return View(horseInfo);
        }

        // GET: HorseInfoes/Delete/5
        public async Task<IActionResult> Delete(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horseInfo = await _context.HorseInfo
                .FirstOrDefaultAsync(m => m.Birthday == id);
            if (horseInfo == null)
            {
                return NotFound();
            }

            return View(horseInfo);
        }

        // POST: HorseInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id)
        {
            var horseInfo = await _context.HorseInfo.FindAsync(id);
            _context.HorseInfo.Remove(horseInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorseInfoExists(DateTime id)
        {
            return _context.HorseInfo.Any(e => e.Birthday == id);
        }
    }
}
