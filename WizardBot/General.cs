using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.IO;

namespace WizardBot
{
    public class General : ModuleBase
    {

        [Command("ping")]
        [Summary("Call the bot to check if it is awake or not.")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync($"Hello {Context.Message.Author}!");
        }

        [Command("info")]
        [Summary("Provides you your info.")]
        public async Task Info()
        {
            var username = Context.Message.Author;
            var latency = CommandHandler._discord.Latency;
            var currentchannel = Context.Channel.Name;
            var accountcreationdate = Context.Message.Author.CreatedAt;

            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.WithTitle("Here's your info:");
            embedBuilder.AddField("Username", username);
            embedBuilder.AddField("Latency", latency + "ms");
            embedBuilder.AddField("Account Creation Date", accountcreationdate);
            embedBuilder.AddField("Current Channel", currentchannel);

            await ReplyAsync("", false, embedBuilder.Build());
        }

        [Command("userstatus")]
        [Summary("Provides you the info of the given user status")]
        public async Task UserStatus([Remainder] SocketUser user = null)
        {
            if (user is null)
            {
                await ReplyAsync($"{Context.User.Mention} Please @mention user to use this command.");
            }
            else
            {
                var author = Context.Message.Author;
                var status = CommandHandler._discord.CurrentUser.Status;
                var username = Context.Message.Author.Username;
                var channel = Context.Channel.Name;

                var embedBuilder = new EmbedBuilder();
                embedBuilder.WithAuthor(author);
                embedBuilder.AddField("Username", username);
                embedBuilder.WithColor(Color.Blue);
                embedBuilder.AddField("Status", status);
                embedBuilder.AddField("Connected to channel", channel);
                Console.WriteLine("dsfsdfs");
                await ReplyAsync("Status of the user provided.", false, embedBuilder.Build());
            }
        }


        [Command("purge")]
        [Summary("Deletes the specific amount of messages.")]
        public async Task Purge(int amt)
        {
            if (amt == 999999999)
                await Context.Channel.SendMessageAsync("purge command requires a number to tell how many messages to be deleted");
            if (amt == 0)
                await Context.Channel.SendMessageAsync("You can't just delete zero messages.");
            if (amt < 0)
                await Context.Channel.SendMessageAsync("You can't tell me to delete messages in a negative number.");
            if(amt > 0)
            {
                var messages = Context.Channel.GetMessagesAsync(amt).Flatten();
                foreach(var n in await messages.ToArrayAsync())
                {
                    await this.Context.Channel.DeleteMessageAsync(n);
                }
            }
        }

            /*[Command("setactivity")]
            [Alias("mystatus")]
            [Summary("Set your custom status.")]
            public async Task SetActivity(String ?status) {
                if (String.IsNullOrEmpty(status))
                    await Context.Channel.SendMessageAsync($"{Context.Message.Author}" +
                        $"! Please provide string status to set as a custom status for you.");
                else {
                    await CommandHandler._discord.SetActivityAsync(new Game(status, ActivityType.Watching));
                }
            }*/

        [Command("meme")]
        [Alias("reddit")]
        [Summary("Sends your random meme at a time from subreddit of reddit r/ProgrammerHumor")]
        public async Task Meme()
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync("https://reddit.com/r/ProgrammerHumor/random.json?limit=1");
            JArray arr = JArray.Parse(result);
            var post = JObject.Parse(arr[0]["data"]["children"][0]["data"].ToString());
            var builder = new EmbedBuilder();
            builder.WithImageUrl(post["url"].ToString());
            builder.WithColor(Color.Gold);
            builder.WithTitle(post["title"].ToString());
            builder.WithUrl("https://reddit.com"+post["permalink"].ToString());
            builder.WithFooter($"⬆️{post["ups"]}  🗨{post["num_comments"]}");

            await Context.Channel.SendMessageAsync(null, false, builder.Build());
        }

        [Command("roast")]
        public async Task RoastUser(IUser user = null) {
            string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).
        Parent.Parent.FullName;
            string resxFile = projectDir + "\\Roast.resx";
            using (ResourceReader reader = new ResourceReader(resxFile))
            await Context.Channel.SendMessageAsync();
        }


        [Command("help")]
        [Summary("List all the possible commands.")]
        public async Task Help()
        {
            List<CommandInfo> commands = CommandHandler._commands.Commands.ToList();
            EmbedBuilder embedBuilder = new EmbedBuilder();

            foreach (CommandInfo command in commands)
            {
                // Get the command Summary attribute information
                string embedFieldText = command.Summary ?? "No description available\n";

                embedBuilder.AddField(command.Name, embedFieldText).WithColor(Color.DarkMagenta);
            }

            await ReplyAsync("Here's a list of commands and their description: ", false, embedBuilder.Build());
        }
    }
}
