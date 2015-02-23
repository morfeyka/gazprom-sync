using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using FIProd;
using Sofia.Domain.Factelligence;
using Sofia.Domain.Inventory;

namespace Sofia.Domain.Order
{
    /// <summary>
    /// Предоставляет информацию о заказе
    /// </summary>
    /// <remarks>table ProdTable id ProdID reftableid 262 from fiaxIntfTabl </remarks>
    public class Production:EntityWithTypedId<string>, ICrude //:Entity
    {


        public Production()
        {
            _productionRoutes = new HashSet<ProductionRoute>();
            _boms = new HashSet<ProductionBOM>();
            _routes = new HashSet<Route>();
        }
        private readonly ISet<ProductionRoute> _productionRoutes;
        private readonly ISet<ProductionBOM> _boms;
        private readonly ISet<Route> _routes;
        /// <summary>
        /// Возвращает коллекцию номенклатурных позиций данной группы
        /// </summary>
        /// <remarks>table SF_WO2Ent routeId</remarks>
        public virtual ReadOnlyCollection<Route> Routes
        {
            get { return new ReadOnlyCollection<Route>(new List<Route>(_routes)); }
        }
        /// <summary>
        /// Возвращает коллекцию <see cref="ProductionBOM"/>
        /// </summary>
        public virtual ReadOnlyCollection<ProductionBOM> Boms
        {
            get { return new ReadOnlyCollection<ProductionBOM>(new List<ProductionBOM>(_boms)); }
        }
        /// <summary>
        /// Возвращает коллекцию номенклатурных позиций данной группы
        /// </summary>
        /// <remarks>table ProdRoute</remarks>
        public virtual ReadOnlyCollection<ProductionRoute> ProductionRoutes
        {
            get { return new ReadOnlyCollection<ProductionRoute>(new List<ProductionRoute>(_productionRoutes)); }
        }
        public virtual long RecId { get; set; }
        /// <summary>
        /// Возвращает или задает наименование продукции
        /// </summary>
        /// <remarks>field Name</remarks>
        public virtual string Name { get; set; }

        /// <summary>
        /// Возвращает или задает идентификатор области данных
        /// </summary>
        /// <example>sof</example>
        /// <remarks>field DataAreaId</remarks>
        public virtual string DataAreaId { get; set; }
        /// <summary>
        /// Возращает или задает информацию о правилах, состоянии и статусе импорта
        /// </summary>
        /// <remarks>field RecId</remarks>
   //     public virtual Interface.DataImported DataImported { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор маршрута
        /// </summary>
        /// <remarks>field RouteId</remarks>
        public virtual string RouteId { get; set; }
        /// <summary>
        /// Возвращает или задает идентификатор коллеции
        /// </summary>
        /// <remarks>field CollectRefProdId</remarks>
        public virtual string CollectRefProdId { get; set; }
        /// <summary>
        /// Возвращает или задает номенклатурную позицию
        /// </summary>
        /// <remarks>field ItemId</remarks>
        /// <example>is null - сводный заказ</example>
        public virtual Inventory.Item Item { get; set; }
        /// <summary>
        /// Возвращает или задает распродажу
        /// </summary>
        /// <remarks>field InventRefId</remarks>
        public virtual Sale Sale{ get; set; }
        /// <summary>
        /// Возвращает или задает тип номенклатурной позиции
        /// </summary>
        /// <remarks>field InventRefType</remarks>
        /// <returns>1 - Заказ на продажу</returns>
        public virtual int InventRefType { get; set; }

        /// <summary>
        /// Возвращает или задает дату заказа
        /// </summary>
        /// <remarks>field Dlvdate</remarks>
        public virtual DateTime DeliveryDate { get; set; }

        /// <summary>
        /// Возвращает или задает количество
        /// </summary>
        /// <remarks>field QtyStUp</remarks>
        public virtual int QuantityStUp { get; set; }

        /// <summary>
        /// Возвращает или задает приоритет заказа
        /// </summary>
        /// <remarks>field Prodprio</remarks>
        public virtual int Priority { get; set; }

        /// <summary>
        /// Возвращает или задает группу продуктов
        /// </summary>
        /// <remarks>field Prodgroupid</remarks>
        public virtual string GroupId { get; set; }

        /// <summary>
        /// Возвращает или задает номер задачи
        /// </summary>
        /// <remarks>field SF_ProdTaskNum</remarks>
        public virtual string SF_ProdTaskNum { get; set; }

        /// <summary>
        /// Возвращает или задает статус об импорте заказа в Factelligence
        /// </summary>
        /// <remarks>field fiaxProdStatus</remarks>
        public virtual StatusImport ProductionStatus { get; set; }

        /// <summary>
        /// Возращает или задает на какой склад пойдет заказ
        /// </summary>
        public virtual string SF_InventLocationIssueId { get; set; }
        /// <summary>
        /// Возращает или задает 
        /// </summary>
        /// <remarks>field InventDimId </remarks>
        public virtual Inventory.Dimensions Dimensions { get; set; }
        /// <summary>
        /// Возращает или задает 
        /// </summary>
        /// <remarks>field SF_NOSTANDART</remarks>
        public virtual short SF_NOSTANDART { get; set; }

        /// <summary>
        /// Возвращает или задает вычисляемое количество
        /// </summary>
        /// <remarks>field QtyCalc</remarks>
        public virtual double QtyCalc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>field sf_RFIDCommentsID_1</remarks>
        public virtual string sf_RFIDCommentsID_1 {get;set;}

        public virtual string sf_RFIDCommentsID_2 { get; set; }

        public virtual string sf_RFIDCommentsID_3 { get; set; }

        public virtual string sf_RFIDCommentsID_4 { get; set; }

        public  virtual string SF_OBLTYPE { get; set; }

        public virtual string SF_LAKTYPE_1 { get; set; }

        public virtual string SF_LAKTYPE_2 { get; set; }

        public virtual string SF_LogisticColors { get; set; }

        public virtual string BomId { get; set; }


        public virtual string CreateWoFromProcess()
        {
            var sError = String.Empty;
            int iSuccess;
            var sWODesc = string.Empty;
            var sMO = string.Empty;
            var sCustName = string.Empty;
            var sCustId = string.Empty;
            var sEmplId = string.Empty;
            DateTime dDlvDate;
            if (GlobalMethods.Instance.Parameters.WoDescIsWoId)
            {
                sWODesc = Id.ToString();
            }
            sMO = CollectRefProdId;

            if (Sale == null) // oProdTable("InventRefId").Value = "" Then
            {
                sCustName = "сводный заказ";
                sCustId = "0";
            }
            else
            {
                //        'Только если "Заказ на продажу" (1), выбираем заказчика
                if (InventRefType == 1)
                {
                    //            GetCustName oProdTable("InventRefId").Value, sCustName, sCustId
                    sCustName = Sale.Name;
                    sCustId = Sale.CustAccount;
                    #region OldCode

//'CSEH: Standard
//Private Function GetCustName(ByVal sSalesID As String, ByRef sCustName As String, ByRef sCustId As String) As Variant

//'--------------------------------------------------------------------------------
//' Procedure  :       GetCustName
//' Description:       Finds the Customer Name for the current Production Order
//'                    if it was the result of a Sales Order
//' Created by :       Debi Reese
//' Date-Time  :       4/21/2003-2:59:11 PM
//'
//' Parameters :       sSalesID
//'--------------------------------------------------------------------------------


//On Error GoTo GetCustName_Err

//    Dim oSalesTable As ADODB.Recordset
//    Set oSalesTable = goAxapta.Execute("SELECT * FROM SalesTable WHERE SalesId = '" & sSalesID & "' AND DataAreaId='" & gsDataAreaID & "'")
//    If Not oSalesTable.EOF Then
//        sCustName = oSalesTable("SalesName").Value
//        sCustId = oSalesTable("CustAccount").Value
//    Else
//        sCustName = ""
//        sCustId = ""
//    End If

//Exit Function

//GetCustName_Err:
//    Resume Next

//End Function

                    #endregion OldCode
                }
                else
                {
                    sCustName = Sale.Id;// oProdTable("InventRefId").Value
                    sCustId = "";
                }
            }
            dDlvDate = DeliveryDate;// oProdTable("Dlvdate").Value
            //TODO: Вместо DataImported.CreatedBy использовать GetEmplIdFromUserId(goIntfTable("createdBy").Value)
            sEmplId = CreatedBy;  //GetEmplIdFromUserId(goIntfTable("createdBy").Value)

            if (!GlobalMethods.Instance.Wo.GetByKey(Id.ToString()))
            {
                try
                {
                    iSuccess = GlobalMethods.Instance.JobExec.CreateWOFromProcess(GlobalMethods.Instance.SessionId, Id,
                                                                                  RouteId, Item.Id,
                                                                                  QuantityStUp, "Default Background User",
                                                                                  QuantityStUp, 1, sWODesc, DateTime.Now,
                                                                                  dDlvDate,
                                                                                  Priority, sCustName, sMO);
                    //UpdateProdStatus oProdTable("ProdID").Value, ProdOrderStatus.SentToFactelligence
                    ProductionStatus = StatusImport.SentToFactelligence;
                    //TODO:сделать сохранение в базе Axapta
                    //var executeUpdateStatus ="Update ProdTable set fiaxProdStatus=" + ProductionStatus.Status + " WHERE ProdId='" + Id +
                    //"' AND DataAreaId='" + DataImported.DataAreaId + "'";
                }
                catch(Exception e)
                {
                    if(e.Message.Contains("User is not allowed to create"))
                    {
                        sError += (e + "User = (" + sEmplId + ") Axapta userId = (" + CreatedBy + ").");
                    }
                }
            }
            else
            {
                //sError = UpdateJobStatus(Id, StatusImport.SentToFactelligence);
                var rs = GlobalMethods.Instance.Job.GetAll(Id);
                while (!rs.EOF)
                {
                    if(rs.get_Collect("State_CD") <3 || rs.get_Collect("State_CD") > 4)
                    {
                        GlobalMethods.Instance.Job.UpdateSpecific(GlobalMethods.Instance.SessionId,
                                                                  rs.get_Collect("WO_ID"),
                                                                  rs.get_Collect("Oper_ID"),
                                                                  rs.get_Collect("Seq_No"),
                                                                  Missing.Value, Missing.Value,
                                                                  StatusImport.SentToFactelligence);
                    }
                    rs.MoveNext();
                }
                #region OldCode
//Private Function UpdateJobStatus(ByVal sWorkOrder As String, _
//                                            ByVal iStatus As Integer) As String

//'--------------------------------------------------------------------------------
//' Procedure  :       UpdateJobStatus
//' Description:       Reset the job status to NEW or READY
//' Created by :       Debi Reese
//' Date-Time  :       4/28/2003-5:01:49 PM
//'
//' Parameters :       oProdTable
//'--------------------------------------------------------------------------------


//On Error GoTo UpdateJobStatus_Err

//    Dim oFIProdJob As New FIProd.Job
//    Dim rs As New ADODB.Recordset
//    Dim bSuccess As Boolean
//    Dim sError As String
//    Set rs = oFIProdJob.GetAll(sWorkOrder)
//    Do While Not rs.EOF
//        If rs.Fields("State_CD").Value < 3 Or rs.Fields("State_CD").Value > 4 Then
//            'only update the status if it is not current running or complete
//            bSuccess = oFIProdJob.UpdateSpecific(gvlngSessionID, rs.Fields("WO_ID").Value, _
//                        rs.Fields("Oper_ID").Value, rs.Fields("Seq_No").Value, , , iStatus)
//        End If
//       rs.MoveNext
//    Loop
//    
    
//    UpdateJobStatus = sError

//Exit Function

//UpdateJobStatus_Err:
//    sError = Err.Description & " (UpdateJobStatus)"
//    Resume Next

//End Function

                #endregion OldCode
                sError = Update();//UpdateProdOrder
                #region OldCode
//'CSEH: Standard
//Private Function UpdateProdOrder(ByVal oProdTable As ADODB.Recordset) As String

//'--------------------------------------------------------------------------------
//' Procedure  :       InsertProdOrder
//' Description:       Insert Production Order
//' Created by :       Renat Shaimardanov
//' Date-Time  :       26/08/2009
//'
//' Parameters :       oProdTable
//'--------------------------------------------------------------------------------



//On Error GoTo errUpdateProdOrder


//    Dim sError As String
//    Dim bSuccess As Boolean
//    Dim sJobError As String
//    Dim sBomError As String
//    Dim sRouteError As String
//    'Update WO Record
//    bSuccess = UpdateWO(oProdTable, sError)
    
//    If bSuccess = True Then
//        bSuccess = InsertJob(oProdTable, sJobError)
//         If bSuccess = True Then
//            'Update fiaxProdStatus
//            UpdateProdStatus oProdTable("ProdID").Value, ProdOrderStatus.SentToFactelligence
//         End If
//    End If

//    UpdateProdOrder = Trim$(sError & " " & sJobError & " " & sRouteError & " " & sBomError)


//Exit Function
//errUpdateProdOrder:
//        sError = Err.Number & " " & Err.Description & "  " & " (UpdateProdOrder)"
//    Resume Next


//End Function
                #endregion OldCode
            }
            return sError;
            #region OldCode

//Private Function CreateWOFromProcess(ByVal oProdTable As ADODB.Recordset) As String

//'--------------------------------------------------------------------------------
//' Procedure  :       CreateWOFromProcess
//' Description:       Create Work Order FROM Process
//' Created by :       Debi Reese
//' Date-Time  :       4/28/2003-2:30:17 PM
//'
//' Parameters :       oProdTable
//'--------------------------------------------------------------------------------


//On Error GoTo CreateWOFromProcess_Err

//    Dim sError As String
//    Dim oFIProdJobExec As New FIProd.Job_Exec
//    Dim iSuccess As Integer
//    Dim sWODesc As String
//    Dim sMO As String
//    Dim sCustName As String
//    Dim sCustId As String
//    Dim sEmplId As String
//    Dim dDlvDate As Date
//    If gbWoDescIsWoID = True Then sWODesc = oProdTable("ProdID").Value
//    sMO = oProdTable("CollectRefProdId").Value

//    If oProdTable("InventRefId").Value = "" Then
//        sCustName = "сводный заказ"
//        sCustId = "0"
//    Else
//        'Только если "Заказ на продажу" (1), выбираем заказчика
//        If oProdTable("InventRefType").Value = 1 Then
//            GetCustName oProdTable("InventRefId").Value, sCustName, sCustId
//        Else
//            sCustName = oProdTable("InventRefId").Value
//            sCustId = ""
//        End If
//    End If

//    'dDlvDate = DateAdd("s", oProdTable("Dlvtime").Value, oProdTable("Dlvdate").Value)
//    dDlvDate = oProdTable("Dlvdate").Value
//    sEmplId = GetEmplIdFromUserId(goIntfTable("createdBy").Value)
//    iSuccess = oFIProdJobExec.CreateWOFromProcess(gvlngSessionID, oProdTable("ProdID").Value, _
//                oProdTable("RouteID").Value, oProdTable("ItemID").Value, _
//                oProdTable("QtyStUp").Value, sEmplId, _
//                oProdTable("QtyStUp").Value, 1, sWODesc, Now, dDlvDate, _
//                oProdTable("Prodprio").Value, sCustName, sMO)

//    'Note CreateWOFromProcess returns 0 (Success) even if the WO already exists
//    If Len(sError) > 0 Then
//        If InStr(1, sError, "3363") > 0 Then
//            'the WO already exists so just update the status FROM on hold to new instead
//            sError = UpdateJobStatus(oProdTable("ProdID").Value, 1)
//            'update any quantity/delivery/priority changes
//            sError = UpdateProdOrder(oProdTable)
//        'note checking to see if returned error was "User is not allowed to create Work Order."
//        'but the error is truncated.
//        ElseIf (InStr(1, sError, "User is not allowed to create") > 0) Then
//            sError = sError + "User = (" + sEmplId + ") Axapta userId = (" + goIntfTable("createdBy").Value + ")."
//        End If

//    Else
//        'Update fiaxProdStatus
//        UpdateProdStatus oProdTable("ProdID").Value, ProdOrderStatus.SentToFactelligence
//    End If

//    CreateWOFromProcess = sError

//Exit Function

//CreateWOFromProcess_Err:
//    sError = Err.Description & " (CreateWOFromProcess)"
//    Resume Next

//End Function

            #endregion OldCode
        }

