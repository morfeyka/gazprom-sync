using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sofia.Domain.Inventory
{
    /// <summary>
    /// Предоставляет информацию о группе номенлатуры
    /// </summary>
    /// <remarks>table InventItemGroup</remarks>
    public class Group:EntityWithTypedId<string>,Factelligence.ICrude //: Entity
    {
        private const string IdExpressGp = "ГП";
        private const string IdExpressPf = "ПФ-RFID";
        /// <summary>
        /// Возвращает или задает индентификатор /// <example>ПФ-RFID</example><example>ГП*</example>
        /// </summary>
        /// <remarks>field ItemGroupID</remarks>
        //public string Id { get; set; }
        private readonly ISet<Item> _items;

        public Group()
        {
            _items = new HashSet<Item>();
        }
        /// <summary>
        /// Возращает или задает информацию о правилах, состоянии и статусе импорта
        /// </summary>
        /// <remarks>field RecId</remarks>
        public virtual Interface.DataImported DataImported { get; set; }
        /// <summary>
        /// Возвращает или задает стандартное количество в упаковке
        /// </summary>
        /// <remarks>field StandartPackQty</remarks>
        public virtual double StandartPackQty { get; set; }

        /// <summary>
        /// Возвращает или задает наименование
        /// </summary>
        /// <remarks>field Name</remarks>
        public virtual string Name { get; set; }

        /// <summary>
        /// Возвращает или задает наименование
        /// </summary>
        /// <remarks>field RECID</remarks>
        public virtual long RecId { get; set; }
        /// <summary>
        /// Возвращает коллекцию номенклатурных позиций данной группы
        /// </summary>
        public virtual ReadOnlyCollection<Item> Items
        {
            get { return new ReadOnlyCollection<Item>(new List<Item>(_items)); }
        }

        #region Implementation of ICrude

        public virtual string Insert()
        {
            try
            {
                if (!Factelligence.GlobalMethods.Instance.InstanceItemClass.GetByKey(Id))
                {
                    
                    if(!Factelligence.GlobalMethods.Instance.InstanceItemClass.Add(
                                         Factelligence.GlobalMethods.Instance.SessionId, Id, Name, true, true, false))
                            return  String.Format("При вставке Item_Class Id-{0}, Name-{1} произошла ошибка, возможно уже существует такая запись", Id, Name);
                    if(Id.StartsWith(IdExpressGp) || Id.Contains(IdExpressPf))
                    {
                        return Factelligence.GlobalMethods.Instance.InsertItemClassAttr(Id, "RFID", "1");
                    }

                    return Factelligence.GlobalMethods.Instance.InsertItemClassAttr(Id, "RFID", "0");
                }
            }
            catch(Exception e)
            {
                return String.Format("При вставке Item_Class Id-{0}, Name-{1} произошла ошибка, возможно уже существует такая запись", Id, Name) + e;
            }
            return Update();

            #region OldCode

//Private Function InsertInventItemGroup(ByVal oInventItemGroup As ADODB.Recordset) As String

//'--------------------------------------------------------------------------------
//' Routine    :       InsertInventTable
//' Description:       Insert Item_Class
//' Created by :       Debi Reese
//' Date-Time  :       3/12/2003-9:06:26 PM
//'
//' Parameters :       oInventItemGroup
//'--------------------------------------------------------------------------------


//On Error GoTo errInsertInventItemGroup

//    Dim oFIProdItemClass As New FIProd.Item_Class
//    Dim sError As String, sError2 As String
//    Dim bSuccess As Boolean
//    bSuccess = oFIProdItemClass.Add(gvlngSessionID, _
//                        oInventItemGroup("ItemGroupID").Value, _
//                        oInventItemGroup("Name").Value, _
//                        True, _
//                        True, _
//                        False)

//    If bSuccess = False Then
//        If InStr(1, sError, "PRIMARY KEY") > 0 Then
//            'the item class already exists so just update it instead
//            sError = UpdateInventItemGroup(oInventItemGroup)
//        End If
//    Else
//        'Add Attribute Item Class for RFID
//        If oInventItemGroup("ItemGroupID").Value Like "ГП*" Or oInventItemGroup("ItemGroupID").Value Like "ПФ-RFID" Then
//            sError2 = InsertItemClassAttr(oInventItemGroup("ItemGroupID").Value, "RFID", "1")
//            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        Else
//            sError2 = InsertItemClassAttr(oInventItemGroup("ItemGroupID").Value, "RFID", "0")
//            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        End If
//    End If

//    InsertInventItemGroup = sError

//Exit Function
//errInsertInventItemGroup:
//    sError = Err.Number & " " & Err.Description
//    Resume Next

//End Function

            #endregion OldCode

        }

        public virtual string Update()
        {
            try
            {
                if (Factelligence.GlobalMethods.Instance.InstanceItemClass.GetByKey(Id))
                {

                    if (! Factelligence.GlobalMethods.Instance.InstanceItemClass.UpdateSpecific(Factelligence.GlobalMethods.Instance.SessionId,Id,Name))
                        return String.Format("При обновлении Item_Class Id-{0}, Name-{1} произошла ошибка, возможно уже существует такая запись", Id, Name);
                    if (Id.StartsWith(IdExpressGp) || Id.Contains(IdExpressPf))
                    {
                        return Factelligence.GlobalMethods.Instance.InsertItemClassAttr(Id, "RFID", "1");
                    }

                    return Factelligence.GlobalMethods.Instance.InsertItemClassAttr(Id, "RFID", "0");
                }
            }
            catch (Exception e)
            {
                return String.Format("При вставке Item_Class Id-{0}, Name-{1} произошла ошибка, возможно уже существует такая запись", Id, Name) + e;
            }
            return Insert();
#region OldCode
//Private Function UpdateInventItemGroup(ByVal oInventItemGroup As ADODB.Recordset) As String

//'--------------------------------------------------------------------------------
//' Routine    :       UpdateInventItemGroup
//' Description:       Update Item_Class
//' Created by :       Debi Reese
//' Date-Time  :       3/12/2003-9:16:13 PM
//'
//' Parameters :       oInventItemGroup
//'--------------------------------------------------------------------------------


//On Error GoTo errUpdateInventItemGroup

//    Dim oFIProdItem_Class As New FIProd.Item_Class
//    Dim sError As String, sError2 As String
//    Dim bSuccess As Boolean
//    Dim rs As New ADODB.Recordset
//    Set rs = oFIProdItem_Class.GetAll(oInventItemGroup("ItemGroupID").Value)
    
//    bSuccess = oFIProdItem_Class.UpdateSpecific(gvlngSessionID, _
//                        oInventItemGroup("ItemGroupID").Value, _
//                        oInventItemGroup("Name").Value, , , , , , , , , , , , , , , , , rs("last_edit_at").Value)
//    If bSuccess = False Then
//        If InStr(1, sError, errRowNotFound) > 0 Then
//            'the item_class doesn't exist so just insert it instead
//            sError = InsertInventItemGroup(oInventItemGroup)
//        End If
//    Else
//     'Add Attribute Item Class for RFID
//        If oInventItemGroup("ItemGroupID").Value Like "ГП*" Or oInventItemGroup("ItemGroupID").Value Like "ПФ-RFID" Then
//            sError2 = InsertItemClassAttr(oInventItemGroup("ItemGroupID").Value, "RFID", "1")
//            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        Else
//            sError2 = InsertItemClassAttr(oInventItemGroup("ItemGroupID").Value, "RFID", "0")
//            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        End If
//    End If
    
//    UpdateInventItemGroup = sError


//Exit Function
//errUpdateInventItemGroup:
//    sError = Err.Number & " " & Err.Description
//    Resume Next

//End Function

#endregion OldCode
        }

        public virtual string Delete()
        {
            try
            {
                if (Factelligence.GlobalMethods.Instance.InstanceItemClass.GetByKey(Id))
                {
                   if(!Factelligence.GlobalMethods.Instance.InstanceItemClass.Delete(
                        Factelligence.GlobalMethods.Instance.SessionId, Id))
                       return String.Format("При удалении Item_Class Id-{0}, Name-{1} произошла ошибка, возможно такая запись уже удалена", Id, Name);

                }
            }catch(Exception e)
            {
                return String.Format("При удалении Item_Class Id-{0}, Name-{1} произошла ошибка, возможно такая запись уже удалена", Id, Name) + e;
            }
            return string.Empty;
        }

        #endregion
    }
}
