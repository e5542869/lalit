﻿// Generated by .NET Reflector from E:\replace\uploadfilestodev\kccaccounts.com\Bin\AccountsAppWeb.Core.dll
namespace AccountsAppWeb.Core
{
    using AccountsAppWeb.Core.com.kccsasr.accounts;
    using AccountsAppWeb.Core.Extensions;
    using AccountsAppWeb.Core.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class TransactionsManager
    {
        private readonly AccountsAppAPI accountsAppAPI = new AccountsAppAPI();
        private readonly string sKey = string.Empty;

        public string CashBalance(int instituteId, int financialYearId)
        {
            DataSet set = this.accountsAppAPI.OpeningBalance(this.sKey, instituteId, financialYearId, instituteId, DateTime.Now.AddDays(1.0), 1, 0x10f447);
            return ((set.Tables[0].Rows.Count != 0) ? (set.Tables[0].Rows[0]["Debit"].ToString() + " Dr") : "No Op.Balance found");
        }

        [return: TupleElementNames(new string[] { "transactionLedger", "totalTransactionAmount" })]
        public ValueTuple<List<TransactionSearchLedgerViewModel>, TotalTransactionAmount> CreateLedgerVoucherBykeyword(int instId, int finId, string searchValue)
        {
            DataSet set = this.accountsAppAPI.CreateLedgerVoucherBykeyword(this.sKey, instId, finId, searchValue);
            List<TransactionSearchLedgerViewModel> d1 = this.TransactionLedgerTableToList(set.Tables[0]);
            TotalTransactionAmount amount = set.Tables[1].DataTableToList<TotalTransactionAmount>().FirstOrDefault<TotalTransactionAmount>();
            if (set != null)
            {
                return new ValueTuple<List<TransactionSearchLedgerViewModel>, TotalTransactionAmount>(d1, amount);
            }
            return new ValueTuple<List<TransactionSearchLedgerViewModel>, TotalTransactionAmount>();
        }

        [return: TupleElementNames(new string[] { "transactionLedger", "totalTransactionAmount" })]
        public ValueTuple<List<TransactionSearchLedgerViewModel>, TotalTransactionAmount> CreateLedgerVoucherList(int deptmentId, TransactionListRequestModel requestModel)
        {

            DataSet set = this.accountsAppAPI.CreateLedgerVoucherList(this.sKey, requestModel.FromDate, requestModel.ToDate, requestModel.ToInstituteId, requestModel.VoucherTypeId, deptmentId, requestModel.ForInstituteId);
            List<TransactionSearchLedgerViewModel> d1 = this.TransactionLedgerTableToList(set.Tables[0]);
            TotalTransactionAmount amount = set.Tables[1].DataTableToList<TotalTransactionAmount>().FirstOrDefault<TotalTransactionAmount>();
            if (set != null)
            {
                return new ValueTuple<List<TransactionSearchLedgerViewModel>, TotalTransactionAmount>(d1, amount);
            }
            return new ValueTuple<List<TransactionSearchLedgerViewModel>, TotalTransactionAmount>();
        }

        public List<VoucherTypeModel> GetVoucherType()
        {
            DataSet set = this.accountsAppAPI.UniversalClassesvVoucherType();
            return ((set == null) ? new List<VoucherTypeModel>() : set.Tables[0].DataTableToList<VoucherTypeModel>());
        }

        public bool MoveTransactiontoOtherLedtger(int insttituteId, int financialYearId, int FromLedgerId, int ToLedgerId)
        {
            return this.accountsAppAPI.MoveTransactiontoOtherLedtger(this.sKey, insttituteId, financialYearId, FromLedgerId, ToLedgerId);
        }

        public int SelectMaximumNewVoucherNoAutoGenerate(int insttituteId, int financialYearId, int VoucherTypeId)
        {
            return Convert.ToInt32(this.accountsAppAPI.SelectMaximumNewVoucherNoAutoGenerate(insttituteId, financialYearId, VoucherTypeId).Tables[0].Rows[0]["UniqueTransactionNo"]);
        }

        public List<TransactionSearchLedgerViewModel> TransactionLedgerTableToList(DataTable dt)
        {
            List<TransactionSearchLedgerViewModel> list = new List<TransactionSearchLedgerViewModel>();
            string str = string.Empty;
            int num = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TransactionSearchLedgerViewModel item = new TransactionSearchLedgerViewModel
                {
                    SerialId = num
                };
                num++;
                if (Convert.ToInt64(dt.Rows[i]["Debit"]) != 0)
                {
                    item.Debit = dt.Rows[i]["Debit"].ToString();
                }
                if (Convert.ToInt64(dt.Rows[i]["Credit"]) != 0)
                {
                    item.Credit = dt.Rows[i]["Credit"].ToString();
                }
                if (Convert.ToString(dt.Rows[i]["TransactionMasterId"]) != str)
                {
                    str = dt.Rows[i]["TransactionMasterId"].ToString();
                    item.TransactionMasterId = dt.Rows[i]["TransactionMasterId"].ToString();
                    item.TransactionDate = dt.Rows[i]["TransactionDate"].ToString();
                    item.VoucherTypeName = dt.Rows[i]["VoucherTypeName"].ToString();
                    item.VoucherNo = dt.Rows[i]["VoucherNo"].ToString();
                    item.ChequeNo = dt.Rows[i]["ChequeNo"].ToString();
                    item.AccountShortTitle = dt.Rows[i]["AccountShortTitle"].ToString();
                }
                item.LedgerName = dt.Rows[i]["LedgerName"].ToString();
                item.className = "bluecolor";
                list.Add(item);
                try
                {
                    if (Convert.ToInt32(dt.Rows[i]["TransactionMasterId"]) != Convert.ToInt32(dt.Rows[i + 1]["TransactionMasterId"]))
                    {
                        TransactionSearchLedgerViewModel model2 = new TransactionSearchLedgerViewModel
                        {
                            SerialId = num
                        };
                        num++;
                        model2.LedgerName = dt.Rows[i - 1]["MasterNarration"].ToString();
                        model2.TransactionMasterId = dt.Rows[i - 1]["TransactionMasterId"].ToString();
                        model2.className = "redcolor";
                        list.Add(model2);
                    }
                }
                catch
                {
                    TransactionSearchLedgerViewModel model3 = new TransactionSearchLedgerViewModel
                    {
                        SerialId = num
                    };
                    num++;
                    model3.LedgerName = dt.Rows[dt.Rows.Count - 1]["MasterNarration"].ToString();
                    model3.TransactionMasterId = dt.Rows[dt.Rows.Count - 1]["TransactionMasterId"].ToString();
                    model3.className = "redcolor";
                    list.Add(model3);
                }
            }
            return list;
        }

