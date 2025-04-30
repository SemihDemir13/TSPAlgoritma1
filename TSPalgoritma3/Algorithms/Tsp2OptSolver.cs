using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSPalgoritma.Models;

namespace TSPalgoritma.Algorithms
{
    public class Tsp2OptSolver
    {
        // Nearest Neighbor başlangıç çözümü
        public static (List<City> path, double cost) SolveNearestNeighbor(List<City> cities)
        {
            if (cities == null || cities.Count == 0)
                return (new List<City>(), 0);

            var unvisited = new HashSet<City>(cities);
            var path = new List<City>();
            var current = cities[0];
            path.Add(current);
            unvisited.Remove(current);

            while (unvisited.Count > 0)
            {
                var nearest = unvisited.OrderBy(c => current.DistanceTo(c)).First();
                path.Add(nearest);
                unvisited.Remove(nearest);
                current = nearest;
            }

            double totalDistance = GetTotalDistance(path);
            return (path, totalDistance);
        }

        // Sınırsız 2-opt (küçük datasetler için)
        public static (List<City> path, double cost) Optimize2Opt(List<City> initialPath)
        {
            var bestPath = new List<City>(initialPath);
            double bestDistance = GetTotalDistance(bestPath);
            bool improved = true;

            while (improved)
            {
                improved = false;

                for (int i = 1; i < bestPath.Count - 2; i++)
                {
                    for (int j = i + 1; j < bestPath.Count - 1; j++)
                    {
                        var newPath = TwoOptSwap(bestPath, i, j);
                        double newDistance = GetTotalDistance(newPath);

                        if (newDistance < bestDistance)
                        {
                            bestPath = newPath;
                            bestDistance = newDistance;
                            improved = true;
                        }
                    }
                }
            }

            return (bestPath, bestDistance);
        }

        // Süre sınırı ile 2-opt (büyük datasetler için)
        public static (List<City> path, double cost) Optimize2OptWithTimeLimit(List<City> initialPath, double timeLimitSeconds)
        {
            var bestPath = new List<City>(initialPath);
            double bestDistance = GetTotalDistance(bestPath);
            bool improved = true;

            var stopwatch = Stopwatch.StartNew();

            while (improved && stopwatch.Elapsed.TotalSeconds < timeLimitSeconds)
            {
                improved = false;

                for (int i = 1; i < bestPath.Count - 2; i++)
                {
                    for (int j = i + 1; j < bestPath.Count - 1; j++)
                    {
                        if (stopwatch.Elapsed.TotalSeconds >= timeLimitSeconds)
                            break;

                        var newPath = TwoOptSwap(bestPath, i, j);
                        double newDistance = GetTotalDistance(newPath);

                        if (newDistance < bestDistance)
                        {
                            bestPath = newPath;
                            bestDistance = newDistance;
                            improved = true;
                        }
                    }

                    if (stopwatch.Elapsed.TotalSeconds >= timeLimitSeconds)
                        break;
                }
            }

            return (bestPath, bestDistance);
        }

        // 2-opt takası
        private static List<City> TwoOptSwap(List<City> path, int i, int k)
        {
            var newPath = new List<City>();
            newPath.AddRange(path.Take(i));
            newPath.AddRange(path.Skip(i).Take(k - i + 1).Reverse());
            newPath.AddRange(path.Skip(k + 1));
            return newPath;
        }

        // Toplam mesafe hesaplama
        public static double GetTotalDistance(List<City> path)
        {
            double total = 0;
            for (int i = 0; i < path.Count - 1; i++)
                total += path[i].DistanceTo(path[i + 1]);

            total += path[^1].DistanceTo(path[0]); // Son şehirden başlangıca dönüş
            return total;
        }
    }
}