        public virtual string Insert()
        {
            throw new NotImplementedException();
        }

        public virtual string Update()
        {
            var sError2 = string.Empty;
            var sCustName = string.Empty;
            var sCustId = string.Empty;
            DateTime dDlvDate;
            long lSuccess;
            bool bSuccess;
            string sError=string.Empty;
            if (Sale == null)
            {
                sCustName = "сводный заказ";
                sCustId = "0";
            }
            else
            {
                //        'Только если "Заказ на продажу" (1), выбираем заказчика
                if(InventRefType == 1)
                {
                    sCustName = Sale.Name;
                    sCustId = Sale.CustAccount;
                }else
                {
                    sCustName = Sale.Id;
                    sCustId = "";
                }
            }

            if (!string.IsNullOrWhiteSpace(GroupId))
            {
                sError = GlobalMethods.Instance.UpdateWoAttr(Id, "Production Group", GroupId);
            }


            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "CUST_ID", sCustId);
            if (!string.IsNullOrWhiteSpace(sError2))
                sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "TASK_NUM", SF_ProdTaskNum);
            if (!string.IsNullOrWhiteSpace(sError2))
                sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "FINAL_HEIGHT", Item.SF_Height.ToString());
            if (!string.IsNullOrWhiteSpace(sError2))
                sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "FINAL_WIDTH", Item.SF_Width.ToString());
            if (!string.IsNullOrWhiteSpace(sError2))
                sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "PACK_HEIGHT", Item.GrossHeight.ToString());
            if (!string.IsNullOrWhiteSpace(sError2))
                sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "PACK_THICKNESS", Item.GrossDepth.ToString());
            if (!string.IsNullOrWhiteSpace(sError2))
                sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "PACK_WIDTH", Item.GrossWidth.ToString());
            if (!string.IsNullOrWhiteSpace(sError2))
                sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "TARGET_WAREHOUSE",
                                                          string.IsNullOrWhiteSpace(SF_InventLocationIssueId)
                                                              ? "МСК-ГП"
                                                              : SF_InventLocationIssueId);
            if (!string.IsNullOrWhiteSpace(sError2))
                sError += (" " + sError2);
            var listRoute = Routes;
            var Route = listRoute.Where(x => RouteId == this.RouteId).OrderBy(x => x.OperationNumber).FirstOrDefault();
            if(Route!=null)
            {

                sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "TYPE_OPENING_WAY", Route.OW);
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
                sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "TOP_SIDE_SHAPE_NAME", Route.TS);
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
                sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "HUNG_SIDE_SHAPE_NAME", Route.HS);
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
                sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "BOTTOM_SIDE_SHAPE_NAME", Route.BS);
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
                sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "LOCK_SIDE_SHAPE_NAME", Route.LS);
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
                sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "MATERIAL_TYPE_EDGES", Route.EM);
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
                sError2 = GlobalMethods.Instance.UpdateWoAttr(Id,  "MATERIAL_TYPE_SURFACE", Route.SM);
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
                sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "NET_WEIGHT", Route.WG.ToString());
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
                sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "SEALING_TYPE", Route.ST);
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
                sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "FINAL_THICKNESS", Route.TN.ToString());
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
            }

            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "LABEL1", "");
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "LABEL2", "");
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "LABEL3", "");
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "LABEL4", "");
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "LABEL5", "");
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "MATERIAL_TYPE_LEFT", Item.SF_ColorOfMaterialId_1);
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
            sError2 = GlobalMethods.Instance.UpdateWoAttr(Id, "MATERIAL_TYPE_RIGHT", Item.SF_ColorOfMaterialId_1);
            if (!string.IsNullOrWhiteSpace(sError2))
                sError += (" " + sError2);
            var rs = GlobalMethods.Instance.Wo.GetAll(Id);
            var dCurrentQty = !rs.EOF ? rs.get_Collect("Req_Qty"):0d;

            //    'Update Qty only if it has changed
            if (QuantityStUp != dCurrentQty)
            {
                lSuccess = GlobalMethods.Instance.JobExec.ChangeWOValues(GlobalMethods.Instance.SessionId, Id,
                                                                         Priority, DeliveryDate, QuantityStUp,
                                                                         QuantityStUp);
            }else
            {
                //    Else
                //        'just update DlvDate/DlvTime & ProdPrio
                lSuccess = GlobalMethods.Instance.JobExec.ChangeWOValues(GlobalMethods.Instance.SessionId, Id,
                                                                         Priority, DeliveryDate);
            }

            //    'The above should work to update all required values but it is not updating the wo table
            //    'must update it also
            bSuccess = GlobalMethods.Instance.Wo.UpdateSpecific(GlobalMethods.Instance.SessionId, Id,
                                                                      Missing.Value, Missing.Value, Missing.Value,
                                                                      Missing.Value, Missing.Value,
                                                                      QuantityStUp, Missing.Value, Missing.Value,
                                                                      Missing.Value, Priority, sCustName);
            if( lSuccess == -1 || !bSuccess)
            {
                sError += (" " + "Unable to update Production Order");
            }

            if(string.IsNullOrWhiteSpace(sError))
            {
                //    If bSuccess = True Then
                if(InsertJob(out sError))
                {
                    //            'Update fiaxProdStatus
                    //            UpdateProdStatus oProdTable("ProdID").Value, ProdOrderStatus.SentToFactelligence
                    //TODO: Сохранить состояние в базе Axapta
                    ProductionStatus = StatusImport.SentToFactelligence;
                }
            }
            return sError;

            #region OldCode

//'CSEH: Standard
//Private Function UpdateWO(ByVal oProdTable As ADODB.Recordset, _
//                            ByRef sError As String) As Boolean

//'--------------------------------------------------------------------------------
//' Procedure  :       UpdateWO
//' Description:       Update Work Order
//'                    Handles changes to the Production Order for the following
//'                    fields:  ProdGroupID, QtySched, DlvDate/DlvTime, ProdPrio
//' Created by :       Debi Rese
//' Date-Time  :       4/28/2003-10:19:35 PM
//'
//' Parameters :       oProdTable
//'--------------------------------------------------------------------------------


//On Error GoTo UpdateWO_Err

//    Dim sError2 As String
//    Dim sCustName As String
//    Dim sCustId As String
//    Dim dCurrentQty As Double
//    Dim dDlvDate As Date
//    Dim lSuccess As Long
//    Dim bSuccess As Boolean
//    Dim oFIProdJobExec As New FIProd.Job_Exec
//    Dim oFIProdWO As New FIProd.Wo
//    Dim oInventTable As ADODB.Recordset
//    Dim oSF_WO2Ent As ADODB.Recordset

//    Set oInventTable = goAxapta.Execute("SELECT * FROM InventTable WHERE ItemId = '" _
//                           & oProdTable("ItemId").Value & "'")

//     Set oSF_WO2Ent = goAxapta.Execute("SELECT * FROM SF_WO2Ent WHERE RouteId = '" _
//                            & oProdTable("RouteId").Value & "' AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum")

//    If oProdTable("InventRefId").Value = "" Then
//        sCustName = "сводный заказ"
//        sCustId = "0"
//    Else
//        'Только если "Заказ на продажу" (1), выбираем заказчика
//        If oProdTable("InventRefType").Value = 1 Then
//            GetCustName oProdTable("InventRefId").Value, sCustName, sCustId
//        Else
//            sCustName = oProdTable("InventRefId").Value
//            sCustId = ""
//        End If
//    End If

//    'update WO attribute
//    If Len(oProdTable("Prodgroupid").Value) > 0 Then
//        'Write WO Attribute for the Production Group
//        sError2 = UpdateWOAttr(oProdTable, "Production Group", oProdTable("ProdGroupID").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    End If


//    sError2 = UpdateWOAttr(oProdTable, "CUST_ID", sCustId)
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    sError2 = UpdateWOAttr(oProdTable, "TASK_NUM", oProdTable("SF_ProdTaskNum").Value)
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)

//    sError2 = UpdateWOAttr(oProdTable, "FINAL_HEIGHT", oInventTable("SF_Height").Value)
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    sError2 = UpdateWOAttr(oProdTable, "FINAL_WIDTH", oInventTable("SF_Width").Value)
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)

//    sError2 = UpdateWOAttr(oProdTable, "PACK_HEIGHT", oInventTable("grossHeight").Value)
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    sError2 = UpdateWOAttr(oProdTable, "PACK_THICKNESS", oInventTable("grossDepth").Value)
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    sError2 = UpdateWOAttr(oProdTable, "PACK_WIDTH", oInventTable("grossWidth").Value)
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    sError2 = UpdateWOAttr(oProdTable, "TARGET_WAREHOUSE", IIf(oProdTable("SF_InventLocationIssueId").Value = "", "МСК-ГП", oProdTable("SF_InventLocationIssueId").Value))
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)

//    If Not oSF_WO2Ent.EOF Then
//        sError2 = UpdateWOAttr(oProdTable, "TYPE_OPENING_WAY", "" & oSF_WO2Ent("OW").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = UpdateWOAttr(oProdTable, "TOP_SIDE_SHAPE_NAME", "" & oSF_WO2Ent("TS").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = UpdateWOAttr(oProdTable, "HUNG_SIDE_SHAPE_NAME", "" & oSF_WO2Ent("HS").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = UpdateWOAttr(oProdTable, "BOTTOM_SIDE_SHAPE_NAME", "" & oSF_WO2Ent("BS").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = UpdateWOAttr(oProdTable, "LOCK_SIDE_SHAPE_NAME", "" & oSF_WO2Ent("LS").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = UpdateWOAttr(oProdTable, "MATERIAL_TYPE_EDGES", "" & oSF_WO2Ent("EM").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = UpdateWOAttr(oProdTable, "MATERIAL_TYPE_SURFACE", "" & oSF_WO2Ent("SM").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = UpdateWOAttr(oProdTable, "NET_WEIGHT", "" & oSF_WO2Ent("WG").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = UpdateWOAttr(oProdTable, "SEALING_TYPE", "" & oSF_WO2Ent("ST").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = UpdateWOAttr(oProdTable, "FINAL_THICKNESS", "" & oSF_WO2Ent("TN").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    End If

//    sError2 = UpdateWOAttr(oProdTable, "LABEL1", "")
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    sError2 = UpdateWOAttr(oProdTable, "LABEL2", "")
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    sError2 = UpdateWOAttr(oProdTable, "LABEL3", "")
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    sError2 = UpdateWOAttr(oProdTable, "LABEL4", "")
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    sError2 = UpdateWOAttr(oProdTable, "LABEL5", "")
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)

//    sError2 = UpdateWOAttr(oProdTable, "MATERIAL_TYPE_LEFT", oInventTable("SF_ColorOfMaterialId_1").Value)
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//    sError2 = UpdateWOAttr(oProdTable, "MATERIAL_TYPE_RIGHT", oInventTable("SF_ColorOfMaterialId_1").Value)
//    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)

//    dCurrentQty = GetWOQty(oProdTable("ProdID").Value)
//    'dDlvDate = DateAdd("s", oProdTable("Dlvtime"), oProdTable("Dlvdate"))
//    dDlvDate = oProdTable("Dlvdate").Value

//    'Update Qty only if it has changed
//    If oProdTable("QtyStUp").Value <> dCurrentQty Then
//        lSuccess = oFIProdJobExec.ChangeWOValues(gvlngSessionID, oProdTable("ProdID").Value, _
//                    oProdTable("ProdPrio").Value, dDlvDate, oProdTable("QtyStUp").Value, oProdTable("QtyStUp").Value)
//      

//    Else
//        'just update DlvDate/DlvTime & ProdPrio
//        lSuccess = oFIProdJobExec.ChangeWOValues(gvlngSessionID, oProdTable("ProdID").Value, _
//                    oProdTable("ProdPrio").Value, dDlvDate)
//    End If

//    'The above should work to update all required values but it is not updating the wo table
//    'must update it also
//    bSuccess = oFIProdWO.UpdateSpecific(gvlngSessionID, oProdTable("ProdID").Value, , , , , , _
//                    oProdTable("QtyStUp").Value, , , , oProdTable("ProdPrio").Value, sCustName)
//    If lSuccess = -1 Or bSuccess = False Then
//        sError = Trim(sError & " " & "Unable to update Production Order")
//    End If
//    UpdateWO = bSuccess