        public EditTransactionViewModel TransactionMasterAndDetailById(int transactionMasterId, int insttituteId)
        {
            if (transactionMasterId == 0)
            {
                return new EditTransactionViewModel();
            }
            DataSet set1 = this.accountsAppAPI.TransactionMasterAndDetailById(this.sKey, insttituteId, transactionMasterId);
            EditTransactionViewModel model = set1.Tables[0].DataTableToList<EditTransactionViewModel>().FirstOrDefault<EditTransactionViewModel>();
            model.ledgerViewModels = set1.Tables[1].DataTableToList<EditTransactionLedgerViewModel>();
            return model;
        }

        public int TransactionMasterAndDetailByVoucherNo(int insttituteId, int vocherType, string vocherNumber)
        {
            DataSet set = this.accountsAppAPI.TransactionMasterAndDetailByVoucherNo(this.sKey, insttituteId, vocherNumber, vocherType);
            if ((set == null) || (set.Tables[0].Rows.Count <= 0))
            {
                return 0;
            }
            return Convert.ToInt32(set.Tables[0].Rows[0]["TransactionMasterId"]);
        }

        public string TransactionMasterInsert(TransactionsViewModel transactionsViewModel, int insttituteId, int financialYearId, int deptmentId, int userId)
        {
            TransactionMaster transactionMaster = new TransactionMaster
            {
                DepartmentId = deptmentId,
                FinancialYearId = financialYearId,
                InsertDate = DateTime.Now,
                InsertUserAccountId = userId,
                InstId = insttituteId,
                MasterLedgerId = 0,
                ForInstId = transactionsViewModel.DeptId,
                TotalAmount = transactionsViewModel.TotalCredit
            };
            DateTime transactionDate = transactionsViewModel.TransactionDate;
            transactionMaster.TransactionDate = transactionDate.Date;
            transactionMaster.UpdateDate = DateTime.Now;
            transactionMaster.UpdateUserAccountId = userId;
            transactionMaster.VoucherNo = transactionsViewModel.NewVocherNo;
            transactionMaster.VoucherTypeId = transactionsViewModel.VocherType;
            transactionMaster.MasterNarration = transactionsViewModel.MasterNarration;
            transactionMaster.ChequeNo = transactionsViewModel.ChequeorCash;
            char[] separator = new char[] { '/' };
            transactionMaster.UniqueTransactionNo = new int?(Convert.ToInt32(transactionsViewModel.NewVocherNo.Split(separator)[0]));
            List<AccountsAppWeb.Core.com.kccsasr.accounts.TransactionDetail> list = new List<AccountsAppWeb.Core.com.kccsasr.accounts.TransactionDetail>();
            using (List<AccountLedgers>.Enumerator enumerator = transactionsViewModel.accountLedgers.GetEnumerator())
            {
                while (true)
                {
                    int ledgerId;
                    decimal debit;
                    decimal credit;
                    bool flag;
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            string str;
                            AccountLedgers current = enumerator.Current;
                            ledgerId = current.LedgerId;
                            debit = current.Debit;
                            credit = current.Credit;
                            flag = false;
                            if (ledgerId == 1)
                            {
                                flag = true;
                                DataSet set1 = new DataSet();
                                DataSet set = this.accountsAppAPI.OpeningBalance(this.sKey, transactionsViewModel.DeptId, financialYearId, insttituteId, DateTime.Now.AddDays(1.0), 1, 0x10f447);
                                int num4 = 0;
                                if (set.Tables[0].Rows.Count == 0)
                                {
                                    num4 = 0;
                                }
                                else if (Convert.ToDecimal(set.Tables[0].Rows[0]["Debit"]) > 0M)
                                {
                                    num4 = Convert.ToInt32(set.Tables[0].Rows[0]["Debit"]);
                                }
                                else if (Convert.ToDecimal(set.Tables[0].Rows[0]["Credit"]) > 0M)
                                {
                                    num4 = Convert.ToInt32(set.Tables[0].Rows[0]["Credit"]);
                                }
                                if ((debit > num4) && (transactionMaster.VoucherTypeId == 4))
                                {
                                    return "Cash Balance Is Going Negative";
                                }
                            }
                            if (!flag)
                            {
                                break;
                            }
                            if ((credit > 20000M) && (financialYearId == 5))
                            {
                                str = "Cash Amount is too large";
                            }
                            else
                            {
                                if (((credit <= 10000M) || ((financialYearId <= 5) || (transactionMaster.VoucherTypeId == 3))) || (transactionMaster.VoucherTypeId == 5))
                                {
                                    break;
                                }
                                str = "Cash amount is exceeding Rs.10000/-, Please check again";
                            }
                            return str;
                        }
                        else
                        {
                            goto TR_0000;
                        }
                        break;
                    }
                    AccountsAppWeb.Core.com.kccsasr.accounts.TransactionDetail item = new AccountsAppWeb.Core.com.kccsasr.accounts.TransactionDetail();
                    item.Debit = debit;
                    item.Credit = credit;
                    item.LedgerId = ledgerId;
                    item.IsCashEntry = flag;
                    item.TransactionDetailsId = 0;
                    item.TransactionMasterId = 0;
                    item.UniqueID = "";
                    item.ForInstId = new int?(transactionsViewModel.DeptId);
                    list.Add(item);
                }
            }
        TR_0000:
            return ((this.accountsAppAPI.TransactionMasterInsert(insttituteId, transactionMaster, list.ToArray(), 0) <= 0) ? "Transaction Failed" : "Transaction saved");
        }

        public string TransactionMasterUpdate(TransactionsViewModel transactionsViewModel, int insttituteId, int financialYearId, int deptmentId, int userId)
        {
            TransactionMaster transactionMaster = new TransactionMaster
            {
                DepartmentId = deptmentId,
                FinancialYearId = financialYearId,
                InsertDate = DateTime.Now,
                InsertUserAccountId = userId,
                InstId = insttituteId,
                MasterLedgerId = 0,
                ForInstId = transactionsViewModel.DeptId,
                TotalAmount = transactionsViewModel.TotalCredit
            };
            DateTime transactionDate = transactionsViewModel.TransactionDate;
            transactionMaster.TransactionDate = transactionDate.Date;
            transactionMaster.UpdateDate = DateTime.Now;
            transactionMaster.UpdateUserAccountId = userId;
            transactionMaster.VoucherNo = transactionsViewModel.NewVocherNo;
            transactionMaster.VoucherTypeId = transactionsViewModel.VocherType;
            transactionMaster.MasterNarration = transactionsViewModel.MasterNarration;
            transactionMaster.ChequeNo = transactionsViewModel.ChequeorCash;
            char[] separator = new char[] { '/' };
            string[] strArray = transactionsViewModel.NewVocherNo.Split(separator);
            transactionMaster.UniqueTransactionNo = new int?(Convert.ToInt32(strArray[0]));
            List<AccountsAppWeb.Core.com.kccsasr.accounts.TransactionDetail> list = new List<AccountsAppWeb.Core.com.kccsasr.accounts.TransactionDetail>();
            using (List<AccountLedgers>.Enumerator enumerator = transactionsViewModel.accountLedgers.GetEnumerator())
            {
                while (true)
                {
                    int ledgerId;
                    decimal debit;
                    decimal credit;
                    bool flag;
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            string str;
                            AccountLedgers current = enumerator.Current;
                            ledgerId = current.LedgerId;
                            debit = current.Debit;
                            credit = current.Credit;
                            flag = false;
                            if (ledgerId == 1)
                            {
                                flag = true;
                                DataSet set1 = new DataSet();
                                DataSet set = this.accountsAppAPI.OpeningBalance(this.sKey, transactionsViewModel.DeptId, financialYearId, insttituteId, DateTime.Now.AddDays(1.0), 1, 0x10f447);
                                int num4 = 0;
                                if (set.Tables[0].Rows.Count == 0)
                                {
                                    num4 = 0;
                                }
                                else if (Convert.ToDecimal(set.Tables[0].Rows[0]["Debit"]) > 0M)
                                {
                                    num4 = Convert.ToInt32(set.Tables[0].Rows[0]["Debit"]);
                                }
                                else if (Convert.ToDecimal(set.Tables[0].Rows[0]["Credit"]) > 0M)
                                {
                                    num4 = Convert.ToInt32(set.Tables[0].Rows[0]["Credit"]);
                                }
                                if ((debit > num4) && (Convert.ToInt32(strArray[0]) == 4))
                                {
                                    return "Cash Balance Is Going Negative";
                                }
                            }
                            if (!flag)
                            {
                                break;
                            }
                            if ((credit > 20000M) && (financialYearId == 5))
                            {
                                str = "Cash Amount is too large";
                            }
                            else
                            {
                                if (((credit <= 10000M) || ((financialYearId <= 5) || (transactionMaster.VoucherTypeId == 3))) || (transactionMaster.VoucherTypeId == 5))
                                {
                                    break;
                                }
                                str = "Cash amount is exceeding Rs.10000/-, Please check again";
                            }
                            return str;
                        }
                        else
                        {
                            goto TR_0000;
                        }
                        break;
                    }
                    AccountsAppWeb.Core.com.kccsasr.accounts.TransactionDetail item = new AccountsAppWeb.Core.com.kccsasr.accounts.TransactionDetail();
                    item.Debit = debit;
                    item.Credit = credit;
                    item.LedgerId = ledgerId;
                    item.IsCashEntry = flag;
                    item.TransactionDetailsId = 0;
                    item.TransactionMasterId = 0;
                    item.UniqueID = "";
                    item.ForInstId = new int?(transactionsViewModel.DeptId);
                    list.Add(item);
                }
            }
        TR_0000:
            return ((this.accountsAppAPI.TransactionMasterInsert(insttituteId, transactionMaster, list.ToArray(), transactionsViewModel.MasterTransactionId) <= 0) ? "Transaction Failed" : "Transaction saved");
        }

        public List<ResetVochersViewModel> UpdateTransactionMasterByMasterID(int insttituteId, int financialYearId, DateTime fromDate, int voucherTypeId)
        {
            DataSet set = this.accountsAppAPI.UpdateTransactionMasterByMasterID(this.sKey, insttituteId, fromDate, voucherTypeId, financialYearId);
            return ((set == null) ? new List<ResetVochersViewModel>() : set.Tables[0].DataTableToList<ResetVochersViewModel>().Select<ResetVochersViewModel, ResetVochersViewModel>(delegate (ResetVochersViewModel rec) {
                object[] objArray1 = new object[] { rec.NewNotiNum, "/", rec.TransactionDateOriginal.ToString("dd/MM/yyyy").Replace("/", "."), "/", insttituteId.ToString() };
                rec.NewVoucherNo = string.Concat(objArray1);
                return rec;
            }).ToList<ResetVochersViewModel>());
        }

        public bool UpdateTransactionMasterWithTable(int insttituteId, int financialYearId, int voucherTypeId, List<NewResetVochers> resetVochers)
        {
            List<UpdateTransactionMasterModel> items = new List<UpdateTransactionMasterModel>();
            long num = 0x5f5e0ffL;
            foreach (NewResetVochers vochers in resetVochers)
            {
                UpdateTransactionMasterModel item = new UpdateTransactionMasterModel();
                item.StringColumn1 = vochers.TransactionMasterId.ToString();
                item.StringColumn2 = vochers.NewNotiNum.ToString();
                item.StringColumn3 = vochers.NewVoucherNo;
                item.StringColumn4 = (num += 1L).ToString();
                items.Add(item);
            }
            DataTable dt = items.ToDataTable<UpdateTransactionMasterModel>();
            dt.TableName = "Gurpreet";
            return this.accountsAppAPI.UpdateTransactionMasterWithTable(this.sKey, insttituteId, voucherTypeId, financialYearId, dt);
        }
    }
}
