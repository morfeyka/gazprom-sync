using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Setting.Appointment.Sheduling;
using Sofia.Domain.Setting.Log;

namespace Sofia.Mapping.Setting.Log
{
    public class StatusTaskMap : ClassMapping<StatusTask>
    {
        public StatusTaskMap()
        {
            Table("StatusTask");
            Id(x => x.Id, map =>
            {
                map.Column("Id");
                map.Generator(Generators.Identity);
                map.Type((IIdentifierType)TypeFactory.Basic(typeof(int).FullName));
                map.UnsavedValue(0);
            });
            Property(x => x.StartRun, map =>
            {
                map.NotNullable(true);
                map.Type(TypeFactory.Basic(typeof(DateTime).FullName));
            });
            Property(x => x.Error, map =>
            {
                map.NotNullable(false);
                map.Type(TypeFactory.Basic(typeof(string).FullName));
            });
            Property(x => x.TotalRows, map =>
            {
                map.NotNullable(false);
                map.Type(TypeFactory.Basic(typeof(int?).FullName));
            });
            Property(x => x.ErrorRows, map =>
            {
                map.NotNullable(false);
                map.Type(TypeFactory.Basic(typeof(int?).FullName));
            });

            Property(x => x.EndRun, map =>
            {
                map.NotNullable(false);
                map.Type(TypeFactory.Basic(typeof(DateTime?).FullName));
            });
            Property(x => x.TaskExecType, map =>
            {
                map.NotNullable(true);
                map.Type(NHibernateUtil.
                            GuessType(
                                typeof(
                                    TaskExecType
                                    )));
            });
            ManyToOne(x => x.Sheduler, map =>
            {
                map.Cascade(Cascade.None);
                map.Class(typeof(Sheduler));
                map.Column("IdShedule");
                map.Lazy(LazyRelation.Proxy);
                map.NotNullable(false);
            });
        }
    }
}
