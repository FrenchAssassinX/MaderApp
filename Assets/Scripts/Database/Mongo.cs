using MongoDB.Driver;
using UnityEngine;

public class Mongo
{
    private const string MONGO_URI = "mongodb+srv://Aurelien:root@cluster0-rh08y.mongodb.net/test?retryWrites=true&w=majority"; // URI is a string contains username, password and the address for the database
    private const string DATABASE_NAME = "Cluster0"; // This name have to match with the Mongo database name

    // Get this from the MongoDB.Driver 
    private MongoClient client;     
    private MongoServer server;
    private MongoDatabase database;

    // Function to initazlize client, server and database
    public void Init()
    {
        client = new MongoClient(MONGO_URI);
        server = client.GetServer();
        database = server.GetDatabase(DATABASE_NAME);

        // Initializing collections of datas (get from tables of the database)

        // Verify if it is working
        Debug.Log("Databse has been initialized");
    }

    // Function to turn off the client, server and databse
    public void Shutdown()
    {
        client = null;
        server.Shutdown();
        database = null;
    }



}
