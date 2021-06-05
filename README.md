# Terminal 3
eCommerce system. Workshop on Software Engineering Project.

## Usage
The initiation of the system needs to be in two phases. First starti up SignalR server, and after that start the Web API server
Setting up SignalR derver
```bash
dotnet run --project '.\Version 1\Terminal3\SignalRGateway\SignalRGateway.csproj'
```
Setting up Terminal3 WebAPI
```bash
dotnet run --project '.\Version 1\Terminal3\WebApplication3\Terminal3WebAPI.csproj'
```
## Configurate Terminal3
Our system support custom configuration for defaults attrobutes e.g default system admin credentials and external system API URL.
the configuration file (Config.json) located in '.\Version 1\Terminal3\Terminal3' and in json format with the following attributes:

```yaml
{
  "email": <defaultAdmin>,
  "password": <defaultPassword>,
  "signalRServer_url": <websocket server url>,
  "mongoDB_url": <mongoDB connection string> ,
  "externalSystem_url": <external system API URL>
}
```

In Addition you may want to test the framework by using written scenarios `Story` and provide it to the WebAPI. Providing such file will result execution of each function listen in the stories.json file.
Example of story.json format:

```yaml
{
  "story": [
    {
      "function": "Register",
      "args": [ "user2@email.com", "password2", "user2ID" ]
    },
    {
      "function": "Register",
      "args": [ "user3@email.com", "password3", "user3ID" ]
    },
    {
      "function": "Login",
      "args": [ "user2@email.com", "password2" ]
    },
    {
      "function": "OpenNewStore",
      "args": [ "Store1", "user2ID", "StoreID1" ]
    },
    {
      "function": "AddProductToStore",
      "args": [ "user2ID", "StoreID1", "Bamba", 30.0, 20, "Snacks", null ]
    },
    {
      "function": "AddStoreManager",
      "args": [ "user3ID", "user2ID", "StoreID1" ]
    }
  ]
}
```
