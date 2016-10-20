﻿namespace TQL.CronExpression.Converter
{
    public interface IConverter<in TRequest, out TResponse>
    {
        TResponse Convert(TRequest request);
    }
}