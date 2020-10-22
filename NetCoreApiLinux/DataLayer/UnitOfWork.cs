using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Serilog;

namespace DataLayer
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task SaveChanges();
        T GetRepository<T>();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoClient mongoClient;
        private readonly IMongoDbSettings settings;
        private readonly IMemoryCache cache;
        private readonly IClientSessionHandle session;

        public UnitOfWork(IMongoClient mongoClient, IMongoDbSettings settings, IMemoryCache cache)
        {
            this.mongoClient = mongoClient;
            this.settings = settings;
            this.cache = cache;
            session = mongoClient.StartSession();
            session.StartTransaction();
        }

        public T GetRepository<T>()
        {
            return RepositoryInitializer<T>.Get()(new object[]{mongoClient.GetDatabase(settings.DatabaseName), session, cache});
        }

        public async Task SaveChanges()
        {
            try
            {
                await session.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                Log.Error("Transaction failed");
                await session.AbortTransactionAsync();
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await SaveChanges();
            session?.Dispose();
        }

        private class RepositoryInitializer<TResult>
        {
            private static readonly ConcurrentDictionary<string, Func<object[], TResult>> cache =
                new ConcurrentDictionary<string, Func<object[], TResult>>();

            public static Func<object[], TResult> Get()
            {
                var factoryMethod = cache.GetOrAdd(typeof(TResult).FullName,
                    _ =>
                    {
                        var type = AppDomain.CurrentDomain
                            .GetAssemblies()
                            .SelectMany(x => x.GetTypes())
                            .Single(x => typeof(TResult).IsAssignableFrom(x) && !x.IsInterface);

                        return CreateFactoryMethod(
                            type.GetConstructor(
                                BindingFlags.Instance | BindingFlags.Public,
                                null,
                                CallingConventions.HasThis,
                                new Type[] { typeof(IMongoDatabase), typeof(IClientSessionHandle), typeof(IMemoryCache) },
                                null));
                    });

                return factoryMethod;
            }

            private static Func<object[], TResult> CreateFactoryMethod(ConstructorInfo constructorInfo)
            {
                var parameterInfo = constructorInfo.GetParameters();
                var args = new Expression[parameterInfo.Length];
                var param = Expression.Parameter(typeof(object[]));

                for (int i = 0; i < parameterInfo.Length; i++)
                    args[i] = Expression.Convert(Expression.ArrayIndex(param, Expression.Constant(i)), parameterInfo[i].ParameterType);
                return Expression
                    .Lambda<Func<object[], TResult>>(Expression.Convert(Expression.New(constructorInfo, args), typeof(TResult)), param)
                    .Compile();
            }
        }
    }
}
