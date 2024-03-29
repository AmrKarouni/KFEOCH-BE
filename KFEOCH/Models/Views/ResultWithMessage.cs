﻿namespace KFEOCH.Models.Views
{
    public class ResultWithMessage
    {
        public ResultWithMessage() { }
        public ResultWithMessage(bool success,string message) 
        {
            Success = success;
            Message = message;
        }
        public ResultWithMessage(bool success, object result)
        {
            Success = success;
            Result = result;
        }
        public ResultWithMessage(bool success, string message, object result)
        {
            Success = success;
            Message = message;
            Result = result;
        }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? MessageArabic { get; set; }
        public string? MessageEnglish { get; set; }
        public Object? Result { get; set; }
    }


    public class ObservableData
    {
        public ObservableData() { }

        public ObservableData(object? data, int dataSize)
        {
            Data = data;
            DataSize = dataSize;
        }

        public Object? Data { get; set; }
        public int DataSize { get; set; }
    }
}
