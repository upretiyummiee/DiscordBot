using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardBot
{
    public class General: ModuleBase
    {
        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync($"Hello {Context.Message.Author}!");
        }

        [Command("help")]
        public void Help()
        {
            var list = CommandHandler._commands.Commands.ToList();
            Console.WriteLine("");
        }
    }
}
