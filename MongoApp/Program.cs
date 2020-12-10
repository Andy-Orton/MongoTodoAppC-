using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoApp
{
    class Program
    { 

        static void Main(string[] args)
        {
            MongoClient mongoClient;
            Console.WriteLine("Todo List Application Using Mongo");
            try
            {
                mongoClient = new MongoClient("mongodb+srv://andy:HelloThere@mongoappcluster.yvcym.mongodb.net/sample_airbnb.listingsAndReviews?retryWrites=true&w=majority");
            }
            catch(Exception e)
            {
                Console.WriteLine("Database Connection Issue");
                Console.WriteLine(e.ToString());
                return;
            }
            var database = mongoClient.GetDatabase("TodoItems");
            var collection = database.GetCollection<BsonDocument>("Todos");
            int choice = -1;
            while (choice != 0)
            {
                Console.WriteLine("Current Todos");
                var allTodos = collection.Find(_ => true).ToList();
                Console.WriteLine("Id | Todo | isFinished");
                foreach(var c in allTodos)
                {
                    Console.WriteLine(c[1] + " | " + c[2] + " | " + c[3]);
                }
                Console.WriteLine("1. Insert Another Todo");
                Console.WriteLine("2. Mark Todo as Done");
                Console.WriteLine("3. Delete Todo");
                Console.WriteLine("4. Delete All Marked As Done");
                Console.WriteLine("0. Exit Program");
                choice = int.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    Console.WriteLine("Todo Description: ");
                    string description = Console.ReadLine();
                    var newTodo = new BsonDocument { { "Todo_id", collection.EstimatedDocumentCount() + 1 }, { "description", description }, { "IsDone", false } };
                    collection.InsertOne(newTodo);
                }
                else if (choice == 2)
                {
                    Console.WriteLine("Which todo do you want to mark as done? Insert ID");
                    var todoId = Console.ReadLine();
                    var Filter = Builders<BsonDocument>.Filter.Eq("Todo_id", int.Parse(todoId));
                    var Update = Builders<BsonDocument>.Update.Set("IsDone", true);
                    collection.UpdateOne(Filter, Update);
                }
                else if (choice == 3)
                {
                    Console.WriteLine("Which todo do you want to delete? Insert ID");
                    var todoId = Console.ReadLine();
                    var filter = Builders<BsonDocument>.Filter.Eq("Todo_id", int.Parse(todoId));
                    collection.DeleteOne(filter);
                }
                else if (choice == 4)
                {
                    Console.WriteLine("Deleting All Todos that are Completed");
                    var filter = Builders<BsonDocument>.Filter.Eq("IsDone", true);
                    collection.DeleteMany(filter);
                }
                else if (choice == 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter a valid input");
                }
            }
        }
    }
}
