using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaAC.Data;
using SistemaAC.Models;

namespace SistemaAC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        ////////////////////
          UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        UsuarioRole _usuarioRole;
        public List<SelectListItem> usuarioRole;
        ///////////////////


        public UsuariosController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
             RoleManager<IdentityRole> roleManager)
        {
            _context = context;

            _userManager = userManager;
            _roleManager = roleManager;
            _usuarioRole = new UsuarioRole();
            usuarioRole = new List<SelectListItem>();

        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            //Decalro una variable ID inicializado vacia
            var ID = "";
            //declaron un objeto list que despnde de  la clase Usuario
            List<Usuario> usuario = new List<Usuario>();
            //ahora tengo todos los registros de la tabla donde almaceno los usuarios
            // y lo almaceno en el objeto
            var appUsuario = await _context.ApplicationUser.ToListAsync();
            ///ahora con una estructura foreach vamos a recorrer
            /////todos los valores del objeto appUsuario
            foreach(var Data in appUsuario)
            {
                ID = Data.Id;
                /////utilizamos el metodo getrole que vide de la clase usuario role 
                usuarioRole = await _usuarioRole.GetRole(_userManager,_roleManager,ID);

                usuario.Add(new Usuario() {
                    Id = Data.Id,
                    UserName = Data.UserName,
                    PhoneNumber = Data.PhoneNumber,
                    Email = Data.Email,
                    Role = usuarioRole[0].Text


                });

            }
            //modificamos el return  para poder enviar el objeto usuario de tipo list, para poder enviarlo a la vista
            return View(usuario.ToList());

            //return View(await _context.ApplicationUser.ToListAsync());

        }

        //metodo para recibir los datos del usuario
        public async Task<List<Usuario>> GetUsuario(string id)
        {
            //objeto list que depende de la clase usuario
            List<Usuario> usuario = new List<Usuario>();
            var appUsuario = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            usuarioRole = await _usuarioRole.GetRole(_userManager, _roleManager, id);
            usuario.Add(new Usuario(){
                Id = appUsuario.Id,
                UserName = appUsuario.UserName,
                PhoneNumber=appUsuario.PhoneNumber,
                Email=appUsuario.Email,
                Role=usuarioRole[0].Text,
                RoleId= usuarioRole[0].Value,
                AccessFailedCount = appUsuario.AccessFailedCount,
                ConcurrencyStamp = appUsuario.ConcurrencyStamp,
                EmailConfirmed = appUsuario.EmailConfirmed,
                LockoutEnabled= appUsuario.LockoutEnabled,
                LockoutEnd= appUsuario.LockoutEnd,
                NormalizedEmail= appUsuario.NormalizedEmail,
                NormalizedUserName=appUsuario.NormalizedUserName,
                PasswordHash =appUsuario.PasswordHash,
                PhoneNumberConfirmed= appUsuario.PhoneNumberConfirmed,
                SecurityStamp=appUsuario.SecurityStamp,
                TwoFactorEnabled=appUsuario.TwoFactorEnabled
            });

            return usuario;
            //List<ApplicationUser> usuario = new List<ApplicationUser>();
            //var appUsuario = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            //usuario.Add(appUsuario);

        }

        public async Task<string> EditUsuario(string id, string userName, string email, string phoneNumber,int accessFailedCount,
            string concurrencyStamp,bool emailConfirmed,
            bool lockoutEnabled, DateTimeOffset lockoutEnd ,string normalizedEmail, string normalizedUserName,
            string passwordHash,bool phoneNumberConfirmed,string securityStamp,bool twoFactorEnabled, ApplicationUser applicationUser)
        {
           
            var resp = "";
            try
            {
                applicationUser = new ApplicationUser
                {
                    Id = id,
                    UserName = userName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    AccessFailedCount= accessFailedCount,
                    ConcurrencyStamp = concurrencyStamp,
                    EmailConfirmed = emailConfirmed,
                    LockoutEnabled= lockoutEnabled,
                    LockoutEnd= lockoutEnd,
                    NormalizedEmail= normalizedEmail,
                    NormalizedUserName= normalizedUserName,
                    PasswordHash = passwordHash,
                    PhoneNumberConfirmed= phoneNumberConfirmed,
                    SecurityStamp= securityStamp,
                    TwoFactorEnabled= twoFactorEnabled,
                };
                //actualizar los datos
                _context.Update(applicationUser);
                await _context.SaveChangesAsync();
                resp = "Save";
            }
            catch
            {
                resp = "No Save";
            }
            return resp;
        }


        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}