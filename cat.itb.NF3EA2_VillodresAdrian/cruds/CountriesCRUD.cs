using cat.itb.NF3EA1_VillodresAdrian.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UF3_test.connections;

namespace cat.itb.NF3EA2_VillodresAdrian.cruds
{
    public class CountriesCRUD
    {

        public void LoadCountriesCollection()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("countries");
            var collection = database.GetCollection<BsonDocument>("countries");

            FileInfo file = new FileInfo("../../../files/countries.json");

            using (StreamReader sr = file.OpenText())
            {
                string jsonContent = sr.ReadToEnd();
                List<Country> countries = JsonConvert.DeserializeObject<List<Country>>(jsonContent);

                foreach (var country in countries)
                {
                    Console.WriteLine(country.name);
                    string json = JsonConvert.SerializeObject(country);
                    var document = new BsonDocument();
                    document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(document);
                }
            }
        }

        public void SelectEuropePopulation()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<Country>("countries");

            var filter = Builders<Country>.Filter.Eq(c => c.region, "Europe");
            var europeanCountries = collection.Find(filter).ToList();

            int totalPopulation = europeanCountries.Sum(c => c.population);

            Console.WriteLine($"La població total a Europa es: {totalPopulation}");
        }

        public void SelectMadagascarInfo()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("countries");

            var filter = Builders<BsonDocument>.Filter.Eq("name", "Madagascar");
            var doc = collection.Find(filter).FirstOrDefault();

            if (doc != null)
            {
                var capital = doc["capital"].AsString;
                var poblacion = doc["population"].ToInt32();
                var latlng = doc["latlng"].AsBsonArray;

                Console.WriteLine("Informació de Madagascar:");
                Console.WriteLine($"Capital: {capital}");
                Console.WriteLine($"Població: {poblacion}");
                Console.WriteLine($"LatLng: {string.Join(", ", latlng)}");
            }
            else
            {
                Console.WriteLine("No s'ha trobat Madagascar.");
            }
        }

        public void AddCallingCodeToACountry(string country)
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("countries");

            var filter = Builders<BsonDocument>.Filter.Eq("name", country);

            var beforeUpdate = col.Find(filter).FirstOrDefault();
            Console.WriteLine("Abans de l'actualització:");
            Console.WriteLine(beforeUpdate?.ToJson() ?? "No s'ha trobat cap document.");
            Console.WriteLine(new string('-', 50));

            var update = Builders<BsonDocument>.Update.AddToSet("callingCodes", "356");

            col.UpdateOne(filter, update);

            var afterUpdate = col.Find(filter).FirstOrDefault();
            Console.WriteLine("Després de l'actualització:");
            Console.WriteLine(afterUpdate?.ToJson() ?? "No s'ha trobat el document.");
        }

        public void DeleteCountriesWhereSpanishIsSpoken()
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("countries");

            var filter = Builders<BsonDocument>.Filter.Eq("languages.name", "Spanish");

            var result = col.DeleteMany(filter);

            Console.WriteLine($"Països eliminats on es parla espanyol: {result.DeletedCount}");
        }


    }
}
