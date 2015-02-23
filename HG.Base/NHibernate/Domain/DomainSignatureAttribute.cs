using System;

namespace HG.Base.NHibernate.Domain
{
    /// <summary>
    ///     Facilitates indicating which property(s) describe the unique signature of an 
    ///     entity.  See Entity.GetTypeSpecificSignatureProperties() for when this is leveraged.
    /// </summary>
    /// <remarks>
    ///  It may NOT be used on a <see cref = "ValueObject" />.
    /// </remarks>
    [Serializable]
    public class DomainSignatureAttribute : Attribute
    {
    }
}