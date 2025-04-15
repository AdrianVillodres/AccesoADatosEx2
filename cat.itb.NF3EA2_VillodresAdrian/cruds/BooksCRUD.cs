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
    public class BooksCRUD
    {
        public void LoadBooksCollection()
        {
            FileInfo file = new FileInfo("../../../files/books.json");
            StreamReader sr = file.OpenText();
            string fileString = sr.ReadToEnd();
            sr.Close();
            List<Book> books = JsonConvert.DeserializeObject<List<Book>>(fileString);

            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("books");
            var collection = database.GetCollection<BsonDocument>("books");

            if (books != null)
                foreach (var book in books)
                {
                    Console.WriteLine(book.title);
                    string json = JsonConvert.SerializeObject(book);
                    var document = new BsonDocument();
                    document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(document);
                }
        }
        public void SelectBooksOrderByPage()
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("books");

            var llibres = col.Find(new BsonDocument())
                             .Sort(Builders<BsonDocument>.Sort.Descending("pageCount"))
                             .ToList();

            foreach (var llibre in llibres)
            {
                var titol = llibre["title"];
                var pagines = llibre["pageCount"];
                var categories = llibre.Contains("categories") ? string.Join(", ", llibre["categories"].AsBsonArray) : "Cap";

                Console.WriteLine($"Títol: {titol}");
                Console.WriteLine($"Pàgines: {pagines}");
                Console.WriteLine($"Categories: {categories}");
                Console.WriteLine("--------------------------");
            }
        }

        public void SelectBooksLessThan(int pages)
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("books");

            var filter = Builders<BsonDocument>.Filter.Lt("pageCount", pages);
            var llibres = col.Find(filter).ToList();

            Console.WriteLine("Llibres amb menys de 130 pàgines:\n");

            foreach (var llibre in llibres)
            {
                var titol = llibre.GetValue("title", new BsonString("Sense títol"));
                var pagines = llibre.GetValue("pageCount", new BsonInt32(0));
                var autors = llibre.Contains("authors") ? string.Join(", ", llibre["authors"].AsBsonArray.Select(a => a.ToString())) : "Sense autor";

                Console.WriteLine($"Títol: {titol}");
                Console.WriteLine($"Pàgines: {pagines}");
                Console.WriteLine($"Autors: {autors}");
                Console.WriteLine("--------------------------");
            }
        }

        public void AddAuthorToBook()
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("books");

            var filter = Builders<BsonDocument>.Filter.Eq("title", "Code Generation in Action");

            var bookBefore = col.Find(filter).FirstOrDefault();
            if (bookBefore != null)
            {
                var autorsAntics = bookBefore.Contains("authors")
                    ? string.Join(", ", bookBefore["authors"].AsBsonArray.Select(a => a.ToString()))
                    : "Sense autors";

                Console.WriteLine("Abans de l'actualització:");
                Console.WriteLine($"Autors: {autorsAntics}");
            }
            else
            {
                Console.WriteLine("No s'ha trobat el llibre amb el títol especificat.");
                return;
            }

            var update = Builders<BsonDocument>.Update.Push("authors", "Sam Watters");
            col.UpdateOne(filter, update);

            var bookAfter = col.Find(filter).FirstOrDefault();
            if (bookAfter != null)
            {
                var autorsNous = bookAfter.Contains("authors")
                    ? string.Join(", ", bookAfter["authors"].AsBsonArray.Select(a => a.ToString()))
                    : "Sense autors";

                Console.WriteLine("\nDesprés de l'actualització:");
                Console.WriteLine($"Autors: {autorsNous}");
            }
        }

        public void DeleteBooksBetweenPages(int page1, int page2)
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("books");

            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Gte("pageCount", page1),
                Builders<BsonDocument>.Filter.Lte("pageCount", page2)
            );

            var result = col.DeleteMany(filter);

            Console.WriteLine($"Nombre de llibres eliminats: {result.DeletedCount}");
        }

        public void DeleteLastCategoryFromBookByIsbn(string isbn)
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("books");

            var filter = Builders<BsonDocument>.Filter.Eq("isbn", isbn);

            var book = col.Find(filter).FirstOrDefault();

            if (book == null)
            {
                Console.WriteLine($"No s'ha trobat cap llibre amb ISBN {isbn}");
                return;
            }

            if (!book.Contains("categories") || !book["categories"].AsBsonArray.Any())
            {
                Console.WriteLine($"El llibre amb ISBN {isbn} no té categories o ja està buit.");
                return;
            }

            var categories = book["categories"].AsBsonArray;
            var lastCategory = categories.Last().ToString();

            var update = Builders<BsonDocument>.Update.Pull("categories", lastCategory);
            var result = col.UpdateOne(filter, update);

            Console.WriteLine($"Categoria eliminada: {lastCategory}");
            Console.WriteLine($"Documents modificats: {result.ModifiedCount}");
        }



    }
}