//Exit Function

//UpdateWO_Err:

//    sError = Trim$(sError & Err.Number & " " & Err.Description) & " (UpdateWO)"
//    Resume Next

//End Function

            #endregion OldCode
        }
        public virtual bool InsertJob(out string sJobError)
        {
//'CSEH: Standard
//Private Function InsertJob(ByVal oProdTable As ADODB.Recordset, _
//                            ByRef sError As String) As Boolean

//On Error GoTo InsertJob_Err
            sJobError = string.Empty;
            var oFIProdJob = GlobalMethods.Instance.Job;
            var oFIProdJobRoute = GlobalMethods.Instance.JobRoute;
//    Dim oFIDXMapImp As New FIDX.Dx_Map_Imp
            int lSeqNo;
            int lStateCD;
            bool bFirstJob =false;
            bool bFinalJob=false;
            DateTime dDlvDate;
            DateTime dSchedStart;
            DateTime dSchedFinish;
            int lEntID;
            string sAssocFile = string.Empty;
            string sAssocFileType = string.Empty;
            int lDisplaySeq=0;
            int lDataLogGrpID;
            int lFolderVerID;
            int sInputOperID = 0;
            float dInputPercent;
            int lProdLineType;
            string sError2 = string.Empty;
            //ADODB.Recordset oInventDim;
            var oInventTable = Item;
            Route oSF_WO2Ent;
            var oFIEnProdJobBOM = GlobalMethods.Instance.JobBom;
            var oFIProdItem = GlobalMethods.Instance.InstanceItem;
            bool bSuccess;
            string StrTableName;
            string strSQL;
            Dimensions sInventDimId;
            string sItemId;
            string sJob_Notes = string.Empty;
            string sCustCode;

            ADODB.Recordset rsUOM;
            ADODB.Recordset rsMap;
            int UomID;
            object ColorID = string.Empty;


            bFirstJob = true;
                         
//    Set oProdRoute = goAxapta.Execute("SELECT * FROM ProdRoute WHERE ProdId = '" _
//                            & oProdTable("ProdID").Value & "' AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum")
           var oProdRoutes = ProductionRoutes.OrderBy(x=>x.OprNum).ToList();
            dDlvDate = DeliveryDate;
    
//     '!!!!!!!!!!!!!!!!!!!
//     ' После доработки в Axapta данное поле необходимо будет брать из серийного номера
//     '!!!!!!!!!!!!!!!!!!!
//    'Только если "Заказ на продажу" (1), выбираем код заказчика
    if(InventRefType==1)
    {
        sCustCode = Name;
    }else
    {
        sCustCode = "";
    }
            var sError = string.Empty;
            foreach (var item in oProdRoutes)
            {
                          var oProdRoute = item;
                           lStateCD = bFirstJob ? 2 : 1;
                //TODO:goAxapta.Execute("SELECT * FROM SF_WO2Ent WHERE RouteId = '" & oProdTable("RouteId").Value & "' AND OprNum =" & oProdRoute("Oprnum").Value & " AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum")
                                var listSF_WO2Ent = Routes;
                                oSF_WO2Ent = listSF_WO2Ent.Where(x => x.RouteId == this.RouteId && item.OprNum == x.OperationNumber).OrderBy(x=>x.OperationNumber).FirstOrDefault();
                               if (oSF_WO2Ent != null)
                               {
                                   sJob_Notes = oSF_WO2Ent.OM;
                               }

                if( oProdRoute.OprNumNext== 0)
             bFinalJob = true;
         lDisplaySeq++;
                           dSchedStart = oProdRoute.fromdate;
                           dSchedFinish = oProdRoute.Todate;
                           dInputPercent = 1;
           lEntID =GlobalMethods.Instance.GetEntID(oProdRoute.WrkCtrID);
           rsUOM = oFIProdItem.GetAll(Item.Id);
           if (rsUOM != null && rsUOM.RecordCount > 0)
           {
               UomID = rsUOM.get_Collect("uom_id");
           }
           else
           {
               sError += " No match UOM_id in table";
               return false;
           }

                sInventDimId = null;
                sItemId = "";
        
    
//          'Ищем конечный (выходной) продукт по каждой операции и при вставке операции он попадает в 0 позицию BOM (Renat Shaimardanov 12/02/2009)
                var oProdBOM =
                    Boms.Where(x => x.BOMQty < 0 && x.OprNum == oProdRoute.OprNum).OrderBy(x => x.OprNum).ThenBy(
                        x => x.LineNum).FirstOrDefault();
                //goAxapta.Execute("SELECT * FROM ProdBOM WHERE ProdId = '" _
//                            & oProdTable("ProdID").Value & "' AND BOMQty < 0 AND OprNum =" _
//                            & oProdRoute("Oprnum").Value & " AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum, LineNum")
                try
                {
                    if (oProdBOM != null)
                    {
                        //           'UomID не устанваливается - в базе четко прописано ограничение - от 0 до 5 (констрейн), по умолчанию поставили 0  в UomID (Renat Shaimardanov 12/02/2009)
                        //           'в поле vdblQty_Reqd значение oProdTable("Qtysched") заменено на 1, для того чтобы следующий WO сразу переводился в Ready после изготовления первой шт.
                        //           'oProdTable("Prodprio").Value, _ -->  oProdRoute("SF_PRIOR").Value, _ (Nikolay Tsymbalyk 18/04/2010)
                        bSuccess = oFIProdJob.Add(GlobalMethods.Instance.SessionId, Id,
                                                  oProdRoute.OprNum, 0, oProdRoute.Oprid,
                                                  oProdBOM.Item.Id, lStateCD, oProdRoute.SF_PRIOR,
                                                  DBNull.Value, lEntID, lEntID, DBNull.Value,
                                                  oProdRoute.Calcqty, 0, DBNull.Value, 0, DBNull.Value,
                                                  oProdRoute.Calcqty, Missing.Value,
                                                  Missing.Value,
                                                  oProdRoute.Processperqty, bFirstJob, bFinalJob, DBNull.Value,
                                                  GlobalMethods.Instance.Parameters.CheckInv, DBNull.Value, DBNull.Value,
                                                  oProdRoute.Setuptime, DBNull.Value,
                                                  oProdRoute.Processtime, 0, oProdRoute.Transptime,
                                                  dSchedStart, DBNull.Value, dSchedFinish, DBNull.Value, DBNull.Value,
                                                  DBNull.Value,
                                                  DBNull.Value, DBNull.Value, Missing.Value
                                                  , sJob_Notes, Missing.Value, Missing.Value, "FIAX", lDisplaySeq,
                                                  DBNull.Value, false,
                                                  DBNull.Value);
                        sInventDimId = oProdBOM.Dimensions;
                        sItemId = oProdBOM.Item.Id.ToString();
                        lProdLineType = oProdBOM.ProdLineType;
                    }
                    else
                    {
                        //           'UomID не устанваливается - в базе четко прописано ограничение - от 0 до 5 (констрейн), по умолчанию поставили 0  в UomID (Renat Shaimardanov)
                        //            'oProdTable("Prodprio").Value, _ -->  oProdRoute("SF_PRIOR").Value, _ (Nikolay Tsymbalyk 18/04/2010)
                        bSuccess = oFIProdJob.Add(GlobalMethods.Instance.SessionId, Id,
                                                  oProdRoute.OprNum, 0, oProdRoute.Oprid,
                                                  Item.Id, lStateCD, oProdRoute.SF_PRIOR,
                                                  DBNull.Value, lEntID, lEntID, DBNull.Value, oProdRoute.Calcqty
                                                  , 0, DBNull.Value, 0, DBNull.Value, oProdRoute.Calcqty, Missing.Value,
                                                  Missing.Value,
                                                  oProdRoute.Processperqty, bFirstJob, bFinalJob, DBNull.Value,
                                                  GlobalMethods.Instance.Parameters.CheckInv, DBNull.Value, DBNull.Value,
                                                  oProdRoute.Setuptime, DBNull.Value,
                                                  oProdRoute.Processtime, 0, oProdRoute.Transptime,
                                                  dSchedStart, DBNull.Value, dSchedFinish, DBNull.Value, DBNull.Value,
                                                  DBNull.Value,
                                                  DBNull.Value, DBNull.Value, Missing.Value
                                                  , sJob_Notes, Missing.Value, Missing.Value, "FIAX", lDisplaySeq,
                                                  DBNull.Value, false,
                                                  DBNull.Value);
                        sInventDimId = Dimensions;
                        sItemId = Item.Id.ToString();
                        lProdLineType = 2; //'Для нулевой позиции всегда производство
                    }
                }catch(Exception e)
                {
                    sError = e.ToString();
                    bSuccess = false;
                }
//        'Update Specification
        if(!bSuccess )
        {
            if (sError.Contains("PRIMARY KEY"))
            {
                if (oProdBOM != null)
                {
//                    'UomID не устанваливается - в базе четко прописано ограничение - от 0 до 5 (констрейн), по умолчанию поставили 0  в UomID (Renat Shaimardanov 12/02/2009)
//                    'в поле vdblQty_Reqd значение oProdTable("Qtysched") заменено на 1, для того чтобы следующий WO сразу переводился в Ready после изготовления первой шт.
//                     'oProdTable("Prodprio").Value, _ -->  oProdRoute("SF_PRIOR").Value, _ (Nikolay Tsymbalyk 18/04/2010)
                    bSuccess = oFIProdJob.UpdateSpecific(GlobalMethods.Instance.SessionId, Id,
                                                         oProdRoute.OprNum, 0, oProdRoute.Oprid,
                                                         oProdBOM.Item.Id, lStateCD, oProdRoute.SF_PRIOR,
                                                         DBNull.Value, lEntID, lEntID, DBNull.Value, oProdRoute.Calcqty
                                                         , 0, DBNull.Value, 0, DBNull.Value, oProdRoute.Calcqty,
                                                         Missing.Value,
                                                         Missing.Value,
                                                         oProdRoute.Processperqty, bFirstJob, bFinalJob, DBNull.Value,
                                                         GlobalMethods.Instance.Parameters.CheckInv, DBNull.Value,
                                                         DBNull.Value,
                                                         oProdRoute.Setuptime, DBNull.Value,
                                                         oProdRoute.Processtime, 0, oProdRoute.Transptime,
                                                         dSchedStart, DBNull.Value, dSchedFinish, DBNull.Value,
                                                         DBNull.Value,
                                                         DBNull.Value, DBNull.Value, DBNull.Value, Missing.Value
                                                         , sJob_Notes, Missing.Value, Missing.Value, "FIAX", lDisplaySeq,
                                                         DBNull.Value,
                                                         false, DBNull.Value);
                    sInventDimId = oProdBOM.Dimensions;
                    sItemId = oProdBOM.Item.Id.ToString();
                    lProdLineType = oProdBOM.ProdLineType;
                }
                else
                {

//                    'UomID не устанваливается - в базе четко прописано ограничение - от 0 до 5 (констрейн), по умолчанию поставили 0  в UomID (Renat Shaimardanov)
//                     'oProdTable("Prodprio").Value, _ -->  oProdRoute("SF_PRIOR").Value, _ (Nikolay Tsymbalyk 18/04/2010)
                    bSuccess = oFIProdJob.UpdateSpecific(GlobalMethods.Instance.SessionId, Id,
                                                         oProdRoute.OprNum, 0, oProdRoute.Oprid,
                                                         Item.Id, lStateCD, oProdRoute.SF_PRIOR,
                                                         DBNull.Value, lEntID, lEntID, DBNull.Value, oProdRoute.Calcqty
                                                         , 0, DBNull.Value, 0, DBNull.Value, oProdRoute.Calcqty,
                                                         Missing.Value,
                                                         Missing.Value,
                                                         oProdRoute.Processperqty, bFirstJob, bFinalJob, DBNull.Value,
                                                         GlobalMethods.Instance.Parameters.CheckInv, DBNull.Value,
                                                         DBNull.Value,
                                                         oProdRoute.Setuptime, DBNull.Value,
                                                         oProdRoute.Processtime, 0, oProdRoute.Transptime,
                                                         dSchedStart, DBNull.Value, dSchedFinish, DBNull.Value,
                                                         DBNull.Value,
                                                         DBNull.Value, DBNull.Value, DBNull.Value, Missing.Value
                                                         , sJob_Notes, Missing.Value, Missing.Value, "FIAX", lDisplaySeq,
                                                         DBNull.Value,
                                                         false, DBNull.Value);
                    sInventDimId = Dimensions;
                    sItemId = Item.Id.ToString();
                    lProdLineType = 2;
                    //'Для нулевой позиции всегда производство
                }
            }
        }else
        {
            if(!bFirstJob )
            {
                //TODO:InsertJobRoute(oProdRoute, sInputOperID, dInputPercent, sError2)
                //bSuccess = InsertJobRoute(oProdRoute, sInputOperID, dInputPercent, sError2)
            }

            if(!string.IsNullOrWhiteSpace(sError2))
            {
                sError += (" " + sError2);
            }
        }

                bSuccess = oFIEnProdJobBOM.UpdateSpecific(GlobalMethods.Instance.SessionId, Id,
                                                          oProdRoute.OprNum, 0, 0,
                                                          sItemId, Missing.Value, Missing.Value, Missing.Value,
                                                          Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                          true, false,
                                                          Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                                          Missing.Value, Missing.Value, true);
                                   
//        Add Job Attribute
                var sColor = string.Empty; var sWidth = string.Empty;var sHeight = string.Empty;var sComment = string.Empty;
                var oInventDim = Dimensions; 
                //goAxapta.Execute("SELECT * FROM InventDim WHERE inventDimId = '" _
//                            & sInventDimId & "' AND DataAreaId='" & gsDataAreaID & "' ORDER BY SF_WidthId")
                            
        if(oInventDim!=null)
        {
            sColor = oInventDim.SF_ColorId;
            sWidth = oInventDim.SF_WidthId;
            sHeight = oInventDim.SF_HeightId;
        }

        if(string.IsNullOrWhiteSpace(sColor))
        {
            sColor = oInventTable.SF_ColorOfMaterialId_1;
        }
        if(string.IsNullOrWhiteSpace(sWidth))
        {
            sWidth = oInventTable.SF_Width.ToString();
        }
        if (string.IsNullOrWhiteSpace(sHeight))
        {
            sHeight = oInventTable.SF_Height.ToString();
        }
        if (string.IsNullOrWhiteSpace(sColor))
        {
            sColor = "-";
        }

        if (string.IsNullOrWhiteSpace(sWidth))
        {
            sWidth = "-";
        }
        if (string.IsNullOrWhiteSpace(sHeight))
        {
            sHeight = "-";
        }

        if (sHeight == "-" && sWidth == "-" && sColor == "-")
        {
            sComment = Item.Name;
        }
        else
        {
            sComment = Item.Name + " (" + sHeight + "x" + sWidth + "/" + sColor + ")";
        }

        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "COMMENT", sComment);
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "WIDTH", sWidth);
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "HEIGHT", sHeight);
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "COLOR", sColor);
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "ITEM_CODE", Item.Id.ToString());
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "STANDARD_TYPE",
                                                     SF_NOSTANDART == 0 ? "стандарт" : "нестандарт");
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "RELEASE_DATE", dDlvDate.ToString());
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "COMMENT1", sf_RFIDCommentsID_1);
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "COMMENT2", this.sf_RFIDCommentsID_2);
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "COMMENT3", sf_RFIDCommentsID_3);
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "COMMENT4", sf_RFIDCommentsID_4);
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
//Взводим флаг обработки на DUBUS - даже если зарезки не происходит все равно изделие проходит через станок без зарезки
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "RUN_PROCESS_DUBUS", "0");

        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
