namespace Evently.Modules.Users.Infrastracture.Identity;

internal sealed record CredentialRepresentation(string Type, string Value, bool Temporary);