﻿namespace HotelManagement.Application.Dtos
{
    public class EmailDto
    {
        public class SmtpSettings
        {
            public string Server { get; set; }
            public int Port { get; set; }
            public string User { get; set; }
            public string Pass { get; set; }
        }
    }
}
