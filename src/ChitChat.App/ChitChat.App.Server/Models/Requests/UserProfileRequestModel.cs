﻿namespace ChitChat.App.Server.Models.Requests
{
    public class UserProfileRequestModel
    {
        public string? ProfilePicture { get; set; }

        public string? Bio { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}