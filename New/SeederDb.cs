using BusinessLogic.BookingServices;
using BusinessLogic.Interfaces;
using DataAccess.Constants;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using static System.Net.WebRequestMethods;

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

                    context.ViewOfTheHouse.Add(house1);
                    context.ViewOfTheHouse.Add(house2);                    
                    context.SaveChanges();
                }

                if (!context.Buildings.Any())
                {
                    BuildingEntity[] buildingsArray = new BuildingEntity[]
                    {
                        new BuildingEntity
                        {
                            Name="Продаж квартири у центрі",
                            Description="Продам 3 кімнатну квартира покращеного планування .Центр .Набережна. Цегляний утеплений будинок .\nТри окремі кімнати, великий коридор. Санвузол та ванна окремі.\n-Два балкони, один засклений (металопластик), вигляд на дві сторони будинку ( на Набережну) та у двір.\n-Вікна в квартирі металопластикові.\n-невеличка кладовочка для зберігання необхідних господарських речей.\n-Створене ОСББ, чистий підїзд. Ліфт .\n7 поверх з 9",
                            Address="м.Рівне, вул. Набережна 14",
                            Area=67.6,
                            NumberOfRooms=3,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=46000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path =imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/z4r8l4g61tpz2-UA/image")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/8k8r1e94ai0a2-UA/image")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/e1ekh37ciabj1-UA/image")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/yu0u2fhv1iy73-UA/image")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/eudy2g6be7nu3-UA/image")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/nin5psvtmutd3-UA/image")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/j4e3swkne0mm3-UA/image")}
                            }
                        },

                         new BuildingEntity
                        {
                            Name="Затишна новенька квартира",
                            Description="Хороший невисокий поверх 2 з 5.\nКвартира з новим сучасним якісним ремонтом:на стінах декоративна штукатурка 4(дихаюча,екологічно чиста, гарно миється)\r\nПідлога ламінат та керамічна плитка(Польща)\nБроньовані двері\nЕнергозберігаючі вікна\nКвартира має гарний вигляд з вікна\nВиходить на південну сторону\nУкомплектована новими меблями та технікою, цікавими деталями інтер'єру.\nДуже зручне розташування-поруч зупики. До центру міста 2 зупинки\nВідеодомофон та Відео нагляд\nВхід за картками",
                            Address="м.Рівне, вул. Набережна 14",
                            Area=34,
                            NumberOfRooms=1,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=46103,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path =imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/kp843omm12d61-UA/image")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/mkkzw39m5zxo2-UA/image;s=1000x700")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/5cjnlbgagx661-UA/image;s=1000x700")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/zcvndtbfom532-UA/image;s=1000x700")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/jra72hb2bi31-UA/image;s=1000x700")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/rvddmafc13iu-UA/image;s=1000x700")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/v04h7pgfyc2l2-UA/image;s=1000x700")}
                            }
                        },

                          new BuildingEntity
                        {
                            Name="Елітна квартира в новобудові",
                            Description="✓ Хороший невисокий поверх 2 з 5.\n✓ Квартира з новим сучасним якісним ремонтом:на стінах декоративна штукатурка 4(дихаюча,екологічно чиста, гарно миється)\n✓Підлога ламінат та керамічна плитка(Польща)\n✓Броньовані двері\n✓Енергозберігаючі вікна\n✓Квартира має гарний вигляд з вікна\n✓Виходить на південну сторону\n✓Укомплектована новими меблями та технікою, цікавими деталями інтер'єру.\n✓Дуже зручне розташування-поруч зупики. До центру міста 2 зупинки\n✓Відеодомофон та Відео нагляд\n✓Вхід за картками",
                            Address="Рівне, вул. Чехова 17-А",
                            Area=71,
                            NumberOfRooms=2,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=62403,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path =imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/eqg83j2zxsa1-UA/image;s=1000x700")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/5ohbajajybhy-UA/image;s=1000x700")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/f0uprxoce5tw2-UA/image;s=1000x700")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/k9omtl7bvzr41-UA/image;s=1000x700")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/f5p23hlyt07p-UA/image;s=1000x700")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/rvddmafc13iu-UA/image;s=1000x700")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://ireland.apollo.olxcdn.com/v1/files/bu487i39myyw2-UA/image;s=1000x700")}
                            }
                        },

                           new BuildingEntity
                        {
                            Name="Продаж 1к квартири 46.1 кв. м на вул. Студентська 32",
                            Description="Характеристика будівлі: стан будинку і під'їзду: відмінний\nтип перекриття: залізобетонне\nХарактеристика приміщення: житло не використовувалось з моменту побудови  планування: роздільні кімнати  \r\nстан квартири: дизайнерський ремонт\nутеплення: зовнішнє\nОздоблення стелі: натяжна стеля\nПідлога (покриття): керамічна плитка, ламінат\nЗовнішня обробка: пофарбовано (фарбування)  фарбування\r\nКомунікації: газ: є\nтелефон: немає\nвентиляція / кондиціонування: вентиляція\nВода: центральне (водопровід)\nДвері та вікна: вхідні двері: броньовані  •  тип вікон: металопластикові  •  панорамні / вітринні вікна  •  кількість камер склопакета: 2  •  захист вікон: немає  •  окремий вхід: немає\nОпалення: індивідуальне газове  •  двоконтурний котел\nПідігрів води: двоконтурний котел\nПриміщення: санвузол: суміжний  •  лоджія: 1  •  засклених лоджій: 1  •  кількість спалень: 1  •  площа, м²: 18.1  •  гараж: немає  •  гардероб  •  балкон (и): немає\nОбладнання  •  меблі  •  зручності: кухонний куток  •  пральна машина  •  посудомийна машина  •  ванна  •  газова плита  •  духовка  •  вбудована шафа-гардероб  •  мебльована кухня  •  лічильник на електрику  •  лічильник на газ  •  лічильник на воду  •  витяжка  •  ліжко\nПід'їзд: домофон",
                            Address="м. Рівне, вулиця Студентська, 32 (район Щасливе)",
                            Area=46.1,
                            NumberOfRooms=1,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=64000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path =imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-naberejnaya-ulitsa__231087156xl.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-naberejnaya-ulitsa__231087158xl.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-naberejnaya-ulitsa__231087165fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-naberejnaya-ulitsa__231087173fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-naberejnaya-ulitsa__231087191fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-naberejnaya-ulitsa__231087212fx.webp")}                                
                            }
                        },

                             new BuildingEntity
                        {
                            Name="Продаж 1к квартири 35 кв. м на вул. Корольова",
                            Description="1К квартира в Новобудові \nвул Корольова р-н Ювілейний\nокремо: кухня та кімната\nрозташування всередині\nсонячна сторона\nавтономне опалення ( дахова котельня)\nнаповнення: чистова стяжка та штукатурка несучих стін , лічильники на світло воду та окремий на опалення , панорамні вікна , встановлено радіатори з терморегуляторами , вихід та корзина для кондиціонера\nплоща 35//10.38 поверх 3/9Ц",
                            Address="м.Рівне, вул. Корольова",
                            Area=35,
                            NumberOfRooms=1,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=31100,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path =imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-koroleva-ulitsa__273215593xl.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-koroleva-ulitsa__273497871xl.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-koroleva-ulitsa__273215615fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-koroleva-ulitsa__273497895fx.webp")}
                            }
                        },

                                  new BuildingEntity
                        {
                            Name="Продаж 2к квартири 43.6 кв. м на просп. Князя Романа 11",
                            Description="Коротенько характеристика:\nЗагальна площа 43, 6 м.кв\nКімнати роздільні\nЛічильник є на газ та електрику\nЗнаходиться по вулиці Князя Романа ( поруч школа та садочки)\nРозташова на п'ятому поверсі (з дахом проблем жодних немає!!)\nХороший двір з дитячим майданчиком\r\nПоруч вся інфраструктура\nКвартира потребує ремонту",
                            Address="м. Рівне, просп. Князя Романа 11",
                            Area=43.6,
                            NumberOfRooms=2,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=27800,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path =imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-knyazya-romana-prospekt__229024368fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-knyazya-romana-prospekt__229024335fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-knyazya-romana-prospekt__229024339fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-knyazya-romana-prospekt__229024346fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-knyazya-romana-prospekt__229024349fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-knyazya-romana-prospekt__229024366fx.webp")}
                            }
                        },

                          new BuildingEntity
                        {
                            Name="Продаж 2к квартири 44 кв. м на вул. Василя Червонія",
                            Description="Продам 2 кімнатну квартиру в прекрасному районі Чайка.\r\nЦегляний будинок.\r\nПоверх 4/4Ц.\r\nПлоща 43, 5м.кв.\r\nКухня 5, 7м.кв.\r\nЗ дахом проблем немає, жодного разу не протікав.\r\nДуже тепла, не кутова, з чудовим місцем розташування.\r\nЄ балкон засклений.\r\nВанна та санвузол разом.\r\nОпалення централізоване.\r\nПарковка автомобіля біля будинку.\r\nХороша транспортна розвязка, поруч школи, садочки, супермаркети АТБ та Сільпо, поліклініка, аптеки, Нова Пошта та Укрпошта, зупинки, парк та МПК Текстильник.\r\n",
                            Address="Р‑н. Чайка, вул. Василя Червонія, Рівне",
                            Area=43.5,
                            NumberOfRooms=2,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=34000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path =imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-gagarina-ulitsa__246842161fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-gagarina-ulitsa__246842166fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-gagarina-ulitsa__246842169fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-gagarina-ulitsa__246842172fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-gagarina-ulitsa__246842182fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-chayka-gagarina-ulitsa__246842217fx.webp")}
                            }
                        },

                           new BuildingEntity
                        {
                            Name="Продаж 2к квартири 63 кв. м на вул. Курчатова",
                            Description="Цегляний будинок\r\nавтономне газове опалення\r\nякісний ремонт\r\nпосудомийна машина на кухні\r\nгардеробна\r\nзалишається все\r\nПідходить під кредитування!\r\nДуже зручна локація- поруч супермаркети, аптека, школа, садочок, лікарня, зупинка громадського транспорту",
                            Address="Район Мототреку/вул.В.Стельмаха( Курчатова), Рівне",
                            Area=63,
                            NumberOfRooms=2,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=85000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path =imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-vidinskaya-kurchatova-ulitsa__278014376xl.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-vidinskaya-kurchatova-ulitsa__278014378fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-vidinskaya-kurchatova-ulitsa__278014382fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-vidinskaya-kurchatova-ulitsa__278014383fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-vidinskaya-kurchatova-ulitsa__278014384fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-vidinskaya-kurchatova-ulitsa__278014389fx.webp")}
                            }
                        },

                                new BuildingEntity
                        {
                            Name="Продаж 2к квартири 44 кв. м на вул. Кавказька 13",
                            Description="Продається 2 кімнатна квартира.\nЗнаходиться в цегляному будинку по вул.Кавказькій.\nПоверх 3.\nДуже зручне розташування - центр міста.\nВікна в двір.\nКімнати окремі.",
                            Address="м. Рівне, на вул. Кавказька 13",
                            Area=44,
                            NumberOfRooms=2,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=42000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path =imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-avkazkaya-lia__277502170fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-avkazkaya-lia__277502173fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-avkazkaya-lia__277502174fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-avkazkaya-lia__277502186fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-avkazkaya-lia__277502184fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-avkazkaya-lia__277502187fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tsentr-avkazkaya-lia__277502190fx.webp")}
                            }
                        },

                          new BuildingEntity
                        {
                            Name="Продаж 3к квартири 82 кв. м на пров. Дубенський",
                            Description="Квартира у Клубному будинку\nЗакритий двір з мангалом та дитячим майданчиком\nБудинок знаходиться у тихому районі міста\nІндивідуальне опалення\nТепла підлога, майже по всій квартирі\nЗроблений сучасний ремонт\nПлоща квартири 82кв.м.\nДва балкони\nДва санвузли\nКомірка",
                            Address="м. Рівне, мікрорайон Тинне, Дубенський провулок",
                            Area=47,
                            NumberOfRooms=3,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=79000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path =imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tyinnoe-dbenkiy-peelok__268668076fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tyinnoe-dbenkiy-peelok__268668070fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tyinnoe-dbenkiy-peelok__268668071fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tyinnoe-dbenkiy-peelok__273545477fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tyinnoe-dbenkiy-peelok__268668072fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tyinnoe-dbenkiy-peelok__273545476fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-tyinnoe-dbenkiy-peelok__273545490fx.webp")}
                            }
                        },

                             new BuildingEntity
                        {
                            Name="Продаж 3к квартири 67 кв. м на вул. Кулика і Гудачека",
                            Description="Квартира в хорошому жилому стані\nна дві сторони будинка.\nБажаний третій поверх.\nМПВ. Збільшена площа кухні.\nПеретерті стелі та стіни.\nСанвузол роздільний, в плитці.\nЗамінені вхідні двері.\nВбудовані меблі в кухні.\nШафа -купе в коридорі.\nВелика засклена лоджія.\nКвартира суха, тепла та сонячна.\nНовий ліфт.\n",
                            Address="м.Рівне, вулиця Кулика і Гудачека (мікрорайон Боярка)",
                            Area=67,
                            NumberOfRooms=3,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=40000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {                             
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-kulika-i-gudacheka-makarova-ulitsa__278147981fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-kulika-i-gudacheka-makarova-ulitsa__278147983fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-kulika-i-gudacheka-makarova-ulitsa__278147985fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-kulika-i-gudacheka-makarova-ulitsa__278147979fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-kulika-i-gudacheka-makarova-ulitsa__278147989fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-kulika-i-gudacheka-makarova-ulitsa__278147990fx.webp")}
                            }
                        },

                                   new BuildingEntity
                        {
                            Name="Продаж 3к квартири 90.6 кв. м на вул. Йосипа Драганчука 4",
                            Description="Автономне ідивідуальне газове опаленення;\r\nжитлова площа 54.7 м к.в.; 2 лоджиї по 4.2 м к.в.;\r\nпідлога - стяжка, стіни - чорнова штукатурка; стеля - гіпоскартон;\r\nпросторі кімнати з можливістю перепланування;\r\nванна та санвузол окремо;\r\nрозведена електрика;\r\nвеликий двір, присутній майданчик для паркування.\r\nХороша транспортна розв'язка, садочок, школа, лікарня, супермаркети",
                            Address="м.Рівне, вулиця Йосипа Драганчука, 4 (мікрорайон Мототрек)",
                            Area=90.6,
                            NumberOfRooms=3,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=67950,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-mototrek-iosifa-draganchuka-ulitsa__260440897fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-mototrek-iosifa-draganchuka-ulitsa__260443539fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-mototrek-iosifa-draganchuka-ulitsa__260444947fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-mototrek-iosifa-draganchuka-ulitsa__260445781fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-mototrek-iosifa-draganchuka-ulitsa__260445835fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-mototrek-iosifa-draganchuka-ulitsa__260445838fx.webp")}
                            }
                        },
                          new BuildingEntity
                        {
                            Name="Продаж 3к квартири 140 кв. м на вул. Дворецька",
                            Description="Продаж великої 3км дворівневої квартири\r\nЗнаходиться по вул. Дворецька\r\nЗагальна площа 140кв.м\r\nАвтономне газове опалення\r\nРозведена електрика\r\nСтан після забудовників\r\nДуже гарний краєвид\r\nБудинок закритого типу, відеонагляд",
                            Address="м. Рівне, вулиця. Дворецька (район Пивзавод)",
                            Area=140,
                            NumberOfRooms=3,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=68000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-pivzavod-dvoretskaya-ulitsa__278013977fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-pivzavod-dvoretskaya-ulitsa__278013978fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-pivzavod-dvoretskaya-ulitsa__278013981fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-pivzavod-dvoretskaya-ulitsa__278013982fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-pivzavod-dvoretskaya-ulitsa__278013986fx.webp")}
                            }
                        },

                           new BuildingEntity
                        {
                            Name="Продаж 3к квартири 59.8 кв. м на вул. Назара Небожинського 18",
                            Description="Продам 3 кімнатну квартиру у цегляному будинку по вулиці Назара Небожинського\r\nПокращене планування.\r\nТретій поверх\r\nГарний жилий стан.\r\nПродаж з меблями і частково з технікою .\r\nОСББ. Новий сучасний ліфт . Вікна у підʼїздах замінені.\r\n",
                            Address="м. Рівне, вулиця Назара Небожинського, 18 (мікрорайон Боярка), район Пивзавод",
                            Area=59.8,
                            NumberOfRooms=3,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=44800,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-nazara-nebojinskogo-pavlyuchenko-ulitsa__230220183fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-nazara-nebojinskogo-pavlyuchenko-ulitsa__230220180fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-nazara-nebojinskogo-pavlyuchenko-ulitsa__230220185fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-nazara-nebojinskogo-pavlyuchenko-ulitsa__230220191fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-nazara-nebojinskogo-pavlyuchenko-ulitsa__230220205fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-boyarka-nazara-nebojinskogo-pavlyuchenko-ulitsa__230220209fx.webp")}
                            }
                        },

                             new BuildingEntity
                        {
                            Name="Продаж 1к квартири 34 кв. м на просп. Генерала Безручка",
                            Description="Затишна однокімнатна квартира\r\n34 м кв\r\nКухня 8.9 м кв\r\nЄ кладовочка\r\nКвартира в тихому районі\r\nПоруч розвинена інфраструктура/ садочок, школа, зупинка тран порту, супермаркет, базарчик\r\nЄ дозвіл на добудову балкону/тому можна на майбутнє зробити ще кімнату.\r\nНа вікнах захист.",
                            Address="м. Рівне, вул. Генерала Безручка, район Північний",
                            Area=34,
                            NumberOfRooms=1,
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=26990,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-severnyiy-eneala-bezka-popek__244132906fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-severnyiy-eneala-bezka-popek__244132905fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-severnyiy-eneala-bezka-popek__244132904fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-severnyiy-eneala-bezka-popek__244132908fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-kvartira-rovno-severnyiy-eneala-bezka-popek__244146753fx.webp")}
                            }
                        },
                           
                          new BuildingEntity
                        {
                            Name="Продаж 2 поверхового будинку з гаражем і ділянкою на 7 соток, 140 кв. м, 4 кімнати",
                            Description="Чудова видова локація в приватному секторі, де все поруч, вся інфраструктура міста в пішому доступі, також поруч парк з озером, АТБ, зупинка г/т, школа\nВелика ділянка площею 7 соток\nПобудований з червоної цегли (товщина стін 380мм)\nУтеплення: повітряна камера + цегла + пінопласт (100мм)\nПланування: три спальні, гостьова кімната, кухня з виходом до тераси та простора вітальня 25м2, 2 санвузли з вікнами, гардеробна кімната, підвал (котельня, пральна кімната, кладова), гараж\nПідключено міське централізоване водопостачання, електрику (10 кВт), переливний септик об'ємом 12 м3\nМонолітне перекриття даху з утепленням 300мм та мембранною ПВХ-покрівлею з ізоляцією\nЗакладні під паркан (підпорна стінка зі стовбчиками)",
                            Address="м. Рівне, вулиця Красильникова (мікрорайон Гідропарк)",
                            Area=140,                           
                            ViewOfTheHouseId=1,
                            TypeOfSaleId=2,
                            Price=115000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-gidropark-krasilnikova-ulitsa__273750134fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-gidropark-krasilnikova-ulitsa__273750141fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-gidropark-krasilnikova-ulitsa__273750143fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-gidropark-krasilnikova-ulitsa__273750149fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-gidropark-krasilnikova-ulitsa__273750155fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-gidropark-krasilnikova-ulitsa__273750160fx.webp")}
                            }
                        },

                          new BuildingEntity
                        {
                            Name="Продаж 1 поверхового будинку з ділянкою на 5 соток",
                            Description="Продам сучасний будинок з подвійним світлом в р-ні Льонокомбінату\nПідключено міську воду, каналізацію та 9 кВт електрики\nГаз по вулиці\nШтукатурка стін, розведено електрику, утеплено та підшито стелю\nУ вартість входить паркан по периметру ділянки\nПланування: 3 окремих кімнати, кухня-вітальня з подвійним світлом, с/в, гардеробна та мансардний поверх 60м²\nЗбудований в 1, 5 цеглини + 15см утеплення\nМонолітний фундамент\nНімецькі вікна Rehau з фурнітурою Winkhaus\nФальцева покрівля даху та комина, канал під камін",
                            Address="м. Рівне, вулиця Льонокомбінатівська (район Північний)",
                            Area=105,                            
                            ViewOfTheHouseId=1,
                            TypeOfSaleId=2,
                            Price=90000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-lenokombinat-enokombinaovkaya-lia__277513662fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-lenokombinat-enokombinaovkaya-lia__277477705fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-lenokombinat-enokombinaovkaya-lia__277477707fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-lenokombinat-enokombinaovkaya-lia__277477720fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-lenokombinat-enokombinaovkaya-lia__277513701fx.webp")}
                            }
                        },

                          new BuildingEntity
                        {
                            Name="Продаж 2 поверхового будинку з ділянкою на 6 соток",
                            Description="На продаж будинок в приватному секторі. Будинок знаходиться в дуже тихому, затишному місці. Поряд школа, магазин.\nВ дворі гараж\r\nФундамент армований, з бутовим каменем, глибиною до 2 метрів\nБудинок з Славутського піноблоку, утеплений пінопластом 5 мм,\nзданий в експлуатацію в 2001 році. Будували якісно, для себе.\nВ будинку є можливість жити двом сімям.\nНа І-поверсі: 3 кімнати, кухня , санвузол;\nна ІІ-поверсі: 4 кімнати, санвузол;\nНа ІІ-поверсі зроблений хороший ремонт, дорога столярка, лишаються меблі.\nВікна пластикові\nЄ місце в дворі влаштувати бесідку та дитячий майданчик.\nОпалення: індивідуальне газове. За рахунок того, що будинок утеплений пінопластом, а також утеплений дах будинку, дуже мала витрата газу в зимовий період. Комунікації всі міські (вода, газ, світло), міська каналізація, що дуже важливо. Хороші сусіди. По всім деталям звертайтесь по телефону. Продаж без комісії для покупця",
                            Address="місто Рівне, район Басівкута",
                            Area=197,                            
                            ViewOfTheHouseId=2,
                            TypeOfSaleId=2,
                            Price=100000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-basov-ugol-povl-zainiy__226125369fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-basov-ugol-povl-zainiy__226125377fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-basov-ugol-povl-zainiy__226125386fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-basov-ugol-povl-zainiy__226125393fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-basov-ugol-povl-zainiy__226125396fx.webp")}
                            }
                        },

                          new BuildingEntity
                        {
                            Name="Продаж 2 поверхового будинку з ділянкою на 11 соток",
                            Description="В будинку є цокольний поверх . На цокольному поверсі розміщено : гараж , кладовка , бойлерна .\r\nБудинок збудований з червоної цегли . Зовні утеплений та штукатурений .\r\nВ середині будинку стіни штукатурені .\r\nПерекриття між поверхами -- бетоні плити .\r\nНа кожному поверсі є туалет та ванна кімната .\r\nКомунікації :\r\nВода -- є\r\nГаз -- є\r\nСвітло -- є\r\nСептик з переливом -- є .\r\nБудинок загороджений від сусідів .\r\nДо будинку зручний доїзд .\r\nПоруч зупинка .\r\nВ вартість будинку входить 11 соток землі .\r\nЯкщо до будинку додати не 11 соток , а 5, 5 соток , то вартість будинку буде 62000 доларів .\r\nДокументи на будинок впорядку .",
                            Address="м. Рівне, вулиця Надвірна (район Щасливе)",
                            Area=280,                           
                            ViewOfTheHouseId=1,
                            TypeOfSaleId=2,
                            Price=80000,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-schastlivoe-nadvornaya-ulitsa__188818380fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-schastlivoe-nadvornaya-ulitsa__188818400fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-schastlivoe-nadvornaya-ulitsa__188818405fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-schastlivoe-nadvornaya-ulitsa__188818415fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-dom-rovno-schastlivoe-nadvornaya-ulitsa__188818419fx.webp")}
                            }
                        },

                          new BuildingEntity
                        {
                            Name="Продаж 2 поверхового таунхауса з ділянкою на 3 сотки",
                            Description="Котедж в спокійному районі міста\nРайон Басового Кута\nДілянка 3 сот\nКотедж з міськими мережами\n-газ\n-центральне водопостачання\n-центральна каналізація\nелектрика\nКотедж з штукатуркою, стяжкою та теплою підлогою\nЗа усіма деталями телефонуйте\nАсфальтований доїзд , територія з бруківкою",
                            Address="м. Рівне, район Щасливе",
                            Area=108,                            
                            ViewOfTheHouseId=1,
                            TypeOfSaleId=2,
                            Price=26990,
                            UserEntityId=1,
                            ImagesBulding= new List<ImagesBulding>()
                            {
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-taunhaus-rovno-schastlivoe-rojdestvenskaya-ulitsa__273685052fx.jpg")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-taunhaus-rovno-schastlivoe-rojdestvenskaya-ulitsa__273685066fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-taunhaus-rovno-schastlivoe-rojdestvenskaya-ulitsa__273685116fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-taunhaus-rovno-schastlivoe-rojdestvenskaya-ulitsa__273685219fx.webp")},
                                new ImagesBulding() {Path=imageWorker.ImageSave("https://cdn.riastatic.com/photosnew/dom/photo/prodaja-taunhaus-rovno-schastlivoe-rojdestvenskaya-ulitsa__273685222fx.webp")}
                            }
                        }
                    };

                    //Вантажимо нові фото з обєкту BuildingEntity                    
                    //foreach (var building in buildingsArray)
                    //{
                    //    foreach (var image in building.ImagesBulding)
                    //    {
                    //         var path = image.Path;                            

                    //        // Тут ви можете використовувати path за вашим призначенням
                    //        // Наприклад, додати шлях в imageWorker
                    //        imageWorker.ImageSave(path);                            
                    //    }
                    //}

                    // Додаємо всі об'єкти з масиву до бази даних
                    context.Buildings.AddRange(buildingsArray);

                    // Зберігаємо зміни в базі даних
                    context.SaveChanges();
                  
                }
            }
        }
    }
}
