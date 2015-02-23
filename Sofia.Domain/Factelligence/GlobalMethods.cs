using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FIClient;
using FIProd;
using Sofia.Domain.Inventory;
using Sofia.Domain.Setting;
using Item = FIProd.Item;

namespace Sofia.Domain.Factelligence
{
    public sealed class GlobalMethods : IDisposable
    {
        private static FICore.Attr instAttr;
        public const string errRowNotFound = "10001";
        private static volatile GlobalMethods instance;
        private static object syncRoot = new Object();
        private ClientSession clientSession;
        private FIProd.Item_Attr instItemAttr;
        private FIProd.Item_Class_Attr instItemClassAttr;
        private Job_Attr instanceJob_Attr;
        private FIProd.Item instanceItem;
        private FIProd.Item_Class instanceItem_Class;
        private FIProd.Job_Exec instanceJob_Exec;
        private FIProd.Wo instanceItem_Prod;
        private FIProd.Wo_Attr instanceWo_Attr;
        private FIProd.Job instanceJob;
        private FIProd.Job_Route instanceJob_Route;
        private FIEnProd.Job_Bom instanceJob_Bom;
        private FIEnProd.Bom_Item instanceBom_Item;
        private FIEnProd.Bom_Item_Subst instanceBom_Item_Subst;
        private FICore.Ent instanceEnt;
        private FIDX.Dx_Map_Imp instanceDx_Map_Imp;


        private Setting.Parameters settings;

        static GlobalMethods()
        {
            instAttr = (FICore.Attr) Activator.CreateInstance(Type.GetTypeFromProgID("FICore.Attr"));
        }

        public static int GetAttrID(string sAttrDesc, int lAttrGrp)
        {

            var rs = instAttr.GetAll(Missing.Value, Missing.Value, Missing.Value, sAttrDesc, lAttrGrp);
            if (!rs.EOF)
            {
                return rs.get_Collect("Attr_ID"); //..Value;
            }
            return 0;
        }

        private GlobalMethods()
        {
            clientSession = (ClientSession) Activator.CreateInstance(Type.GetTypeFromProgID("FIClient.ClientSession"));
            var boo= clientSession.StartSession(ClientType.clientBackgroundProcess);
            instItemAttr = (Item_Attr) Activator.CreateInstance(Type.GetTypeFromProgID("FIProd.Item_Attr"));
            instItemClassAttr =
                (Item_Class_Attr) Activator.CreateInstance(Type.GetTypeFromProgID("FIProd.Item_Class_Attr"));
            instanceItem_Class =
                (Item_Class) Activator.CreateInstance(Type.GetTypeFromProgID("FIProd.Item_Class"));
            instanceItem = (Item) Activator.CreateInstance(Type.GetTypeFromProgID("FIProd.Item"));
            instanceJob_Exec = (Job_Exec) Activator.CreateInstance(Type.GetTypeFromProgID("FIProd.Job_Exec"));
            instanceItem_Prod = (Wo) Activator.CreateInstance(Type.GetTypeFromProgID("FIProd.Wo"));
            instanceJob = (Job) Activator.CreateInstance(Type.GetTypeFromProgID("FIProd.Job"));
            instanceJob_Attr = (Job_Attr) Activator.CreateInstance(Type.GetTypeFromProgID("FIProd.Job_Attr"));
            instanceWo_Attr = (Wo_Attr) Activator.CreateInstance(Type.GetTypeFromProgID("FIProd.Wo_Attr"));
            instanceJob_Route = (Job_Route) Activator.CreateInstance(Type.GetTypeFromProgID("FIProd.Job_Route"));
            instanceJob_Bom = (FIEnProd.Job_Bom) Activator.CreateInstance(Type.GetTypeFromProgID("FIEnProd.Job_Bom"));
            instanceBom_Item = (FIEnProd.Bom_Item) Activator.CreateInstance(Type.GetTypeFromProgID("FIEnProd.Bom_Item"));
            instanceBom_Item_Subst =
                (FIEnProd.Bom_Item_Subst) Activator.CreateInstance(Type.GetTypeFromProgID("FIEnProd.Bom_Item_Subst"));
            instanceEnt = (FICore.Ent) Activator.CreateInstance(Type.GetTypeFromProgID("FICore.Ent"));
            instanceDx_Map_Imp = (FIDX.Dx_Map_Imp) Activator.CreateInstance(Type.GetTypeFromProgID("FIDX.Dx_Map_Imp"));
            //TODO:settings = new Parameters(); тестовые параметры
            settings = new Parameters();
            settings.CreateWOFromProcess = true;
            settings.WoDescIsWoId = true;
        }

