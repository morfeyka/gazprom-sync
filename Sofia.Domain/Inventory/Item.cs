using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Sofia.Domain.Interface;

namespace Sofia.Domain.Inventory
{
    /// <summary>
    /// Предоставляет информацию о номенклатурной позиции
    /// </summary>
    /// <remarks>table InventTable reftableid 175 from fiaxIntfTable</remarks>
    public class Item : EntityWithTypedId<string>, Factelligence.ICrude
    {
        private readonly ISet<BarcodeItem> _barcodeItems;
        private readonly ISet<PriceItem> _priceItems;
        private readonly ISet<UnitConvert> _unitConverts;

        public Item()
        {
            _barcodeItems = new HashSet<BarcodeItem>();
            _priceItems = new HashSet<PriceItem>();
            _unitConverts = new HashSet<UnitConvert>();
        }

        public virtual void AddBarcodeItem(BarcodeItem item)
        {
            _barcodeItems.Add(item);
        }
        /// <summary>
        /// Возвращает коллекцию баркодов
        /// </summary>
        public virtual ReadOnlyCollection<BarcodeItem> BarcodeItems
        {
            get { return new ReadOnlyCollection<BarcodeItem>(new List<BarcodeItem>(_barcodeItems)); }
        }
        /// <summary>
        /// Возвращает или задает данные для конвертации единиц измерений
        /// </summary>
        public virtual ReadOnlyCollection<UnitConvert> UnitConverts
        {
            get { return new ReadOnlyCollection<UnitConvert>(new List<UnitConvert>(_unitConverts)); }
        }

        /// <summary>
        /// Возращает или задает информацию о правилах, состоянии и статусе импорта
        /// </summary>
        /// <remarks>field RecId</remarks>
        public virtual Interface.DataImported DataImported { get; set; }

        public virtual long RecId { get; set; }

        /// <summary>
        /// Возвращает или задает наименование
        /// </summary>
        /// <remarks>field ItemName</remarks>
        public virtual string Name { get; set; }

        /// <summary>
        /// Возвращает или задает группу номенклатуры <see cref="Inventory.Group"/>
        /// </summary>
        /// <remarks>field ItemGroupID</remarks>
        public virtual Group Group { get; set; }

        /// <summary>
        /// Возвращает коллекцию баркодов
        /// </summary>
        public virtual ReadOnlyCollection<PriceItem> PriceItems
        {
            get { return new ReadOnlyCollection<PriceItem>(new List<PriceItem>(_priceItems)); }
        }

        /// <summary>
        /// Возвращает или задает об импортируемых данных
        /// </summary>
        /// <remarks>field RECID</remarks>
        public virtual ItemContainer Container { get; set; }
        /// <summary>
        /// Возвращает или задает единицу измерения Bom
        /// </summary>
        /// <remarks>field BomUnitID</remarks>
        public virtual Unit BomUnit { get; set; }

        /// <summary>
        /// Возвращает или задает что то там
        /// </summary>
        /// <remarks>field SF_PBAMODELTYPEID</remarks>
        public virtual string SF_PBAMODELTYPEID { get; set; }

        /// <summary>
        /// Возвращает или задает тип размера
        /// </summary>
        /// <remarks> field SF_SizeTypeId</remarks>
        public virtual SizeType SizeType { get; set; }

        /// <summary>
        /// Возвращает или задает конечную высоту
        /// </summary>
        /// <remarks> field SF_Height</remarks>
        public virtual double SF_Height { get; set; }

        /// <summary>
        /// Возвращает или задает конечную ширину
        /// </summary>
        /// <remarks> field SF_Width</remarks>
        public virtual double SF_Width { get; set; }

        /// <summary>
        /// Возвращает или задает высоту брутто
        /// </summary>
        /// <remarks> field grossHeight</remarks>
        public virtual double GrossHeight { get; set; }
        /// <summary>
        /// Возвращает или задает глубину брутто
        /// </summary>
        /// <remarks> field grossDepth</remarks>
        public virtual double GrossDepth { get; set; }
        /// <summary>
        /// Возвращает или задает ширина брутто
        /// </summary>
        /// <remarks> field grossWidth</remarks>
        public virtual double GrossWidth { get; set; }

