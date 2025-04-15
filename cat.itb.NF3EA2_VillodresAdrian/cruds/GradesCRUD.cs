using cat.itb.NF3EA1_VillodresAdrian.Model;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UF3_test.connections;

namespace cat.itb.NF3EA2_VillodresAdrian.cruds
{
    public class GradesCRUD
    {
        public void LoadGradesCollection()
        {
            var database = MongoLocalConnection.GetDatabase("itb");
            database.DropCollection("grades");
            var collection = database.GetCollection<BsonDocument>("grades");

            FileInfo file = new FileInfo("../../../files/grades.json");

            using (StreamReader sr = file.OpenText())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Grade grades = JsonConvert.DeserializeObject<Grade>(line);
                    if (grades._id != null && !string.IsNullOrEmpty(grades._id.oid))
                    {
                        Console.WriteLine(grades._id.oid);
                    }
                    string json = JsonConvert.SerializeObject(grades);
                    var document = BsonDocument.Parse(json);
                    document.Remove("_id");
                    collection.InsertOne(document);
                }
            }
        }
    }
}
