namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsMail
    {
        DeleteMailResult DeleteMail(string strMailId, long lgDeletedBy);
        long SaveMail(ClsMail objSave);
        List<SearchMailResult> SearchMail(int inRow, int inPage, string strSearch, string strSort);
    }

}