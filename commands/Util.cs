using DSharpPlus.SlashCommands;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System.Collections.Generic;

namespace Cerberus.Commands {
    [SlashCommandGroup("Util", "General utility commands")]
    public class Util : ApplicationCommandModule {
        [SlashCommand("Reaction-Role", "Creates a reaction-role listener")]
        public async Task ReactionRole(InteractionContext ctx, [Option("Role", "Name of the role to associate")] string roleName,
            [Option("MessageID", "ID of the message to attach to")] string messageId,
            [Option("Emoji-Name", "Name of the emoji to use")] string emojiName) {

            // await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Give me a sec..."));
            await ctx.DeferAsync();

            // Should insert reaction listener into db
            // When listener is retreived by role, will get the message and emoji from db, then add role

            DiscordRole role = ctx.Guild.Roles.Where((pair, index) => {
                return pair.Value.Name.ToLower().Equals(roleName.ToLower()) ? true : false;
            }).First().Value;

            ulong messageIdInt;
            if (!ulong.TryParse(messageId, out messageIdInt)) {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Something's wrong with that message id?  Don't ask me"));
                return;
            }

            DiscordMessage message;
            DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, String.Format(":{0}:", emojiName));

            try {
                message = await ctx.Channel.GetMessageAsync(messageIdInt);
            } catch (NotFoundException) {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Yeaa I don't think that message exists"));
                return;
            }

            await message.CreateReactionAsync(emoji);
        }
    }
}