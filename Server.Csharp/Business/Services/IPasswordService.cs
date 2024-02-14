﻿namespace Server.Csharp.Business.Services
{
    public interface IPasswordService
    {
        string Hash(string password);
        bool Verify(string password, string passwordHash);
    }
}
