﻿namespace Server.Csharp.Business.Models.Common
{
    public class MessageResponse
    {
        public MessageResponse(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public string Title { get; set; }
        public string Message { get; set; }
    }
}
