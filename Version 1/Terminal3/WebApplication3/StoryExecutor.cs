using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Terminal3.DomainLayer;
using Terminal3.ServiceLayer;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3WebAPI
{
    public interface IStoryExecutor { };
    public class StoryExecutor : IStoryExecutor
    {
        private readonly IECommerceSystem system;
        public StoryExecutor(IECommerceSystem system)
        {
            this.system = system;
        }
        public void Execute()
        {
            Dictionary<string, Delegate> functionDict = new Dictionary<string, Delegate>()
                                    {
                                        { "Register",new Func<string,string,string,Result<RegisteredUserService>>(system.Register)},
                                        { "Login",new Func< string, string,Result<RegisteredUserService>>(system.Login)},
                                        { "LogOut",new Func< string,Result<UserService>>(system.LogOut)},
                                        { "OpenNewStore",new Func< string, string,string,Result<StoreService>>(system.OpenNewStore)},
                                        { "AddProductToStore",new Func< String , String , String , double , int , String , LinkedList<String> ,Result<ProductService>>(system.AddProductToStore)},
                                        { "AddStoreManager",new Func< String , String , String ,Result<Boolean>>(system.AddStoreManager)},
                                        { "AddStoreOwner",new Func< String , String , String ,Result<Boolean>>(system.AddStoreOwner) }
                                    };

            string path = @"stories.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                StoryConfig storyConfig = JsonConvert.DeserializeObject<StoryConfig>(json);
                if (storyConfig.story is null)
                {
                    Logger.LogError("Invalid JSON format - One or more missing attribute has been found in the config JSON");
                    Environment.Exit(0);
                }
                foreach (Story story in storyConfig.story)
                {

                    functionDict[story.function].DynamicInvoke(convertArgs(functionDict[story.function], story.args));
                }
            }

        }

        public object[] convertArgs(Delegate function, object[] args)
        {
            object[] convertedArgs = new object[args.Length];
            ParameterInfo[] parameterInfos = function.Method.GetParameters();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is null) { convertedArgs[i] = null; continue; }
                ParameterInfo parameterInfo = parameterInfos[i];
                string type = parameterInfo.ParameterType.FullName;
                switch (type)
                {
                    case "System.String":
                        convertedArgs[i] = (String)args[i];
                        break;
                    case "System.Double":
                        Double d;
                        Double.TryParse((String)args[i], out d);
                        convertedArgs[i] = d;
                        break;
                    case "System.Int32":
                        Int32 integer;
                        Int32.TryParse((String)args[i], out integer);
                        convertedArgs[i] = integer;
                        break;

                }
            }
            return convertedArgs;
        }


    }
}