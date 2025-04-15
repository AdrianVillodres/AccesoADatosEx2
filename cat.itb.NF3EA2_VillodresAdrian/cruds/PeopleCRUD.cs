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
    public class PeopleCRUD
    {
        public void LoadPeopleCollection()
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
                    document.Add(BsonDocument.Parse(json));
                    collection.InsertOne(document);
                }
        }

        public void SelectPersonFriends(string person)
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("people");

            var filter = Builders<BsonDocument>.Filter.Eq("name", person);
            var persona = col.Find(filter).FirstOrDefault();

            if (persona != null && persona.Contains("friends"))
            {
                var amics = persona["friends"].AsBsonArray;

                Console.WriteLine("Amics de " + person);

                foreach (var amic in amics)
                {
                    var nom = amic["name"];
                    Console.WriteLine($"- {nom}");
                }
            }
            else
            {
                Console.WriteLine("No s'ha trobat Caroline Webster o no té amics registrats.");
            }
        }

        public void DeleteTagsFromTeachers()
        {
            var db = MongoLocalConnection.GetDatabase("itb");
            var col = db.GetCollection<BsonDocument>("people");

            var filter = Builders<BsonDocument>.Filter.Eq("randomArrayItem", "teacher");
            var update = Builders<BsonDocument>.Update.Unset("tags");

            var result = col.UpdateMany(filter, update);

            Console.WriteLine($"Camp 'tags' eliminat de {result.ModifiedCount} professors.");
        }


    }
}
