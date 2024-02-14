using BusinessLogic.BookingServices;
using BusinessLogic.Interfaces;
using DataAccess.Constants;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace New
{
    public static class SeederDb
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var service = scope.ServiceProvider;
                //Отримую посилання на наш контекст
                var context = service.GetRequiredService<BookingDbContext>();
                var userManager = service.GetRequiredService<UserManager<UserEntity>>();
                var roleManager = service.GetRequiredService<RoleManager<RoleEntity>>();
                var imageWorker = service.GetRequiredService<IImageWorker>();                

                context.Database.Migrate();

                //Якщо ролей в БД немає, то ми їх створимо по default

                if (!context.Roles.Any())
                {
                    var admin = new RoleEntity
                    {
                        Name = Roles.Admin
                    };
                    var roleResult = roleManager.CreateAsync(admin).Result;
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine("-----Problem create role {0}-----", Roles.Admin);
                    }

                    var user = new RoleEntity { Name = Roles.User };
                    roleResult = roleManager.CreateAsync(user).Result;
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine("-----Problem create role {0}-----", Roles.User);
                    }

                    var seller = new RoleEntity
                    {
                        Name = Roles.Seller
                    };
                    roleResult = roleManager.CreateAsync(seller).Result;
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine("-----Problem create role {0}-----", Roles.Seller);
                    }
                }

                if (!context.Users.Any())
                {
                    UserEntity user = new UserEntity
                    {
                        FirstName = "Валерій",
                        LastName = "Підкаблучник",
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com",
                        PhoneNumber = "+380-00-00",
                        Image = imageWorker.ImageSave("https://cdn-icons-png.flaticon.com/512/4919/4919646.png")
                    };
                    var result = userManager.CreateAsync(user, "123456").Result;
                    if (!result.Succeeded)
                    {
                        Console.WriteLine("------Propblem create user {0}-----", user.Email);
                    }
                    else
                    {
                        result = userManager.AddToRoleAsync(user, Roles.Admin).Result;
                        if (!result.Succeeded)
                        {
                            Console.WriteLine("-------Propblem add user {0} role {1}--------", user.Email, Roles.Admin);
                        }
                    }
                }

                if (!context.TypeOfSale.Any())
                {
                    TypeOfSale type1 = new TypeOfSale
                    {
                        Name= "Оренда"
                    };

                    TypeOfSale type2 = new TypeOfSale
                    {
                        Name = "Продаж"
                    };                    

                    context.TypeOfSale.Add(type1);
                    context.TypeOfSale.Add(type2);
                    context.SaveChanges();
                }

                if (!context.ViewOfTheHouse.Any())
                {
                    ViewOfTheHouse house1 = new ViewOfTheHouse
                    {
                        Name = "Будинок"
                    };

                    ViewOfTheHouse house2 = new ViewOfTheHouse
                    {
                        Name = "Квартира"
                    };

                    ViewOfTheHouse house3 = new ViewOfTheHouse
                    {
                        Name = "Котедж"
                    };

                    context.ViewOfTheHouse.Add(house1);
                    context.ViewOfTheHouse.Add(house2);
                    context.ViewOfTheHouse.Add(house3);
                    context.SaveChanges();
                }
            }
        }
    }
}
