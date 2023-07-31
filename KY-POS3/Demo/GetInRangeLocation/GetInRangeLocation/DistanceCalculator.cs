using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetInRangeLocation {
    public class Location {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class DistanceCalculator {
        public static double CalculateDistance(double currentLat, double currentLon, double targetLat, double targetLon) {
            const double earthRadiusKm = 6371.0; // Earth's radius in kilometers

            // Convert latitude and longitude from degrees to radians
            double dLat = DegreesToRadians(targetLat - currentLat);
            double dLon = DegreesToRadians(targetLon - currentLon);

            // Haversine formula
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(currentLat)) * Math.Cos(DegreesToRadians(targetLat)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate the distance
            double distance = earthRadiusKm * c;
            return distance;
        }

        private static double DegreesToRadians(double degrees) {
            return degrees * Math.PI / 180.0;
        }
    }
}