//        'Сокращенный код клиента
        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "SCREEN_COMMENT_DUBUS", sCustCode);
        if (string.IsNullOrWhiteSpace(sError2))
            sError += (" " + sError2);
                
//        'Logistic Color
                if(!GlobalMethods.Instance.DxMapImp.GetByKey("LOGISTIC_COLOR", SF_LogisticColors, ref ColorID))
                {
                    ColorID = "Нет";
                }
        sError2=GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "LOGISTIC_COLOR", (string)ColorID);
        if (string.IsNullOrWhiteSpace(sError2))
              sError += (" " + sError2);                    
//        'Logistic Color RGB
          if (!GlobalMethods.Instance.DxMapImp.GetByKey("LOGISTIC_COLOR_RGB", SF_LogisticColors, ref ColorID))
          {
              ColorID = "16777215";
          }
                sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "LOGISTIC_COLOR_RGB", (string)ColorID);
                if (string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);

        
//        'Lacquer Type
//        'Вид лака 1 сторона
                if (!GlobalMethods.Instance.DxMapImp.GetByKey("LACQUER_TYPE", SF_LAKTYPE_1, ref ColorID))
                {
                    ColorID = "неизв. тип";
                }
                sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "LACQUER_TYPE_1", (string)ColorID);
                if (string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
        
//        'Вид лака 2 сторона
                if (!GlobalMethods.Instance.DxMapImp.GetByKey("LACQUER_TYPE", SF_LAKTYPE_2, ref ColorID))
                {
                    ColorID = "неизв. тип";
                }
                sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "LACQUER_TYPE_2", (string)ColorID);
                if (string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
                
//        'Тип облицовки
                sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "COATING_TYPE",
                                                               SF_OBLTYPE);
                if (string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);

                if (oSF_WO2Ent != null)
                {
                    //            'Job Attribute
                    sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "THICKNESS", oSF_WO2Ent.TN.ToString());
                    if (string.IsNullOrWhiteSpace(sError2))
                        sError += (" " + sError2);
                    sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "SHAPE_NAME", oSF_WO2Ent.SN);
                    if (string.IsNullOrWhiteSpace(sError2))
                        sError += (" " + sError2);
                    sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "ANGEL_FRONT", oSF_WO2Ent.FA.ToString());
                    if (string.IsNullOrWhiteSpace(sError2))
                        sError += (" " + sError2);
                    sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "ANGEL_REAR", oSF_WO2Ent.RA.ToString());
                    if (string.IsNullOrWhiteSpace(sError2))
                        sError += (" " + sError2);
                    sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdRoute.OprNum, "NC_PROGRAMM", oSF_WO2Ent.SOR);
                    if (string.IsNullOrWhiteSpace(sError2))
                        sError += (" " + sError2);
                    //            'WO Attribute
                    if (!String.IsNullOrWhiteSpace(oSF_WO2Ent.OW))
                    {
                        sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "TYPE_OPENING_WAY", oSF_WO2Ent.OW);
                        if (string.IsNullOrWhiteSpace(sError2))
                            sError += (" " + sError2);
                    }
                    if (!String.IsNullOrWhiteSpace(oSF_WO2Ent.TS))
                    {
                        sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "TOP_SIDE_SHAPE_NAME", oSF_WO2Ent.TS);
                        if (string.IsNullOrWhiteSpace(sError2))
                            sError += (" " + sError2);
                    }
                    if (!String.IsNullOrWhiteSpace(oSF_WO2Ent.HS))
                    {

                        sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "HUNG_SIDE_SHAPE_NAME", oSF_WO2Ent.HS);
                        if (string.IsNullOrWhiteSpace(sError2))
                            sError += (" " + sError2);
                    }
                    if (!String.IsNullOrWhiteSpace(oSF_WO2Ent.BS))
                    {
                        sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "BOTTOM_SIDE_SHAPE_NAME", oSF_WO2Ent.BS);
                        if (string.IsNullOrWhiteSpace(sError2))
                            sError += (" " + sError2);
                    }
                    if (!String.IsNullOrWhiteSpace(oSF_WO2Ent.LS))
                    {
                        sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "LOCK_SIDE_SHAPE_NAME", oSF_WO2Ent.LS);
                        if (string.IsNullOrWhiteSpace(sError2))
                            sError += (" " + sError2);
                    }
                    if (!String.IsNullOrWhiteSpace(oSF_WO2Ent.EM))
                    {
                        sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "MATERIAL_TYPE_EDGES", oSF_WO2Ent.EM);
                        if (string.IsNullOrWhiteSpace(sError2))
                            sError += (" " + sError2);
                    }
                    if (!String.IsNullOrWhiteSpace(oSF_WO2Ent.SM))
                    {
                        sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "MATERIAL_TYPE_SURFACE", oSF_WO2Ent.SM);
                        if (string.IsNullOrWhiteSpace(sError2))
                            sError += (" " + sError2);
                    }
                    double wg = 0d;
                    if (oSF_WO2Ent.WG>0)
                    {
                            sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "NET_WEIGHT", oSF_WO2Ent.WG.ToString());
                            if (string.IsNullOrWhiteSpace(sError2))
                                sError += (" " + sError2);
                    }
                    if (!String.IsNullOrWhiteSpace(oSF_WO2Ent.ST))
                    {
                        sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "SEALING_TYPE", oSF_WO2Ent.ST);
                        if (string.IsNullOrWhiteSpace(sError2))
                            sError += (" " + sError2);
                    }
                    if (oSF_WO2Ent.TN > 0)
                    {
                        sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "FINAL_THICKNESS", oSF_WO2Ent.TN.ToString());
                        if (!string.IsNullOrWhiteSpace(sError2))
                            sError += (" " + sError2);
                    }
                }

        
//        'save the current operation number to be the input_oper_id of the next operation
                sInputOperID = oProdRoute.OprNum;
                           bFirstJob = false;
                       }

            if (!String.IsNullOrWhiteSpace(sError))
            {
//    'no errors on Job/Job Route so insert the Job BOM record
                sError2 = InsertJobBOM();
                if (!string.IsNullOrWhiteSpace(sError2))
                    sError += (" " + sError2);
            }
            return true;
#region OldCode
//'CSEH: Standard
//Private Function InsertJob(ByVal oProdTable As ADODB.Recordset, _
//                            ByRef sError As String) As Boolean

//On Error GoTo InsertJob_Err

//    Dim oFIProdJob As New FIProd.Job
//    Dim oFIProdJobRoute As New FIProd.Job_Route
//    Dim oFIDXMapImp As New FIDX.Dx_Map_Imp
//    Dim lSeqNo As Long
//    Dim lStateCD As Long
//    Dim bFirstJob As Boolean
//    Dim bFinalJob As Boolean
//    Dim dDlvDate As Date
//    Dim dSchedStart As Date
//    Dim dSchedFinish As Date
//    Dim lEntID As Long
//    Dim sAssocFile As String
//    Dim sAssocFileType As String
//    Dim lDisplaySeq As Long
//    Dim lDataLogGrpID As Long
//    Dim lFolderVerID As Long
//    Dim sInputOperID As String
//    Dim dInputPercent As Double
//    Dim lProdLineType As Long
//    Dim sError2 As String
//    Dim oProdBOM As ADODB.Recordset
//    Dim oProdRoute As ADODB.Recordset
//    Dim oInventDim As ADODB.Recordset
//    Dim oInventTable As ADODB.Recordset
//    Dim oSF_WO2Ent As ADODB.Recordset
//    Dim oFIEnProdJobBOM As New FIEnProd.Job_Bom
//    Dim oFIProdItem As New FIProd.Item
//    Dim bSuccess As Boolean
//    Dim StrTableName As String
//    Dim strSQL As String
//    Dim sInventDimId As String
//    Dim sItemId As String
//    Dim sJob_Notes As String
//    Dim sCustCode As String
        
//    Dim rsUOM As ADODB.Recordset
//    Dim rsMap As ADODB.Recordset
//    Dim UomID As Long
//    Dim ColorID As String
    
    
//    bFirstJob = True
                         
//    Set oProdRoute = goAxapta.Execute("SELECT * FROM ProdRoute WHERE ProdId = '" _
//                            & oProdTable("ProdID").Value & "' AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum")
                                                        
//    'dDlvDate = DateAdd("s", oProdTable("Dlvtime").Value, oProdTable("Dlvdate").Value)
//    dDlvDate = oProdTable("Dlvdate").Value
    
//     '!!!!!!!!!!!!!!!!!!!
//     ' После доработки в Axapta данное поле необходимо будет брать из серийного номера
//     '!!!!!!!!!!!!!!!!!!!
//    'Только если "Заказ на продажу" (1), выбираем код заказчика
//    If oProdTable("InventRefType").Value = 1 Then
//        sCustCode = oProdTable("Name").Value
//    Else
//        sCustCode = ""
//    End If
    
              
//    Do While Not oProdRoute.EOF
//         If bFirstJob = True Then
//            lStateCD = 2    'ready
//         Else
//            lStateCD = 1    'new
//         End If
         
//         Set oSF_WO2Ent = goAxapta.Execute("SELECT * FROM SF_WO2Ent WHERE RouteId = '" _
//                            & oProdTable("RouteId").Value & "' AND OprNum =" & oProdRoute("Oprnum").Value _
//                            & " AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum")
                            
//         'Set oInventTable = goAxapta.Execute("SELECT * FROM InventTable WHERE ItemId = '" _
//                            '& oProdTable("ItemId").Value & "' AND DataAreaId='" & gsDataAreaID & "'")
//         Set oInventTable = goAxapta.Execute("SELECT * FROM InventTable WHERE ItemId = '" _
//                            & oProdTable("ItemId").Value & "'")
                            
//         If Not oSF_WO2Ent.EOF Then
//            sJob_Notes = "" & oSF_WO2Ent("OM").Value
//         End If
        
//         If oProdRoute("OprNumNext").Value = 0 Then bFinalJob = True
//         lDisplaySeq = lDisplaySeq + 1
//         'dSchedStart = DateAdd("s", oProdRoute("fromtime").Value, oProdRoute("fromdate").Value)
//         dSchedStart = oProdRoute("fromdate").Value
//         'dSchedFinish = DateAdd("s", oProdRoute("Totime").Value, oProdRoute("Todate").Value)
//         dSchedFinish = oProdRoute("Todate").Value
//         dInputPercent = 1
        
//         lEntID = GetEntID(oProdRoute("WrkCtrID").Value)
//         '-----------------------------------------------------------------------------------
                   
//         Set rsUOM = oFIProdItem.GetAll(oProdTable("ItemID").Value)
            
//         If rsUOM.RecordCount > 0 Then
//             UomID = CLng(rsUOM.Fields("uom_id").Value)
//         Else
//             sError = Trim$(sError & " No match UOM_id in table")
//             InsertJob = False
//             GoTo InsertJob_Err
//         End If
        
//         sInventDimId = ""
//         sItemId = ""
        
    
//          'Ищем конечный (выходной) продукт по каждой операции и при вставке операции он попадает в 0 позицию BOM (Renat Shaimardanov 12/02/2009)
//         Set oProdBOM = goAxapta.Execute("SELECT * FROM ProdBOM WHERE ProdId = '" _
//                            & oProdTable("ProdID").Value & "' AND BOMQty < 0 AND OprNum =" _
//                            & oProdRoute("Oprnum").Value & " AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum, LineNum")
         
