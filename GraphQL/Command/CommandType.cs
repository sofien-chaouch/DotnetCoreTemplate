using System.Linq;
using PlatformService.Data;
using PlatformService.Model;
using HotChocolate;
using HotChocolate.Types;
using PlatformService.Models;

namespace PlatformService.GraphQL.Command
{
    public class CommandType : ObjectType<Model.Command>
    {
        protected override void Configure(IObjectTypeDescriptor<Model.Command> descriptor)
        {
            descriptor.Description("Represents any executable command.");

            descriptor
                .Field(c => c.Id)
                .Description("Represents the unique ID for the command.");

            descriptor
                .Field(c => c.HowTo)
                .Description("Represents the how-to for the command.");

            descriptor
                .Field(c => c.CommandLine)
                .Description("Represents the command line.");

            descriptor
                .Field(c => c.PlatformId)
                .Description("Represents the unique ID of the platform which the command belongs.");

            descriptor
                .Field(c => c.Platform)
                .ResolveWith<Resolvers>(c => c.GetPlatform(default!, default!))
                .UseDbContext<AppDbContext>()
                .Description("This is the platform to which the command belongs.");

        }

        private class Resolvers
        {
            public Platform GetPlatform(Model.Command command, [ScopedService] AppDbContext context)
            {
                return context.Platforms.FirstOrDefault(p => p.Id == command.PlatformId);
            }
        }
    }
}