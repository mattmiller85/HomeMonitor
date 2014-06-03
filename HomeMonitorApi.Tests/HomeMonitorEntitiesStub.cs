using HomeMonitorDataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace HomeMonitorApi.Tests
{
    internal class HomeMonitorEntitiesStub : IHomeMonitorDataContext
    {
        private readonly DbSet<Temperature> _defaultTemperatureData;
        private readonly DbSet<SoilMoistureReading> _defaultSoilData;
        public HomeMonitorEntitiesStub()
        {
            _defaultTemperatureData = new TestDbSet<Temperature>
            {
                new Temperature {Taken = DateTime.Parse("1/5/2014 10:10:10 AM"), TemperatureFarenheit = 30},
                new Temperature {Taken = DateTime.Parse("1/1/2014 10:10:40 AM"), TemperatureFarenheit = 31},
                new Temperature {Taken = DateTime.Parse("1/4/2014 10:20:40 AM"), TemperatureFarenheit = 32},
                new Temperature {Taken = DateTime.Parse("1/6/2014 10:20:41 AM"), TemperatureFarenheit = 33},
                new Temperature {Taken = DateTime.Parse("1/2/2014 10:30:41 AM"), TemperatureFarenheit = 34},
                new Temperature {Taken = DateTime.Parse("1/5/2014 10:30:41 AM"), TemperatureFarenheit = 35}
            };

            _defaultSoilData = new TestDbSet<SoilMoistureReading>
            {
                new SoilMoistureReading {Taken = DateTime.Parse("1/5/2014 10:20:10 AM"), SensorNumber = 1, MilliVolts = 1000m },
                new SoilMoistureReading {Taken = DateTime.Parse("1/7/2014 11:10:10 AM"), SensorNumber = 2, MilliVolts = 900m },
                new SoilMoistureReading {Taken = DateTime.Parse("1/2/2014 12:10:11 AM"), SensorNumber = 6, MilliVolts = 950m },
                new SoilMoistureReading {Taken = DateTime.Parse("1/2/2014 10:10:10 AM"), SensorNumber = 4, MilliVolts = 901m },
                new SoilMoistureReading {Taken = DateTime.Parse("1/2/2014 09:10:34 AM"), SensorNumber = 1, MilliVolts = 0m },
                new SoilMoistureReading {Taken = DateTime.Parse("1/4/2014 04:10:30 AM"), SensorNumber = 2, MilliVolts = 2000m },
                new SoilMoistureReading {Taken = DateTime.Parse("1/1/2014 10:10:20 AM"), SensorNumber = 5, MilliVolts = 50m },
                new SoilMoistureReading {Taken = DateTime.Parse("1/1/2014 10:10:30 AM"), SensorNumber = 3, MilliVolts = 500m },
                new SoilMoistureReading {Taken = DateTime.Parse("1/5/2014 04:10:30 AM"), SensorNumber = 2, MilliVolts = 2200m },
                new SoilMoistureReading {Taken = DateTime.Parse("1/6/2014 04:10:30 AM"), SensorNumber = 2, MilliVolts = 2300m },
            };
        }
        public DbSet<Temperature> Temperatures
        {
            get { return _defaultTemperatureData; }
            set { throw new NotImplementedException(); }
        }


        public DbSet<SoilMoistureReading> SoilMoistureReadings
        {
            get { return _defaultSoilData; }
            set { throw new NotImplementedException(); }
        }


        public int SaveChanges()
        {
            return 1;
        }
    }

    public class TestDbSet<TEntity> : DbSet<TEntity>, IQueryable, IEnumerable<TEntity>, IDbAsyncEnumerable<TEntity>
        where TEntity : class
    {
        readonly ObservableCollection<TEntity> _data;
        readonly IQueryable _query;

        public TestDbSet()
        {
            _data = new ObservableCollection<TEntity>();
            _query = _data.AsQueryable();
        }

        public override TEntity Add(TEntity item)
        {
            _data.Add(item);
            return item;
        }

        public override TEntity Remove(TEntity item)
        {
            _data.Remove(item);
            return item;
        }

        public override IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            _data.Clear();
            return _data;
        }

        public override TEntity Attach(TEntity item)
        {
            _data.Add(item);
            return item;
        }

        public override TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public override ObservableCollection<TEntity> Local
        {
            get { return _data; }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<TEntity>(_query.Provider); }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<TEntity>(_data.GetEnumerator());
        }
    }

    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
}
