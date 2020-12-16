using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace WizardBot
{
    public class CommandHandler
    {
        public static IServiceProvider _provider;
        public static DiscordSocketClient _discord;
        public static CommandService _commands;
        public static IConfigurationRoot _config;
        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IServiceProvider provider)
        {
            _provider = provider;
            _discord = discord;
            _config = config;
            _commands = commands;

            _discord.Ready += OnReady;
            _discord.MessageReceived += OnMessageRecieved;
        }

        private async Task OnMessageRecieved(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (arg.Author.IsBot)
                return;
            var context = new SocketCommandContext(_discord, message);
            int pos = 0;
            if(message.HasStringPrefix(_config["prefix"], ref pos) || message.HasMentionPrefix(_discord.CurrentUser, ref pos))
            {
                var result = await _commands.ExecuteAsync(context, pos, _provider);
                if (!result.IsSuccess)
                {
                    var reason = result.Error;
                    await context.Channel.SendMessageAsync("Mujii katti mistake garxas jatho.");
                    Console.WriteLine(reason);
                }
            }
        }

        private Task OnReady()
        {
            Console.WriteLine($"Connected as {_discord.CurrentUser.Username}#{_discord.CurrentUser.Discriminator}");
            return Task.CompletedTask;
        }
    }
}