        /// <summary>
        /// Возвращает или задает цвет материала
        /// </summary>
        /// <remarks>field SF_ColorOfMaterialId_1</remarks>
        public virtual string SF_ColorOfMaterialId_1 { get; set; }

        #region Implementation of ICrude

        public virtual string Insert()
        {
            string sEAN13;
            string sQtyInPack;
            string sStdQty;
            string noMatchUomIdInTable;
            short UnitDecimals;
            int UomID;
            string sRFID;
            string sError;
            double dItemPrice;
            if (NoMatchUomIdInTable(out sEAN13, out sQtyInPack, out sStdQty, out noMatchUomIdInTable, out UnitDecimals, out UomID, out sRFID, out sError, out dItemPrice)) return noMatchUomIdInTable;
            try
            {
                if (!Factelligence.GlobalMethods.Instance.InstanceItem.GetByKey(Id))
                {
                    if (Factelligence.GlobalMethods.Instance.InstanceItemClass.GetByKey(Group.Id))
                    {

                        if (!Factelligence.GlobalMethods.Instance.InstanceItem.Add(
                            Factelligence.GlobalMethods.Instance.SessionId,
                            Id, Name, Group.Id, UomID, false, null, dItemPrice,
                            false, false,false,UnitDecimals, 
                            Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                            Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                            Missing.Value, Missing.Value, Missing.Value, Missing.Value, 
                            Missing.Value, Missing.Value, Missing.Value, Missing.Value, 
                            Missing.Value, Missing.Value, sRFID))
                            return "Произошла критическая ошибка Item с Id" + Id + " при вставке в Factelligence";
                        return InsertItemAttr(sStdQty, sQtyInPack, sError, sEAN13);
                    }
                    return "Вставка Item, ItemClass с Id = '" + Group.Id + "' не содержится в Factelligence";
                }
                return Update();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
#region OldCode
//Private Function InsertInventTable(ByVal oInventTable As ADODB.Recordset) As String

//'--------------------------------------------------------------------------------
//' Routine    :       InsertInventTable
//' Description:       Insert Item
//' Created by :       Debi Reese
//' Date-Time  :       3/12/2003-8:04:44 PM
//'                    12/02/2009  - Add UnitDecimals (Renat Shaimardanov)
//'
//' Parameters :       oInventTable
//'--------------------------------------------------------------------------------


//On Error GoTo errInsertInventTable

//    Dim oFIProdItem As New FIProd.Item
//    Dim oFIProdUom As New FIProd.Uom
//    Dim sError As String, sError2 As String
//    Dim dItemPrice As Double
//    Dim sItemNotes As String
//    Dim sRFID As String
//    Dim sQtyInPack As String
//    Dim sStdQty As String
//    Dim sEAN13 As String
//    Dim bSuccess As Boolean
//    Dim rsUOM As ADODB.Recordset
//    Dim UomID As String
//    Dim UnitDecimals As Integer
    
//    dItemPrice = GetItemPrice(oInventTable("ItemId").Value)
//    'Start Не используется
//    sItemNotes = GetItemNotes(oInventTable("ItemId").Value)
//    'End Не используется
//    GetQtyInPackEAN13 oInventTable("ItemId").Value, sQtyInPack, sEAN13
//    sStdQty = GetStdQty(oInventTable("ItemId").Value)
    
//    rsUOM = oFIProdUom.GetAll(, , oInventTable("BomUnitID").Value)
    
//    If rsUOM.RecordCount = 0 Then
//        sError = "No match UOM_id in table"
//        InsertInventTable = sError
//        GoTo errInsertInventTable
//    Else
//        UomID = rsUOM("uom_id").Value
//        'UnitDecimals = GetUnitDecimals(oInventTable("BomUnitID").Value)
//        UnitDecimals = 4
//    End If
    
//    sRFID = GetRFID(oInventTable("ItemID").Value)
        
//    bSuccess = oFIProdItem.Add(gvlngSessionID, _
//                        oInventTable("ItemID").Value, _
//                        oInventTable("ItemName").Value, _
//                        oInventTable("ItemGroupID").Value, _
//                        UomID, _
//                        False, _
//                        Null, _
//                        dItemPrice, _
//                        False, _
//                        False, _
//                        False, _
//                        UnitDecimals, , , , , , , , , , , , , , , , , , , sRFID)
//    If bSuccess = False Then
//        If InStr(1, sError, "FOREIGN KEY") > 0 Then
//            'the class matching ItemGroupID does not exist in FI
                         
//            sError = sError & " - Item Class matching Item Group ID '" _
//                            & oInventTable("ItemGroupID").Value _
//                            & "' does not exist in Factelligence"
//        ElseIf InStr(1, sError, "PRIMARY KEY") > 0 Then
//            'the item already exists so just update it instead
//            sError = UpdateInventTable(oInventTable)
//        End If
//    Else
//        sError2 = InsertItemAttr(oInventTable("ItemID").Value, "EAN13", sEAN13)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = InsertItemAttr(oInventTable("ItemID").Value, "PACK_QTY", sQtyInPack)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = InsertItemAttr(oInventTable("ItemID").Value, "MODEL", CStr(oInventTable("SF_PBAMODELTYPEID").Value))
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = InsertItemAttr(oInventTable("ItemID").Value, "STANDARD_QTY", sStdQty)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    End If
    
  
            
//    InsertInventTable = sError

//Exit Function
//errInsertInventTable:
//    sError = Err.Number & " " & Err.Description
//    Resume Next

//End Function

#endregion OldCode
        }

        private string InsertItemAttr(string sStdQty, string sQtyInPack, string sError, string sEAN13)
        {
            string sError2;
            sError2 = Factelligence.GlobalMethods.Instance.InsertItemAttr(Id, "EAN13", sEAN13);
            if (!string.IsNullOrWhiteSpace(sError2))
            {
                sError += " " + sError2.Trim();
            }
            sError2 = Factelligence.GlobalMethods.Instance.InsertItemAttr(Id, "PACK_QTY", sQtyInPack);
            if (!string.IsNullOrWhiteSpace(sError2))
            {
                sError += " " + sError2.Trim();
            }
            sError2 = Factelligence.GlobalMethods.Instance.InsertItemAttr(Id, "MODEL", SF_PBAMODELTYPEID);
            if (!string.IsNullOrWhiteSpace(sError2))
            {
                sError += " " + sError2.Trim();
            }
            sError2 = Factelligence.GlobalMethods.Instance.InsertItemAttr(Id, "STANDARD_QTY", sStdQty);
            if (!string.IsNullOrWhiteSpace(sError2))
            {
                sError += " " + sError2.Trim();
            }
            return sError;
        }

        private bool NoMatchUomIdInTable(out string sEAN13, out string sQtyInPack, out string sStdQty,
                                         out string noMatchUomIdInTable, out short UnitDecimals,
                                         out int UomID, out string sRFID, out string sError, out double dItemPrice)
        {
            dItemPrice = UnitDecimals = 0;
            UomID = 0;
            noMatchUomIdInTable = sEAN13  = sError = sQtyInPack = sRFID = sStdQty = string.Empty;
            var oFIProdUom = (FIProd.Uom) Activator.CreateInstance(Type.GetTypeFromProgID("FIProd.Uom"));
            var priceItem = PriceItems.FirstOrDefault(x => x.TypePrice == TypePrice.Zero);
            if (priceItem != null)
                dItemPrice = priceItem.Price / priceItem.Unit;
            ADODB.Recordset rsUOM = oFIProdUom.GetAll(Missing.Value,Missing.Value,BomUnit.Id);//oFIProdUom.GetAll(, , oInventTable("BomUnitID").Value)
            if (rsUOM.RecordCount == 0)
            {
                {
                    noMatchUomIdInTable = "No match UOM_id in table";
                    return true;
                }
            }
            UomID = rsUOM.get_Collect("uom_id");
            UnitDecimals = 4;
            sRFID = string.Empty;
            var barCod = BarcodeItems;
            var rFids = barCod.Where(x => x.Type == "RFID").FirstOrDefault();
            if (rFids != null)
                sRFID = rFids.Barcode; 
            var ean13 = BarcodeItems.Where(x => x.Type == "EAN13-сбст");
            var items = ean13.Select(x => new {x.Quantity, x.Barcode}).ToList();
            sQtyInPack = string.Empty;
            sEAN13 = string.Empty;
            var bufStr = string.Empty;
            var bufSean = string.Empty;
            items.ForEach(x =>
                              {
                                  if (x.Quantity> 0)
                                  {
                                      bufStr += (x.Quantity + ";");
                                  }
                                  else
                                  {
                                      bufStr += "1;";
                                  }
                                  bufSean += (x.Barcode + ";");
                              });
            sQtyInPack = bufStr;
            sEAN13 = bufSean;
            if (SizeType != null && SizeType.StandartPackQty > 0)
            {
                sStdQty = SizeType.StandartPackQty.ToString();
            }
            else if (Group != null && Group.StandartPackQty > 0)
            {
                sStdQty = Group.StandartPackQty.ToString();
            }
            else
            {
                sStdQty = "1";
            }
            return false;
        }

        public virtual string Update()
        {
            string sEAN13;
            string sQtyInPack;
            string sStdQty;
            string noMatchUomIdInTable;
            short UnitDecimals;
            int UomID;
            string sRFID;
            string sError;
            double dItemPrice;
            if (NoMatchUomIdInTable(out sEAN13, out sQtyInPack, out sStdQty, out noMatchUomIdInTable, out UnitDecimals, out UomID, out sRFID, out sError, out dItemPrice)) 
                return noMatchUomIdInTable;
            try
            {
                if (Factelligence.GlobalMethods.Instance.InstanceItem.GetByKey(Id.ToString()))
                {
                    if (Factelligence.GlobalMethods.Instance.InstanceItemClass.GetByKey(Group.Id))
                    {
                        Factelligence.GlobalMethods.Instance.InstanceItem.UpdateSpecific(
                            Factelligence.GlobalMethods.Instance.SessionId,
                            Id, Name, Group.Id, UomID, Missing.Value, Missing.Value,
                            dItemPrice, Missing.Value, Missing.Value,false, UnitDecimals, 
                            Missing.Value, Missing.Value, Missing.Value, Missing.Value, 
                            Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                            Missing.Value, Missing.Value, Missing.Value, Missing.Value, 
                            Missing.Value, Missing.Value, Missing.Value, Missing.Value, 
                            Missing.Value, Missing.Value, sRFID);
                        return InsertItemAttr(sStdQty, sQtyInPack, sError, sEAN13);
                    }
                    return "Обновление Item, ItemClass с Id = '" + Group.Id + "' не содержится в Factelligence"; 
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return Insert();
#region OldCode
//Private Function UpdateInventTable(ByVal oInventTable As ADODB.Recordset) As String

//'--------------------------------------------------------------------------------
//' Routine    :       UpdateInventTable
//' Description:       Update Item
//' Created by :       Debi Reese
//' Date-Time  :       3/12/2003-8:39:00 PM
//'
//' Parameters :       oInventTable
//'--------------------------------------------------------------------------------


//On Error GoTo errUpdateInventTable

//    Dim oFIProdItem As New FIProd.Item
//    Dim oFIProdUom As New FIProd.Uom
//    Dim sError As String, sError2 As String
//    Dim dItemPrice As Double
//    Dim sItemNotes As String
//    Dim sRFID As String
//    Dim sQtyInPack As String
//    Dim sStdQty As String
//    Dim sEAN13 As String
//    Dim bSuccess As Boolean
//    Dim rs As New ADODB.Recordset
//    Dim rsUOM As ADODB.Recordset
//    Dim UomID As String
//    Dim UnitDecimals As Integer
    
//    dItemPrice = GetItemPrice(oInventTable("ItemID").Value)
//    'Start Не используется
//    sItemNotes = GetItemNotes(oInventTable("ItemID").Value)
//    'End Не используется
//    GetQtyInPackEAN13 oInventTable("ItemId").Value, sQtyInPack, sEAN13
//    sStdQty = GetStdQty(oInventTable("ItemId").Value)
//    '--------------

//    Set rsUOM = oFIProdUom.GetAll(, , oInventTable("BomUnitID").Value)
    
//    If rsUOM.RecordCount = 0 Then
//        sError = "No match UOM_id in table"
//        UpdateInventTable = sError
//        GoTo errUpdateInventTable
//    Else
//        UomID = rsUOM("uom_id").Value
//        'UnitDecimals = GetUnitDecimals(oInventTable("BomUnitID").Value)
//        UnitDecimals = 4
//    End If
//    '---------------------
//    sRFID = GetRFID(oInventTable("ItemID").Value)
    
//    Set rs = oFIProdItem.GetAll(oInventTable("ItemID").Value)
//    If rs.RecordCount > 0 Then
        
//        bSuccess = oFIProdItem.UpdateSpecific(gvlngSessionID, _
//                        oInventTable("ItemID").Value, _
//                        oInventTable("ItemName").Value, _
//                        oInventTable("ItemGroupID").Value, _
//                        UomID, , , _
//                        dItemPrice, , , _
//                        False, UnitDecimals, , , , , , , , , , , , , , , , , , , sRFID, , , , , rs("last_edit_at").Value)
//    Else
//        bSuccess = False
//    End If
//    If bSuccess = False Then
//        If InStr(1, sError, errRowNotFound) > 0 Then
//            'the item doesn't exist so just insert it instead
//            sError = InsertInventTable(oInventTable)
//        End If
//    Else
//        sError2 = InsertItemAttr(oInventTable("ItemID").Value, "EAN13", sEAN13)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = InsertItemAttr(oInventTable("ItemID").Value, "PACK_QTY", sQtyInPack)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = InsertItemAttr(oInventTable("ItemID").Value, "MODEL", CStr(oInventTable("SF_PBAMODELTYPEID").Value))
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = InsertItemAttr(oInventTable("ItemID").Value, "STANDARD_QTY", sStdQty)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    End If
//    UpdateInventTable = sError


//Exit Function
//errUpdateInventTable:
//    sError = Err.Number & " " & Err.Description
//    Resume Next

//End Function

#endregion OldCode
        }

        public virtual string Delete()
        {
            var sError = string.Empty;
            //TODO: oFIProdItem.Delete(gvlngSessionID, goIntfTable("RefId").Value) RefId - неизвестное поле
            try
            {
                if(Factelligence.GlobalMethods.Instance.InstanceItem.GetByKey(Id.ToString()))
                {
                    Factelligence.GlobalMethods.Instance.InstanceItem.Delete(Factelligence.GlobalMethods.Instance.SessionId, Id);
                }
            }
            catch (Exception e)
            {
                sError = e.Message;
                sError += " - Item '" + Id + "' поставлена на удаление, возможно данная позиция не удалена или данной позиции не существует";
            }
            return sError;

            #region OldCode

            //    Dim oFIProdItem As New FIProd.Item
            //    Dim sError As String
            //    Dim bSuccess As Boolean
            //    bSuccess = oFIProdItem.Delete(gvlngSessionID, _
            //                        goIntfTable("RefId").Value)
            //    If bSuccess = False Then
            //        If InStr(1, sError, errRowNotFound) > 0 Then
            //            'the item doesn't exist so it can't be deleted
            //            sError = sError & " - Item '" & goIntfTable("RefId").Value & "' doesn't exist in Factelligence and can't be deleted"
            //        End If
            //    End If
            //    DeleteInventTable = sError

            //Exit Function
            //errDeleteInventTable:
            //    sError = Err.Number & " " & Err.Description
            //    Resume Next

            #endregion OldCode
        }
#endregion ICrud
    }
}
