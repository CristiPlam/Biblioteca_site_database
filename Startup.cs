using Atestat2._0.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Atestat2._0.Startup))]
namespace Atestat2._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // configurez aplicația să permită crearea de useri și administratori
            ConfigureAuth(app);
            CreateAdminAndUserRoles();
        }
        private void CreateAdminAndUserRoles() // aici pot crea rolul de user și administrator
        {
            var ctx = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));

            // adaugam rolurile pe care le poate avea un utilizator
            // din cadrul aplicatiei
            if (!roleManager.RoleExists("Admin"))
            {
                // adaugam rolul de administrator
                var role = new IdentityRole();
                role.Name = "Admin"; // îi dăm un nume care va putea fi apelat ulterior atunci când vrem să impunem restricții pe anumite acțiuni în aplicație
                roleManager.Create(role);

                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "admin@admin.com"; // numele de utilizator al administratorului
                user.Email = "admin@admin.com"; // adresa de mail cu care se va loga administratorul

                var adminCreated = userManager.Create(user, "Admin2021!"); // parola prestabilită pentru contul de administrator
                if (adminCreated.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin"); // adăugăm user-ul în userManager
                }
            }
        }
    }
}
