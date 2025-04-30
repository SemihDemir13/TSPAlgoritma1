using System;
using System.Diagnostics;
using System.Globalization;
using TSPalgoritma.Algorithms;
using TSPalgoritma.Models;
using TSPalgoritma.Utils;
using TSPalgoritma.Algorithms;

class Program
{
    static void Main()
    {
        Console.WriteLine("Lütfen dataset boyutunu girin (örn: 51, 150, 318, 3038, 14051): ");
        string input = Console.ReadLine();

        string fileName = $"tsp_{input}_1.txt";
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName);

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Dosya bulunamadı: " + filePath);
            return;
        }

        var cities = FileHelper.LoadCitiesFromFile(filePath);

        var stopwatch = Stopwatch.StartNew();

        // 1. Aşama: Nearest Neighbor ile başlangıç çözümü
        var (nnPath, nnCost) = Tsp2OptSolver.SolveNearestNeighbor(cities);

        List<City> finalPath;
        double finalCost;

        if (cities.Count < 1000)
        {
            // Küçük datasetlerde tam 2-opt uygula
            (finalPath, finalCost) = Tsp2OptSolver.Optimize2Opt(nnPath);
        }
        else
        {
            // Büyük datasetlerde süre sınırlı 2-opt uygula (örneğin 60 saniye)
            double timeLimitSeconds = 1200; // İstersen bunu artırabilirsin: 120, 180, 300...
            (finalPath, finalCost) = Tsp2OptSolver.Optimize2OptWithTimeLimit(nnPath, timeLimitSeconds);
        }

        stopwatch.Stop();

      
        Console.WriteLine("Optimal maliyet değeri: " + finalCost.ToString("F2", CultureInfo.InvariantCulture));

        Console.WriteLine("\nOptimal path:");
        foreach (var city in finalPath)
            Console.Write(city.Id + " → ");
        Console.WriteLine(finalPath[0].Id); // Döngü tamamlansın diye başlangıca dönüş

        
    }
}