namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;
    using System.Transactions;

    public partial class ClsMail : IClsMail
    {
        /// <summary>   Context for the object data. </summary>
        private MailDataContext objDataContext = null;

        public int inId { get; set; }
        public string strMessage { get; set; }
        public string strUserIds { get; set; }

        public DeleteMailResult DeleteMail(string strMailId, long lgDeletedBy)
        {
            try
            {
                using (this.objDataContext = new MailDataContext(Functions.StrConnection))
                {
                    DeleteMailResult result = this.objDataContext.DeleteMail(strMailId, lgDeletedBy, PageMaster.Inbox).FirstOrDefault();
                    if (result == null)
                    {
                        result = new DeleteMailResult();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Inbox, mySession.Current.UserId);
                return null;
            }
        }
        public long SaveMail(ClsMail objSave)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new MailDataContext(Functions.StrConnection))
                    {
                        var result = this.objDataContext.InsertMail(objSave.strMessage, objSave.strUserIds, PageMaster.Compose.ToString().intSafe(), mySession.Current.UserId).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.inId = result.InsertedId;
                        }
                        scope.Complete();
                    }
                    return objSave.inId;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Compose, mySession.Current.UserId);
                return 0;
            }
        }

        public List<SearchMailResult> SearchMail(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new MailDataContext(Functions.StrConnection))
                {
                    return this.objDataContext.SearchMail(mySession.Current.UserId, inRow, inPage, strSort, strSearch).ToList();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Inbox, mySession.Current.UserId);
                return null;
            }
        }
    }
}