using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WizardBot
{
    public class StartupService
    {
        public static IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            IConfigurationRoot config,
            CommandService commands
            )
        {
            _provider = provider;
            _discord = discord;
            _config = config;
            _commands = commands;
        }

        public async Task StartAsync()
        {
            string token = "Nzg4NjQ3MTk4NTE0MjE2OTYx.X9mi8A.AiBGIdvFZSoaWi1I2ErAJOe2hHw";
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Please, provide token in the config.json file.");
                return;
            }
            await _discord.LoginAsync(TokenType.Bot, token);
            await _discord.StartAsync();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }
    }
}