        public int SessionId
        {
            get { return clientSession.SessionID; }
        }

        public static GlobalMethods Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new GlobalMethods();
                    }
                }
                return instance;
            }
        }


        #region Settings

        public Setting.Parameters Parameters
        {
            get { return settings; }
        }

        #endregion Settings

        #region ItemClass

        public Item_Class InstanceItemClass
        {
            get { return instanceItem_Class; }
        }

        public string InsertItemClassAttr(string id, string name, string value)
        {
            try
            {
                var lAttrId = GetAttrID(name, 4);
                if (lAttrId > 0)
                {
                    if (!instItemClassAttr.GetByKey(id, lAttrId))
                    {
                        var success = instItemClassAttr.Add(clientSession.SessionID, id, lAttrId, value,
                                                            DBNull.Value, "Sofia.Import.Insert");
                        if (!success)
                        {
                            return
                                string.Format(
                                    "Ошибка при вставке значения aтрибута FIProd.Item_Class_Attr: ItemClass.Id -{0}, ItemClass.Attr - {1}, ItemClass.AttrValue - {2}",
                                    id, name, value);
                        }
                    }
                    else
                    {
                        return UpdateItemClassAttr(id, name, value);
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return string.Empty;
        }

        public string UpdateItemClassAttr(string id, string name, string value)
        {
            string sError = string.Empty;
            try
            {
                var lAttrId = GetAttrID(name, 4);
                if (lAttrId > 0)
                {
                    if (instItemClassAttr.GetByKey(id, lAttrId))
                    {
                        if (!instItemClassAttr.Update(clientSession.SessionID, id, lAttrId, value,
                                                      DBNull.Value, "Sofia.Import.Update",
                                                      DateTime.Now))
                        {
                            return
                                string.Format(
                                    "Ошибка при обновлении значения aтрибута FIProd.Item_Class_Attr: ItemClass.Id -{0}, ItemClass.Attr - {1}, ItemClass.AttrValue - {2}",
                                    id, name, value);
                        }
                    }
                    else
                    {
                        return InsertItemClassAttr(id, name, value);
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains(errRowNotFound))
                {
                    return string.Empty;
                }
                return e.Message;
            }
            return sError;
        }

        #endregion ItemClass

        #region Item

        public Item InstanceItem
        {
            get { return instanceItem; }
        }

        public string InsertItemAttr(string sItemId, string sItemAttr, string sAttrValue)
        {
            try
            {
                var lAttrId = GetAttrID(sItemAttr, 1);
                if (lAttrId > 0)
                {
                    if (!instItemAttr.GetByKey(sItemId.ToString(), lAttrId))
                    {
                        var success = instItemAttr.Add(clientSession.SessionID, sItemId, lAttrId, sAttrValue,
                                                       DBNull.Value, "Sofia.Import.Insert");
                        if (!success)
                        {
                            return
                                string.Format(
                                    "Ошибка при вставке значения aтрибута FIProd.Item_Att: Item.Id -{0}, Item.Attr - {1}, Item.AttrValue - {2}",
                                    sItemId, sItemAttr, sAttrValue);
                        }
                    }
                    else
                    {
                        return UpdateItemAttr(sItemId, sItemAttr, sAttrValue);
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return string.Empty;
        }

        public string UpdateItemAttr(string sItemId, string sItemAttr, string sAttrValue)
        {
            string sError = string.Empty;
            try
            {
                var lAttrId = GetAttrID(sItemAttr, 1);
                if (lAttrId > 0)
                {
                    if (instItemAttr.GetByKey(sItemId.ToString(), lAttrId))
                    {
                        var sModId = GetItemAttrModID(sItemId, lAttrId);
                        if (!string.IsNullOrWhiteSpace(sModId))
                        {
                            var bSuccess = instItemAttr.Update(clientSession.SessionID, sItemId, lAttrId, sAttrValue,
                                                               DBNull.Value, "Sofia.Import.Update",
                                                               sModId);
                            if (!bSuccess)
                            {

                                throw new Exception();
                            }
                        }
                    }
                    else
                    {
                        sError = InsertItemAttr(sItemId, sItemAttr, sAttrValue);
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains(errRowNotFound))
                {
                    return string.Empty;
                }
                return e.Message;
            }
            return sError;
        }

        public string GetItemAttrModID(string sItemId, int lAttrID)
        {
            //var rs = instItemAttr.GetAll(sItemId, lAttrID);
            object modId = string.Empty;
            object miss = Missing.Value;
            if (instItemAttr.GetByKey(sItemId, lAttrID, ref miss, ref miss, ref miss, ref miss, ref miss, ref modId))
                return (string) modId;
            return string.Empty;
        }

        #endregion Item

        #region Production

        public FICore.Ent Ent
        {
            get { return instanceEnt; }
        }

        public int GetEntID(object sWrkCtrID)
        {
            var rs = Ent.GetAll(Missing.Value, sWrkCtrID);
            if (!rs.EOF)
                return rs.get_Collect("Ent_ID");
            return -1;

            #region OldCode

            //        'CSEH: Standard
            //Public Function GetEntID(ByVal sWrkCtrID) As Long

            //'--------------------------------------------------------------------------------
            //' Procedure  :       GetEntID
            //' Description:       Get the Entity ID for the Axapta Work Center
            //' Created by :       Debi Reese
            //' Date-Time  :       4/23/2003-1:52:18 PM
            //'
            //' Parameters :       sWrkCtrID
            //'--------------------------------------------------------------------------------


            //On Error GoTo GetEntID_Err

            //    Dim oFICoreEnt As New FICore.Ent
            //    Dim rs As New ADODB.Recordset
            //    Set rs = oFICoreEnt.GetAll(, sWrkCtrID)
            //    If Not rs.EOF Then
            //        GetEntID = rs.Fields("Ent_ID").Value
            //    Else
            //        GetEntID = -1
            //    End If

            //Exit Function

            //GetEntID_Err:
            //    Resume Next

            //End Function

            #endregion OldCode
        }

        public Job_Exec JobExec
        {
            get { return instanceJob_Exec; }
        }

        public Wo Wo
        {
            get { return instanceItem_Prod; }
        }

        public Job Job
        {
            get { return instanceJob; }
        }

        public FIEnProd.Bom_Item BomItem
        {
            get { return instanceBom_Item; }
        }

        public FIEnProd.Bom_Item_Subst BomItemSubst
        {
            get { return instanceBom_Item_Subst; }
        }

        public FIEnProd.Job_Bom JobBom
        {
            get { return instanceJob_Bom; }
        }

        public Job_Route JobRoute
        {
            get { return instanceJob_Route; }
        }

        public FIDX.Dx_Map_Imp DxMapImp
        {
            get { return instanceDx_Map_Imp; }
        }

        public double ConvertUnit(double dValBom, string sFromUnit, string sToUnit, string sItemId)
        {
            if (sFromUnit == sToUnit)
            {
                return dValBom;
            }
            //TODO:Сделать инициализацию всех конверторов
            List<UnitConvert> list = new List<UnitConvert>();
            var oUnitConvert =
                list.SingleOrDefault(x => x.FromUnit == sFromUnit && x.ToUnit == sToUnit && x.Item.Id.ToString() == sItemId);
            if (oUnitConvert != null)
            {
                return dValBom/oUnitConvert.Factor;
            }
            oUnitConvert =
                list.SingleOrDefault(x => x.FromUnit == sToUnit && x.ToUnit == sFromUnit && x.Item.Id.ToString() == sItemId);
            if (oUnitConvert != null)
            {
                return dValBom*oUnitConvert.Factor;
            }
            oUnitConvert =
                list.FirstOrDefault(x => x.FromUnit == sFromUnit && x.ToUnit == sToUnit);
            if (oUnitConvert != null)
            {
                return dValBom/oUnitConvert.Factor;
            }
            oUnitConvert =
                list.FirstOrDefault(x => x.FromUnit == sToUnit && x.ToUnit == sFromUnit);
            if (oUnitConvert != null)
            {
                return dValBom*oUnitConvert.Factor;
            }
            return dValBom;
    #region OldCode

//Private Function ConvertUnit(ByVal dValBom As Double, ByVal sFromUnit As String, ByVal sToUnit As String, sItemId As String) As Double

//'--------------------------------------------------------------------------------
//' Procedure  :       ConvertUnit
//' Description:       Recalculation Unit
//' Created by :       Renat Shaimardanov
//' Date-Time  :       09/04/2009
//'
//' Parameters :       dValBom, sFromUnit, sToUnit, sItemId
//'--------------------------------------------------------------------------------


//On Error GoTo ConvertUnit_Err


//   'Convert not needed
//   If sFromUnit = sToUnit Then
//    ConvertUnit = dValBom
//    Exit Function
//   End If

//   'Recalculation
//   Dim oUnitConvert As ADODB.Recordset

//   'Search with ItemId (direct)
//   Set oUnitConvert = goAxapta.Execute("SELECT * FROM UnitConvert WHERE FromUnit = '" _
//    & sFromUnit & "' AND ToUnit = '" & sToUnit & "' AND ItemId = '" _
//    & sItemId & "' AND DataAreaId='" & gsDataAreaID & "'")
//   If Not oUnitConvert.EOF Then
//    ConvertUnit = dValBom / CDbl(oUnitConvert("Factor").Value)
//   'Search with ItemId (reverse)
//   Else
//    Set oUnitConvert = goAxapta.Execute("SELECT * FROM UnitConvert WHERE FromUnit = '" _
//     & sToUnit & "' AND ToUnit = '" & sFromUnit & "' AND .ItemId = '" _
//     & sItemId & "' AND DataAreaId='" & gsDataAreaID & "'")
//    If Not oUnitConvert.EOF Then
//        ConvertUnit = dValBom * CDbl(oUnitConvert("Factor").Value)
//    'Search without ItemId (direct)
//    Else
//        Set oUnitConvert = goAxapta.Execute("SELECT * FROM UnitConvert WHERE FromUnit = '" _
//         & sFromUnit & "' AND ToUnit = '" & sToUnit & "' AND DataAreaId='" & gsDataAreaID & "'")
//        If Not oUnitConvert.EOF Then
//            ConvertUnit = dValBom / CDbl(oUnitConvert("Factor").Value)
//        'Search without ItemId (reverse)
//        Else
//            Set oUnitConvert = goAxapta.Execute("SELECT * FROM UnitConvert WHERE FromUnit = '" _
//             & sToUnit & "' AND ToUnit = '" & sFromUnit & "' AND DataAreaId='" & gsDataAreaID & "'")
//            If Not oUnitConvert.EOF Then
//                ConvertUnit = dValBom * CDbl(oUnitConvert("Factor").Value)
//            Else
//                ConvertUnit = dValBom
//            End If
//        End If
//    End If
//   End If


//Exit Function

//ConvertUnit_Err:
//    Resume Next

//End Function

            #endregion OldCode
        }

        public string UpdateWoAttr(string id, string name, string value)
        {
            object sModId = string.Empty;
            int lAttrID = GetAttrID(name, 6);
            if(lAttrID > 0)
            {
                object miss = Missing.Value;
                if (instanceWo_Attr.GetByKey(id, lAttrID, ref miss, ref miss, ref miss, ref miss, ref miss, ref sModId))
                {
                    if (!string.IsNullOrWhiteSpace((string)sModId))
                    {
                        if (!instanceWo_Attr.Update(SessionId, id,
                                                    lAttrID, value, DBNull.Value, "Mod_id",
                                                    sModId))
                        {
                            return
                                string.Format(
                                    "Ошибка при вставке значения aтрибута FIProd.Wo_Attr: Wo.Id -{0}, Wo.Attr - {1}, Wo.AttrValue - {2}",
                                    id, name, value);
                        }
                        return String.Empty;
                    }
                    return string.Format("Ошибка при вставке значения aтрибута FIProd.Wo_Attr: Wo.Id -{0}, Wo.Attr - {1}, Wo.AttrValue - {2}",
                        id, name, value);
                }
                return InsertWOAttr(id, name, value);
            }
            return string.Empty;
            #region OldCode
            //        'CSEH: Standard
            //Private Function UpdateWOAttr(ByVal oProdTable As ADODB.Recordset, ByVal sWOAttrName As String, ByVal sAttrValue As String) As String

            //'--------------------------------------------------------------------------------
            //' Procedure  :       UpdateWOAttr
            //' Description:       Updates WorkOrder Attributes
            //' Created by :       Debi Reese
            //' Date-Time  :       4/21/2003-3:34:57 PM
            //'
            //' Parameters :       oProdTable, sWOAttrName, sAttrValue
            //'--------------------------------------------------------------------------------


            //On Error GoTo UpdateWOAttr_Err

            //    Dim oFIProdWOAttr As New FIProd.Wo_Attr
            //    Dim lAttrID As Long
            //    Dim sModId As String
            //    Dim sError As String
            //    Dim bSuccess As Boolean
            //    lAttrID = GetAttrID(sWOAttrName, 6)
            //    If lAttrID > 0 Then
            //StartGetModID:
            //        sModId = GetWOAttrModID(oProdTable("ProdID").Value, lAttrID)
            //        If Len(sModId) > 0 Then

            //            bSuccess = oFIProdWOAttr.Update(gvlngSessionID, oProdTable("ProdID").Value, _
            //                                    lAttrID, sAttrValue, Null, "Mod_id", sModId)
            //            If bSuccess = False Then
            //                If InStr(1, sError, errRowNotFound) > 0 Then
            //                    'the modid was changed so the record was updated by another user
            //                    'get the modid and start the update over
            //                    'reset the error message
            //                    sError = ""
            //                    GoTo StartGetModID
            //                End If
            //            End If
            //        Else
            //            'the record doesn't exist so insert it
            //            sError = InsertWOAttr(oProdTable, sWOAttrName, sAttrValue)
            //        End If

            //    End If
            //    UpdateWOAttr = sError

            //Exit Function

            //UpdateWOAttr_Err:
            //    sError = Err.Description
            //    Resume Next

            //End Function
            #endregion OldCode
        }
        public string InsertWOAttr(string id, string name, string value)
        {
            var lAttrId = GetAttrID(name, 6);
            if (lAttrId > 0)
            {
                        if(instanceWo_Attr.GetByKey(id,lAttrId) || !instanceWo_Attr.Add(SessionId, id,
                                                lAttrId, value))
                        {
                            return UpdateWoAttr(id, name, value);
                        }
            }
            return string.Empty;
            #region OldCode
//'CSEH: Standard
//Private Function InsertWOAttr(ByVal oProdTable As ADODB.Recordset, ByVal sWOAttrName As String, ByVal sAttrValue As String) As String

//'--------------------------------------------------------------------------------
//' Procedure  :       InsertWOAttr
//' Description:       Insert WorkOrder Attributes
//' Created by :       Debi Reese
//' Date-Time  :       4/21/2003-3:35:56 PM
//'
//' Parameters :       oProdTable, sWOAttrName, sAttrValue
//'--------------------------------------------------------------------------------


//On Error GoTo InsertWOAttr_Err

//    Dim oFIProdWOAttr As New FIProd.Wo_Attr
//    Dim lAttrID As Long
//    Dim bSuccess As Boolean
//    Dim sError As String
//    lAttrID = GetAttrID(sWOAttrName, 6)
//    If lAttrID > 0 Then
//        bSuccess = oFIProdWOAttr.Add(gvlngSessionID, oProdTable("ProdID").Value, _
//                                lAttrID, sAttrValue)
//        If bSuccess = False Then
//            If InStr(1, sError, "PRIMARY KEY") > 0 Then
//                'the attribute already exists so just update it instead
//                sError = UpdateWOAttr(oProdTable, sWOAttrName, sAttrValue)
//            End If
//        End If
//    End If

//Exit Function

//InsertWOAttr_Err:
//    sError = Err.Description & " (InsertWOAttr)"
//    Resume Next

//End Function
            #endregion OldCode
        }

        public string InsertJobAttr(string id,int oprNum, string name, string value)
        {
            var lAttrID = GetAttrID(name, 5);
            if(lAttrID > 0)
            {
                if (instanceJob_Attr.GetByKey(id, oprNum.ToString(), 0, lAttrID) || !instanceJob_Attr.Add(SessionId, id, oprNum, 0,
                                              lAttrID, value))
                {
                    return UpdateJobAttr(id, oprNum, name, value);
                }
            }
            return string.Empty;

            #region OldCode

//Private Function InsertJobAttr(ByVal oProdTable As ADODB.Recordset, ByVal sOprNum As String, ByVal sJobAttrName As String, ByVal sAttrValue As String) As String

//'--------------------------------------------------------------------------------
//' Procedure  :       InsertJobAttr
//' Description:       Insert Job Attributes
//' Created by :       Renat Shaimardanov
//' Date-Time  :       16/02/2009
//'
//' Parameters :       oProdTable, sOprNum, sJobAttrName, sAttrValue
//'--------------------------------------------------------------------------------


//On Error GoTo InsertJobAttr_Err

//    Dim oFIProdJobAttr As New FIProd.Job_Attr
//    Dim lAttrID As Long
//    Dim bSuccess As Boolean
//    Dim sError As String

//    lAttrID = GetAttrID(sJobAttrName, 5)
//    If lAttrID > 0 Then
//        bSuccess = oFIProdJobAttr.Add(gvlngSessionID, oProdTable("ProdID").Value, sOprNum, 0, _
//                                lAttrID, sAttrValue)
//        If bSuccess = False Then
//            If InStr(1, sError, "PRIMARY KEY") > 0 Then
//                'the attribute already exists so just update it instead
//                sError = UpdateJobAttr(oProdTable, sOprNum, sJobAttrName, sAttrValue)
//            End If
//        End If
//    End If

//Exit Function

//InsertJobAttr_Err:
//    sError = Err.Description & " (InsertJobAttr)"
//    Resume Next

//End Function

            #endregion OldCode
        }
        public string GetJobAttrModID(string id, int oprNum, int attrId)
        {
            object rstrMod_Id = string.Empty;
            object miss = Missing.Value;
            if (instanceJob_Attr.GetByKey(id, oprNum.ToString(), 0, attrId, ref miss, ref miss, ref miss, ref miss,
                                      ref miss, ref rstrMod_Id))
                return (string)rstrMod_Id;
            return string.Empty;

            #region OldCode

            //    Dim rs As New ADODB.Recordset

            //    Set rs = oFIProdJobAttr.GetAll(sWorkOrder, sOprNum, 0, lAttrID)
            //    If Not rs.EOF Then
            //        GetJobAttrModID = rs.Fields("Mod_ID").Value
            //    End If


            //        Private Function GetJobAttrModID(ByVal sWorkOrder, ByVal sOprNum As String, ByVal lAttrID As String) As String

            //'--------------------------------------------------------------------------------
            //' Procedure  :       GetJobAttrModID
            //' Description:       Get the Mod_ID for Job_Attr
            //' Created by :       Renat Shaimardanov
            //' Date-Time  :       16/02/2009
            //'
            //' Parameters :       sWorkOrder, sOprNum, lAttrID
            //'--------------------------------------------------------------------------------

            //On Error GoTo errGetJobAttrModID

            //    Dim oFIProdJobAttr As New FIProd.Job_Attr
            //    Dim rs As New ADODB.Recordset

            //    Set rs = oFIProdJobAttr.GetAll(sWorkOrder, sOprNum, 0, lAttrID)
            //    If Not rs.EOF Then
            //        GetJobAttrModID = rs.Fields("Mod_ID").Value
            //    End If

            //Exit Function
            //errGetJobAttrModID:
            //    Resume Next

            //End Function

            #endregion OldCode
        }

        public string UpdateJobAttr(string id, int oprNum, string name, string value)
        {
            var lAttrID = GetAttrID(name, 5);

            if(lAttrID > 0)
            {
                var sModId = GetJobAttrModID(id, oprNum, lAttrID);
                if (!string.IsNullOrWhiteSpace(sModId))
                {
                    if (!instanceJob_Attr.Update(SessionId, id, oprNum, 0,
                                                       lAttrID, value, DBNull.Value, "Mod_id", sModId))
                    {
                         return
                                    string.Format(
                                        "Ошибка при вставке значения aтрибута FIProd.Job_Attr: Prod.Id -{0}, Prod.Attr - {1}, Prod.AttrValue - {2}, Prod.OprNum - {3}",
                                        id, name, value, oprNum);
                    }
                }
                else
                {
                    return InsertJobAttr(id, oprNum, name, value);
                }
            }
            return string.Empty;

            #region OldCode

            //'CSEH: Standard

            //Private Function UpdateJobAttr(ByVal oProdTable As ADODB.Recordset, ByVal sOprNum As String, ByVal sJobAttrName As String, ByVal sAttrValue As String) As String

            //'--------------------------------------------------------------------------------
            //' Procedure  :       UpdateJobAttr
            //' Description:       Updates Job Attributes
            //' Created by :       Renat Shaimardanov
            //' Date-Time  :       16/02/2009
            //'
            //' Parameters :       oProdTable, sOprNum, sJobAttrName, sAttrValue
            //'--------------------------------------------------------------------------------


            //On Error GoTo UpdateJobAttr_Err

            //    Dim oFIProdJobAttr As New FIProd.Job_Attr
            //    Dim lAttrID As Long
            //    Dim sModId As String
            //    Dim sError As String
            //    Dim bSuccess As Boolean
            //    lAttrID = GetAttrID(sJobAttrName, 5)

            //    If lAttrID > 0 Then
            //StartGetModID:
            //        sModId = GetJobAttrModID(oProdTable("ProdID").Value, sOprNum, lAttrID)
            //        If Len(sModId) > 0 Then

            //            bSuccess = oFIProdJobAttr.Update(gvlngSessionID, oProdTable("ProdID").Value, sOprNum, 0, _
            //                                    lAttrID, sAttrValue, Null, "Mod_id", sModId)
            //            If bSuccess = False Then
            //                If InStr(1, sError, errRowNotFound) > 0 Then
            //                    'the modid was changed so the record was updated by another user
            //                    'get the modid and start the update over
            //                    'reset the error message
            //                    sError = ""
            //                    GoTo StartGetModID
            //                End If
            //            End If
            //        Else
            //            'the record doesn't exist so insert it
            //            sError = InsertJobAttr(oProdTable, sOprNum, sJobAttrName, sAttrValue)
            //        End If

            //    End If
            //    UpdateJobAttr = sError

            //Exit Function

            #endregion OldCode
        }

        #endregion Production
        #region Implementation of IDisposable

        public void Dispose()
        {
            instanceItem = null;
            instItemAttr = null;
            instanceItem_Class = null;
            instAttr = null;
            clientSession.EndSession();
            

        }

        #endregion
    }
}
