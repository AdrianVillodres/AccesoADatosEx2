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
    public class RestaurantsCRUD
    {
        public void LoadRestaurantsCollection()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("restaurants");
            var collection = database.GetCollection<BsonDocument>("restaurants");

            FileInfo file = new FileInfo("../../../files/restaurants.json");

            using (StreamReader sr = file.OpenText())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Restaurant restaurants = JsonConvert.DeserializeObject<Restaurant>(line);
                    if (restaurants.name != null && !string.IsNullOrEmpty(restaurants.name))
                    {
                        Console.WriteLine(restaurants.name);
                    }
                    string json = JsonConvert.SerializeObject(restaurants);
                    var document = new BsonDocument();
                    document = BsonDocument.Parse(json);
                    collection.InsertOne(document);
                }
            }
        }

        public void ShowRestaurantByZipcode(string zipcode)
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("restaurants");

            var filter = Builders<BsonDocument>.Filter.Eq("address.zipcode", zipcode);
            var restaurants = col.Find(filter).ToList();

            Console.WriteLine($"Restaurants amb codi postal {zipcode}:\n");

            foreach (var r in restaurants)
            {
                var nom = r["name"];
                var cuina = r["cuisine"];
                Console.WriteLine($"Nom: {nom}");
                Console.WriteLine($"Cuina: {cuina}");
                Console.WriteLine("--------------------");
            }
        }
        public void SelectBronxChineseRestaurants()
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("restaurants");

            var filtre = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("borough", "Bronx"),
                Builders<BsonDocument>.Filter.Eq("cuisine", "Chinese")
            );

            var restaurants = col.Find(filtre).ToList();

            Console.WriteLine("Restaurants xinesos al Bronx:\n");

            foreach (var r in restaurants)
            {
                Console.WriteLine(r.ToJson());
                Console.WriteLine(new string('-', 50));
            }
        }

        public void UpdateZipcodeDriggsAvenue()
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("restaurants");

            var filter = Builders<BsonDocument>.Filter.Eq("address.street", "Driggs Avenue");

            var docAbans = col.Find(filter).FirstOrDefault();
            if (docAbans != null)
            {
                var oldZip = docAbans["address"]["zipcode"];
                Console.WriteLine($"Zipcode abans: {oldZip}");
            }
            else
            {
                Console.WriteLine("No s'ha trobat cap restaurant a Driggs Avenue.");
                return;
            }

            var update = Builders<BsonDocument>.Update.Set("address.zipcode", "10443");
            col.UpdateMany(filter, update);

            var docDespres = col.Find(filter).FirstOrDefault();
            var newZip = docDespres["address"]["zipcode"];
            Console.WriteLine($"Zipcode després: {newZip}");
        }

        public void DeleteRestaurantsInManhattan()
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("restaurants");

            var filter = Builders<BsonDocument>.Filter.Eq("borough", "Manhattan");

            var result = col.DeleteMany(filter);

            Console.WriteLine($"Número de restaurants eliminats: {result.DeletedCount}");
        }


    }
}
