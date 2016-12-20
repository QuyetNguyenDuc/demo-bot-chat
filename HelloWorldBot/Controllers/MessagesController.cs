using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;
using System.Text;

namespace HelloWorldBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            bool boolAskedForUserName = false;
            string strUserName = "";

            if (message.Type == "Message")
            {
                // Get any saved values
                boolAskedForUserName =
                    message.GetBotPerUserInConversationData<bool>("AskedForUserName");
                strUserName =
                    message.GetBotPerUserInConversationData<string>("UserName");

                // Create text for a reply message   
                StringBuilder strReplyMessage = new StringBuilder();

                if (boolAskedForUserName == false)
                {
                    strReplyMessage.Append($"Hello, I am **AIHelpWebsite.com** Bot");
                    strReplyMessage.Append($"{Environment.NewLine}");
                    strReplyMessage.Append($"You can say anything");
                    strReplyMessage.Append($"{Environment.NewLine}");
                    strReplyMessage.Append($"to me and I will repeat it back");
                    strReplyMessage.Append($"{Environment.NewLine}{Environment.NewLine}");
                    strReplyMessage.Append($"What is your name?");
                }
                else
                {
                    if (strUserName == null)
                    {
                        strReplyMessage.Append($"Hello {message.Text}!");
                    }
                    else
                    {
                        strReplyMessage.Append($"{strUserName}, You said: {message.Text}");
                    }
                }

                // Create a reply message
                var replyMessage = message.CreateReplyMessage(strReplyMessage.ToString());

                if (boolAskedForUserName == true & strUserName == null)
                {
                    // If we have asked for a username but it has not been set
                    // the current response is the user name
                    strUserName = message.Text;
                }

                // Set BotUserData
                replyMessage.SetBotPerUserInConversationData("UserName", strUserName);
                replyMessage.SetBotPerUserInConversationData("AskedForUserName", true);

                return replyMessage;
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                var replyMessage = message.CreateReplyMessage("Personal data has been deleted");
                replyMessage.SetBotPerUserInConversationData("UserName", null);
                replyMessage.SetBotPerUserInConversationData("AskedForUserName", null);
                return replyMessage;
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}