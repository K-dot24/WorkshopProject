using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.ServiceLayer;

namespace Terminal3WebAPI
{
    public interface IStoryExecutor { };
    public class StoryExecutor : IStoryExecutor
    {
        public StoryExecutor(IECommerceSystem system)
        {
            //Dictionary<string, Delegate> functionDict = new Dictionary<string, Delegate>()
            //                        {
            //                            { "Register",new Func<string,string,string,int>()},
            //                            { "Login",new Func< string, string,int>(login)},
            //                            { "OpenNewStore",new Func< string, string,int>(login)},
            //                            { "AddProductToStore",new Func< string, string,int>(login)},
            //                            { "AddStoreManager",new Func< string, string,int>(login)}
            //                        };

            string json = File.ReadAllText(@"stories.json");
            StoryConfig storyConfig = JsonConvert.DeserializeObject<StoryConfig>(json);
            foreach (Story story in storyConfig.story)
            {
                //functionDict[story.function].DynamicInvoke(story.args);
            }
        }
    }
}
