using Anex.Domain.Abstract;
using System;

namespace Anex.Api.Exceptions;

public class TransactionDeleteException : Exception
{
    public TransactionDeleteException(ITransaction transaction)
        : base($"{transaction.GetType().Name} with id:{transaction.Id} was attempted deleted.")
    {
    }
}