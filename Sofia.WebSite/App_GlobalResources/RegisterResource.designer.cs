//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "12.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class RegisterResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RegisterResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.RegisterResource", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Введите адрес..
        /// </summary>
        internal static string AdressEmpty {
            get {
                return ResourceManager.GetString("AdressEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Введите юридический адрес..
        /// </summary>
        internal static string CompanyAdress {
            get {
                return ResourceManager.GetString("CompanyAdress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Выберите направление деятельности..
        /// </summary>
        internal static string CompanyDirectionEmpty {
            get {
                return ResourceManager.GetString("CompanyDirectionEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Введите название организации..
        /// </summary>
        internal static string CompanyNameEmpty {
            get {
                return ResourceManager.GetString("CompanyNameEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Выберите тип организации..
        /// </summary>
        internal static string CompanyTypeEmpty {
            get {
                return ResourceManager.GetString("CompanyTypeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Введите ИНН..
        /// </summary>
        internal static string InnEmpty {
            get {
                return ResourceManager.GetString("InnEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Введенные ИНН и КПП уже зарегистрированны, введите другие или обратитесь к менеджеру..
        /// </summary>
        internal static string InnKppNotValid {
            get {
                return ResourceManager.GetString("InnKppNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы должны согласиться с условиями договора..
        /// </summary>
        internal static string IsAgreement {
            get {
                return ResourceManager.GetString("IsAgreement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Введите КПП..
        /// </summary>
        internal static string KppEmpty {
            get {
                return ResourceManager.GetString("KppEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Выберите центр обслуживания.
        /// </summary>
        internal static string LockCenterEmpty {
            get {
                return ResourceManager.GetString("LockCenterEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Такй логин уже есть, выберите другой..
        /// </summary>
        internal static string LoginNotValid {
            get {
                return ResourceManager.GetString("LoginNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пароли не совпадают..
        /// </summary>
        internal static string PasswordsNotEquals {
            get {
                return ResourceManager.GetString("PasswordsNotEquals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Выберите способ доставки.
        /// </summary>
        internal static string TypeDeliveryEmpty {
            get {
                return ResourceManager.GetString("TypeDeliveryEmpty", resourceCulture);
            }
        }
    }
}