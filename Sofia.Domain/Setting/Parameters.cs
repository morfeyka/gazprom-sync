using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Setting
{
    /// <summary>
    /// Предоставляет информацию о настройках экспрорта из базы Axapta в Factelligence
    /// </summary>
    /// <remarks>table FiaxParameters</remarks>
    public class Parameters
    {
        /// <summary>
        /// Возвращает или задает идентификатор записи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Возвращает или задает идентификатор области данных
        /// </summary>
        /// <example>sof</example>
        /// <remarks>field DataAreaId</remarks>
        public virtual string DataAreaId { get; set; }

        /// <summary>
        /// Возвращает или задает признак, что описание является идентификатором заказа<see cref="Sofia.Domain.Order.Production"/>
        /// </summary>
        public virtual bool WoDescIsWoId { get; set; }

        /// <summary>
        /// Возвращает или задает признак, что при создании заказа необходимо использовать <see cref="FIProd.Job_Exec"/>
        /// </summary>
        public virtual bool CreateWOFromProcess { get; set; }

        /// <summary>
        /// Возвращает или задает, необходимо ли проверять инвентарную позицию
        /// </summary>
        public virtual bool CheckInv { get; set; }

        /// <summary>
        /// Возращает или задает
        /// </summary>
        /// <remarks>field  BOMItemMustConsFromWip</remarks>
        public virtual bool gbMustConsumeFromWip{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>field BOMItemMustConsBefProd</remarks>
        public bool gbMustConsumeBeforeProd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>field BOMItemAltInvLoc</remarks>
        public bool gbMayChooseAltInvLoc { get; set; }

        #region OldCode
//On Error GoTo errGetPrefSettings
//    Dim oFiaxParameters As ADODB.Recordset
    
//    Set oFiaxParameters = goAxapta.Execute("SELECT * FROM FiaxParameters WHERE DataAreaId='" & gsDataAreaID & "'")
    
//    If Not oFiaxParameters.EOF Then
//        'TODO:  get other parameters FROM the pref setting and write to global variable?
        
//        'BOM_Item
//        gbUpdateInv = oFiaxParameters("BOMItemUpdateInv").Value
//        gbBackflush = oFiaxParameters("BOMItemBackflush").Value
//        gbMustConsumeFromInv = oFiaxParameters("BOMItemConsFromInv").Value
//        gbMayChooseAltInvLoc = oFiaxParameters("BOMItemAltInvLoc").Value
//        gbMayCreateNewLots = oFiaxParameters("BOMItemMayCreateNewLots").Value
//        gbMustConsumeFromWip = oFiaxParameters("BOMItemMustConsFromWip").Value
//        gbMustConsumeBeforeProd = oFiaxParameters("BOMItemMustConsBefProd").Value
        
//        If oFiaxParameters("PrefBOMVerMethod").Value = PrefBOMVerMethod.MostRecentlyActivated Then
//            gbMakeMostRecentBOMActive = True
//        End If
//        'User
//        gsDefaultPassword = oFiaxParameters("DefaultPassword").Value
//        gbForcePasswordChange = oFiaxParameters("ForceNewPassword").Value
//        glDefaultLangId = GetDefaultLangId(oFiaxParameters("DefaultLanguageId").Value)
//        gvDefaultPOP3 = oFiaxParameters("DefaultPOP3").Value
//        gvDefaultSMTP = oFiaxParameters("DefaultSMTP").Value
//        If gvDefaultPOP3 = "" Then gvDefaultPOP3 = Null
//        If gvDefaultSMTP = "" Then gvDefaultSMTP = Null
//        'Entities / WorkCenters
//        'TODO: hard coded "UseDepartment" until added to preferences table
//        gbUseDepartment = oFiaxParameters("UseDepartments").Value
        
//        'Production Order
//        gbWoDescIsWoID = oFiaxParameters("WoDescIsWoID").Value
//        gbCheckInv = oFiaxParameters("CheckInv").Value
//        gbCreateWOFromProcess = oFiaxParameters("UseCWOFP").Value
//        GetPrefSettings = True
//    Else
//        GetPrefSettings = False
//    End If
//Exit Function
//errGetPrefSettings:
//    GetPrefSettings = False
//    Exit Function
//End Function

        #endregion OldCode
    }
}