//         If Not oProdBOM.EOF Then
//           'UomID не устанваливается - в базе четко прописано ограничение - от 0 до 5 (констрейн), по умолчанию поставили 0  в UomID (Renat Shaimardanov 12/02/2009)
//           'в поле vdblQty_Reqd значение oProdTable("Qtysched") заменено на 1, для того чтобы следующий WO сразу переводился в Ready после изготовления первой шт.
//           'oProdTable("Prodprio").Value, _ -->  oProdRoute("SF_PRIOR").Value, _ (Nikolay Tsymbalyk 18/04/2010)
//           bSuccess = oFIProdJob.Add(gvlngSessionID, oProdRoute("ProdID").Value, _
//                        oProdRoute("Oprnum").Value, 0, oProdRoute("Oprid").Value, _
//                        oProdBOM("ItemId").Value, lStateCD, oProdRoute("SF_PRIOR").Value, _
//                        Null, lEntID, lEntID, Null, oProdRoute("Calcqty").Value _
//                        , 0, Null, 0, Null, oProdRoute("Calcqty").Value, , , _
//                        oProdRoute("Processperqty").Value, bFirstJob, bFinalJob, Null, _
//                        gbCheckInv, Null, Null, oProdRoute("Setuptime").Value, Null, _
//                        oProdRoute("Processtime").Value, 0, oProdRoute("Transptime").Value, _
//                        dSchedStart, Null, dSchedFinish, Null, Null, Null, Null, Null, _
//                        , sJob_Notes, , , "FIAX", lDisplaySeq, Null, False, Null)
//                        sInventDimId = oProdBOM("InventDimId").Value
//                        sItemId = oProdBOM("ItemId").Value
//                        lProdLineType = oProdBOM("ProdLineType").Value
//         Else
//           'UomID не устанваливается - в базе четко прописано ограничение - от 0 до 5 (констрейн), по умолчанию поставили 0  в UomID (Renat Shaimardanov)
//            'oProdTable("Prodprio").Value, _ -->  oProdRoute("SF_PRIOR").Value, _ (Nikolay Tsymbalyk 18/04/2010)
//           bSuccess = oFIProdJob.Add(gvlngSessionID, oProdRoute("ProdID").Value, _
//                        oProdRoute("Oprnum").Value, 0, oProdRoute("Oprid").Value, _
//                        oProdTable("ItemID").Value, lStateCD, oProdRoute("SF_PRIOR").Value, _
//                        Null, lEntID, lEntID, Null, oProdRoute("Calcqty").Value _
//                        , 0, Null, 0, Null, oProdRoute("Calcqty").Value, , , _
//                        oProdRoute("Processperqty").Value, bFirstJob, bFinalJob, Null, _
//                        gbCheckInv, Null, Null, oProdRoute("Setuptime").Value, Null, _
//                        oProdRoute("Processtime").Value, 0, oProdRoute("Transptime").Value, _
//                        dSchedStart, Null, dSchedFinish, Null, Null, Null, Null, Null, _
//                        , sJob_Notes, , , "FIAX", lDisplaySeq, Null, False, Null)
//                        sInventDimId = oProdTable("InventDimId").Value
//                        sItemId = oProdTable("ItemID").Value
//                        lProdLineType = 2 'Для нулевой позиции всегда производство
//         End If
        
//        'Update Specification
//        If bSuccess = False Then
//            If InStr(1, sError, "PRIMARY KEY") > 0 Then
//                'the Job already exists so just update it instead
//                'Grachev 13/02/2007
//                'sError = UpdateJobRoute(oProdTable)
//                'Grachev 13/02/2007
//                'Renat 27/08/2009
//                If Not oProdBOM.EOF Then
//                    'UomID не устанваливается - в базе четко прописано ограничение - от 0 до 5 (констрейн), по умолчанию поставили 0  в UomID (Renat Shaimardanov 12/02/2009)
//                    'в поле vdblQty_Reqd значение oProdTable("Qtysched") заменено на 1, для того чтобы следующий WO сразу переводился в Ready после изготовления первой шт.
//                     'oProdTable("Prodprio").Value, _ -->  oProdRoute("SF_PRIOR").Value, _ (Nikolay Tsymbalyk 18/04/2010)
//                    bSuccess = oFIProdJob.UpdateSpecific(gvlngSessionID, oProdRoute("ProdID").Value, _
//                        oProdRoute("Oprnum").Value, 0, oProdRoute("Oprid").Value, _
//                        oProdBOM("ItemId").Value, lStateCD, oProdRoute("SF_PRIOR").Value, _
//                        Null, lEntID, lEntID, Null, oProdRoute("Calcqty").Value _
//                        , 0, Null, 0, Null, oProdRoute("Calcqty").Value, , , _
//                        oProdRoute("Processperqty").Value, bFirstJob, bFinalJob, Null, _
//                        gbCheckInv, Null, Null, oProdRoute("Setuptime").Value, Null, _
//                        oProdRoute("Processtime").Value, 0, oProdRoute("Transptime").Value, _
//                        dSchedStart, Null, dSchedFinish, Null, Null, Null, Null, Null, _
//                        , sJob_Notes, , , "FIAX", lDisplaySeq, Null, False, Null)
//                        sInventDimId = oProdBOM("InventDimId").Value
//                        sItemId = oProdBOM("ItemId").Value
//                        lProdLineType = oProdBOM("ProdLineType").Value
//                Else
//                    'UomID не устанваливается - в базе четко прописано ограничение - от 0 до 5 (констрейн), по умолчанию поставили 0  в UomID (Renat Shaimardanov)
//                     'oProdTable("Prodprio").Value, _ -->  oProdRoute("SF_PRIOR").Value, _ (Nikolay Tsymbalyk 18/04/2010)
//                    bSuccess = oFIProdJob.UpdateSpecific(gvlngSessionID, oProdRoute("ProdID").Value, _
//                        oProdRoute("Oprnum").Value, 0, oProdRoute("Oprid").Value, _
//                        oProdTable("ItemID").Value, lStateCD, oProdRoute("SF_PRIOR").Value, _
//                        Null, lEntID, lEntID, Null, oProdRoute("Calcqty").Value _
//                        , 0, Null, 0, Null, oProdRoute("Calcqty").Value, , , _
//                        oProdRoute("Processperqty").Value, bFirstJob, bFinalJob, Null, _
//                        gbCheckInv, Null, Null, oProdRoute("Setuptime").Value, Null, _
//                        oProdRoute("Processtime").Value, 0, oProdRoute("Transptime").Value, _
//                        dSchedStart, Null, dSchedFinish, Null, Null, Null, Null, Null, _
//                        , sJob_Notes, , , "FIAX", lDisplaySeq, Null, False, Null)
//                        sInventDimId = oProdTable("InventDimId").Value
//                        sItemId = oProdTable("ItemID").Value
//                        lProdLineType = 2 'Для нулевой позиции всегда производство
//                End If
//                'Renat 27/08/2009
//            End If
//        Else
//            If bFirstJob = False Then
//                bSuccess = InsertJobRoute(oProdRoute, sInputOperID, dInputPercent, sError2)
//            End If
            
//            If Len(sError2) > 0 Then
//                sError = Trim$(sError & " " & sError2)
//            Else
//                'no errors to this point so get the Job Attachments
//                'Grachev 13/02/2007
//                'sError2 = GetJobAttachments(oProdRoute)
//                'If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//                'Grachev 13/02/2007
//            End If
//        End If
             
//        bSuccess = oFIEnProdJobBOM.UpdateSpecific(gvlngSessionID, oProdRoute("ProdID").Value, _
//                            oProdRoute("Oprnum").Value, 0, 0, _
//                            sItemId, , , , , , , , True, False, , , , , , , True)
                                   
//        'Add Job Attribute
//        Dim sColor As String, sWidth As String, sHeight As String, sComment As String
//        Set oInventDim = goAxapta.Execute("SELECT * FROM InventDim WHERE inventDimId = '" _
//                            & sInventDimId & "' AND DataAreaId='" & gsDataAreaID & "' ORDER BY SF_WidthId")
                            
//        If Not oInventDim.EOF Then
//            sColor = Trim(CStr(oInventDim("SF_ColorId").Value))
//            sWidth = Trim(CStr(oInventDim("SF_WidthId").Value))
//            sHeight = Trim(CStr(oInventDim("SF_HeightId").Value))
//        End If
                     
//        If sColor = "" Then sColor = Trim(CStr(oInventTable("SF_ColorOfMaterialId_1").Value))
//        If sWidth = "" Then sWidth = Trim(CStr(oInventTable("SF_Width").Value))
//        If sHeight = "" Then sHeight = Trim(CStr(oInventTable("SF_Height").Value))
        
//        If sColor = "" Then sColor = "-"
//        If sWidth = "" Then sWidth = "-"
//        If sHeight = "" Then sHeight = "-"
                
//        If sHeight = "-" And sWidth = "-" And sColor = "-" Then
//            sComment = oInventTable("ItemName").Value
//        Else
//            sComment = oInventTable("ItemName").Value & " (" & sHeight & "x" & sWidth & "/" & sColor & ")"
//        End If
        
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "COMMENT", sComment)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        'Add Job Attribute - width
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "WIDTH", sWidth)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        'Add Job Attribute - height
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "HEIGHT", sHeight)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        'Add Job Attribute - color
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "COLOR", sColor)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
        
//        'Add Job Attribute - Item_code
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "ITEM_CODE", oProdTable("ItemId").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        'Add Job Attribute - NO_STANDARD
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "STANDARD_TYPE", IIf(oProdTable("SF_NOSTANDART").Value = 0, "стандарт", "нестандарт"))
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        'Add Job Attribute - Release_date
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "RELEASE_DATE", dDlvDate)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
        
//        'Комментарии к заказу
//        'Add Job Attribute - Comment1
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "COMMENT1", oProdTable("sf_RFIDCommentsID_1").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "COMMENT2", oProdTable("sf_RFIDCommentsID_2").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "COMMENT3", oProdTable("sf_RFIDCommentsID_3").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "COMMENT4", oProdTable("sf_RFIDCommentsID_4").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
        
                
//        'Взводим флаг обработки на DUBUS - даже если зарезки не происходит все равно изделие проходит через станок без зарезки
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "RUN_PROCESS_DUBUS", "0")
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//        'Сокращенный код клиента
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "SCREEN_COMMENT_DUBUS", sCustCode)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                
//        'Logistic Color
//        Set rsMap = oFIDXMapImp.GetAll("LOGISTIC_COLOR", oProdTable("SF_LogisticColors").Value)
//        If rsMap.RecordCount > 0 Then
//             ColorID = rsMap.Fields("internal_id").Value
//        Else
//             ColorID = "Нет"
//        End If
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "LOGISTIC_COLOR", ColorID)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    
//        'Logistic Color RGB
//        Set rsMap = oFIDXMapImp.GetAll("LOGISTIC_COLOR_RGB", oProdTable("SF_LogisticColors").Value)
        
//        If rsMap.RecordCount > 0 Then
//             ColorID = CLng(rsMap.Fields("internal_id").Value)
//        Else
//             ColorID = "16777215"
//        End If
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "LOGISTIC_COLOR_RGB", ColorID)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
        
//        'Lacquer Type
//        'Вид лака 1 сторона
//        Set rsMap = oFIDXMapImp.GetAll("LACQUER_TYPE", oProdTable("SF_LAKTYPE_1").Value)
//        If rsMap.RecordCount > 0 Then
//             ColorID = rsMap.Fields("internal_id").Value
//        Else
//             ColorID = "неизв. тип"
//        End If
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "LACQUER_TYPE_1", ColorID)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
        
//        'Вид лака 2 сторона
//        Set rsMap = oFIDXMapImp.GetAll("LACQUER_TYPE", oProdTable("SF_LAKTYPE_2").Value)
//        If rsMap.RecordCount > 0 Then
//             ColorID = rsMap.Fields("internal_id").Value
//        Else
//             ColorID = "неизв. тип"
//        End If
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "LACQUER_TYPE_2", ColorID)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                
//        'Тип облицовки
//        sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "COATING_TYPE", oProdTable("SF_OBLTYPE").Value)
//        If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
        
        
//        If Not oSF_WO2Ent.EOF Then
//            'Job Attribute
//            sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "THICKNESS", "" & oSF_WO2Ent("TN").Value)
//            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "SHAPE_NAME", "" & oSF_WO2Ent("SN").Value)
//            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "ANGEL_FRONT", "" & oSF_WO2Ent("FA").Value)
//            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "ANGEL_REAR", "" & oSF_WO2Ent("RA").Value)
//            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "NC_PROGRAMM", "" & oSF_WO2Ent("SOR").Value)
//            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            'WO Attribute
//            If Trim(CStr("" & oSF_WO2Ent("OW").Value)) <> "" Then
//                sError2 = InsertWOAttr(oProdTable, "TYPE_OPENING_WAY", "" & oSF_WO2Ent("OW").Value)
//                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            End If
//            If Trim(CStr("" & oSF_WO2Ent("TS").Value)) <> "" Then
//                sError2 = InsertWOAttr(oProdTable, "TOP_SIDE_SHAPE_NAME", "" & oSF_WO2Ent("TS").Value)
//                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            End If
//            If Trim(CStr("" & oSF_WO2Ent("HS").Value)) <> "" Then
//                sError2 = InsertWOAttr(oProdTable, "HUNG_SIDE_SHAPE_NAME", "" & oSF_WO2Ent("HS").Value)
//                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            End If
//            If Trim(CStr("" & oSF_WO2Ent("BS").Value)) <> "" Then
//                sError2 = InsertWOAttr(oProdTable, "BOTTOM_SIDE_SHAPE_NAME", "" & oSF_WO2Ent("BS").Value)
//                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            End If
//            If Trim(CStr("" & oSF_WO2Ent("LS").Value)) <> "" Then
//                sError2 = InsertWOAttr(oProdTable, "LOCK_SIDE_SHAPE_NAME", "" & oSF_WO2Ent("LS").Value)
//                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            End If
//            If Trim(CStr("" & oSF_WO2Ent("EM").Value)) <> "" Then
//                sError2 = InsertWOAttr(oProdTable, "MATERIAL_TYPE_EDGES", "" & oSF_WO2Ent("EM").Value)
//                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            End If
//            If Trim(CStr("" & oSF_WO2Ent("SM").Value)) <> "" Then
//                sError2 = InsertWOAttr(oProdTable, "MATERIAL_TYPE_SURFACE", "" & oSF_WO2Ent("SM").Value)
//                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            End If
//            If CDbl("" & oSF_WO2Ent("WG").Value) > 0 Then
//                sError2 = InsertWOAttr(oProdTable, "NET_WEIGHT", "" & oSF_WO2Ent("WG").Value)
//                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            End If
//            If Trim(CStr("" & oSF_WO2Ent("ST").Value)) <> "" Then
//                sError2 = InsertWOAttr(oProdTable, "SEALING_TYPE", "" & oSF_WO2Ent("ST").Value)
//                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            End If
//            If CDbl("" & oSF_WO2Ent("TN").Value) > 0 Then
//                sError2 = InsertWOAttr(oProdTable, "FINAL_THICKNESS", "" & oSF_WO2Ent("TN").Value)
//                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
//            End If
//        End If
        
//        'save the current operation number to be the input_oper_id of the next operation
//        sInputOperID = oProdRoute("OprNum").Value
//        bFirstJob = False
//        oProdRoute.MoveNext
                
//    Loop

//    If Len(sError) = 0 Then
//    'no errors on Job/Job Route so insert the Job BOM record
//        sError2 = InsertJobBOM(oProdTable, sError)
//        If Len(sError2) > 0 Then
//            sError = Trim$(sError & " " & sError2)
//        End If
//    End If

