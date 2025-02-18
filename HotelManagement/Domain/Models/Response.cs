using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace HotelManagement.Domain.Models
{
    public class Response<T>
    {
        public T Data { get; set; }

        [JsonIgnore]
        public bool Success { get; set; }
        public string Message { get; set; }

        public Response(T data, bool success, string message)
        {
            Data = data;
            Success = success;
            Message = message;
        }

        public Response(T data) : this(data, true, string.Empty) { }

        public Response(string message) : this(default, false, message) { }

    }
}
