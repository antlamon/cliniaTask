using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.IO;
using System;

namespace CliniaApi.Services
{
    public class EmergencyService : IEmergencyService
    {
        private JsonDocument document;
        private List<IResource> resources;
        public EmergencyService()
        {
            resources = new List<IResource>();
            string json = File.ReadAllText("emergencies.json");
            document = JsonDocument.Parse(json);
            getResources();
        }

        private void getResources()
        {
            foreach (JsonElement element in document.RootElement.EnumerateArray())
            {
                string elem = element.ToString();
                string id = element.GetProperty("Id").GetString();
                string name = element.GetProperty("Name").GetProperty("iv").GetString();
                double lat = element.GetProperty("Lat").GetDouble();
                double lng = element.GetProperty("Lng").GetDouble();

                string streetNumber = element.GetProperty("StreetNumber").GetString();
                string route = element.GetProperty("Route").GetString();
                string place = element.GetProperty("Place").GetString();
                string regionCode = element.GetProperty("RegionCode").GetString();
                string postalCode = element.GetProperty("PostalCode").GetString();
                string countryCode = element.GetProperty("CountryCode").GetString();

                string address = $"{streetNumber} {route}, {place}, {regionCode}, {postalCode}, {countryCode}";

                if (element.TryGetProperty("EmergencySlugId", out var emergencySlugId))
                {
                    emergencySlugId.GetString();
                    resources.Add(new Emergency()
                    {
                        Id = id,
                        Name = name,
                        Lat = lat,
                        Lng = lng,
                        EmergencySlugId = emergencySlugId.GetString(),
                        Address = address
                    });
                }
                else
                {
                    resources.Add(new Hospital()
                    {
                        Id = id,
                        Name = name,
                        Lat = lat,
                        Lng = lng,
                        Address = address
                    });
                }
            }
        }

        public List<IResource> Search(string query, string point, string aroundRadius)
        {
            List<IResource> sortedList = resources;
            if (query != string.Empty)
            {
                sortedList = sortedList.Where(r => r.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            
            if (point != string.Empty)
            {
                string[] points = point.Split(",");
                double lat = double.Parse(points[0]);
                double lng = double.Parse(points[1]);

                double CalculateDistance(double x, double y)
                {
                    return Math.Pow(x - lat, 2) + Math.Pow(y - lng, 2);
                }

                if (aroundRadius != string.Empty)
                {
                    double maxDistance = Math.Pow(double.Parse(aroundRadius), 2);
                    sortedList = sortedList.Where(r => r is Emergency && CalculateDistance(r.Lat, r.Lng) <= maxDistance).ToList();
                }

                sortedList = sortedList.OrderBy(r => CalculateDistance(r.Lat, r.Lng)).ToList();
            }

            return sortedList;
        }
    }
}