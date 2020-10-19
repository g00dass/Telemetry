using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
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
        private readonly IClientSessionHandle session;

        public UnitOfWork(IMongoClient mongoClient, IMongoDbSettings settings)
        {
            this.mongoClient = mongoClient;
            this.settings = settings;
            session = mongoClient.StartSession();
            session.StartTransaction();
        }

        public T GetRepository<T>()
        {
            var type = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Single(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface);

            return TypeInitializer<T>.CreateInstance(
                type.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public,
                    null,
                    CallingConventions.HasThis,
                    new Type[] { typeof(IMongoDatabase), typeof(IClientSessionHandle) },
                    null),
                mongoClient.GetDatabase(settings.DatabaseName),
                session);
        }

        public async Task SaveChanges()
        {
            try
            {
                await session.CommitTransactionAsync();
            }
            catch (Exception)
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

        private class TypeInitializer<TResult>
        {
            private static readonly ConcurrentDictionary<string, Func<object[], TResult>> instanceCreationMethods =
                new ConcurrentDictionary<string, Func<object[], TResult>>();

            public static TResult CreateInstance(ConstructorInfo constructorInfo, params object[] arguments)
            {
                ParameterInfo[] parameterInfo = constructorInfo.GetParameters();
                IEnumerable<Type> parameterTypes = parameterInfo.Select(p => p.ParameterType);
                string constructorSignatureKey = GetConstructorSignatureKey(constructorInfo.DeclaringType, parameterTypes);

                Func<object[], TResult> factoryMethod = instanceCreationMethods.GetOrAdd(constructorSignatureKey, key =>
                {
                    var args = new Expression[parameterInfo.Length];
                    ParameterExpression param = Expression.Parameter(typeof(object[]));
                    for (int i = 0; i < parameterInfo.Length; i++)
                        args[i] = Expression.Convert(Expression.ArrayIndex(param, Expression.Constant(i)), parameterInfo[i].ParameterType);
                    return Expression
                        .Lambda<Func<object[], TResult>>(Expression.Convert(Expression.New(constructorInfo, args), typeof(TResult)), param)
                        .Compile();
                });

                return factoryMethod(arguments);
            }

            private static string GetConstructorSignatureKey(Type type, IEnumerable<Type> argumentTypes) =>
                $"{type.FullName} ({string.Join(", ", argumentTypes.Select(at => at.FullName))})";
        }
    }
}
