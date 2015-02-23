using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Setting.Training
{
    /// <summary>
    /// Предоставляет шифры из БД ООО «Газпром трансгаз Санкт-Петербург» для формирования файлов для тренажера диспетчер "Веста-ТР"
    /// </summary>
    public class IdsForTraining
    {
        public virtual string Id { get; set; }
        public virtual string Value { get; set; }
    }
}
