using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using UF3_test.connections;
using UF3_test.model;

namespace UF3_test
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //EA1
            //GetAllDBs();         
            //GetCollections();
            //SelectAllStudents();
            //InsertOneStudent();
            //SelectOneStudent();
            //SelectStudentFields();            
            //LoadPeopleCollection();
            //LoadBooksCollection();
            //LoadProductsCollection();            


            //EA2
            //LoadPeopleCollection();
            //SelectPeopleByFriend();
            //SelectPeopleByAge();
            //UpdateOnePerson();
            //UpdateManyPeople();
            //DeleteOnePerson();
            //DeleteManyPeople();
            //UpdatePeopleArrayPopLast();
            //UpdatePeopleNewField();
        }

        private static void GetAllDBs()
        {


            var dbClient = MongoLocalConnection.GetMongoClient();

            var dbList = dbClient.ListDatabases().ToList();
            Console.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
            {
                Console.WriteLine(db);
            }
            Console.ReadKey();
            Console.Clear();

        }

        private static void GetCollections()
        {

            var database = MongoLocalConnection.GetDatabase("sample_training");

            var colList = database.ListCollections().ToList();
            Console.WriteLine("The list of collection on this database is: ");
            foreach (var col in colList)
            {
                Console.WriteLine(col);
            }
            Console.ReadKey();
            Console.Clear();
        }

        private static void SelectAllStudents()
        {


            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var studentDocuments = collection.Find(new BsonDocument()).ToList();

            foreach (var student in studentDocuments)
            {
                Console.WriteLine(student.ToString());
            }
            Console.ReadKey();
            Console.Clear();

        }

        private static void InsertOneStudent()
        {

            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var document = new BsonDocument
            {
                { "student_id", 9999923 },
                { "scores", new BsonArray
                {
                        new BsonDocument{ {"type", "exam"}, {"score", 88.12334193287023 } },
                        new BsonDocument{ {"type", "quiz"}, {"score", 74.92381029342834 } },
                        new BsonDocument{ {"type", "homework"}, {"score", 89.97929384290324 } },
                        new BsonDocument{ {"type", "homework"}, {"score", 82.12931030513218 } }
                    }
                },
                { "class_id", 480}
            };


            collection.InsertOne(document);
            Console.WriteLine(document.GetElement("student_id").ToString() + "   inserted");
            Console.ReadKey();
            Console.Clear();

        }

        private static void SelectOneStudent()
        {


            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var filter = Builders<BsonDocument>.Filter.Eq("student_id", 9999923);
            var studentDocument = collection.Find(filter).FirstOrDefault();
            Console.WriteLine(studentDocument.ToString());
            Console.ReadKey();
            Console.Clear();

        }


        private static void SelectStudentFields()
        {

            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var filter = Builders<BsonDocument>.Filter.Eq("student_id", 9999923);
            var studentDocument = collection.Find(filter).FirstOrDefault();
            var id = studentDocument.GetElement("student_id");
            var scores = studentDocument.GetElement("scores");

            Console.WriteLine(id.ToString());
            Console.WriteLine(scores.ToString());
            Console.ReadKey();
            Console.Clear();


        }

        private static void LoadPeopleCollection()
        {
            FileInfo file = new FileInfo("../../../files/people.json");
            StreamReader sr = file.OpenText();
            string fileString = sr.ReadToEnd();
            sr.Close();
            List<Person> people = JsonConvert.DeserializeObject<List<Person>>(fileString);

            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("people");
            var collection = database.GetCollection<BsonDocument>("people");

            if (people != null)
                foreach (var person in people)
                {
                    Console.WriteLine(person.name);
                    string json = JsonConvert.SerializeObject(person);
                    var document = new BsonDocument();
                    //document.Add(BsonDocument.Parse(json));
                    document.AddRange(BsonDocument.Parse(json));
                    collection.InsertOne(document);
                }
            Console.ReadKey();
            Console.Clear();

        }

        private static void LoadBooksCollection()
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
                    //document.Add(BsonDocument.Parse(json));
                    document.AddRange(BsonDocument.Parse(json));
                    collection.InsertOne(document);
                }
            Console.ReadKey();
            Console.Clear();

        }

        private static void LoadProductsCollection()
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
                    //document.Add(BsonDocument.Parse(json));
                    document.AddRange(BsonDocument.Parse(json)); 
                    collection.InsertOne(document);
                }
            }
            Console.ReadKey();
            Console.Clear();

        }

        private static void SelectPeopleByFriend()
        {

            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

          
            var friendFilter1 = Builders<BsonDocument>.Filter.Eq("friends.name", "Serenity Watson");

            var people = collection.Find(friendFilter1).ToList();

            foreach (var person in people)
            {
                Console.WriteLine(person.ToString());
            }

            Console.WriteLine();

            var friendFilter2 = Builders<BsonDocument>.Filter.ElemMatch<BsonValue>(
                "friends", new BsonDocument { { "name", "Rachel Hancock" } });

            var cursor = collection.Find(friendFilter2).ToCursor();
            foreach (var document in cursor.ToEnumerable())
            {
                Console.WriteLine(document.ToString());
                Console.WriteLine();

            }
            Console.ReadKey();
            Console.Clear();
        }
        private static void SelectPeopleByAge()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var ageFilter = Builders<BsonDocument>.Filter.Gt("age", 38);
            //var cursor = collection.Find(ageFilter).ToCursor();

            var sort = Builders<BsonDocument>.Sort.Descending("age");
            var cursor = collection.Find(ageFilter).Sort(sort).ToCursor();

            foreach (var document in cursor.ToEnumerable())
            {
                Console.WriteLine(document.ToString());
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();
        }

        private static void UpdateOnePerson()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var filter = Builders<BsonDocument>.Filter.Eq("name", "Sophie Gibbs");
            var update = Builders<BsonDocument>.Update.Set("isActive", false);

            var personDoc1 = collection.Find(filter).First();

            var name1 = personDoc1.GetElement("name");
            var isActive1 = personDoc1.GetElement("isActive");

            Console.WriteLine(name1.ToString() + "  " + isActive1.ToString());
            collection.UpdateOne(filter, update);

            var personDoc2 = collection.Find(filter).First();

            var name2 = personDoc2.GetElement("name");
            var isActive2 = personDoc2.GetElement("isActive");

            Console.WriteLine(name2.ToString() + "  " + isActive2.ToString());

            Console.ReadKey();
            Console.Clear();

        }

        private static void UpdateManyPeople()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var filter = Builders<BsonDocument>.Filter.Eq("randomArrayItem", "teacher") & Builders<BsonDocument>
                .Filter.Eq("gender", "male");
            var update = Builders<BsonDocument>.Update.Set("company", "Treson");

            var docsUpdated = collection.UpdateMany(filter, update);
            Console.WriteLine("Docs modificats: " + docsUpdated.ModifiedCount);
            Console.WriteLine();

            var cursor = collection.Find(filter).ToCursor();

            foreach (var doc in cursor.ToEnumerable())
            {
                Console.WriteLine(doc.GetElement("company"));
                Console.WriteLine();

            }

            Console.ReadKey();
            Console.Clear();
        }

        private static void DeleteOnePerson()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var deleteFilter = Builders<BsonDocument>.Filter.Eq("name", "Eva Watson");

            var docsDeleted = collection.DeleteOne(deleteFilter);
            Console.WriteLine("Docs eliminats: " + docsDeleted.DeletedCount);

            Console.ReadKey();
            Console.Clear();

        }

        private static void DeleteManyPeople()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var deleteFilter = Builders<BsonDocument>.Filter.Lt("age", 23);

            var docsDeleted = collection.DeleteMany(deleteFilter);
            Console.WriteLine("Docs eliminats: " + docsDeleted.DeletedCount);

            Console.ReadKey();
            Console.Clear();
        }
        private static void UpdatePeopleArrayPopLast()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var filter = Builders<BsonDocument>.Filter.Eq("name", "Sophie Gibbs");
            var update = Builders<BsonDocument>.Update.PopLast("tags");

            var personDoc1 = collection.Find(filter).First();
            var tags1 = personDoc1.GetElement("tags");
            Console.WriteLine(tags1.ToString());

            collection.UpdateOne(filter, update);

            var personDoc2 = collection.Find(filter).First();
            var tags2 = personDoc2.GetElement("tags");
            Console.WriteLine(tags2.ToString());

            Console.ReadKey();
            Console.Clear();

        }

        private static void UpdatePeopleNewField()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            var collection = database.GetCollection<BsonDocument>("people");

            var filter = Builders<BsonDocument>.Filter.Eq("name", "Madelyn Murphy");
            var update = Builders<BsonDocument>.Update.Set("phone_aux", "666-470-34822");

            collection.UpdateOne(filter, update);

            var personDoc = collection.Find(filter).First();
            Console.WriteLine(personDoc.ToString());

            Console.ReadKey();
            Console.Clear();

        }
        
    }
}