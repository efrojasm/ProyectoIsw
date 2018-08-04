using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eventos.Data;
using Eventos.Models;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MailKit.Net.Smtp;

namespace Eventos.Controllers
{
    public class EventoDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        

        public EventoDatasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager )
        {
            _context = context;
            _userManager = userManager;
          
        }

        // GET: EventoDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventoData.ToListAsync());
        }

        // GET: EventoDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoData = await _context.EventoData
                .SingleOrDefaultAsync(m => m.ID == id);
            if (eventoData == null)
            {
                return NotFound();
            }

            return View(eventoData);
        }

        // GET: EventoDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventoDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Cupo,Lugar,Detalle,Fecha")] EventoData eventoData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventoData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventoData);
        }

        // GET: EventoDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoData = await _context.EventoData.SingleOrDefaultAsync(m => m.ID == id);
            if (eventoData == null)
            {
                return NotFound();
            }
            return View(eventoData);
        }

        // POST: EventoDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Cupo,Lugar,Detalle,Fecha")] EventoData eventoData)
        {
            if (id != eventoData.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventoData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoDataExists(eventoData.ID))
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
            return View(eventoData);
        }

        public async Task<IActionResult> Edit2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoData = await _context.EventoData.SingleOrDefaultAsync(m => m.ID == id);
            if (eventoData == null)
            {
                return NotFound();
            }
            return View(eventoData);
        }

        // POST: EventoDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(int id, [Bind("ID,Nombre,Cupo,Lugar,Detalle,Fecha,UserName")] EventoData eventoData)
        {
           
            if (id != eventoData.ID)
            {
                return NotFound();
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventoData);
                    await _context.SaveChangesAsync();
                  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoDataExists(eventoData.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var user = await _userManager.FindByEmailAsync(eventoData.UserName);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress("EventoUTN Registrado", "pprograweb2@gmail.com"));
                mensaje.To.Add(new MailboxAddress(eventoData.UserName));
                mensaje.Subject = "Evento Registrado";
                mensaje.Body = new TextPart("plain")
                {
                    Text = "Estas registrado para acudir al evento " + eventoData.Nombre + " La fecha " + eventoData.Fecha
                };
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("pprograweb2@gmail.com", "visualstudio");
                    client.Send(mensaje);
                    client.Disconnect(true);
                }
                return RedirectToAction(nameof(Index));
            }
           
            return View(eventoData);
        }

        // GET: EventoDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoData = await _context.EventoData
                .SingleOrDefaultAsync(m => m.ID == id);
            if (eventoData == null)
            {
                return NotFound();
            }

            return View(eventoData);
        }

        // POST: EventoDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventoData = await _context.EventoData.SingleOrDefaultAsync(m => m.ID == id);
            _context.EventoData.Remove(eventoData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoDataExists(int id)
        {
            return _context.EventoData.Any(e => e.ID == id);
        }
    }
}
