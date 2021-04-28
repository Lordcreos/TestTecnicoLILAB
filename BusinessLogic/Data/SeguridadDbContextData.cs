using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class SeguridadDbContextData 
    {

        public static async Task SeedUserAsync(UserManager<Usuario> userManager) {

            if (!userManager.Users.Any()) {
                var usuario = new Usuario
                {
                    Nombre = "Vaxi",
                    Apellido = "Drez",
                    UserName = "vaxidrez",
                    Email = "vaxi.drez@gmail.com",
                    Direccion = new Direccion
                    {
                        Calle = "Los Proceres 321",
                        Ciudad = "Mexico DF",
                        CodigoPostal = "28985",
                        Departamento = "Mexico",
                    }
                };

                await userManager.CreateAsync(usuario, "VaxiDrez2025$");
            }

        }

    }
}
