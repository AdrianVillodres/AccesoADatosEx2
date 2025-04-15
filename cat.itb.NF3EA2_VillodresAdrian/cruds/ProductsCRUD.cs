using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UF3_test.connections;
using UF3_test.model;

namespace cat.itb.NF3EA2_VillodresAdrian.cruds
{
    public class ProductsCRUD
    {
        public void LoadProductsCollection()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("products");
            var collection = database.GetCollection<BsonDocument>("products");

            FileInfo file = new FileInfo("../../../files/products.json");

            using (StreamReader sr = file.OpenText())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Product product = JsonConvert.DeserializeObject<Product>(line);
                    Console.WriteLine(product.name);
                    string json = JsonConvert.SerializeObject(product);
                    var document = new BsonDocument();
                    document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(document);
                }
            }
        }

        public void AddStockMinimToExpensiveProducts()
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("products");

            var filter = Builders<BsonDocument>.Filter.Gt("price", 2000);

            var update = Builders<BsonDocument>.Update.Set("stockminim", 20);

            var result = col.UpdateMany(filter, update);

            Console.WriteLine($"Documents actualitzats: {result.ModifiedCount}\n");

            var updatedDocs = col.Find(filter).ToList();

            foreach (var doc in updatedDocs)
            {
                Console.WriteLine(doc.ToJson());
                Console.WriteLine(new string('-', 50));

            }
        }

        public void AddGamaFieldToProducts()
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("products");

            var baixaFilter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Gte("price", 1),
                Builders<BsonDocument>.Filter.Lte("price", 500)
            );
            var mitjaFilter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Gte("price", 501),
                Builders<BsonDocument>.Filter.Lte("price", 2000)
            );
            var extraFilter = Builders<BsonDocument>.Filter.Gt("price", 2000);

            var updateBaixa = Builders<BsonDocument>.Update.Set("gama", "baixa");
            var updateMitja = Builders<BsonDocument>.Update.Set("gama", "mitja");
            var updateExtra = Builders<BsonDocument>.Update.Set("gama", "extra");

            var resultBaixa = col.UpdateMany(baixaFilter, updateBaixa);
            var resultMitja = col.UpdateMany(mitjaFilter, updateMitja);
            var resultExtra = col.UpdateMany(extraFilter, updateExtra);

            Console.WriteLine($"Documents actualitzats (baixa): {resultBaixa.ModifiedCount}");
            Console.WriteLine($"Documents actualitzats (mitja): {resultMitja.ModifiedCount}");
            Console.WriteLine($"Documents actualitzats (extra): {resultExtra.ModifiedCount}");

            Console.WriteLine("\n📦 Productes amb camp 'gama' afegit:\n");

            var allWithGama = col.Find(new BsonDocument { { "gama", new BsonDocument { { "$exists", true } } } }).ToList();

            foreach (var doc in allWithGama)
            {
                Console.WriteLine(doc.ToJson());
                Console.WriteLine(new string('-', 50));
            }
        }

        public void UpdateProductCategory(string productName, string oldCategory, string newCategory)
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("products");

            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("name", productName),
                Builders<BsonDocument>.Filter.Eq("categories", oldCategory)
            );

            var beforeUpdate = col.Find(filter).FirstOrDefault();
            Console.WriteLine("Abans de l'actualització:");
            Console.WriteLine(beforeUpdate?.ToJson() ?? "No s'ha trobat cap document.");
            Console.WriteLine(new string('-', 50));

            var update = Builders<BsonDocument>.Update.Set("categories.$[elem]", newCategory);

            var arrayFilter = new List<ArrayFilterDefinition>
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("elem", oldCategory))
            };

            var updateOptions = new UpdateOptions { ArrayFilters = arrayFilter };

            var result = col.UpdateOne(filter, update, updateOptions);

            var afterUpdate = col.Find(Builders<BsonDocument>.Filter.Eq("name", productName)).FirstOrDefault();
            Console.WriteLine("Després de l'actualització:");
            Console.WriteLine(afterUpdate?.ToJson() ?? "No s'ha trobat el document.");
        }

        public void UpdateStockForPriceRange()
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("products");

            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Gte("price", 800),
                Builders<BsonDocument>.Filter.Lte("price", 1000)
            );

            var update = Builders<BsonDocument>.Update.Set("stock", 60);

            var result = col.UpdateMany(filter, update);

            Console.WriteLine($"Documents actualitzats: {result.ModifiedCount}\n");

            var updatedDocs = col.Find(filter).ToList();

            Console.WriteLine("Documents actualitzats:\n");

            foreach (var doc in updatedDocs)
            {
                Console.WriteLine(doc.ToJson());
                Console.WriteLine(new string('-', 50));
            }
        }

        public void DeleteFirstCategoryFromProduct(string product)
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("products");

            var filter = Builders<BsonDocument>.Filter.Eq("name", product);

            var before = col.Find(filter).FirstOrDefault();

            if (before == null)
            {
                Console.WriteLine("No s'ha trobat cap producte anomenat " + product);
                return;
            }

            Console.WriteLine("Abans de l'actualització:");
            Console.WriteLine(before.ToJson());
            Console.WriteLine(new string('-', 50));

            if (!before.Contains("categories") || !before["categories"].AsBsonArray.Any())
            {
                Console.WriteLine("El producte no té categories per eliminar.");
                return;
            }

            var firstCategory = before["categories"].AsBsonArray[0].AsString;

            var update = Builders<BsonDocument>.Update.Pull("categories", firstCategory);
            col.UpdateOne(filter, update);

            var after = col.Find(filter).FirstOrDefault();

            Console.WriteLine("Després de l'actualització:");
            Console.WriteLine(after?.ToJson() ?? "No s'ha trobat el document.");
        }

        public void DeleteProductByName(string productName)
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("products");

            var filter = Builders<BsonDocument>.Filter.Eq("name", productName);

            var result = col.DeleteOne(filter);

            Console.WriteLine($"Nombre de productes eliminats amb el nom \"{productName}\": {result.DeletedCount}");
        }



        public void DeleteProductsByCategory(string category)
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("products");

            var filter = Builders<BsonDocument>.Filter.AnyEq("categories", category);

            var result = col.DeleteMany(filter);

            Console.WriteLine($"Nombre de productes eliminats amb la categoria \"{category}\": {result.DeletedCount}");
        }


    }
}
