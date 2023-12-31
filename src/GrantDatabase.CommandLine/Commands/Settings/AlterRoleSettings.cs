﻿using Spectre.Console.Cli;

namespace GrantDatabase.CommandLine.Commands.Settings
{
    public class AlterRoleSettings
        : SqlCommandSettings
    {
        [CommandArgument(3, "<Managed Identity Name>")]
        public string ManagedIdentityName { get; set; } = string.Empty;

        [CommandArgument(4, "<Role Name>")]
        public string RoleName { get; set; } = string.Empty;
    }
}
