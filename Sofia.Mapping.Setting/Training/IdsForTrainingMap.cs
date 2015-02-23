using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sofia.Domain.Setting.Training;

namespace Sofia.Mapping.Setting.Training
{
    public class IdsForTrainingMap : ClassMapping<IdsForTraining>
    {
        public IdsForTrainingMap()
        {
            Table("IdsForTraining");
            Id(x => x.Id, map =>
                              {
                                  map.Column("Id");
                                  map.Generator(Generators.Assigned);
                                  map.Type((IIdentifierType) TypeFactory.Basic(typeof (string).FullName));
                              });
            Property(x => x.Value, map =>
                                       {
                                           map.NotNullable(false);
                                           map.Type(TypeFactory.Basic(typeof (string).FullName));
                                       });
        }

    }
}