//    InsertJob = bSuccess

//Exit Function

//InsertJob_Err:
//    If Len(sError) > 0 Then
//        sError = sError & " " & Err.Description & " (InsertJob)"
//    Else
//        sError = Err.Description & " (InsertJob)"
//    End If
//    Resume Next

//End Function
#endregion OldCode
        }



        private string InsertJobBOM()
        {


            var oFIEnProdJobBOM = GlobalMethods.Instance.JobBom; // As New FIEnProd.Job_Bom
            var oFIEnProdBomItem = GlobalMethods.Instance.BomItem;
            var oFIEnProdJobBomSubst = GlobalMethods.Instance.BomItemSubst;
            var oFIProdJobExec = GlobalMethods.Instance.JobExec;
            int lDefStorageId = 0;
            int lBOMPos = 0;
            int lBOMPos1 = 0;
            string sAXPos = string.Empty;
            double dBOMQty = 0;
            int lByProductBOMPos = 0;
            int lAltNo = 0;
            bool bSuccess;
            bool bSubstitute;
            //    Dim oProdRoute As ADODB.Recordset
            //    Dim oProdBOM As ADODB.Recordset
            //    Dim oInventDim As ADODB.Recordset
            //    Dim oInventTable As ADODB.Recordset
            //    Dim oInventTableUp As ADODB.Recordset
            //    Dim rs As New ADODB.Recordset
            int? Oper_id = null;
            Dimensions sInventDimId = null;
            string sError2 = string.Empty;
            string sError = string.Empty;
            string sAttrName = string.Empty;
            string sAttrValue = string.Empty;


            var oProdRoute = ProductionRoutes.OrderBy(x => x.OprNum).OrderBy(x=>x.OprNum).FirstOrDefault();
            // goAxapta.Execute("SELECT * FROM ProdRoute WHERE ProdId = '" _
            //                            & oProdTable("ProdID").Value & "' AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum")
            var itemsProdBOM = Boms.OrderBy(x => x.OprNum).ThenBy(x => x.Position).ThenBy(x => x.LineNum);
            // goAxapta.Execute("SELECT * FROM ProdBOM WHERE ProdId = '" _
            //                            & oProdTable("ProdID").Value & "' AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum, Position, LineNum")

            foreach (var oProdBOM in itemsProdBOM)
            {
                //        'Начинаем нумерацию в BOM заново, если новая операция
                if (oProdBOM.OprNum != Oper_id)
                {
                    lBOMPos = 0;
                    Oper_id = oProdBOM.OprNum;
                    sAXPos = "-1000";
                }
                //        'Для проекте Софья не учитываем побочные продукты (а выходные на операции вставили в BOM - InsertJob())
                if (oProdBOM.BOMQty < 0 && oProdBOM.QtyBOMCalc < 0)
                {
                    // 'item is a by product
                    dBOMQty = -oProdBOM.BOMQty;
                    lByProductBOMPos = lByProductBOMPos - 1;
                    lBOMPos1 = lByProductBOMPos;
                }
                else
                {
                    //BOM Quantity calculate
                    if (oProdBOM.BOMQty <= 0 && oProdBOM.QtyBOMCalc > 0)
                    {
                        //BOMConsumpType
                        //Constant
                        if (oProdBOM.BOMConsump == 1)
                        {
                            dBOMQty = oProdBOM.QtyBOMCalc;
                            //Variable
                        }
                        else if (oProdBOM.BOMConsump == 0)
                        {
                            dBOMQty = oProdBOM.QtyBOMCalc/this.QtyCalc;
                        }
                        else
                        {
                            dBOMQty = oProdBOM.QtyBOMCalc;
                        }
                    }
                    else
                    {
                        dBOMQty = oProdBOM.BOMQty;
                    }

                    //if repetition 'Position' then create substitute
                    if (sAXPos == oProdBOM.Position && !string.IsNullOrWhiteSpace(oProdBOM.Position))
                    {
                        bSubstitute = true;
                        //'sItemId,lBOMPos - from previus position of bom
                        //var rs = oFIEnProdJobBomSubst.GetAll(Id, oProdBOM.OprNum, 0, lBOMPos);
                        lAltNo = oFIEnProdJobBomSubst.GetByKey(Id, oProdBOM.OprNum.ToString(), 0, lBOMPos) ? 2 : 1;
                        //If Not rs.EOF Then
                        //    lAltNo = rs.RecordCount + 1
                        //Else
                        //    lAltNo = 1
                        //End If
                    }
                    else
                    {
                        bSubstitute = false;
                        sAXPos = oProdBOM.Position;
                        lBOMPos = lBOMPos + 1;
                        lBOMPos1 = lBOMPos;
                    }

                    //            'Add Job Attribute
                    sInventDimId = oProdBOM.Dimensions;
                    if (sInventDimId!=null)
                    {
                        var oInventTable = oProdBOM.Item;
                        // goAxapta.Execute("SELECT * FROM InventTable WHERE ItemId = '" & oProdBOM("ItemId").Value & "'")
                        if (oInventTable != null)
                        {
                            //                    'Recalculation Unit
                            if (oInventTable.BomUnit.Id != oProdBOM.Unit.Id)
                            {
                                dBOMQty = GlobalMethods.Instance.ConvertUnit(dBOMQty, oProdBOM.Unit.Id,
                                                                             oInventTable.BomUnit.Id,
                                                                             oProdBOM.Item.Id);
                            }

                            //                    'Write Job Attribute for BOM and build lot_no (sLotNo) from bom attribute (Issue 430)
                            var sLotNo = string.Empty;
                            var oInventDim = sInventDimId;
                            // goAxapta.Execute("SELECT * FROM InventDim WHERE inventDimId = '" _
                            //                            & sInventDimId & "' AND DataAreaId='" & gsDataAreaID & "' ORDER BY SF_WidthId")

                            if (oInventDim != null)
                            {
                                var sColor = string.Empty;
                                var sWidth = string.Empty;
                                var sHeight = string.Empty;

                                sColor = string.IsNullOrWhiteSpace(oInventDim.SF_ColorId)
                                             ? "-"
                                             : oInventDim.SF_ColorId.Trim();
                                sWidth = string.IsNullOrWhiteSpace(oInventDim.SF_WidthId) ? "-" : oInventDim.SF_WidthId;
                                sHeight = string.IsNullOrWhiteSpace(oInventDim.SF_HeightId)? "-": oInventDim.SF_HeightId;
                                if (sColor == "-" && sWidth == "-" && sHeight == "-")
                                {
                                    sLotNo = string.Empty;
                                }
                                else
                                {
                                    sLotNo = sHeight + "x" + sWidth + "/" + sColor;
                                    //Max Attribute count
                                    if (lBOMPos1 < 9)
                                    {
                                        //Add Job Attribute - bom position
                                        sAttrName = "BomPos" + lBOMPos1;
                                        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum, sAttrName,
                                                                                       lBOMPos1.ToString());
                                        if (!string.IsNullOrWhiteSpace(sError2))
                                        {
                                            sError += (" " + sError2);
                                        }
                                        //Add Job Attribute - color
                                        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum,
                                                                                       "Color" + lBOMPos1,
                                                                                       sColor);
                                        if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                        //Add Job Attribute - width
                                        sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum,
                                                                                       "Width" + lBOMPos1,
                                                                                       sWidth);
                                        if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                        //Смотрим к какому классу относится нулевая позиция в спецификации на нулевой позиции (производимая номенклатура)
                                        var oInventTableUp = Item;
                                            //goAxapta.Execute("SELECT * FROM InventTable WHERE ItemId = '" & oProdTable("ItemId").Value & "'")
                                        //Обычная номенклатура (НЕ ВХОДИТ в группы ГП-Пг-Нл и ГП-Пг-Кр)
                                        if((oInventTableUp.Group.Id!="ГП-Пг-Нл") && (oInventTableUp.Group.Id !="ГП-Пг-Кр") ){
                                        //Add Job Attribute - height
                                            sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum,
                                                                                           "Height" + lBOMPos1, sHeight);
                                            if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                            //Для погонажа (классы ГП-Пг-Нл и ГП-Пг-Кр)
                                        }else
                                        {

                                            //Insert Job Attribute - height
                                            sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum,
                                                                                           "HEIGHT_PB", sHeight);
                                            if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);

                                            //TODO:Set oSF_WO2Ent = goAxapta.Execute("SELECT * FROM SF_WO2Ent WHERE BomID = '" _
                                            //                                         & oProdTable("BomID").Value & "' AND OprNum=" & CStr(oProdBOM("LineNum").Value) & _
                                            //                                           " AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum")

                                            var listoSF_WO2Ent = Routes;
                                            var oSF_WO2Ent =
                                                listoSF_WO2Ent.Where(
                                                    x =>
                                                    x.OperationNumber == oProdBOM.LineNum && x.BomId == oProdBOM.BomId).
                                                    OrderBy(x => x.OperationNumber).FirstOrDefault();
                                            if (oSF_WO2Ent != null)
                                            {
                                            //Add Job Attribute - height + bom_pos
                                                sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum,
                                                                                               "Height" + lBOMPos1, oSF_WO2Ent.HEIGHT.ToString());
                                                if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                                //Add Job Attribute - Type_opening_way
                                                sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum,
                                                                                               "Type_opening_way" + lBOMPos1, oSF_WO2Ent.OW);
                                                if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                                //Add Job Attribute - ANGLE_FRONT
                                                sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum,
                                                                                               "ANGLE_FRONT"+lBOMPos1, oSF_WO2Ent.FA.ToString());
                                                if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                                //Add Job Attribute - ANGLE_FRONT
                                                sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum,
                                                                                               "ANGLE_REAR" +lBOMPos1, oSF_WO2Ent.RA.ToString());
                                                if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                                //                                    'Add Job Attribute - OPERATION_RECORD_DUBUS + #
                                                sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum,
                                                                                               "OPERATION_RECORD_DUBUS" +lBOMPos1, oSF_WO2Ent.SOR);
                                                if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                                //                                    'Add Job Attribute - Dub_qty + #
                                                sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum,
                                                                                               "Dub_qty" +lBOMPos1, oSF_WO2Ent.QTY.ToString());
                                                if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                                //                                     'Add Job Attribute - SHAPE_NAME
                                                sError2 = GlobalMethods.Instance.InsertJobAttr(Id, oProdBOM.OprNum,
                                                                                               "SHAPE_NAME",
                                                                                               oSF_WO2Ent.SN);
                                                if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                                //                                    'Add WO Attribute - MATERIAL_TYPE_EDGES
                                                sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "MATERIAL_TYPE_EDGES",
                                                                                              oSF_WO2Ent.EM);
                                                if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                                //                                    'Add WO Attribute - MATERIAL_TYPE_SURFACE
                                                sError2 = GlobalMethods.Instance.InsertWOAttr(Id,"MATERIAL_TYPE_SURFACE", oSF_WO2Ent.SM);
                                                if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                                //                                    'Add WO Attribute - SEALING_TYPE
                                                sError2 = GlobalMethods.Instance.InsertWOAttr(Id, "SEALING_TYPE",
                                                                                              oSF_WO2Ent.ST);
                                                if (!string.IsNullOrWhiteSpace(sError2)) sError += (" " + sError2);
                                            }
                                    }
                                }
                                }
                            }
                            else
                            {
                                sLotNo = "";
                            }

                            //Склад с которого берется по умолчанию

                            object miss = Missing.Value;
                            object def_from_ent_Id = 0;
                            var res =
                                GlobalMethods.Instance.JobExec.GetByKey(
                                    GlobalMethods.Instance.GetEntID(oProdRoute.WrkCtrID), miss,ref miss,ref miss,ref miss, ref miss, ref miss, ref def_from_ent_Id );
                            // oFIProdJobExec.GetAll(GetEntID(oProdRoute("WrkCtrID").Value))
                            if(res && def_from_ent_Id != null)
                            {
                                lDefStorageId = (int)def_from_ent_Id;
                            }
                            else
                            {
                                lDefStorageId = -1;
                            }
                            //Только готовая продукция (ГП) и полуфабрикаты (ПФ) содержат RFID, поэтому аттрибуты подгружаем только для них

                            //Ранее было еще оно условие - "Or (oInventTable("ItemGroupId").Value Like "ПФ*" And oProdBOM("ProdLineType").Value = 2)", т.е
                            // в ERP пометкой "Производство"(2(ProdLineType)), условие исключено,
                            // поскольку стекло хоть и производиться, но в MES потребляется может только по нормативу, т.к. не имеет RFID метки и
                            // автоматизированный учет не возможен (Renat - Issue 456)
                            //if(oInventTable("ItemGroupId").Value Like "ГП*" Or oInventTable("ItemGroupId").Value Like "ПФ-RFID"){
                            var isRFIDandStartGP = Item.Group.Id.StartsWith("ГП") || Item.Group.Id == "ПФ-RFID";
                            var isPFprochee = (oInventTable.Group.Id == "ПФ-Проч");
                                //Get addition parameters of BOM Line
                                object Reqd_Grade_Cd = 0;
                                object Def_Reas_Cd = 0;
                                object Def_Storage_Ent_Id = 0;
                                var resultGetKey = oFIEnProdBomItem.GetByKey(Item.Id.ToString(), BomId, lBOMPos1, ref miss,
                                                          ref Reqd_Grade_Cd, ref miss, ref miss, ref miss, ref miss,
                                                          ref miss, ref miss, ref Def_Reas_Cd, ref miss, ref miss,
                                                          ref Def_Storage_Ent_Id);
                                    //var rs = oFIEnProdBomItem.GetAll(oProdTable("ItemId").Value, oProdTable("BomId").Value, ,
                                    //                                 oProdBOM("ItemId").Value);
                                    if (bSubstitute)
                                    {
                                        if(!oFIEnProdJobBomSubst.GetByKey(this.Id,oProdBOM.OprNum.ToString(),0,lBOMPos1))
                                        {
                                            bSuccess = oFIEnProdJobBomSubst.Add(GlobalMethods.Instance.SessionId,
                                                                                this.Id, oProdBOM.OprNum, 0, lBOMPos1,
                                                                                lAltNo, 1, 99, oProdBOM.Item.Id,
                                                                                resultGetKey? Reqd_Grade_Cd:miss, 
                                                                                miss,
                                                                                dBOMQty,
                                                                                miss, miss, miss, miss,
                                                                                isRFIDandStartGP || isPFprochee, 
                                                                                !isRFIDandStartGP && isPFprochee,
                                                                                resultGetKey?Def_Reas_Cd:miss,
                                                                                isRFIDandStartGP?miss:sLotNo,
                                                                                resultGetKey ? Def_Storage_Ent_Id : (isRFIDandStartGP ? miss : (isPFprochee ? (lDefStorageId == -1 ? (object)DBNull.Value : lDefStorageId) : miss)), 
                                                                                miss, miss,
                                                                                isRFIDandStartGP || isPFprochee, 
                                                                                isRFIDandStartGP || GlobalMethods.Instance.Parameters.gbMayChooseAltInvLoc,
                                                                                isRFIDandStartGP,
                                                                                GlobalMethods.Instance.Parameters.gbMustConsumeFromWip,
                                                                                GlobalMethods.Instance.Parameters.gbMustConsumeBeforeProd);
                                        }else
                                            bSuccess = oFIEnProdJobBomSubst.UpdateSpecific(GlobalMethods.Instance.SessionId,
                                                                                    this.Id, oProdBOM.OprNum, 0, lBOMPos1,
                                                                                    lAltNo, 1, 99, oProdBOM.Item.Id,
                                                                                    resultGetKey ? Reqd_Grade_Cd : miss,
                                                                                    miss,
                                                                                    dBOMQty,
                                                                                    miss, miss, miss, miss,
                                                                                    isRFIDandStartGP || isPFprochee,
                                                                                    !isRFIDandStartGP && isPFprochee,
                                                                                    resultGetKey ? Def_Reas_Cd : miss,
                                                                                    isRFIDandStartGP ? miss : sLotNo,
                                                                                    resultGetKey ? Def_Storage_Ent_Id : (isRFIDandStartGP ? miss : (isPFprochee ? (lDefStorageId == -1 ? (object)DBNull.Value : lDefStorageId) : miss)),
                                                                                    miss, miss,
                                                                                    isRFIDandStartGP || isPFprochee,
                                                                                    isRFIDandStartGP || GlobalMethods.Instance.Parameters.gbMayChooseAltInvLoc,
                                                                                    isRFIDandStartGP,
                                                                                    GlobalMethods.Instance.Parameters.gbMustConsumeFromWip,
                                                                                    GlobalMethods.Instance.Parameters.gbMustConsumeBeforeProd);
                                    }
                                    else
                                    {
                                        if(!oFIEnProdJobBOM.GetByKey(this.Id, oProdBOM.OprNum.ToString(),0,lBOMPos1))
                                        {


                                            bSuccess = oFIEnProdJobBOM.Add(GlobalMethods.Instance.SessionId, this.Id,oProdBOM.OprNum, 0, lBOMPos1,oProdBOM.Item.Id,
                                                                           resultGetKey?Reqd_Grade_Cd:miss, 
                                                                           miss, dBOMQty,miss, miss, miss, miss,
                                                                           isRFIDandStartGP || isPFprochee, 
                                                                           !isRFIDandStartGP,
                                                                           resultGetKey?Def_Reas_Cd:miss,
                                                                           isRFIDandStartGP?miss:sLotNo, 
                                                                           miss,
                                                                           resultGetKey ? Def_Storage_Ent_Id : (isRFIDandStartGP ? miss : (isPFprochee ? (lDefStorageId == -1 ? (object)DBNull.Value : lDefStorageId) : miss)),//4
                                                                           miss, miss,
                                                                           isRFIDandStartGP || isPFprochee, 
                                                                           isRFIDandStartGP || GlobalMethods.Instance.Parameters.gbMayChooseAltInvLoc, 
                                                                           isRFIDandStartGP, 
                                                                           GlobalMethods.Instance.Parameters.gbMustConsumeFromWip,
                                                                           GlobalMethods.Instance.Parameters.gbMustConsumeBeforeProd);
                                        }else
                                            bSuccess = oFIEnProdJobBOM.UpdateSpecific(GlobalMethods.Instance.SessionId, this.Id, oProdBOM.OprNum, 0, lBOMPos1, oProdBOM.Item.Id,
                                                                               resultGetKey ? Reqd_Grade_Cd : miss,
                                                                               miss, dBOMQty, miss, miss, miss, miss,
                                                                               isRFIDandStartGP || isPFprochee,
                                                                               !isRFIDandStartGP,
                                                                               resultGetKey ? Def_Reas_Cd : miss,
                                                                               isRFIDandStartGP ? miss : sLotNo,
                                                                               miss,
                                                                               resultGetKey ? Def_Storage_Ent_Id : (isRFIDandStartGP ? miss : (isPFprochee ? (lDefStorageId == -1 ? (object)DBNull.Value : lDefStorageId) : miss)),//4
                                                                               miss, miss,
                                                                               isRFIDandStartGP || isPFprochee,
                                                                               isRFIDandStartGP || GlobalMethods.Instance.Parameters.gbMayChooseAltInvLoc,
                                                                               isRFIDandStartGP,
                                                                               GlobalMethods.Instance.Parameters.gbMustConsumeFromWip,
                                                                               GlobalMethods.Instance.Parameters.gbMustConsumeBeforeProd);
                                    }
                                
                                
                        }
                    }
                    //       oProdBOM.MoveNext
                    //Exit Function

                    //InsertJobBOM_Err:
                    //    If Len(sError) > 0 Then
                    //        sError = sError & " " & Err.Description & " (InsertJobBOM)"
                    //    Else
                    //        sError = Err.Description & " (InsertJobBOM)"
                    //    End If

                    //    Resume Next

                    //End Function

                    #region OldCode

                    //Private Function InsertJobBOM(ByVal oProdTable As ADODB.Recordset, ByRef sError As String) As String


                    //On Error GoTo InsertJobBOM_Err


                    //    Dim oFIEnProdJobBOM As New FIEnProd.Job_Bom
                    //    Dim oFIEnProdBomItem As New FIEnProd.Bom_Item
                    //    Dim oFIEnProdJobBomSubst As New FIEnProd.Job_Bom_Subst
                    //    Dim oFIProdJobExec As New FIProd.Job_Exec
                    //    Dim lDefStorageId As Long
                    //    Dim lBOMPos As Long
                    //    Dim lBOMPos1 As Long
                    //    Dim sAXPos As String
                    //    Dim dBOMQty As Double
                    //    Dim lByProductBOMPos As Long
                    //    Dim lAltNo As Long
                    //    Dim bSuccess As Boolean
                    //    Dim bSubstitute As Boolean
                    //    Dim oProdRoute As ADODB.Recordset
                    //    Dim oProdBOM As ADODB.Recordset
                    //    Dim oInventDim As ADODB.Recordset
                    //    Dim oInventTable As ADODB.Recordset
                    //    Dim oInventTableUp As ADODB.Recordset
                    //    Dim rs As New ADODB.Recordset
                    //    Dim Oper_id As String
                    //    Dim sInventDimId As String
                    //    Dim sError2 As String
                    //    Dim sAttrName As String
                    //    Dim sAttrValue As String


                    //    Set oProdRoute = goAxapta.Execute("SELECT * FROM ProdRoute WHERE ProdId = '" _
                    //                            & oProdTable("ProdID").Value & "' AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum")

                    //    Set oProdBOM = goAxapta.Execute("SELECT * FROM ProdBOM WHERE ProdId = '" _
                    //                            & oProdTable("ProdID").Value & "' AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum, Position, LineNum")

                    //    Do While Not oProdBOM.EOF

                    //        'Начинаем нумерацию в BOM заново, если новая операция
                    //        If oProdBOM("OprNum") <> Oper_id Then
                    //            lBOMPos = 0
                    //            Oper_id = oProdBOM("OprNum").Value
                    //            sAXPos = "-1000"
                    //        End If
                    //        'Для проекте Софья не учитываем побочные продукты (а выходные на операции вставили в BOM - InsertJob())
                    //        If CDbl(oProdBOM("BOMQty").Value) < 0 And CDbl(oProdBOM("QtyBOMCalc").Value) < 0 Then        'item is a by product
                    //            dBOMQty = -oProdBOM("BOMQty").Value
                    //            lByProductBOMPos = lByProductBOMPos - 1
                    //            lBOMPos1 = lByProductBOMPos
                    //        Else
                    //            'BOM Quantity calculate
                    //            If CDbl(oProdBOM("BOMQty").Value) <= 0 And CDbl(oProdBOM("QtyBOMCalc").Value) > 0 Then
                    //                'BOMConsumpType
                    //                'Constant
                    //                If oProdBOM("BOMConsump").Value = 1 Then
                    //                    dBOMQty = oProdBOM("QtyBOMCalc").Value
                    //                'Variable
                    //                ElseIf oProdBOM("BOMConsump").Value = 0 Then
                    //                    dBOMQty = CDbl(oProdBOM("QtyBOMCalc").Value) / CDbl(oProdTable("QtyCalc").Value)
                    //                Else
                    //                    dBOMQty = oProdBOM("QtyBOMCalc").Value
                    //                End If
                    //            Else
                    //                dBOMQty = oProdBOM("BOMQty").Value
                    //            End If

                    //            'if repetition 'Position' then create substitute
                    //            If sAXPos = CStr(oProdBOM("Position").Value) And Trim(CStr(oProdBOM("Position").Value)) <> "" Then
                    //                bSubstitute = True
                    //                'sItemId,lBOMPos - from previus position of bom
                    //                Set rs = oFIEnProdJobBomSubst.GetAll(oProdTable("ProdId").Value, oProdBOM("OprNum").Value, 0, lBOMPos)
                    //                If Not rs.EOF Then
                    //                    lAltNo = rs.RecordCount + 1
                    //                Else
                    //                    lAltNo = 1
                    //                End If
                    //            Else ' AXPos = oProdBOM("Position").Value
                    //              bSubstitute = False
                    //              sAXPos = oProdBOM("Position").Value
                    //              lBOMPos = lBOMPos + 1
                    //              lBOMPos1 = lBOMPos
                    //            End If ' AXPos = oProdBOM("Position").Value

                    //            'Add Job Attribute
                    //            sInventDimId = oProdBOM("InventDimId").Value
                    //            If sInventDimId <> "" Then
                    //                Set oInventTable = goAxapta.Execute("SELECT * FROM InventTable WHERE ItemId = '" & oProdBOM("ItemId").Value & "'")
                    //                If Not oInventTable.EOF Then
                    //                    'Recalculation Unit
                    //                    If oInventTable("BOMUnitID").Value <> oProdBOM("UnitId").Value Then
                    //                        dBOMQty = ConvertUnit(dBOMQty, oProdBOM("UnitId").Value, oInventTable("BOMUnitID").Value, oProdBOM("ItemId").Value)
                    //                    End If

                    //                    'Write Job Attribute for BOM and build lot_no (sLotNo) from bom attribute (Issue 430)
                    //                    Dim sLotNo As String
                    //                    Set oInventDim = goAxapta.Execute("SELECT * FROM InventDim WHERE inventDimId = '" _
                    //                            & sInventDimId & "' AND DataAreaId='" & gsDataAreaID & "' ORDER BY SF_WidthId")

                    //                    If Not oInventDim.EOF Then
                    //                        Dim sColor As String, sWidth As String, sHeight As String

                    //                        sColor = IIf(Trim(CStr(oInventDim("SF_ColorId").Value)) = "", "-", Trim(CStr(oInventDim("SF_ColorId").Value)))
                    //                        sWidth = IIf(Trim(CStr(oInventDim("SF_WidthId").Value)) = "", "-", Trim(CStr(oInventDim("SF_WidthId").Value)))
                    //                        sHeight = IIf(Trim(CStr(oInventDim("SF_HeightId").Value)) = "", "-", Trim(CStr(oInventDim("SF_HeightId").Value)))

                    //                        If sColor = "-" And sWidth = "-" And sHeight = "-" Then
                    //                            sLotNo = ""
                    //                        Else
                    //                          sLotNo = sHeight & "x" & sWidth & "/" & sColor
                    //                          'Max Attribute count
                    //                          If lBOMPos1 < 9 Then
                    //                            'Add Job Attribute - bom position
                    //                            sAttrName = "BomPos" & CStr(lBOMPos1)
                    //                            sError2 = InsertJobAttr(oProdTable, oProdBOM("OprNum").Value, sAttrName, CStr(lBOMPos1))
                    //                            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                            'Add Job Attribute - color
                    //                            sAttrName = "Color" & CStr(lBOMPos1)
                    //                            sError2 = InsertJobAttr(oProdTable, oProdBOM("OprNum").Value, sAttrName, sColor)
                    //                            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                            'Add Job Attribute - width
                    //                            sAttrName = "Width" & CStr(lBOMPos1)
                    //                            sError2 = InsertJobAttr(oProdTable, oProdBOM("OprNum").Value, sAttrName, sWidth)
                    //                            If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                            'Смотрим к какому классу относится нулевая позиция в спецификации на нулевой позиции (производимая номенклатура)
                    //                            Set oInventTableUp = goAxapta.Execute("SELECT * FROM InventTable WHERE ItemId = '" & oProdTable("ItemId").Value & "'")
                    //                            'Обычная номенклатура (НЕ ВХОДИТ в группы ГП-Пг-Нл и ГП-Пг-Кр)
                    //                            If Not (oInventTableUp("ItemGroupId").Value Like "ГП-Пг-Нл") And Not (oInventTableUp("ItemGroupId").Value Like "ГП-Пг-Кр") Then
                    //                                'Add Job Attribute - height
                    //                                sAttrName = "Height" & CStr(lBOMPos1)
                    //                                sError2 = InsertJobAttr(oProdTable, oProdBOM("OprNum").Value, sAttrName, sHeight)
                    //                                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                            ' Для погонажа (классы ГП-Пг-Нл и ГП-Пг-Кр)
                    //                            Else

                    //                                'Insert Job Attribute - height
                    //                                sError2 = InsertJobAttr(oProdTable, oProdBOM("OprNum").Value, "HEIGHT_PB", sHeight)
                    //                                If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)

                    //                                Dim oSF_WO2Ent As ADODB.Recordset

                    //                                Set oSF_WO2Ent = goAxapta.Execute("SELECT * FROM SF_WO2Ent WHERE BomID = '" _
                    //                                         & oProdTable("BomID").Value & "' AND OprNum=" & CStr(oProdBOM("LineNum").Value) & _
                    //                                           " AND DataAreaId='" & gsDataAreaID & "' ORDER BY OprNum")

                    //                                If Not oSF_WO2Ent.EOF Then
                    //                                    'Add Job Attribute - height + bom_pos
                    //                                    sAttrName = "Height" & CStr(lBOMPos1)
                    //                                    sAttrValue = oSF_WO2Ent("HEIGHT").Value
                    //                                    sError2 = InsertJobAttr(oProdTable, oProdBOM("OprNum").Value, sAttrName, sAttrValue)
                    //                                    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                                    'Add Job Attribute - Type_opening_way
                    //                                    sAttrName = "Type_opening_way" & CStr(lBOMPos1)
                    //                                    sAttrValue = oSF_WO2Ent("OW").Value
                    //                                    sError2 = InsertJobAttr(oProdTable, oProdBOM("OprNum").Value, sAttrName, sAttrValue)
                    //                                    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                                    'Add Job Attribute - ANGLE_FRONT
                    //                                    sAttrName = "ANGLE_FRONT" & CStr(lBOMPos1)
                    //                                    sAttrValue = oSF_WO2Ent("FA").Value
                    //                                    sError2 = InsertJobAttr(oProdTable, oProdBOM("OprNum").Value, sAttrName, sAttrValue)
                    //                                    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                                    'Add Job Attribute - ANGLE_FRONT
                    //                                    sAttrName = "ANGLE_REAR" & CStr(lBOMPos1)
                    //                                    sAttrValue = oSF_WO2Ent("RA").Value
                    //                                    sError2 = InsertJobAttr(oProdTable, oProdBOM("OprNum").Value, sAttrName, sAttrValue)
                    //                                    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                                    'Add Job Attribute - OPERATION_RECORD_DUBUS + #
                    //                                    sAttrName = "OPERATION_RECORD_DUBUS" & CStr(lBOMPos1)
                    //                                    sAttrValue = oSF_WO2Ent("SOR").Value
                    //                                    sError2 = InsertJobAttr(oProdTable, oProdBOM("OprNum").Value, sAttrName, sAttrValue)
                    //                                    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                                    'Add Job Attribute - Dub_qty + #
                    //                                    sAttrName = "Dub_qty" & CStr(lBOMPos1)
                    //                                    sAttrValue = oSF_WO2Ent("QTY").Value
                    //                                    sError2 = InsertJobAttr(oProdTable, oProdBOM("OprNum").Value, sAttrName, sAttrValue)
                    //                                    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                                     'Add Job Attribute - SHAPE_NAME
                    //                                    sError2 = InsertJobAttr(oProdTable, oProdRoute("Oprnum").Value, "SHAPE_NAME", "" & oSF_WO2Ent("SN").Value)
                    //                                    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                                    'Add WO Attribute - MATERIAL_TYPE_EDGES
                    //                                    sError2 = InsertWOAttr(oProdTable, "MATERIAL_TYPE_EDGES", "" & oSF_WO2Ent("EM").Value)
                    //                                    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                                    'Add WO Attribute - MATERIAL_TYPE_SURFACE
                    //                                    sError2 = InsertWOAttr(oProdTable, "MATERIAL_TYPE_SURFACE", "" & oSF_WO2Ent("SM").Value)
                    //                                    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                                    'Add WO Attribute - SEALING_TYPE
                    //                                    sError2 = InsertWOAttr(oProdTable, "SEALING_TYPE", "" & oSF_WO2Ent("ST").Value)
                    //                                    If Len(sError2) > 0 Then sError = Trim$(sError & " " & sError2)
                    //                                End If
                    //                            End If
                    //                          End If
                    //                        End If
                    //                    Else
                    //                        sLotNo = ""
                    //                    End If

                    //                    'Склад с которого берется по умолчанию
                    //                    Set rs = oFIProdJobExec.GetAll(GetEntID(oProdRoute("WrkCtrID").Value))
                    //                    If Not rs.EOF Then
                    //                        If IsNull(rs.Fields("def_from_ent_Id").Value) Then
                    //                            lDefStorageId = -1
                    //                        Else
                    //                            lDefStorageId = rs.Fields("def_from_ent_Id").Value
                    //                        End If
                    //                    Else
                    //                        lDefStorageId = -1
                    //                    End If

                    //                    'Только готовая продукция (ГП) и полуфабрикаты (ПФ) содержат RFID, поэтому аттрибуты подгружаем только для них

                    //                    'Ранее было еще оно условие - "Or (oInventTable("ItemGroupId").Value Like "ПФ*" And oProdBOM("ProdLineType").Value = 2)", т.е
                    //                    ' в ERP пометкой "Производство"(2(ProdLineType)), условие исключено,
                    //                    ' поскольку стекло хоть и производиться, но в MES потребляется может только по нормативу, т.к. не имеет RFID метки и
                    //                    ' автоматизированный учет не возможен (Renat - Issue 456)
                    //                    If oInventTable("ItemGroupId").Value Like "ГП*" Or oInventTable("ItemGroupId").Value Like "ПФ-RFID" Then
                    //                        ' Get addition parameters of BOM Line
                    //                        Set rs = oFIEnProdBomItem.GetAll(oProdTable("ItemId").Value, oProdTable("BomId").Value, , oProdBOM("ItemId").Value)
                    //                        If Not rs.EOF Then
                    //                            If bSubstitute Then
                    //                                bSuccess = oFIEnProdJobBomSubst.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                    , , , , True, False, rs.Fields("Def_Reas_Cd").Value, , rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                    , , True, True, True, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            Else
                    //                                bSuccess = oFIEnProdJobBOM.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                    , , , , True, False, rs.Fields("Def_Reas_Cd").Value, , , rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                    , , True, True, True, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            End If
                    //                        Else
                    //                            If bSubstitute Then
                    //                                bSuccess = oFIEnProdJobBomSubst.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                    , , , , True, False, , , , _
                    //                                    , , True, True, True, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            Else
                    //                                bSuccess = oFIEnProdJobBOM.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                    , , , , True, False, , , , _
                    //                                    , , , True, True, True, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            End If
                    //                        End If
                    //                    ElseIf oInventTable("ItemGroupId").Value Like "ПФ-Проч" Then
                    //                        ' Get addition parameters of BOM Line
                    //                        Set rs = oFIEnProdBomItem.GetAll(oProdTable("ItemId").Value, oProdTable("BomId").Value, , oProdBOM("ItemId").Value)
                    //                        If Not rs.EOF Then
                    //                            If bSubstitute Then
                    //                                bSuccess = oFIEnProdJobBomSubst.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                    , , , , True, True, rs.Fields("Def_Reas_Cd").Value, sLotNo, rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                    , , True, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            Else
                    //                                bSuccess = oFIEnProdJobBOM.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                    , , , , True, True, rs.Fields("Def_Reas_Cd").Value, sLotNo, , rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                    , , True, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            End If
                    //                        Else
                    //                            If bSubstitute Then
                    //                                bSuccess = oFIEnProdJobBomSubst.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                    , , , , True, True, , sLotNo, IIf(lDefStorageId = -1, Null, lDefStorageId), _
                    //                                    , , True, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            Else
                    //                                bSuccess = oFIEnProdJobBOM.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                    , , , , True, True, , sLotNo, , IIf(lDefStorageId = -1, Null, lDefStorageId) _
                    //                                    , , , True, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            End If
                    //                        End If
                    //                    Else
                    //                       ' Get addition parameters of BOM Line
                    //                        Set rs = oFIEnProdBomItem.GetAll(oProdTable("ItemId").Value, oProdTable("BomId").Value, , oProdBOM("ItemId").Value)
                    //                        If Not rs.EOF Then
                    //                            If bSubstitute Then
                    //                                bSuccess = oFIEnProdJobBomSubst.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                    , , , , False, True, rs.Fields("Def_Reas_Cd").Value, sLotNo, rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                    , , False, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            Else
                    //                                bSuccess = oFIEnProdJobBOM.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                    , , , , False, True, rs.Fields("Def_Reas_Cd").Value, sLotNo, , rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                    , , False, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            End If
                    //                        Else
                    //                            If bSubstitute Then
                    //                                bSuccess = oFIEnProdJobBomSubst.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                    , , , , False, True, , sLotNo, , _
                    //                                    , , False, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            Else
                    //                                bSuccess = oFIEnProdJobBOM.Add(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                    0, lBOMPos1, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                    , , , , False, True, , sLotNo, , _
                    //                                    , , , False, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                    gbMustConsumeBeforeProd)
                    //                            End If
                    //                        End If
                    //                    End If

                    //                    'Update BOM
                    //                    If bSuccess = False Then
                    //                        If InStr(1, sError, "PRIMARY KEY") > 0 Then
                    //                             'Только готовая продукция (ГП) и полуфабрикаты (ПФ) содержат RFID, поэтому аттрибуты подгружаем только для них
                    //                            'Ранее было еще оно условие - "Or (oInventTable("ItemGroupId").Value Like "ПФ*" And oProdBOM("ProdLineType").Value = 2)", т.е
                    //                            ' в ERP пометкой "Производство"(2(ProdLineType)), условие исключено,
                    //                            ' поскольку стекло хоть и производиться, но в MES потребляется может только по нормативу, т.к. не имеет RFID метки и
                    //                            ' автоматизированный учет не возможен (Renat - Issue 456)
                    //                            If oInventTable("ItemGroupId").Value Like "ГП*" Or oInventTable("ItemGroupId").Value Like "ПФ-RFID" Then
                    //                                ' Get addition parameters of BOM Line
                    //                                Set rs = oFIEnProdBomItem.GetAll(oProdTable("ItemId").Value, oProdTable("BomId").Value, , oProdBOM("ItemId").Value)
                    //                                If Not rs.EOF Then
                    //                                    If bSubstitute Then
                    //                                        bSuccess = oFIEnProdJobBomSubst.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                            , , , , True, False, rs.Fields("Def_Reas_Cd").Value, , rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                            , , True, True, True, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    Else
                    //                                        bSuccess = oFIEnProdJobBOM.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                            , , , , True, False, rs.Fields("Def_Reas_Cd").Value, , , rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                            , , True, True, True, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    End If
                    //                                Else
                    //                                    If bSubstitute Then
                    //                                        bSuccess = oFIEnProdJobBomSubst.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                            , , , , True, False, , , , _
                    //                                            , , True, True, True, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    Else
                    //                                        bSuccess = oFIEnProdJobBOM.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                            , , , , True, False, , , , _
                    //                                            , , , True, True, True, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    End If
                    //                                End If
                    //                            ElseIf oInventTable("ItemGroupId").Value Like "ПФ-Проч" Then
                    //                                 ' Get addition parameters of BOM Line
                    //                                Set rs = oFIEnProdBomItem.GetAll(oProdTable("ItemId").Value, oProdTable("BomId").Value, , oProdBOM("ItemId").Value)
                    //                                If Not rs.EOF Then
                    //                                    If bSubstitute Then
                    //                                        bSuccess = oFIEnProdJobBomSubst.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                            , , , , True, True, rs.Fields("Def_Reas_Cd").Value, sLotNo, rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                            , , True, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    Else
                    //                                        bSuccess = oFIEnProdJobBOM.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                            , , , , True, True, rs.Fields("Def_Reas_Cd").Value, sLotNo, , rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                            , , True, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    End If
                    //                                Else
                    //                                    If bSubstitute Then
                    //                                        bSuccess = oFIEnProdJobBomSubst.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                            , , , , True, True, , sLotNo, , _
                    //                                            , , True, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    Else
                    //                                        bSuccess = oFIEnProdJobBOM.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                            , , , , True, True, , sLotNo, , _
                    //                                            , , , True, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    End If
                    //                                End If
                    //                            Else
                    //                               ' Get addition parameters of BOM Line
                    //                                Set rs = oFIEnProdBomItem.GetAll(oProdTable("ItemId").Value, oProdTable("BomId").Value, , oProdBOM("ItemId").Value)
                    //                                If Not rs.EOF Then
                    //                                    If bSubstitute Then
                    //                                        bSuccess = oFIEnProdJobBomSubst.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                            , , , , False, True, rs.Fields("Def_Reas_Cd").Value, sLotNo, rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                            , , False, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    Else
                    //                                        bSuccess = oFIEnProdJobBOM.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, oProdBOM("ItemId").Value, rs.Fields("Reqd_Grade_Cd").Value, , dBOMQty, _
                    //                                            , , , , False, True, rs.Fields("Def_Reas_Cd").Value, sLotNo, , rs.Fields("Def_Storage_Ent_Id").Value, _
                    //                                            , , False, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    End If
                    //                                Else
                    //                                    If bSubstitute Then
                    //                                        bSuccess = oFIEnProdJobBomSubst.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, lAltNo, 1, 99, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                            , , , , False, True, , sLotNo, , _
                    //                                            , , False, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    Else
                    //                                        bSuccess = oFIEnProdJobBOM.UpdateSpecific(gvlngSessionID, oProdTable("ProdId").Value, oProdBOM("OprNum").Value, _
                    //                                            0, lBOMPos1, oProdBOM("ItemId").Value, , , dBOMQty, _
                    //                                            , , , , False, True, , sLotNo, , _
                    //                                            , , , False, gbMayChooseAltInvLoc, False, gbMustConsumeFromWip, _
                    //                                            gbMustConsumeBeforeProd)
                    //                                    End If
                    //                                End If
                    //                            End If
                    //                        End If
                    //                    End If
                    //                End If 
                    //            End If 
                    //       End If 
                    //       oProdBOM.MoveNext

                    //    Loop

                    //Exit Function

                    //InsertJobBOM_Err:
                    //    If Len(sError) > 0 Then
                    //        sError = sError & " " & Err.Description & " (InsertJobBOM)"
                    //    Else
                    //        sError = Err.Description & " (InsertJobBOM)"
                    //    End If

                    //    Resume Next

                    //End Function

                    #endregion OldCode
                }
             
            }
            return string.Empty;
        }



        public virtual string Delete()
        {
            throw new NotImplementedException();
        }

        public virtual string CreatedBy { get; set; }
    }
}
