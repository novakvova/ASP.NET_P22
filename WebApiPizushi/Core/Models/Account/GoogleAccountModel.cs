﻿using System.Text.Json.Serialization;

namespace Core.Models.Account;

public class GoogleAccountModel
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("given_name")]
    public string FirstName { get; set; } = string.Empty;
    [JsonPropertyName("family_name")]
    public string LastName { get; set; } = string.Empty;
    [JsonPropertyName("picture")]
    public string Picture { get; set; } = string.Empty;
}
