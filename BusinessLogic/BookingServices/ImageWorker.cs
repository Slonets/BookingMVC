using BusinessLogic.Helpers;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BookingServices
{
    public class ImageWorker : IImageWorker
    {
        private readonly IConfiguration _configuration;
        public ImageWorker(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string ImageSave(IFormFile image)
        {
            // Метод для збереження зображення, отриманого з форми.
            
            var imageSizes = _configuration.GetSection("ImageSizes").Value;            

            // Отримання рядка значень розмірів зображень із конфігурації.

            var sizes = imageSizes.Split(",");
            // Розділення рядка розмірів на масив строк за роздільником коми.

            string imageName = Guid.NewGuid().ToString() + ".webp";
            // Генерація унікального імені файлу для збереження зображення.

            foreach (var size in sizes)
            {
                // Цикл для обробки кожного розміру зображення.

                int width = int.Parse(size);
                // Парсинг строкового розміру в ціле число.

                var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                // Формування шляху до папки для збереження зображення.

                var bytes = ImageProcessingHelper.ResizeImage(image, width, width);
                // Зменшення розміру зображення з використанням ImageProcessingHelper.

                System.IO.File.WriteAllBytes(Path.Combine(dir, size + "_" + imageName), bytes);
                // Збереження зменшеного зображення з вказаним розміром та унікальним іменем файлу.
            }

            return imageName;
            // Повернення унікального імені файлу, в якому збережено зображення.
        }

        public string ImageSave(string url)
        {
            // Метод для збереження зображення, отриманого з вказаного URL.

            string imageName = Guid.NewGuid().ToString() + ".webp";
            // Генерація унікального імені файлу для збереження зображення.

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Створення об'єкта HttpClient для виконання HTTP-запитів.

                    HttpResponseMessage response = client.GetAsync(url).Result;
                    // Виконання асинхронного GET-запиту до вказаного URL та отримання відповіді.

                    if (response.IsSuccessStatusCode)
                    {
                        // Перевірка, чи статус відповіді вказує на успішне завершення (наприклад, 200 OK).

                        byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                        // Зчитування байтів зображення із вмісту відповіді.

                        var imageSizes = _configuration.GetSection("ImageSizes").Value;
                        // Отримання рядка значень розмірів зображень із конфігурації.

                        var sizes = imageSizes.Split(",");
                        // Розділення рядка розмірів на масив строк за роздільником коми.

                        foreach (var size in sizes)
                        {
                            // Цикл для обробки кожного розміру зображення.

                            int width = int.Parse(size);
                            // Парсинг строкового розміру в ціле число.

                            var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                            // Формування шляху до папки для збереження зображення.

                            var bytes = ImageProcessingHelper.ResizeImage(imageBytes, width, width);
                            // Зменшення розміру зображення з використанням ImageProcessingHelper.

                            System.IO.File.WriteAllBytes(Path.Combine(dir, size + "_" + imageName), bytes);
                            // Збереження зменшеного зображення з вказаним розміром та унікальним іменем файлу.
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve image. Status code: {response.StatusCode}");
                        // Виведення повідомлення про невдалу спробу отримати зображення, якщо статус відповіді не вказує на успіх.
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Виведення повідомлення про помилку, якщо виникає виняток під час обробки запиту.
            }

            return imageName;
            // Повернення унікального імені файлу, в якому збережено зображення.
        }

        public void RemoveImage(string name)
        {
            // Метод для видалення зображення за вказаним іменем файлу.

            var imageSizes = _configuration.GetSection("ImageSizes").Value;
            // Отримання рядка значень розмірів зображень із конфігурації.

            var sizes = imageSizes.Split(",");
            // Розділення рядка розмірів на масив строк за роздільником коми.

            string baseImagePath = name;
            // Задання базового імені файлу для видалення.

            foreach (var size in sizes)
            {
                // Цикл для обробки кожного розміру зображення.

                string imagePathToDelete = Path.Combine(Directory.GetCurrentDirectory(), "images", size + "_" + baseImagePath);
                // Формування шляху до файлу, який потрібно видалити.

                if (File.Exists(imagePathToDelete))
                {
                    File.Delete(imagePathToDelete);
                    // Перевірка і видалення файлу, якщо він існує.
                }
            }
        }        
    }
}
