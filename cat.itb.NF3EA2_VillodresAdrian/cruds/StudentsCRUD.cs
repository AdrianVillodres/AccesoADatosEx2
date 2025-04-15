using cat.itb.NF3EA1_VillodresAdrian.Model;
using MongoDB.Bson;
using Newtonsoft.Json;
using UF3_test.connections;

namespace cat.itb.NF3EA2_VillodresAdrian.cruds
{
    public class StudentsCRUD
    {
        public void LoadStudentsCollection()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("students");
            var collection = database.GetCollection<BsonDocument>("students");

            FileInfo file = new FileInfo("../../../files/students.json");

            using (StreamReader sr = file.OpenText())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Student students = JsonConvert.DeserializeObject<Student>(line);
                    if (students.firstname != null && !string.IsNullOrEmpty(students.firstname))
                    {
                        Console.WriteLine(students.firstname);
                    }
                    string json = JsonConvert.SerializeObject(students);
                    var document = BsonDocument.Parse(json);
                    document = BsonDocument.Parse(json);
                    collection.InsertOne(document);
                }
            }
        }
    }
}