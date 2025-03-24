using Watcher.Data.Entities;
using Watcher.Data.SQL.Repositories;

namespace Watcher.Data.SQL
{
    public interface ISqlUnitOfWork
    {
        IBaseRepository<Person> People { get; }
        IBaseRepository<EventLog> Events { get; }
    }

    public class SqlUnitOfWork : ISqlUnitOfWork
    {

        //public SqlUnitOfWork(IBaseRepository<Person> personRepository, IBaseRepository<EventLog> eventsRepository, SqlDbContext context)
        //{
        //    //this.People = personRepository.WithContext(context);
        //  //  this.Events = eventsRepository.WithContext(context);


        //}


        public IBaseRepository<Person> People { get; }
        public IBaseRepository<EventLog> Events { get; }
    }
}
