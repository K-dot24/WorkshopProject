using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                                        { "OpenNewStore",new Func< string, string,string,Result<StoreService>>(system.OpenNewStore)},
                                        { "AddProductToStore",new Func< String , String , String , double , int , String , LinkedList<String> ,Result<ProductService>>(system.AddProductToStore)},
                                        { "AddStoreManager",new Func< String , String , String ,Result<Boolean>>(system.AddStoreManager)}
                                    };

            string json = File.ReadAllText(@"stories.json");
            StoryConfig storyConfig = JsonConvert.DeserializeObject<StoryConfig>(json);
            foreach (Story story in storyConfig.story)
            {
                functionDict[story.function].DynamicInvoke(story.args);
            }
        }

    }
}
