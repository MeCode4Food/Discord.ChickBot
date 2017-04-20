using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MyFirstDiscordBot
{
    class DiscordBot
    {
        DiscordClient client;
        CommandService commands;

        public DiscordBot()
        {
            client = new DiscordClient(input =>
            {
                input.LogLevel = LogSeverity.Info;
                input.LogHandler = Log;
            });

            bool isditto = false;
            bool issiv = true;
            client.UsingCommands(input =>
            {
                input.PrefixChar = '!';
                input.AllowMentionPrefix = true;
            });

            commands = client.GetService<CommandService>();

            //returns "World!"
            commands.CreateCommand("Hello").Do(async (e) =>
            {
                await e.Channel.SendMessage("World!");
            });

            //executes the method DoAnnouncement(). requires at least a parameter after !announce
            commands.CreateCommand("announce").Parameter("message",ParameterType.Multiple).Do(async (e)=>
            {
                await DoAnnouncement(e);
            });

            //returns "You're a piece of shit!"
            commands.CreateCommand("Whoami").Do(async (e) =>
            {
                await e.Channel.SendMessage("You're a piece of shit!");
            });

            //short message to ch and sj
            client.UserUpdated += async (s, e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.After;

                //quick name check
                if (
                    ((string)user.Name == ("ditto")) ||
                    ((string)user.Name == ("cheecken0")) ||
                    ((string)user.Name == ("titmyass")) ||
                    ((string)user.Name == ("TestSubject1"))
                   )
                {
                    if(e.After.Status == UserStatus.Online )
                    {
                        if((string)user.Name == ("cheecken0"))
                        {
                            await channel.SendMessage(string.Format("Welcome glorious master {0}", user.Mention));
                        }else
                            await channel.SendMessage(string.Format("{0} You're a piss of shiet.", user.Mention));
                        
                        if(!isditto && ((string)user.Name == ("ditto")))
                        {
                            await channel.SendMessage(string.Format("ch try using this channel from here on out so I can test more bot commands.\nChick"));
                            isditto = true;
                        }
                        else if(!issiv && ((string)user.Name == ("titmyass")))
                        {
                            await channel.SendMessage(string.Format("sj try using this channel from here on out so I can test more bot commands.\nChick"));
                            issiv = true;
                        }
                    }

                }

                
            };

            //when client (guild chat) receives message
            client.MessageReceived += async (s, e) =>
            {
                var user = e.User;
                if ((string)user.Name == ("ditto"))
                {
                    System.Threading.Thread.Sleep(500);

                    await e.Channel.SendMessage(string.Format("{0} Nerd come 1v1 me starcraft la nob", user.Mention));

                    isditto = true;
                }

            };

            //to connect bot with client via token
            client.ExecuteAndWait(async () =>
            {
                await client.Connect("MzA0MzMxMzg4MjU1OTkzODU4.C9lGOw.wSc8U8OMnDvGmycTqlsnm7MjEI4", TokenType.Bot);
            });

        }

        //DoAnnouncement method which calls the ConstructMessage method to create a message based on arguments on !announce
        private async Task DoAnnouncement(CommandEventArgs e)
        {
            
            var channel = e.Server.FindChannels(e.Args[0], ChannelType.Text).FirstOrDefault();

            var message = ConstructMessage(e, channel != null);

            if(channel != null)
            {
                await channel.SendMessage(message);
            }
            else
            {
                await e.Channel.SendMessage(message);
            }
        }

        //Message constructor when called by DoAnnouncement
        private string ConstructMessage(CommandEventArgs e, bool firstArgIsChannel)
        {
            string message = "";

            var name = e.User.Nickname != null ? e.User.Nickname : e.User.Name;

            var startIndex = firstArgIsChannel ? 1 : 0;

            for(int i = startIndex; i < e.Args.Length; i++)
            {
                message += e.Args[i].ToString() + " ";
            }

            var result = name + " says " + message;

            return result;
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
           
        }
    }
}
