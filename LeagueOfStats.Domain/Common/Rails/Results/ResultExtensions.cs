using LeagueOfStats.Domain.Common.Rails.Errors;

namespace LeagueOfStats.Domain.Common.Rails.Results;

public static class ResultExtensions
{
    public static async Task<Result> ToNonValueResult<TIn>(this Task<Result<TIn>> taskResult)
    {
        var result = await taskResult;

        return result.IsFailure
            ? Result.Failure(result.Errors)
            : Result.Success();
    }
    
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mappingFunc) =>
        result.IsSuccess
            ? Result.Success(mappingFunc(result.Value))
            : Result.Failure<TOut>(result.Errors);
    
    public static async Task<Result<TOut>> Map<TIn, TOut>(this Task<Result<TIn>> taskResult, Func<TIn, TOut> mappingFunc)
    {
        var result = await taskResult;
        
        return result.IsSuccess
            ? Result.Success(mappingFunc(result.Value))
            : Result.Failure<TOut>(result.Errors);
    }

    public static Result<TOut> Bind<TOut>(this Result result, Func<Result<TOut>> func) => 
        result.IsFailure 
            ? Result.Failure<TOut>(result.Errors)
            : func();
    
    public static async Task<Result<TOut>> Bind<TOut>(this Result result, Func<Task<Result<TOut>>> func) => 
        result.IsFailure 
            ? Result.Failure<TOut>(result.Errors)
            : await func();
    
    public static async Task<Result<TOut>> Bind<TOut>(this Task<Result> taskResult, Func<Result<TOut>> func)
    {
        var result = await taskResult;
        
        return result.IsFailure
            ? Result.Failure<TOut>(result.Errors)
            : func();
    }
    
    public static async Task<Result<TOut>> Bind<TOut>(this Task<Result> taskResult, Func<Task<Result<TOut>>> funcToAwait)
    {
        var result = await taskResult;
        
        return result.IsFailure
            ? Result.Failure<TOut>(result.Errors)
            : await funcToAwait();
    }

    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func) => 
        result.IsFailure 
            ? Result.Failure<TOut>(result.Errors)
            : func(result.Value);
    
    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> taskResult, Func<TIn, Result<TOut>> func)
    {
        var result = await taskResult;
        
        return result.IsFailure
            ? Result.Failure<TOut>(result.Errors)
            : func(result.Value);
    }

    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> taskResult, Func<TIn, Task<Result<TOut>>> funcToAwait)
    {
        var result = await taskResult;
        
        return result.IsFailure
            ? Result.Failure<TOut>(result.Errors)
            : await funcToAwait(result.Value);
    }

    public static async Task<Result> Bind<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> func) =>
        result.IsFailure
            ? Result.Failure(result.Errors)
            : await func(result.Value);
    
    public static async Task<Result> Bind<TIn>(this Task<Result<TIn>> taskResult, Func<TIn, Task<Result>> func)
    {
        var result = await taskResult;
        
        return result.IsFailure
            ? Result.Failure(result.Errors)
            : await func(result.Value);
    }

    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> func) => 
        result.IsFailure
            ? Result.Failure<TOut>(result.Errors) 
            : await func(result.Value);

    public static async Task<Result<TOut>> Match<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Task<Result<TOut>>> successFuncToAwait, Func<Task<Result<TOut>>> errorFuncToAwait)
    {
        var result = await resultTask;

        return result.IsSuccess
            ? await successFuncToAwait(result.Value)
            : await errorFuncToAwait();
    }

    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }
    
    public static async Task<Result<TIn>> Tap<TIn>(this Result<TIn> result, Func<Task> func)
           {
               if (result.IsSuccess)
               {
                   await func();
               }
       
               return result;
    }
    
    public static async Task<Result<TIn>> Tap<TIn>(this Task<Result<TIn>> resultTask, Func<TIn, Task> func)
    {
        Result<TIn> result = await resultTask;
        
        if (result.IsSuccess)
        {
            await func(result.Value);
        }
       
        return result;
    }
}