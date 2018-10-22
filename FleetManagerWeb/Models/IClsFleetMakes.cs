namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsFleetMakes
    {
        DeleteFleetMakesResult DeleteFleetMakes(string strFleetMakesList, long lgDeletedBy);

        List<SelectListItem> GetAllFleetMakesForDropDown();

        List<GetFleetMakesAllResult> GetFleetMakesAll();

        ClsFleetMakes GetFleetMakesByFleetMakesId(long lgFleetMakesId);

        bool IsFleetMakesExists(long lgFleetMakesId, string strFleetMakesName);

        long SaveFleetMakes(ClsFleetMakes objSave);

        List<SearchFleetMakesResult> SearchFleetMakes(int inRow, int inPage, string strSearch, string strSort);
    }
}